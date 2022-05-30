using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Models
{
    public class HistoryModel
    {
        public string Name { get; set; }
        public string Episode { get; set; }
        // HOST
        public string ErrorMessage { get; set; }
        public bool InError { get; set; }
        public DateTime EndTime { get; set; }
    }
}
