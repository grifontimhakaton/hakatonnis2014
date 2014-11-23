using System;
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
            listAnswers.ItemClick += OnAnswerClicked;

            btnClose = (Button)FindViewById(Resource.Id.btnClose);
            btnClose.Click += CloseReMinder;

            btnNextQuestion = (Button)FindViewById(Resource.Id.btnNextQuestion);
            btnNextQuestion.Click += BindNextQuestion;

            if (userId > 0)
            {
                List<SubjectDTO> subjectList = MethodHelper.GetUserSubjects(userId);
                if (subjectList.Count > 0)
                {
                    foreach (var subject in subjectList)
                    {
                        questionList.AddRange(MethodHelper.GetQuestions(userId, subject.SubjectID));
                    }

                    if (questionList.Count > 0)
                    {
                        BindQuestionWithAnswers();
                    }
                }
                else
                {
                    StartActivity(typeof(SettingsActivity));
                }
            }
        }

        private void RefitText(String text, int textWidth)
        {
            if (textWidth <= 0)
                return;
            var mTestPaint = new Android.Graphics.Paint();
            mTestPaint.Set(txtQuestion.Paint);

            int targetWidth = textWidth - txtQuestion.PaddingLeft - txtQuestion.PaddingRight;
            float hi = 100;
            float lo = 2;
            float threshold = 0.5f; // How close we have to be

            mTestPaint.Set(txtQuestion.Paint);

            while ((hi - lo) > threshold)
            {
                float size = (hi + lo)/2;
                mTestPaint.TextSize = size;
                if (mTestPaint.MeasureText(text) >= targetWidth)
                    hi = size; // too big
                else
                    lo = size; // too small
            }
            // Use lo so that we undershoot rather than overshoot
            //txtQuestion.TextSize = (Android.Util.TypedValue.ComplexToDimensionPixelSize(), lo);
            txtQuestion.TextSize =  (int)lo;
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
            RefitText(txtQuestion.Text, 700);
            if (currentQuestion.QuestionAnswers.Count > 1)
            {
                currentQuestion.QuestionAnswers = currentQuestion.QuestionAnswers.OrderBy(item => rnd.Next()).ToList();

                listAnswers.Adapter = new AnswerAdapter(this, currentQuestion.QuestionAnswers.Select(x => x.QuestionAnswerText).ToArray());
            }
            else
            {
                //TODO Add data for answer to read user
            }
        }

        private void OnAnswerClicked(object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var questionAnswer = currentQuestion.QuestionAnswers[e.Position];
            
            var selectedRowImage = (ImageView)e.Parent.GetChildAt(e.Position).FindViewById(Resource.Id.imgCheckmark);
            if (!questionAnswer.Correct)
            {
                selectedRowImage.SetImageResource(Resource.Drawable.crossmark);
                
                //int correctAnswerIndex = currentQuestion.QuestionAnswers.FindIndex(item => item.Correct);
                //if (correctAnswerIndex > -1)
                //{
                    //TextView correctTextView = (TextView)e.Parent.GetChildAt(correctAnswerIndex).FindViewById(Resource.Id.txtAnswerText);
                    //textView.SetTextColor(Android.Graphics.Color.Green);
                //}
            }
            else
            {
                selectedRowImage.SetImageResource(Resource.Drawable.checkmark);
            }

            //textView = (TextView)e.View.FindViewById(Resource.Id.txtAnswerText);
            //if (textView != null)
            //{
            //    textView.SetTextColor(Resources.GetColor(Resource.Color.textColor));
            //}


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

