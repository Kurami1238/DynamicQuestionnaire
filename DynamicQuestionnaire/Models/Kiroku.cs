using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DynamicQuestionnaire.Models
{
    public class Kiroku
    {
        public Guid KirokuID { get; set; }
        public Guid QuestionID { get; set; }
        public Guid KirokuListID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public DateTime Date { get; set; }
        public int Zyunban { get; set; }

    }
}