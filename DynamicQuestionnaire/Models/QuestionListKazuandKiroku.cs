using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Models
{
    public class QuestionListKazuandKiroku
    {
        public string Title { get; set; }
        public int Type { get; set; }
        //public List<string> Naiyo { get; set; }
        //public List<int> Kazu { get; set; }
        public List<KazuandKiroku> KazuandKiroku { get; set; }
    }
    public class KazuandKiroku
    {
        public string Naiyo { get; set; }
        public int Kazu { get; set; }
    }
}