using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Preferences;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using ReMinder.Adapters;
using ReMinder.Helpers;
using SharedPCL.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

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

        ISharedPreferences localSettings;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Question);

            localSettings = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
            userId = localSettings.GetInt(Helpers.Constants.USER_ID, 0);
            if (userId > 0)
            {
                txtQuestion = (TextView)FindViewById(Resource.Id.txtQuestion);

                listAnswers = (ListView)FindViewById(Resource.Id.listAnswers);

                btnClose = (Button)FindViewById(Resource.Id.btnClose);
                btnClose.Click += CloseReMinder;

                btnNextQuestion = (Button)FindViewById(Resource.Id.btnNextQuestion);
                btnNextQuestion.Click += BindNextQuestion;


                //List<SubjectDTO> subjectList = MethodHelper.GetUserSubjects(userId);
                //List<SubjectDTO> userSubjects = subjectList.FindAll(subject => subject.UserSelected);
                //if (userSubjects.Count > 0)
                //{
                //    localSettings = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
                //    ISharedPreferencesEditor editor = localSettings.Edit();
                //    editor.PutString(Helpers.Constants.USER_SUBJECTS, JsonConvert.SerializeObject(userSubjects));
                //    editor.Apply();

                //    foreach (var subject in userSubjects)
                //    {
                //        questionList.AddRange(MethodHelper.GetQuestions(userId, subject.SubjectID));
                //    }

                //    if (questionList.Count > 0)
                //    {
                //        BindQuestionWithAnswers();
                //    }
                //}
                //else
                //{
                //    StartActivity(typeof(SettingsActivity));
                //}
            }
            else
            {
                StartActivity(typeof(LoginActivity));
            }
        }

        //private void RefitText(String text, int textWidth)
        //{
        //    if (textWidth <= 0)
        //        return;
        //    var mTestPaint = new Android.Graphics.Paint();
        //    mTestPaint.Set(txtQuestion.Paint);

        //    int targetWidth = textWidth - txtQuestion.PaddingLeft - txtQuestion.PaddingRight;
        //    float hi = 100;
        //    float lo = 2;
        //    float threshold = 0.5f; // How close we have to be

        //    mTestPaint.Set(txtQuestion.Paint);

        //    while ((hi - lo) > threshold)
        //    {
        //        float size = (hi + lo)/2;
        //        mTestPaint.TextSize = size;
        //        if (mTestPaint.MeasureText(text) >= targetWidth)
        //            hi = size; // too big
        //        else
        //            lo = size; // too small
        //    }
        //    // Use lo so that we undershoot rather than overshoot
        //    //txtQuestion.TextSize = (Android.Util.TypedValue.ComplexToDimensionPixelSize(), lo);
        //    txtQuestion.TextSize =  (int)lo;
        //}

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

            localSettings = PreferenceManager.GetDefaultSharedPreferences(this.BaseContext);
            var localSubjects = localSettings.GetString(Helpers.Constants.USER_SUBJECTS, "More");
            List<SubjectDTO> subjectList = new List<SubjectDTO>();
            try
            {
                subjectList = JsonConvert.DeserializeObject<List<SubjectDTO>>(localSubjects);
            }
            catch (Exception ex)
            {
                subjectList = MethodHelper.GetUserSubjects(userId);
            }


            List<SubjectDTO> userSubjects = subjectList.FindAll(subject => subject.UserSelected);
            if (userSubjects.Count > 0)
            {
                foreach (var subject in userSubjects)
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
            NotificationHelper.OnResumeActivity(this.BaseContext);
        }

        private void BindQuestionWithAnswers()
        {
            listAnswers.ItemClick -= OnAnswerClicked;
            var rnd = new Random();

            currentQuestion = questionList[rnd.Next(0, questionList.Count)];
            txtQuestion.Text = currentQuestion.QuestionText != null ? currentQuestion.QuestionText.ToUpper() : string.Empty;
            //RefitText(txtQuestion.Text, 700);

            listAnswers.ItemClick += OnAnswerClicked;

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
            var textView = (TextView)e.Parent.GetChildAt(e.Position).FindViewById(Resource.Id.txtAnswerEnum);
            textView.SetTextColor(Resources.GetColor(Resource.Color.action_bar_background));

            var textViewAnswer = (TextView)e.Parent.GetChildAt(e.Position).FindViewById(Resource.Id.txtAnswerText);
            textViewAnswer.SetTextColor(Resources.GetColor(Resource.Color.action_bar_background));
            textViewAnswer.SetBackgroundDrawable(Resources.GetDrawable(Resource.Drawable.QuestionSingleRowStylePurple));
            var scale = Resources.DisplayMetrics.Density;
            var padding_5dp = (int)(5 * scale + 0.5f);
            textViewAnswer.SetPadding(padding_5dp, 0, 0, 0);

            if (!questionAnswer.Correct)
            {
                selectedRowImage.SetImageResource(Resource.Drawable.crossmark);

                int correctAnswerIndex = currentQuestion.QuestionAnswers.FindIndex(item => item.Correct);
                if (correctAnswerIndex > -1)
                {
                    var selectedRowCorrectImage = (ImageView)e.Parent.GetChildAt(correctAnswerIndex).FindViewById(Resource.Id.imgCheckmark);
                    selectedRowCorrectImage.SetImageResource(Resource.Drawable.checkmark);
                }
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

            listAnswers.ItemClick -= OnAnswerClicked;

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

