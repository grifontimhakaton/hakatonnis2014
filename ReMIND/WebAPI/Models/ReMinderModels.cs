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
        public virtual Subject Subject { get; set; }
        public virtual ICollection<User> Users { get; set; }
        //public virtual ICollection<QuestionArea> QuestionAreas { get; set; }
    }

    public class Event
    {
        [Key]
        public int ID { get; set; }
        public string EventText { get; set; }
        public string EventTitle { get; set; }
        public string EventDescription { get; set; }

        public DateTime EventStartUTC { get; set; }
        public DateTime EventEndtUTC { get; set; }

        //calc
        public TimeSpan Duration { get { return EventEndtUTC - EventStartUTC; } }

        //links
        public virtual Subject Subject { get; set; }
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
        public virtual ICollection<User> Users { get; set; }
    }

    //public class QuestionArea
    //{
    //    [Key]
    //    public int ID { get; set; }
    //    public string QuestionAreaTitle { get; set; }
    //    public string QuestionAreaDescription { get; set; }

    //    //links
    //    public virtual QuestionArea ParentArea { get; set; }
    //    public virtual ICollection<Question> Questions { get; set; }
    //    public virtual ICollection<Subject> Subjects { get; set; }
    //}

    public class Subject
    {
        [Key]
        public int ID { get; set; }
        public string SubjectTitle { get; set; }
        public string SubjectDescription { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
        //public virtual ICollection<QuestionArea> Questions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }

    public class User
    {
        [Key]
        public int ID { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }

        //links
        public virtual ICollection<Subject> Subjects { get; set; }
        //public virtual ICollection<QuestionArea> QuestionAreas { get; set; }
        public virtual ICollection<QuestionAnswer> QuestionAnswers { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }

}