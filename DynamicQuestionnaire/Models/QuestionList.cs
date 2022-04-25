using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Models
{
    public class QuestionList
    {
        public Guid QuestionListID { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public Guid? NaiyoListID { get; set; }
        public int Zyunban { get; set; }
        public int Zettai { get; set; }
        public List<NaiyoList> NaiyoList { get; set; }
    }
}