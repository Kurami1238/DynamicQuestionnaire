using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Models
{
    public class Mondai
    {
        public Guid MondaiID { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public string Naiyo { get; set; }
        public int Zettai { get; set; }
    }
}