﻿using System;
using Android.App;
using Android.Content;
using ReMinder.Helpers;
using Android.Preferences;
using Android.Runtime;
using ReMinder.Services;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Content.PM;
using SharedPCL.DataContracts;
using System.Collections.Generic;
using System.Linq;
using ReMinder.Adapters;

namespace ReMinder.Activities
{
    [Activity(ScreenOrientation = ScreenOrientation.Portrait)]
    public class QuestionActivity : Activity
    {
        private int userId;
        private List<SubjectDTO> subjectList = new List<SubjectDTO>();
        private List<QuestionDTO> questionList = new List<QuestionDTO>();

        private QuestionDTO currentQuestion;

        private TextView txtQuestion;
        private ListView listAnswers;
        private Button btnClose;
        private Button btnNextQuestion;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Question);

            ISharedPreferences localSettings = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
            userId = localSettings.GetInt(Helpers.Constants.USER_ID, 0);

            txtQuestion = (TextView)FindViewById(Resource.Id.txtQuestion);
            listAnswers = (ListView)FindViewById(Resource.Id.listAnswers);

            btnClose = (Button)FindViewById(Resource.Id.btnClose);
            btnClose.Click += CloseReMinder;

            btnNextQuestion = (Button)FindViewById(Resource.Id.btnNextQuestion);
            btnNextQuestion.Click += BindNextQuestion;

            if(userId > 0)
            {
                List<SubjectDTO> subjectList = MethodHelper.GetUserSubjects(userId);
                foreach (var subject in subjectList)
                {
                    questionList.AddRange(MethodHelper.GetQuestions(userId, subject.SubjectID));
                }

                if(questionList.Count > 0)
                {
                    BindQuestionWithAnswers();
                }
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.mainMenu, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            OpenSettingsActivity();

            return base.OnOptionsItemSelected(item);
        }

        public override bool OnMenuOpened(int featureId, IMenu menu)
        {
            OpenSettingsActivity();
            
            return base.OnMenuOpened(featureId, menu);
        }

        private void OpenSettingsActivity()
        {
            StartActivity(typeof(SettingsActivity));
        }

        protected override void OnPause()
        {
            base.OnPause();
            NotificationHelper.OnPauseActivity(this.BaseContext);
        }

        protected override void OnResume()
        {
            base.OnResume();
            NotificationHelper.OnResumeActivity(this.BaseContext);
        }

        private void BindQuestionWithAnswers()
        {
            var rnd = new Random();

            currentQuestion = questionList[rnd.Next(0, questionList.Count)];
            txtQuestion.Text = currentQuestion.QuestionText;
            if (currentQuestion.QuestionAnswers.Count > 1)
            {
                
                currentQuestion.QuestionAnswers = currentQuestion.QuestionAnswers.OrderBy(item => rnd.Next()).ToList();

                listAnswers.Adapter = new AnswerAdapter(this, currentQuestion.QuestionAnswers.Select(x => x.QuestionAnswerText).ToArray());
                listAnswers.ItemClick += OnAnswerClicked;
            }
            else
            {
                //TODO Add data for answer to read user
            }
        }

        private void OnAnswerClicked(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var questionAnswer = currentQuestion.QuestionAnswers[e.Position];
            View correctItem;
            if (!questionAnswer.Correct)
            {
                e.View.SetBackgroundColor(Android.Graphics.Color.Red);
                correctItem = e.Parent.GetChildAt(currentQuestion.QuestionAnswers.FindIndex(item => item.Correct));
            }
            else
            {
                correctItem = e.View;
            }

            correctItem.SetBackgroundColor(Android.Graphics.Color.Green);
            if (MethodHelper.AnswerQuestion(questionAnswer.Id, userId))
            {
                questionList.Remove(currentQuestion);
            }
        }

        private void BindNextQuestion(object sender, EventArgs e)
        {
            BindQuestionWithAnswers();
        }

        private void CloseReMinder(object sender, EventArgs e)
        {
            Finish();
        }
    }
}

