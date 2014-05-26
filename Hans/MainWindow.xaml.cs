using System;
using System.Linq;
using System.Net;
using System.Timers;
using System.Windows;
using System.Windows.Documents;
using Hans.SoundCloud;
using NAudio.Wave;

namespace Hans
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private HansAudioPlayer _hansAudioPlayer;
        private Timer formRefresher;
        private bool _ignore;

        public MainWindow()
        {
            InitializeComponent();
            formRefresher = new Timer();
            formRefresher.Interval = 10;
            _hansAudioPlayer = new HansAudioPlayer();
            _hansAudioPlayer.SearchFinished += _hansAudioPlayer_SearchFinished;
        }

        void _hansAudioPlayer_SearchFinished(System.Collections.Generic.IEnumerable<Tests.IOnlineServiceTrack> tracks)
        {
            if (InvokeRequired)
            {
                Invoke(() => _hansAudioPlayer_SearchFinished(tracks));
                return;
            }
            ListViewSearch.Items.Clear();
            ListViewSearch.Items.Add(tracks);
            
        }

        void formRefresher_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => formRefresher_Elapsed(sender, e));
                return;
            }
            //_ignore = true;
            //SongProgress.Value = _audioFileReader.Position;
            //SongProgress.Maximum = _audioFileReader.Length;
            //_ignore = false;
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }

        private void Invoke(Action act)
        {
            Dispatcher.Invoke(act);
        }

        private bool InvokeRequired
        {
            get { return !Dispatcher.CheckAccess(); }
        }

        private void SongProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_ignore)
                return;
            //_audioFileReader.Position = (long) SongProgress.Value;
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Search(new SearchRequest
            {
                OnlineService = new Services.SoundCloud.SoundCloud(),
                Query = TextBoxQuery.Text
            });
        }

        private void ButtonPlayPause_Click(object sender, RoutedEventArgs e)
        {
            if (_hansAudioPlayer.IsPlaying)
            {
                _hansAudioPlayer.Pause();
                ButtonPlayPause.Content = "Play";
            }
            else
            {
                _hansAudioPlayer.Play();
                ButtonPlayPause.Content = "Pause";
            }
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Stop();
        }

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Previous();
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Next();
        }

        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _hansAudioPlayer.Volume = (float) e.NewValue;
        }
    }
}
