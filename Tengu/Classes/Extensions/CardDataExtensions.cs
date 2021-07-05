using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Classes.DataModels;

namespace Tengu.Classes.Extensions
{
    public static class CardDataExtensions
    {
        public static string ConvertToJson(this CardData source)
        {
            return JsonConvert.SerializeObject(source);
        }
    }
}
