using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Windows.Resources;
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Tasks;

namespace BundledFun
{
    public partial class PivotPage : PhoneApplicationPage
    {
        private String resultToShare = "";
        private String lastTrailers = "";
        private String lastSongs = "";

        private Boolean isImagesFinished = false;
        private Boolean isTrailersFinished = false;
        private Boolean isSongsFinished = false;

        private Boolean hasStarted_Images = false;
        private Boolean hasStarted_Trailers = false;
        private Boolean hasStarted_Songs = false;

        private int corrected_Images = 0;
        private int corrected_Trailers = 0;
        private int corrected_Songs = 0;

        private int totalTimeElapsed_Images = 0;
        private int totalTimeElapsed_Trailers = 0;
        private int totalTimeElapsed_Songs = 0;

        private int seconds_Images = 0;
        private int seconds_Trailers = 0;
        private int seconds_Songs = 0;

        private int totalQuestions_Images = 0;
        private int totalQuestions_Trailers = 0;
        private int totalQuestions_Songs = 0;
        

        private List<Question> questions_Images = new List<Question>();
        private Question currentQuestion_Images = new Question();

        private List<Question> questions_Trailers = new List<Question>();
        private Question currentQuestion_Trailers = new Question();

        private List<Question> questions_Songs = new List<Question>();
        private Question currentQuestion_Songs = new Question();

        private System.Windows.Threading.DispatcherTimer timer_Images;
        private System.Windows.Threading.DispatcherTimer timer_Trailers;
        private System.Windows.Threading.DispatcherTimer timer_Songs;

        private SoundEffect correctSound, wrongSound;

        MessagePrompt gameFinished;

        public PivotPage()
        {
            InitializeComponent();

            FrameworkDispatcher.Update();

            this.LoadSound("correct.wav", out correctSound);
            this.LoadSound("wrong.wav", out wrongSound);

            this.initialize_Images();
            this.initialize_Trailers();
            this.initialize_Songs();
            this.startImages();
        }

        void messagePromptImages_Completed(object sender, PopUpEventArgs<String, PopUpResult> e)
        {
            seconds_Images = 0;
            if (questions_Images.Count > 0)
            {
                questions_Images.RemoveAt(0);
                if (questions_Images.Count > 0)
                {
                    this.nextQuestion_Images();
                    timer_Images.Start();
                }
                else
                {
                    this.stopImages();
                }
            }
            else
            {
                this.stopImages();
            }
        }

        void messagePromptSongs_Completed(object sender, PopUpEventArgs<String, PopUpResult> e)
        {
            if (questions_Songs.Count > 0)
            {
                seconds_Songs = 0;
                questions_Songs.RemoveAt(0);
                if (questions_Songs.Count > 0)
                {
                    this.nextQuestion_Songs();
                    timer_Songs.Start();
                }
                else
                {
                    this.stopSongs();
                }
            }
            else
            {
                this.stopSongs();
            }
        }

        void messagePromptTrailers_Completed(object sender, PopUpEventArgs<String, PopUpResult> e)
        {
            seconds_Trailers = 0;
            if (questions_Trailers.Count > 0)
            {
                questions_Trailers.RemoveAt(0);
                if (questions_Trailers.Count > 0)
                {
                    this.nextQuestion_Trailers();
                    timer_Trailers.Start();
                }
                else
                {
                    this.stopTrailers();
                }
            }
            else
            {
                this.stopTrailers();
            }
        }

        void showGameFinished_Completed(object sender, PopUpEventArgs<String, PopUpResult> e)
        {
            gameFinished.Hide();
            this.restart();
        }

        private void btnShare_Click(object sender, RoutedEventArgs e)
        {
            ShareStatusTask shareStatusTask = new ShareStatusTask();
            shareStatusTask.Status = "Windows Phone Facebook Integration Development Test.";
            shareStatusTask.Show();
        }

        private void btnRetry_Click(object sender, RoutedEventArgs e)
        {
            gameFinished.Hide();
            this.restart();
        }

