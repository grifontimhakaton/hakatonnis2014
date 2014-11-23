using System.Collections.Generic;

namespace SharedPCL.DataContracts
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string QuestionText { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionExplanation { get; set; }

        public List<QuestionAnswerDTO> QuestionAnswers { get; set; }
    }

    public class QuestionAnswerDTO
    {
        public int Id { get; set; }
        public string QuestionAnswerText { get; set; }
        public bool Correct { get; set; }
    }
}
