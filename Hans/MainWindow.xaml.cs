using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using Gat.Controls;
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
        private bool _volumeChangeIgnoreIndicator;
        private bool _progressChangeIgnoreIndicator;

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
            _formRefresher = new Timer {Interval = 100};
            _formRefresher.Elapsed += _formRefresher_Elapsed;
            _formRefresher.Start();
        }

        void _formRefresher_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => _formRefresher_Elapsed(sender, e));
                return;
            }
            _volumeChangeIgnoreIndicator = true;
            SliderVolume.Value = _hansAudioPlayer.Volume;
            _volumeChangeIgnoreIndicator = false;
            _progressChangeIgnoreIndicator = true;
            SliderSongProgress.Value = _hansAudioPlayer.CurrentSongPosition;
            _progressChangeIgnoreIndicator = false;
        }

        private void InitHansAudioPlayer()
        {
            _hansAudioPlayer.SearchFinished += _hansAudioPlayer_SearchFinished;
            _hansAudioPlayer.SongQueueChanged += _hansAudioPlayer_SongQueueChanged;
            _hansAudioPlayer.NewSong += _hansAudioPlayer_NewSong;
            _hansAudioPlayer.Library.NewSong += Library_NewSong;
        }

        void Library_NewSong(Database.Songs.HansSong song)
        {
            ListViewLibrarySearch.Items.Add(song);
        }

        void _hansAudioPlayer_NewSong()
        {
            SliderSongProgress.Maximum = _hansAudioPlayer.CurrentSongLength;
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

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Search(new SearchRequest
            {
                OnlineService = Activator.CreateInstance(ComboBoxService.SelectedValue as Type) as IOnlineService,
                Query = TextBoxQuery.Text
            });
        }


        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Play();
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Stop();
        }

        private IOnlineServiceTrack GetSelectionAsOnlineServiceTrack()
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

        private bool IsNoServiceTrack()
        {
            return !(ListViewSearch.SelectedValue is IOnlineServiceTrack);
        }

        private void MenuItemAddToPlaylist_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsSelectionEmpty() || IsNoServiceTrack())
            {
                return;
            }
            _hansAudioPlayer.Download(GetSelectionAsOnlineServiceTrack());
        }

        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_hansAudioPlayer == null || _volumeChangeIgnoreIndicator)
            {
                return;
            }
            _hansAudioPlayer.Volume = (float)e.NewValue;
        }

        private void SliderSongProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_hansAudioPlayer == null || _progressChangeIgnoreIndicator)
            {
                return;
            }
            _hansAudioPlayer.CurrentSongPosition = (long) e.NewValue;
        }

        private void HandleDialogResultToLoadFolder(bool? result, OpenDialogViewModel vm)
        {
            if (result ?? false)
            {
                _hansAudioPlayer.LoadFolder(vm.SelectedFolder.Path);
            }
        }

        private static OpenDialogViewModel BuildOpenDialog(string caption)
        {
            var openDialogView = new OpenDialogView();
            var vm = (OpenDialogViewModel) openDialogView.DataContext;
            vm.Caption = caption;
            return vm;
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _formRefresher.Stop();
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Pause();
        }

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Next();
        }

        private void ButtonPrevious_OnClick(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Previous();
        }

        private void MenuItemAddFromDirectory_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = BuildOpenDialog("Open a folder to add to your music library");
            vm.IsDirectoryChooser = true;
            var result = vm.Show();
            HandleDialogResultToLoadFolder(result, vm);
        }

        private void ButtonLibrarySearch_Click(object sender, RoutedEventArgs e)
        {
            ListViewLibrarySearch.Items.Clear();
            foreach (var song in _hansAudioPlayer.Library.Search(TextBoxLibraryQuery.Text))
            {
                ListViewLibrarySearch.Items.Add(song);
            }
        }
    }
}
