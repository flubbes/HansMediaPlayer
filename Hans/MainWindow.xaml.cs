using System;
using System.Linq;
using System.Net;
using System.Timers;
using System.Windows;
using Hans.SoundCloud;
using NAudio.Wave;

namespace Hans
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private IWavePlayer _waveOut;
        private AudioFileReader _audioFileReader;
        private Timer formRefresher;
        private bool _ignore;

        public MainWindow()
        {
            InitializeComponent();
            formRefresher = new Timer();
            formRefresher.Interval = 10;
            
            formRefresher.Elapsed += formRefresher_Elapsed;
            _waveOut = new WaveOut();
            var tracks = SoundCloud.SoundCloudHelper.GetTracks(new WebClient().DownloadString("https://soundcloud.com/surendly-swaggaboy/capo-my-own-way-prod-by-taeedaproducer-x-josueonthebeat-x-intelonthebeat-glonl2"));
            new WebClient().DownloadFile(tracks.First().Mp3Url, "peter.mp3");
            _audioFileReader = new AudioFileReader("peter.mp3");
            _waveOut.Init(_audioFileReader);
            _waveOut.Play();
            formRefresher.Start();
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
            _audioFileReader.Volume = (float) e.NewValue;
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
    }
}
