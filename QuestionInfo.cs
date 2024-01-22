using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamScraper
{
    internal class QuestionInfo
    {
        public int id { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public bool randomiseAnswers { get; set; }
        public List<ChoiceInfo> answers { get; set; } = new ();
    }

    internal class ChoiceInfo
    {
        public string text { get; set; }
        public bool correct { get; set; } = false;
    }
}
