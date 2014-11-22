using System.Collections.Generic;
using SharedPCL.Enums;

namespace SharedPCL.DataContracts
{
    public class QuestionDto
    {
        public int Id { get; set; } // Remove if not needed
        public string Text { get; set; }
        public QuestionType QuestionType { get; set; }
        public List<string> Answers { get; set; }

    }
}