        private void showGameFinished()
        {
            int totalScore = (corrected_Images + corrected_Songs + corrected_Trailers);
            int totalQuestions = totalQuestions_Images + totalQuestions_Songs + totalQuestions_Trailers;
            int totalSecondsElapsed = (totalTimeElapsed_Images + totalTimeElapsed_Trailers + totalTimeElapsed_Songs);
            gameFinished = new MessagePrompt();
            gameFinished.Title = "Game Finished";
            gameFinished.Message = "Game Finished with a total score of: " + totalScore + "/" + totalQuestions + " and a total of " + totalSecondsElapsed + " seconds of elapsed time.\n\nClick Share Results if you want to share your results to your friends and click the Retry if you want to play again.";
            resultToShare = "I finished the playing BundledFun with a total score of: " + totalScore + "/" + totalQuestions + " and a total of " + totalSecondsElapsed + " seconds of elapsed time.";
            Button btnShare = new Button() { Content = "Share Results" };
            btnShare.Click += new RoutedEventHandler(btnShare_Click);
            Button btnRetry = new Button() { Content = "Retry" };
            btnRetry.Click += new RoutedEventHandler(btnRetry_Click);
            gameFinished.ActionPopUpButtons.Add(btnShare);
            gameFinished.ActionPopUpButtons.Add(btnRetry);
            gameFinished.Show();
        }

        private void restart()
        {
            lastTrailers = "";
            lastSongs = "";

            isImagesFinished = false;
            isTrailersFinished = false;
            isSongsFinished = false;

            hasStarted_Images = false;
            hasStarted_Trailers = false;
            hasStarted_Songs = false;

            corrected_Images = 0;
            corrected_Trailers = 0;
            corrected_Songs = 0;

            totalTimeElapsed_Images = 0;
            totalTimeElapsed_Trailers = 0;
            totalTimeElapsed_Songs = 0;

            seconds_Images = 0;
            seconds_Trailers = 0;
            seconds_Songs = 0;

            totalQuestions_Images = 0;
            totalQuestions_Trailers = 0;
            totalQuestions_Songs = 0;

            txtCorrectImages.Text = "";
            txtCorrectSongs.Text = "";
            txtCorrectTrailers.Text = "";
            txtQuestionImages.Text = "";
            txtQuestionSongs.Text = "";
            txtQuestionTrailers.Text = "";
            txtTimerImages.Text = "";
            txtTimerSongs.Text = "";
            txtTimerTrailers.Text = "";

            btnAImages.IsEnabled = true;
            btnBImages.IsEnabled = true;
            btnCImages.IsEnabled = true;

            btnATrailers.IsEnabled = true;
            btnBTrailers.IsEnabled = true;
            btnCTrailers.IsEnabled = true;

            btnASongs.IsEnabled = true;
            btnBSongs.IsEnabled = true;
            btnCSongs.IsEnabled = true;

            media.Stop();

            timer_Images.Stop();
            timer_Trailers.Stop();
            timer_Songs.Stop();

            questions_Images = new List<Question>();
            currentQuestion_Images = new Question();

            questions_Trailers = new List<Question>();
            currentQuestion_Trailers = new Question();

            questions_Songs = new List<Question>();
            currentQuestion_Songs = new Question();

            this.initialize_Images();
            this.initialize_Trailers();
            this.initialize_Songs();

            pivot.SelectedIndex = 0;

            this.startImages();
        }

        private List<E> ShuffleList<E>(List<E> inputList)
        {
            List<E> randomList = new List<E>();

            Random r = new Random();
            int randomIndex = 0;
            while (inputList.Count > 0)
            {
                randomIndex = r.Next(0, inputList.Count); //Choose a random object in the list
                randomList.Add(inputList[randomIndex]); //add it to the new, random list
                inputList.RemoveAt(randomIndex); //remove to avoid duplicates
            }

            return randomList; //return the new random list
        }

        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            media.Position = TimeSpan.Zero;
            media.Play();
        }

        private void LoadSound(String SoundFilePath, out SoundEffect Sound)
        {
            Sound = null;
            try
            {
                StreamResourceInfo SoundFileInfo = App.GetResourceStream(new Uri(SoundFilePath, UriKind.Relative));
                Sound = SoundEffect.FromStream(SoundFileInfo.Stream);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Couldn't load sound " + SoundFilePath);
            }
        }

