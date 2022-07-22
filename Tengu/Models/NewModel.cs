using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Models
{
    public class NewModel : ReactiveObject
    {
        public string Category { get; private set; } = string.Empty;
        public List<string> News { get; set; } = new();

        public NewModel(string category)
        {
            Category = category;
        }
    }
}
