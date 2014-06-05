using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows;
using Hans.General;
using Hans.Services;
using Hans.Services.YouTube;

namespace Hans
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly HansAudioPlayer _hansAudioPlayer;

        private Timer _formRefresher;

        public MainWindow(HansAudioPlayer hansAudioPlayer)
        {
            _hansAudioPlayer = hansAudioPlayer;
            InitializeComponent();
            InitFormRefresher();
            InitHansAudioPlayer();
            InitServiceComboBox();
        }

        private void InitFormRefresher()
        {
            _formRefresher = new Timer {Interval = 10};
        }

        private void InitHansAudioPlayer()
        {
            _hansAudioPlayer.SearchFinished += _hansAudioPlayer_SearchFinished;
            _hansAudioPlayer.SongQueueChanged += _hansAudioPlayer_SongQueueChanged;
        }

        private void InitServiceComboBox()
        {
            ComboBoxService.DisplayMemberPath = "Name";
            ComboBoxService.Items.Add(typeof(Services.SoundCloud.SoundCloud));
            ComboBoxService.Items.Add(typeof(YouTube));
            ComboBoxService.SelectedIndex = 0;
        }

        private bool InvokeRequired
        {
            get { return !Dispatcher.CheckAccess(); }
        }

        void _hansAudioPlayer_SearchFinished(IEnumerable<IOnlineServiceTrack> tracks)
        {
            if (InvokeRequired)
            {
                Invoke(() => _hansAudioPlayer_SearchFinished(tracks));
                return;
            }
            FillListViewSearch(tracks);
        }

        void _hansAudioPlayer_SongQueueChanged(object sender, EventArgs args)
        {
            if (InvokeRequired)
            {
                Invoke(() => _hansAudioPlayer_SongQueueChanged(sender, args));
                return;
            }
            ListViewSongQueue.Items.Clear();
            AddItemsToSongQueueListView();
        }

        private void AddItemsToSongQueueListView()
        {
            foreach (var track in _hansAudioPlayer.SongQueue)
            {
                ListViewSongQueue.Items.Add(track);
            }
        }

        private void AddItemsToSearchListView(IEnumerable<IOnlineServiceTrack> tracks)
        {
            foreach (var track in tracks)
            {
                ListViewSearch.Items.Add(track);
            }
        }

        private void ButtonNext_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Next();
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

        private void ButtonPrevious_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Previous();
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Search(new SearchRequest
            {
                OnlineService = Activator.CreateInstance(ComboBoxService.SelectedValue as Type) as IOnlineService,
                Query = TextBoxQuery.Text
            });
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Stop();
        }

        private IOnlineServiceTrack ConvertSelectionToOnlineServiceTrack()
        {
            return ListViewSearch.SelectedValue as IOnlineServiceTrack;
        }

        private void FillListViewSearch(IEnumerable<IOnlineServiceTrack> tracks)
        {
            ListViewSearch.Items.Clear();
            AddItemsToSearchListView(tracks);
        }
        void formRefresher_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => formRefresher_Elapsed(sender, e));
            }
        }

        private void Invoke(Action act)
        {
            Dispatcher.Invoke(act);
        }

        private bool IsSelectionEmpty()
        {
            return ListViewSearch.SelectedIndex == -1;
        }

        private bool IsServiceTrack()
        {
            return ListViewSearch.SelectedValue is IOnlineServiceTrack;
        }

        private void MenuItemAddToPlaylist_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsSelectionEmpty() || !IsServiceTrack())
            {
                return;
            }

            var track = ConvertSelectionToOnlineServiceTrack();
            _hansAudioPlayer.Download(track);
        }

        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_hansAudioPlayer == null)
            {
                return;
            }
            _hansAudioPlayer.Volume = (float)e.NewValue;
        }

        private void SongProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }
    }
}