        private Boolean isEverythingFinished() 
        {
            if (isImagesFinished && isTrailersFinished && isSongsFinished)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (pivot.SelectedIndex == 0) // Images
            {
                if (!hasStarted_Images)
                {
                    this.startImages();
                }
                else if(!isImagesFinished)
                {
                    this.resumeImages();
                    this.pauseTrailers();
                    this.pauseSongs();
                }
            }
            else if (pivot.SelectedIndex == 1)// Trailers
            {
                if (!hasStarted_Trailers)
                {
                   this.startTrailers();
                }
                else if (!isTrailersFinished)
                {
                    this.pauseImages();
                    this.resumeTrailers();
                    this.pauseSongs();
                }
            }
            else if (pivot.SelectedIndex == 2)// Songs
            {
                if (!hasStarted_Songs)
                {
                    this.startSongs();
                }
                else if (!isSongsFinished)
                {
                    this.pauseImages();
                    this.pauseTrailers();
                    this.resumeSongs();
                }
            }
        }

        private void pauseImages()
        {
            timer_Images.Stop();
        }

        private void resumeImages()
        {
            timer_Images.Start();
        }

        private void btnAImages_Click(object sender, RoutedEventArgs e)
        {
            this.compareImages(btnAImages.Content.ToString());
        }

        private void btnBImages_Click(object sender, RoutedEventArgs e)
        {
            this.compareImages(btnBImages.Content.ToString());
        }

        private void btnCImages_Click(object sender, RoutedEventArgs e)
        {
            this.compareImages(btnCImages.Content.ToString());
        }

        private void nextQuestion_Images()
        {
            questions_Images = ShuffleList<Question>(questions_Images);
            currentQuestion_Images = questions_Images.ElementAt<Question>(0);
            txtQuestionImages.Text = currentQuestion_Images.text;
            txtTimerImages.Text = "Time Left: " + currentQuestion_Images.timer;
            Images.Source = currentQuestion_Images.image;
            btnAImages.Content = currentQuestion_Images.a;
            btnBImages.Content = currentQuestion_Images.b;
            btnCImages.Content = currentQuestion_Images.c;
        }

        private void compareImages(String answer)
        {
            timer_Images.Stop();

            if (answer.Equals(currentQuestion_Images.answer))
            {
                this.correctSound.Play();
                corrected_Images++;
                totalTimeElapsed_Images += (30 - Convert.ToInt16(txtTimerImages.Text.Substring(10)));
                txtCorrectImages.Text = "Correct: " + corrected_Images + "/" + totalQuestions_Images;

                var messagePrompt = new MessagePrompt();
                messagePrompt.Title = "Correct Answer";
                messagePrompt.OnCompleted(new PopUpEventArgs<string, PopUpResult>
                {
                    Result = messagePrompt.Value,
                    PopUpResult = PopUpResult.Ok
                });

                messagePrompt.Completed += messagePromptImages_Completed;
                messagePrompt.Show();
            }
            else
            {
                this.wrongSound.Play();
                var messagePrompt = new MessagePrompt();
                messagePrompt.Title = "Wrong Answer";
                messagePrompt.Message = "The correct answer is: " + currentQuestion_Images.answer;
                messagePrompt.OnCompleted(new PopUpEventArgs<string, PopUpResult>
                {
                    Result = messagePrompt.Value,
                    PopUpResult = PopUpResult.Ok
                });

                messagePrompt.Completed += messagePromptImages_Completed;
                messagePrompt.Show();
            }
        }

        void timer_Tick_Images(object sender, EventArgs e)
        {
            seconds_Images++;
            int timeLeft = currentQuestion_Images.timer - seconds_Images;
            if (timeLeft > 0)
            {
                txtTimerImages.Text = "Time Left: " + timeLeft;
            }
            else
            {
                seconds_Images = 0;
                if(questions_Images.Count > 0)
                {
                    questions_Images.RemoveAt(0);
                    if (questions_Images.Count > 0)
                    {
                        this.nextQuestion_Images();
                    }
                    else
                    {
                        this.stopImages();
                    }
                }
                else{
                    this.stopImages();
                }
            }
        }

        private void stopImages()
        {
            seconds_Images = 0;
            timer_Images.Stop();
            isImagesFinished = true;
            btnAImages.IsEnabled = false;
            btnBImages.IsEnabled = false;
            btnCImages.IsEnabled = false;
            pivot.SelectedIndex = 1;

            if (this.isEverythingFinished())
            {
                this.showGameFinished();
            }
        }

