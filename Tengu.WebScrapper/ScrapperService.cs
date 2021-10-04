using HtmlAgilityPack;
using NLog;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tengu.Models;
using Tengu.Shared.Enums;
using Tengu.WebScrapper.Extensions;

namespace Tengu.WebScrapper
{
    public class ScrapperService
    {
        public const string _MAIN_URL = "https://www.animesaturn.it";
        public const string _CALENDAR_URL = "https://www.animesaturn.it/calendario";
        public const string _SEARCH_URL = "https://www.animesaturn.it/animelist?search=";

        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private IWebDriver driver;

        private Dictionary<int, List<string>> week_list;

        #region Singleton

        private static ScrapperService _instance = null;
        private static readonly object lockObject = new object();

        private ScrapperService()
            : base()
        {
            Clear();
        }

        public static ScrapperService Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new ScrapperService();
                    }
                    return _instance;
                }
            }
        }

        #endregion

        #region Public Methods

        #region Latest Episodes
        public bool InitializeDriver(out string error_message)
        {
            error_message = string.Empty;

            try
            {
                ChromeOptions chromeOptions = new ChromeOptions();
                chromeOptions.AddArgument("headless");
                chromeOptions.AddArgument("incognito");
                chromeOptions.AddArgument("--ignore-certificate-errors");
                chromeOptions.AddArgument("--disable-popup-blocking");

                ChromeDriverService chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = true;    // This is to hidden the console.

                driver = new ChromeDriver(chromeDriverService, chromeOptions);

                WriteLog("Driver Initialized!");

                // Set implicit wait (10 Seconds)
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                driver.Navigate().GoToUrl(_MAIN_URL);

                WriteLog("Navigated to URL: " + _MAIN_URL);
            }
            catch (Exception ex)
            {
                error_message = ex.Message;
                WriteError(ex.Message);
            }

            return string.IsNullOrEmpty(error_message);
        }
        public void CloseDriver()
        {
            new Thread(() =>
            {
                try
                {
                    if (driver != null)
                    {
                        driver.Quit();
                        driver.Dispose();
                    }
                }
                catch (Exception)
                {
                    // Do nothing .. 
                }
            }).Start();
        }
        public List<Anime> RefreshLatestEpisodes()
        {
            List<Anime> animes;
            bool stale_element;

            do
            {
                animes = new List<Anime>();
                stale_element = false;

                try
                {
                    var elements = driver.FindElement(By.Id("results"), 10000).FindElements(By.XPath("./div/div"));

                    foreach (IWebElement elem in elements)
                    {
                        Anime data = new Anime();

                        IWebElement node_datas = elem.FindElement(By.XPath("./div/a"));

                        if (node_datas != null)
                        {
                            data.LinkVideo = node_datas.GetAttribute("href");

                            // slow down, it can be done later
                            //data.GetAnimeCardFromVideoUrl();

                            data.Title = node_datas.GetAttribute("title");

                            data.ImageCover = node_datas.FindElement(By.XPath("./img")).GetAttribute("src");
                        }

                        IWebElement node_ep = elem.FindElement(By.ClassName("anime-episode"));

                        if (node_ep != null)
                        {
                            data.Episode = node_ep.Text.NormalizeString();
                        }

                        animes.Add(data);
                    }
                }
                catch (StaleElementReferenceException)
                {
                    stale_element = true;
                    Thread.Sleep(50);
                }

            } while (stale_element);

            return animes;
        }

        public bool NextPage()
        {
            bool res = false;
            bool stale_element;

            do
            {
                stale_element = false;

                try
                {
                    IWebElement node_pag = driver.FindElement(By.ClassName("paginationas"));

                    if (node_pag != null)
                    {
                        IWebElement btnNext = node_pag.FindElement(By.XPath("./li/a[@title='Next']"));

                        if (btnNext != null)
                        {
                            // ClickButton
                            IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
                            ex.ExecuteScript("arguments[0].click();", btnNext);

                            WriteLog("Clicked Next Button page..");

                            res = true;
                        }
                    }
                }
                catch (StaleElementReferenceException)
                {
                    stale_element = true;
                    Thread.Sleep(50);
                }
                catch (Exception)
                {
                    // Do nothing ..
                    res = false;
                }

            } while (stale_element);

            return res;
        }
        public bool PrevPage()
        {
            bool res = false;
            bool stale_element;

            do
            {
                stale_element = false;

                try
                {
                    IWebElement node_pag = driver.FindElement(By.ClassName("paginationas"));

                    if (node_pag != null)
                    {
                        IWebElement btnPrev = node_pag.FindElement(By.XPath("./li/a[@title='Last']"));

                        if (btnPrev != null)
                        {
                            // ClickButton
                            IJavaScriptExecutor ex = (IJavaScriptExecutor)driver;
                            ex.ExecuteScript("arguments[0].click();", btnPrev);

                            WriteLog("Clicked Previous Button page..");

                            res = true;
                        }
                    }
                }
                catch (StaleElementReferenceException)
                {
                    stale_element = true;
                    Thread.Sleep(50);
                }
                catch (Exception)
                {
                    // Do nothing ..
                    res = false;
                }

            } while (stale_element);

            return res;
        }
        #endregion

        #region Calendar
        public void RefreshCalendar()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(_CALENDAR_URL);

            week_list.Clear();

            HtmlNode node_table = htmlDoc.DocumentNode.SelectSingleNode("//table/tbody");

            if (node_table != null)
            {
                foreach (HtmlNode node_tr in node_table.SelectNodes("./tr"))
                {
                    HtmlNodeCollection nodes_td = node_tr.SelectNodes("./td");

                    for (int i = 0; i < nodes_td.Count; i++)
                    {
                        if (week_list.ContainsKey(i))
                        {
                            if (week_list[i] == null)
                            {
                                week_list[i] = new List<string>();
                            }
                        }
                        else
                        {
                            week_list.Add(i, new List<string>());
                        }

                        if (!String.IsNullOrEmpty(nodes_td[i].InnerText))
                        {
                            week_list[i].Add(nodes_td[i].InnerText);
                        }
                    }
                }
            }
        }

        public List<string> GetDayAnimes(CalendarDay day)
        {
            return week_list[(int)day];
        }
        #endregion

        #region Search
        public List<Anime> RefreshSearch(string pattern)
        {
            string url = _SEARCH_URL + pattern.Replace(" ", "%20");
            List<Anime> search_list = new List<Anime>();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(url);

            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//div[@class='item-archivio']");

            if (nodes != null)
            {
                foreach (HtmlNode anime in nodes)
                {
                    Anime data = new Anime();

                    HtmlNodeCollection node_images = anime.SelectNodes("./a/img");

                    if (node_images != null)
                    {
                        foreach (HtmlNode image in node_images)
                        {
                            if (image.GetAttributeValue("class", null) == "rounded copertina-archivio")
                            {
                                data.ImageCover = image.GetAttributeValue("src", string.Empty);
                            }
                            if (image.GetAttributeValue("class", null) == "rounded locandina-archivio")
                            {
                                data.ImagePoster = image.GetAttributeValue("src", string.Empty);
                            }
                        }
                    }

                    HtmlNode node_info = anime.SelectSingleNode("./div[@class='info-archivio']");

                    if (node_info != null)
                    {
                        HtmlNode node_a = node_info.SelectSingleNode("./h3/a");

                        if (node_a != null)
                        {
                            data.LinkCard = node_a.GetAttributeValue("href", string.Empty);
                            data.Title = node_a.InnerText;
                        }

                        HtmlNode nodeDescr = node_info.SelectSingleNode("./a/p");

                        if (nodeDescr != null)
                        {
                            data.Description = nodeDescr.InnerText;
                        }
                    }

                    search_list.Add(data);
                }
            }

            return search_list;
        }
        #endregion

        #region Anime Card

        public Card_Data RefreshAnimeCard(string url)
        {
            Card_Data data = new Card_Data();

            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(url);

            HtmlNode nodeMain = htmlDoc.DocumentNode.SelectSingleNode("//div/div[@class='row altezza-row-anime']");

            if (nodeMain != null)
            {
                HtmlNodeCollection node_cols = nodeMain.SelectNodes("./div");

                if (node_cols != null && node_cols.Count == 2)
                {
                    #region LEFT COLUMN

                    HtmlNodeCollection nodesLeft = node_cols[0].SelectNodes("./div");

                    if (nodesLeft != null && nodesLeft.Count > 0)
                    {
                        if (nodesLeft[1] != null)
                        {
                            // IMAGE
                            data.ImageLink = nodesLeft[1].SelectSingleNode("./img")
                                                         .GetAttributeValue("src", string.Empty);
                        }

                        if (nodesLeft[2] != null)
                        {
                            // USEFUL LINKS
                            HtmlNodeCollection node_links = nodesLeft[2].SelectNodes("./a");

                            foreach (HtmlNode link in node_links)
                            {
                                UsefulLinkData link_data = new UsefulLinkData();

                                link_data.Link = link.GetAttributeValue("href", string.Empty);

                                link_data.Name = link.SelectSingleNode("./button").InnerText.NormalizeString();

                                link_data.Image = link.SelectSingleNode("./button/img")
                                                      .GetAttributeValue("src", string.Empty);

                                data.UsefulLinks.Add(link_data);
                            }
                        }
                    }

                    #endregion

                    #region RIGHT COLUMN

                    HtmlNodeCollection nodesRight = node_cols[1].SelectNodes("./div");

                    if (nodesRight != null && nodesRight.Count > 0)
                    {
                        if (nodesRight[0] != null)
                        {
                            // TITLE
                            data.Title = nodesRight[0].SelectSingleNode("./b").InnerText;
                        }

                        if (nodesRight[1] != null)
                        {
                            // ATTRIBUTES
                            foreach (HtmlNode node_hr in nodesRight[1].SelectNodes("./hr"))
                            {
                                HtmlNode node_b = node_hr.NextSibling.NextSibling;

                                data.Attributes.Add(new DictionaryData()
                                {
                                    Key = node_b.InnerText,
                                    Value = node_b.NextSibling.InnerText
                                });
                            }
                        }

                        if (nodesRight[2] != null)
                        {
                            // TAGS
                            foreach (HtmlNode node_tag in nodesRight[2].SelectNodes("./a"))
                            {
                                data.Tags.Add(node_tag.InnerText);
                            }
                        }

                        if (nodesRight[3] != null)
                        {
                            // PLOT
                            data.Plot = nodesRight[3].SelectSingleNode("./div/div[@id='shown-trama']").InnerText;
                        }

                        if (nodesRight[4] != null)
                        {
                            // EPISODES
                            foreach (HtmlNode node_anime in nodesRight[4].SelectNodes("//div[@data-type='anime']/div"))
                            {
                                HtmlNode sub_node = node_anime.SelectSingleNode("./a");

                                if (sub_node != null)
                                {
                                    data.Episodes.Add(new Anime()
                                    {
                                        Title = data.Title,
                                        Episode = sub_node.InnerText.NormalizeString(),
                                        LinkVideo = sub_node.GetAttributeValue("href", string.Empty)
                                    });
                                }
                            }
                        }
                    }

                    #endregion
                }
            }

            HtmlNode nodeCarousel = htmlDoc.DocumentNode.SelectSingleNode("//div/div[@id='carousel']");

            if (nodeCarousel != null)
            {
                HtmlNodeCollection node_relateds = nodeCarousel.SelectNodes("./div");

                foreach (HtmlNode related in node_relateds)
                {
                    RelatedData rel_data = new RelatedData();

                    HtmlNode sub_node = related.SelectSingleNode("./div/a");

                    if (sub_node != null)
                    {
                        rel_data.Link = sub_node.GetAttributeValue("href", string.Empty);
                        rel_data.Title = sub_node.GetAttributeValue("title", string.Empty);

                        rel_data.Image = sub_node.SelectSingleNode("./img")
                                                 .GetAttributeValue("src", string.Empty);

                        data.Related.Add(rel_data);
                    }
                }
            }

            return data;
        }

        #endregion

        #endregion

        #region Private Methods
        private void Clear()
        {
            week_list = new Dictionary<int, List<string>>();
        }

        private void WriteLog(string message)
        {
            log.Info(message);
        }
        private void WriteError(string message)
        {
            log.Error(message);
        }
        #endregion
    }
}
