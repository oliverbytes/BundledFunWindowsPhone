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
using Coding4Fun.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.ComponentModel;
using System.Windows.Controls.Primitives;
using System.Threading;

namespace BundledFun
{
    public partial class MainMenu : PhoneApplicationPage
    {

        private Popup popup;
        private BackgroundWorker backroungWorker;

        public MainMenu()
        {
            InitializeComponent();
            ShowSplash();
        }

        private void ShowSplash()
        {
            this.popup = new Popup();
            this.popup.Child = new SplashScreen();
            this.popup.IsOpen = true;
            StartLoadingData();
        }

        private void StartLoadingData()
        {
            backroungWorker = new BackgroundWorker();
            backroungWorker.DoWork += new DoWorkEventHandler(backroungWorker_DoWork);
            backroungWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backroungWorker_RunWorkerCompleted);
            backroungWorker.RunWorkerAsync();
        }

        void backroungWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(9000);
        }

        void backroungWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                this.popup.IsOpen = false;
            }
            );
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Start the Quiz now?", "BundledFun Quiz", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                NavigationService.Navigate(new Uri("/QuizPage.xaml", UriKind.Relative));
            }
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutPrompt about = new AboutPrompt();
            about.Title = "About BundledFun";
            about.Footer = "nemoryoliver@gmail.com\nhttp://facebook.com/nemoryoliver";
            about.VersionNumber = "Version 1.0";
            about.Body = new TextBlock 
            {
                Text = "BundledFun is a fun quiz game for Windows Phone and developed by a Filipino Indie Developer: Oliver Martinez (or NemOry). \n\nIt supports Image, Text, Audio and Video types of questions.\n\n It is targeted for happy users :)", 
                TextWrapping = TextWrapping.Wrap 
            };
            about.Show();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            AboutPrompt howToPlay = new AboutPrompt();
            howToPlay.Title = "How to Play";
            howToPlay.Footer = "nemoryoliver@gmail.com\nhttp://facebook.com/nemoryoliver";
            howToPlay.VersionNumber = "Version 1.0";
            howToPlay.Body = new TextBlock
            {
                Text = "To earn points: just click on the right answer. :)\n\nTo navigate to other types of questions: Just simply swipe from left to right or from right to left\n\nHow to finnish the game: Just finnish all the questions then you will see your score and time elapsed and be able to share your result. :)\n\nIf you want to quit: just click on the Windows Start Button",
                TextWrapping = TextWrapping.Wrap
            };
            howToPlay.Show();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            new Microsoft.Xna.Framework.Game().Exit();
        }
    }
}