        private void startImages()
        {
            txtTimerImages.Text = "Time Left: " + currentQuestion_Images.timer;
            timer_Images.Start();
            this.nextQuestion_Images();
            hasStarted_Images = true;
        }

        private void initialize_Images()
        {
            Question question = null;
            question = null;
            question = new Question();
            question.image = new BitmapImage(new Uri("Images/adobe.png", UriKind.Relative));
            question.text = "Is an American multinational computer software company founded in 1982 and headquartered in San Jose, California, United States.";
            question.a = "Gimp";
            question.b = "Adobe Systems";
            question.c = "Microsoft";
            question.answer = "Adobe Systems";
            question.timer = 30;
            questions_Images.Add(question);

            // you can add more - template below
            //question = null;
            //question = new Question();
            //question.image = new BitmapImage(new Uri("Images/xxx.png", UriKind.Relative));
            //question.text = "xxx";
            //question.a = "xxx";
            //question.b = "xxx";
            //question.c = "xxx";
            //question.answer = "xxx";
            //question.timer = 30;
            //questions_Images.Add(question);

            totalQuestions_Images = questions_Images.Count;
            txtCorrectImages.Text = "Correct: " + corrected_Images + "/" + totalQuestions_Images;

            timer_Images = new System.Windows.Threading.DispatcherTimer();
            timer_Images.Interval = new TimeSpan(0, 0, 0, 0, 1000); // 500 Milliseconds
            timer_Images.Tick += new EventHandler(timer_Tick_Images);
        }



        ///////////////////////////////////////////////////////////////////////////////



        private void pauseTrailers()
        {
            timer_Trailers.Stop();
            media.Pause();
            lastTrailers = currentQuestion_Trailers.trailer;
        }

        private void resumeTrailers()
        {
            timer_Trailers.Start();
            media.Source = new Uri("Trailers/" + lastTrailers, UriKind.Relative);
            media.Play();
        }

        private void btnATrailers_Click(object sender, RoutedEventArgs e)
        {
            this.compareTrailers(btnATrailers.Content.ToString());
        }

        private void btnBTrailers_Click(object sender, RoutedEventArgs e)
        {
            this.compareTrailers(btnBTrailers.Content.ToString());
        }

        private void btnCTrailers_Click(object sender, RoutedEventArgs e)
        {
            this.compareTrailers(btnCTrailers.Content.ToString());
        }

        private void nextQuestion_Trailers()
        {
            questions_Trailers = ShuffleList<Question>(questions_Trailers);
            currentQuestion_Trailers = questions_Trailers.ElementAt<Question>(0);
            txtQuestionTrailers.Text = currentQuestion_Trailers.text;
            txtTimerTrailers.Text = "Time Left: " + currentQuestion_Trailers.timer;
            media.Source = new Uri("Trailers/" + currentQuestion_Trailers.trailer, UriKind.Relative);
            media.Play();
            btnATrailers.Content = currentQuestion_Trailers.a;
            btnBTrailers.Content = currentQuestion_Trailers.b;
            btnCTrailers.Content = currentQuestion_Trailers.c;
        }

        private void compareTrailers(String answer)
        {
            timer_Trailers.Stop();
            if (answer.Equals(currentQuestion_Trailers.answer))
            {
                this.correctSound.Play();
                corrected_Trailers++;
                totalTimeElapsed_Trailers += (60 - Convert.ToInt16(txtTimerTrailers.Text.Substring(10)));
                txtCorrectTrailers.Text = "Correct: " + corrected_Trailers + "/" + totalQuestions_Trailers;

                var messagePrompt = new MessagePrompt();
                messagePrompt.Title = "Correct Answer";
                messagePrompt.OnCompleted(new PopUpEventArgs<string, PopUpResult>
                {
                    Result = messagePrompt.Value,
                    PopUpResult = PopUpResult.Ok
                });

                messagePrompt.Completed += messagePromptTrailers_Completed;
                messagePrompt.Show();
            }
            else
            {
                this.wrongSound.Play();
                var messagePrompt = new MessagePrompt();
                messagePrompt.Title = "Wrong Answer";
                messagePrompt.Message = "The correct answer is: " + currentQuestion_Trailers.answer;
                messagePrompt.OnCompleted(new PopUpEventArgs<string, PopUpResult>
                {
                    Result = messagePrompt.Value,
                    PopUpResult = PopUpResult.Ok
                });

                messagePrompt.Completed += messagePromptTrailers_Completed;
                messagePrompt.Show();
            }
        }

