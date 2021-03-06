using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Models
{
    public class Question
    {
        public Guid QuestionID { get; set; }
        public Guid QuestionListID { get; set; }
        public string QName { get; set; }
        public string QSetume { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public int State { get; set; }
        public int Zyunban { get; set; }
        public int NowKirokusu { get; set; }
    }
}