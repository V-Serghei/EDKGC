using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDKGC.Enams;

namespace EDKGC.Models.ISO27001
{
    public class QuestionModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Quality Quality { get; set; } = Quality.None;
        public int Number { get; set; }
        public Answer Resolved { get; set; } = Answer.None;

    }
}
