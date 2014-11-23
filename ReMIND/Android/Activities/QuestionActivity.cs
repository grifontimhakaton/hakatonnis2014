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

        private TextView txtQuestion;
        private ListView listAnswers;
        private Button btnClose;

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

            if(userId > 0)
            {
                List<SubjectDTO> subjectList = MethodHelper.GetUserSubjects(userId);
                foreach (var subject in subjectList)
                {
                    questionList.AddRange(MethodHelper.GetQuestions(userId, subject.SubjectID));
                }

                if(questionList.Count > 0)
                {
                    var question = questionList[0];
                    txtQuestion.Text = question.QuestionText + "TEst test test test test";
                    RefitText(txtQuestion.Text, 700);
                    if (question.QuestionAnswers.Count > 1)
                    {
                        listAnswers.Adapter = new AnswerAdapter(this, question.QuestionAnswers.Select(x => x.QuestionAnswerText).ToArray());
                        listAnswers.ItemClick += OnAnswerClicked;
                    }
                    else
                    {
                        //TODO Add data for answer to read user
                    }
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

        private void OnAnswerClicked(object sender, EventArgs e)
        {
            var test = 1;
        }

        private void CloseReMinder(object sender, EventArgs e)
        {
            Finish();
        }
    }
}