        void timer_Tick_Trailers(object sender, EventArgs e)
        {
            seconds_Trailers++;
            int timeLeft = currentQuestion_Trailers.timer - seconds_Trailers;
            if (timeLeft > 0)
            {
                txtTimerTrailers.Text = "Time Left: " + timeLeft;
            }
            else
            {
                seconds_Trailers = 0;
                if(questions_Trailers.Count > 0)
                {
                    questions_Trailers.RemoveAt(0);
                    if(questions_Trailers.Count > 0)
                    {
                        this.nextQuestion_Trailers();
                    }
                    else
                    {
                        this.stopTrailers();
                    }
                }
                else
                {
                    this.stopTrailers();
                }
            }
        }

        private void stopTrailers()
        {
            seconds_Trailers = 0;
            timer_Trailers.Stop();
            media.Stop();
            isTrailersFinished = true;
            pivot.SelectedIndex = 2;
            btnATrailers.IsEnabled = false;
            btnBTrailers.IsEnabled = false;
            btnCTrailers.IsEnabled = false;

            if (this.isEverythingFinished())
            {
                this.showGameFinished();
            }
        }

        private void startTrailers()
        {
            txtTimerTrailers.Text = "Time Left: " + currentQuestion_Trailers.timer;
            timer_Trailers.Start();
            this.nextQuestion_Trailers();
            hasStarted_Trailers = true;
        }

        private void initialize_Trailers()
        {
            Question question = null;
            question = new Question();
            question.trailer = "3idiots.wmv";
            question.text = "Is a 2009 Indian comedy film directed by Rajkumar Hirani";
            question.a = "4 Idiots";
            question.b = "3 Idiots";
            question.c = "2 Idiots";
            question.answer = "3 Idiots";
            question.timer = 60;
            questions_Trailers.Add(question);

            // you can add more - template below
            //question = null;
            //question = new Question();
            //question.trailer = "xxx";
            //question.text = "xxx";
            //question.a = "xxx";
            //question.b = "xxx";
            //question.c = "xxx";
            //question.answer = "xxx";
            //question.timer = 30;
            //questions_Trailers.Add(question);

            totalQuestions_Trailers = questions_Trailers.Count;
            txtCorrectTrailers.Text = "Correct: " + corrected_Trailers + "/" + totalQuestions_Trailers;

            timer_Trailers = new System.Windows.Threading.DispatcherTimer();
            timer_Trailers.Interval = new TimeSpan(0, 0, 0, 0, 1000); // 500 Milliseconds
            timer_Trailers.Tick += new EventHandler(timer_Tick_Trailers);
        }



        ///////////////////////////////////////////////////////////////////////////////


        private void pauseSongs()
        {
            lastSongs = currentQuestion_Songs.song;
            timer_Songs.Stop();
            media.Pause();
        }

        private void resumeSongs()
        {
            timer_Songs.Start();
            media.Source = new Uri("Songs3/" + lastSongs, UriKind.Relative);
            media.Play();
        }

        private void btnASongs_Click(object sender, RoutedEventArgs e)
        {
            this.compareSongs(btnASongs.Content.ToString());
        }

        private void btnBSongs_Click(object sender, RoutedEventArgs e)
        {
            this.compareSongs(btnBSongs.Content.ToString());
        }

        private void btnCSongs_Click(object sender, RoutedEventArgs e)
        {
            this.compareSongs(btnCSongs.Content.ToString());
        }

        private void nextQuestion_Songs()
        {
            questions_Songs = ShuffleList<Question>(questions_Songs);
            currentQuestion_Songs = questions_Songs.ElementAt<Question>(0);
            txtQuestionSongs.Text = currentQuestion_Songs.text;
            txtTimerSongs.Text = "Time Left: " + currentQuestion_Songs.timer;
            media.Source = new Uri("Songs3/" + currentQuestion_Songs.song, UriKind.Relative);
            media.Play();
            btnASongs.Content = currentQuestion_Songs.a;
            btnBSongs.Content = currentQuestion_Songs.b;
            btnCSongs.Content = currentQuestion_Songs.c;
        }

