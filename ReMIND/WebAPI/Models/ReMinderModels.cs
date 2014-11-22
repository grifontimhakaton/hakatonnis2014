using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
   
    public class Question
    {
        [Key]
        public int ID { get; set; }
        public string QuestionText { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionExplanation { get; set; }
        public int VotePositiveCount { get; set; }
        public int VoteNegativeCount { get; set; }

        //links
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual ICollection<QuestionArea> QuestionAreas { get; set; }
    }

    public class QuestionAnswer
    {
        [Key]
        public int ID { get; set; }
        public int QuestionID { get; set; }
        public string  AnswerText { get; set; }
        public int AnswerValue { get; set; }

        //links
        public virtual Question Question { get; set; }
    }

    public class QuestionArea
    {
        [Key]
        public int ID { get; set; }
        public string QuestionAreaTitle { get; set; }
        public string QuestionAreaDescription { get; set; }

        //links
        public virtual QuestionArea ParentArea { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Subject> Subjects { get; set; }
    }

    public class Subject
    {
        [Key]
        public int ID { get; set; }
        public string SubjectTitle { get; set; }
        public string SubjectDescription { get; set; }

        public virtual ICollection<QuestionArea> Questions { get; set; }
    }

}