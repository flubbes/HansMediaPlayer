using Gat.Controls;
using Hans.Database.Songs;
using Hans.General;
using Hans.Library;
using Hans.Services;
using Hans.Services.LinkCrawl;
using Hans.Services.YouTube;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace Hans
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IDisposable
    {
        private readonly DownloaderWindow _downloaderWindow;
        private readonly ExitAppTrigger _exitAppTrigger;
        private readonly HansAudioPlayer _hansAudioPlayer;
        private Timer _formRefresher;
        private bool _progressChangeIgnoreIndicator;
        private bool _volumeChangeIgnoreIndicator;

        public MainWindow(HansAudioPlayer hansAudioPlayer, ExitAppTrigger exitAppTrigger)
        {
            _hansAudioPlayer = hansAudioPlayer;
            _exitAppTrigger = exitAppTrigger;
            InitializeComponent();
            InitFormRefresher();
            InitHansAudioPlayer();
            InitServiceComboBox();
            ListViewSongQueue.ItemsSource = SongQueueListViewItems;
            _downloaderWindow = new DownloaderWindow(_hansAudioPlayer.SongDownloads);
        }

        public IEnumerable<SongQueueListViewItem> SongQueueListViewItems
        {
            get
            {
                return new ObservableCollection<SongQueueListViewItem>(_hansAudioPlayer.SongQueue.Select(hs => new SongQueueListViewItem
                {
                    Artist = hs.Artist,
                    CurrentlyPlaying = _hansAudioPlayer.IsCurrentPlayingSong(hs),
                    Length = new TimeSpan(),
                    Title = hs.Title
                }));
            }
        }

        private bool InvokeRequired
        {
            get { return !Dispatcher.CheckAccess(); }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private static OpenDialogViewModel BuildOpenDialog(string caption)
        {
            var openDialogView = new OpenDialogView();
            var vm = (OpenDialogViewModel)openDialogView.DataContext;
            vm.Caption = caption;
            return vm;
        }

        private void _formRefresher_Elapsed(object sender, ElapsedEventArgs e)
        {
            HandleInvoke(() =>
            {
                _volumeChangeIgnoreIndicator = true;
                SliderVolume.Value = _hansAudioPlayer.Volume;
                _volumeChangeIgnoreIndicator = false;
                _progressChangeIgnoreIndicator = true;
                SliderSongProgress.Value = _hansAudioPlayer.CurrentSongPosition;
                _progressChangeIgnoreIndicator = false;
            });
        }

        private void _hansAudioPlayer_NewSong(object sender, EventArgs eventArgs)
        {
            HandleInvoke(() => SliderSongProgress.Maximum = _hansAudioPlayer.CurrentSongLength);
        }

        private void _hansAudioPlayer_SearchFinished(object sender, SearchFinishedEventArgs e)
        {
            HandleInvoke(() => FillListViewSearch(e.Tracks));
        }

        private void _hansAudioPlayer_SongQueueChanged(object sender, EventArgs args)
        {
            HandleInvoke(RefreshSongQueueListView);
        }

        private void AddItemsToSearchListView(IEnumerable<IOnlineServiceTrack> tracks)
        {
            foreach (var track in tracks)
            {
                ListViewSearch.Items.Add(track);
            }
        }

        private void ButtonLibrarySearch_Click(object sender, RoutedEventArgs e)
        {
            ListViewLibrarySearch.Items.Clear();
            foreach (var song in _hansAudioPlayer.Library.Search(TextBoxLibraryQuery.Text))
            {
                ListViewLibrarySearch.Items.Add(song);
            }
        }

        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Next();
        }

        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Pause();
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Play();
        }

        private void ButtonPrevious_OnClick(object sender, RoutedEventArgs e)
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

        private void Dispose(bool cleanAll)
        {
            if (cleanAll)
            {
                _formRefresher.Dispose();
            }
            _downloaderWindow.Dispose();
        }

        private void FillListViewSearch(IEnumerable<IOnlineServiceTrack> tracks)
        {
            ListViewSearch.Items.Clear();
            AddItemsToSearchListView(tracks);
        }

        private void formRefresher_Elapsed(object sender, ElapsedEventArgs e)
        {
            // HandleInvoke();
        }

        private void HandleDialogResultToLoadFolder(bool? result, OpenDialogViewModel vm)
        {
            if (result ?? false)
            {
                _hansAudioPlayer.LoadFolder(vm.SelectedFolder.Path);
            }
        }

        private void HandleInvoke(Action action)
        {
            if (InvokeRequired)
            {
                Invoke(action);
                return;
            }
            action.Invoke();
        }

        private void InitFormRefresher()
        {
            _formRefresher = new Timer { Interval = 100 };
            _formRefresher.Elapsed += _formRefresher_Elapsed;
            _formRefresher.Start();
        }

        private void InitHansAudioPlayer()
        {
            _hansAudioPlayer.SearchFinished += _hansAudioPlayer_SearchFinished;
            _hansAudioPlayer.SongQueueChanged += _hansAudioPlayer_SongQueueChanged;
            _hansAudioPlayer.NewSong += _hansAudioPlayer_NewSong;
            _hansAudioPlayer.Library.NewSong += Library_NewSong;
        }

        private void InitServiceComboBox()
        {
            ComboBoxService.DisplayMemberPath = "Name";
            ComboBoxService.Items.Add(typeof(Services.SoundCloud.SoundCloud));
            ComboBoxService.Items.Add(typeof(YouTube));
            ComboBoxService.Items.Add(typeof(LinkCrawl));
            ComboBoxService.SelectedIndex = 0;
        }

        private void Invoke(Action act)
        {
            Dispatcher.Invoke(act);
        }

        private bool IsLibrarySelectionEmpty()
        {
            return ListViewLibrarySearch.SelectedIndex == -1;
        }

        private bool IsNoServiceTrack()
        {
            return !(ListViewSearch.SelectedValue is IOnlineServiceTrack);
        }

        private bool IsOnlineSearchListViewSelectionEmpty()
        {
            return ListViewSearch.SelectedIndex == -1;
        }

        private bool IsSongQueueSelectionEmpty()
        {
            return ListViewSongQueue.SelectedIndex != -1;
        }

        private void Library_NewSong(object sender, NewLibrarySongEventArgs e)
        {
            HandleInvoke(() => ListViewLibrarySearch.Items.Add(e.Song));
        }

        private void ListViewSongQueue_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!IsSongQueueSelectionEmpty())
            {
                return;
            }
            _hansAudioPlayer.SetPlayingIndex(ListViewSongQueue.SelectedIndex);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _formRefresher.Stop();
            _exitAppTrigger.Trigger();
            Environment.Exit(0);
        }

        private void MenuItemAddFromDirectory_OnClick(object sender, RoutedEventArgs e)
        {
            var vm = BuildOpenDialog("Open a folder to add to your music library");
            vm.IsDirectoryChooser = true;
            var result = vm.Show();
            HandleDialogResultToLoadFolder(result, vm);
        }

        private void MenuItemAddToPlaylist_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsOnlineSearchListViewSelectionEmpty() || IsNoServiceTrack())
            {
                return;
            }
            foreach (var selectedItem in ListViewSearch.SelectedItems)
            {
                _hansAudioPlayer.Download(selectedItem as IOnlineServiceTrack);
            }
        }

        private void MenuItemAddToPlaylistFromLibrary_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsLibrarySelectionEmpty())
            {
                return;
            }
            foreach (var selectedItem in ListViewLibrarySearch.SelectedItems)
            {
                _hansAudioPlayer.AddToCurrentPlayList(selectedItem as HansSong);
            }
            RefreshSongQueueListView();
        }

        private void MenuItemDownloads_OnClick(object sender, RoutedEventArgs e)
        {
            _downloaderWindow.Show();
        }

        private void RefreshSongQueueListView()
        {
            ListViewSongQueue.ItemsSource = SongQueueListViewItems;
        }

        private void SliderSongProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_hansAudioPlayer == null || _progressChangeIgnoreIndicator)
            {
                return;
            }
            _hansAudioPlayer.CurrentSongPosition = (long)e.NewValue;
        }

        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_hansAudioPlayer == null || _volumeChangeIgnoreIndicator)
            {
                return;
            }
            _hansAudioPlayer.Volume = (float)e.NewValue;
        }

        private void TextBoxQueryOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ButtonSearch_Click(null, null);
            }
        }
    }

    public class SongQueueListViewItem
    {
        private string _artist;

        public string Artist
        {
            get { return CurrentlyPlaying ? "♥ " + _artist : _artist; }
            set { _artist = value; }
        }

        public bool CurrentlyPlaying { get; set; }

        public TimeSpan Length { get; set; }

        public string Title { get; set; }
    }
}