        private void compareSongs(String answer)
        {
            timer_Songs.Stop();

            if (answer.Equals(currentQuestion_Songs.answer))
            {
                this.correctSound.Play();
                corrected_Songs++;
                totalTimeElapsed_Songs += (60 - Convert.ToInt16(txtTimerTrailers.Text.Substring(10)));
                txtCorrectSongs.Text = "Correct: " + corrected_Songs + "/" + totalQuestions_Songs;

                var messagePrompt = new MessagePrompt();
                messagePrompt.Title = "Correct Answer";
                messagePrompt.OnCompleted(new PopUpEventArgs<string, PopUpResult>
                {
                    Result = messagePrompt.Value,
                    PopUpResult = PopUpResult.Ok
                });

                messagePrompt.Completed += messagePromptSongs_Completed;
                messagePrompt.Show();
            }
            else
            {
                this.wrongSound.Play();
                var messagePrompt = new MessagePrompt();
                messagePrompt.Title = "Wrong Answer";
                messagePrompt.Message = "The correct answer is: " + currentQuestion_Songs.answer;
                messagePrompt.OnCompleted(new PopUpEventArgs<string, PopUpResult>
                {
                    Result = messagePrompt.Value,
                    PopUpResult = PopUpResult.Ok
                });

                messagePrompt.Completed += messagePromptSongs_Completed;
                messagePrompt.Show();
            }
        }

        void timer_Tick_Songs(object sender, EventArgs e)
        {
            seconds_Songs++;
            int timeLeft = currentQuestion_Songs.timer - seconds_Songs;
            if (timeLeft > 0)
            {
                txtTimerSongs.Text = "Time Left: " + timeLeft;
            }
            else
            {
                if (questions_Songs.Count > 0)
                {
                    seconds_Songs = 0;
                    questions_Songs.RemoveAt(0);
                    if (questions_Songs.Count > 0)
                    {
                        this.nextQuestion_Songs();
                    }
                    else
                    {
                        this.stopSongs();
                    }
                }
                else
                {
                    this.stopSongs();
                }
            }
        }

        private void stopSongs()
        {
            seconds_Songs = 0;
            timer_Songs.Stop();
            media.Stop();
            isSongsFinished = true;
            pivot.SelectedIndex = 0;
            btnASongs.IsEnabled = false;
            btnBSongs.IsEnabled = false;
            btnCSongs.IsEnabled = false;

            if (this.isEverythingFinished())
            {
                this.showGameFinished();
            }
        }


        private void startSongs()
        {
            txtTimerSongs.Text = "Time Left: " + currentQuestion_Songs.timer;
            timer_Songs.Start();
            this.nextQuestion_Songs();
            hasStarted_Songs = true;
        }

        private void initialize_Songs()
        {
            Question question = null;
            question = new Question();
            question.song = "always.mp3";
            question.text = "Is a power ballad by Bon Jovi. It was released as a single from their 1994 album, Cross Road and went on to become their best selling single, with 3 million copies sold in the U.S. and more than 10 million worldwide.";
            question.a = "Always";
            question.b = "Never Say Goodbye";
            question.c = "Edge of a Broken Heart";
            question.answer = "Always";
            question.timer = 60;
            questions_Songs.Add(question);

            // you can add more - template below
            //question = null;
            //question = new Question();
            //question.song = "xxx";
            //question.text = "xxx";
            //question.a = "xxx";
            //question.b = "xxx";
            //question.c = "xxx";
            //question.answer = "xxx";
            //question.timer = 60;
            //questions_Songs.Add(question);

            totalQuestions_Songs = questions_Songs.Count;
            txtCorrectSongs.Text = "Correct: " + corrected_Songs + "/" + totalQuestions_Songs;

            timer_Songs = new System.Windows.Threading.DispatcherTimer();
            timer_Songs.Interval = new TimeSpan(0, 0, 0, 0, 1000); // 500 Milliseconds
            timer_Songs.Tick += new EventHandler(timer_Tick_Songs);
        }

        private void btnRestart_Click(object sender, RoutedEventArgs e)
        {
            this.restart();
        }
    }
}