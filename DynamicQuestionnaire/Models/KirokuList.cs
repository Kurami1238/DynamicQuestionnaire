using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Models
{
    public class KirokuList
    {
        public Guid KirokuListID { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public string Naiyo { get; set; }
        public int Zyunban { get; set; }
        public List<string> ckbNaiyo { get; set; }
    }
}