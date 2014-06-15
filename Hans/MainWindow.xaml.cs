using Hans.Database.Songs;
using Hans.General;
using Hans.Library;
using Hans.Properties;
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
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using Timer = System.Timers.Timer;

namespace Hans
{
    /// <summary>
    /// Interactionlogic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IDisposable
    {
        private readonly DownloaderWindow _downloaderWindow;
        private readonly ExitAppTrigger _exitAppTrigger;
        private readonly HansAudioPlayer _hansAudioPlayer;
        private Timer _formRefresher;
        private bool _progressChangeIgnoreIndicator;
        private bool _volumeChangeIgnoreIndicator;

        /// <summary>
        /// Initializes the mainform
        /// </summary>
        /// <param name="hansAudioPlayer"></param>
        /// <param name="exitAppTrigger"></param>
        public MainWindow(HansAudioPlayer hansAudioPlayer, ExitAppTrigger exitAppTrigger)
        {
            _hansAudioPlayer = hansAudioPlayer;
            _exitAppTrigger = exitAppTrigger;
            InitializeComponent();
            InitFormRefresher();
            HookHansAudioPlayerEvents();
            InitServiceComboBox();
            ListViewSongQueue.ItemsSource = SongQueueListViewItems;
            _downloaderWindow = new DownloaderWindow(_hansAudioPlayer.SongDownloads);
        }

        /// <summary>
        /// The items from the songQueue for the form
        /// </summary>
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

        /// <summary>
        /// Indicates whether the current scope needs invocation (actually it is a System.Windows.Forms wrapp for me :D)
        /// </summary>
        private bool InvokeRequired
        {
            get { return !Dispatcher.CheckAccess(); }
        }

        /// <summary>
        /// Disposes the form object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes all native objects and managed objects
        /// </summary>
        /// <param name="cleanAll"></param>
        protected virtual void Dispose(bool cleanAll)
        {
            if (cleanAll)
            {
                _formRefresher.Dispose();
            }
            _downloaderWindow.Dispose();
        }

        /// <summary>
        /// gets called when the form refresher is elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Gets called when the hans audio player plays a new song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void _hansAudioPlayer_NewSong(object sender, EventArgs eventArgs)
        {
            HandleInvoke(() => SliderSongProgress.Maximum = _hansAudioPlayer.CurrentSongLength);
        }

        /// <summary>
        /// Gets called when the hans audio player finishes a search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _hansAudioPlayer_SearchFinished(object sender, SearchFinishedEventArgs e)
        {
            HandleInvoke(() => FillListViewSearch(e.Tracks));
        }

        /// <summary>
        /// Gets called when the song queue changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void _hansAudioPlayer_SongQueueChanged(object sender, EventArgs args)
        {
            HandleInvoke(RefreshSongQueueListView);
        }

        /// <summary>
        /// Adds items to the search ListView
        /// </summary>
        /// <param name="tracks"></param>
        private void AddItemsToSearchListView(IEnumerable<IOnlineServiceTrack> tracks)
        {
            foreach (var track in tracks)
            {
                ListViewSearch.Items.Add(track);
            }
        }

        /// <summary>
        /// Gets called when the library search button got clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLibrarySearch_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Library.Search(TextBoxLibraryQuery.Text);
        }

        /// <summary>
        /// Gets called when the next button got clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonNext_OnClick(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Next();
        }

        /// <summary>
        /// Gets called when the pause button got clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPause_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Pause();
        }

        /// <summary>
        /// Gets called when the play button got clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Play();
        }

        /// <summary>
        /// Gets called when the previous button got clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonPrevious_OnClick(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Previous();
        }

        /// <summary>
        /// Gets called when the search button got clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Search(new SearchRequest
            {
                OnlineService = Activator.CreateInstance(ComboBoxService.SelectedValue as Type) as IOnlineService,
                Query = TextBoxQuery.Text
            });
        }

        /// <summary>
        /// Gets called when the stop button got clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            _hansAudioPlayer.Stop();
        }

        /// <summary>
        /// Filles the search listview
        /// </summary>
        /// <param name="tracks"></param>
        private void FillListViewSearch(IEnumerable<IOnlineServiceTrack> tracks)
        {
            ListViewSearch.Items.Clear();
            AddItemsToSearchListView(tracks);
        }

        /// <summary>
        /// Handles the dialog result from the folder browser dialog
        /// </summary>
        /// <param name="result"></param>
        /// <param name="selectedPath"></param>
        private void HandleDialogResultToLoadFolder(DialogResult result, string selectedPath)
        {
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                _hansAudioPlayer.LoadFolder(selectedPath);
            }
        }

        /// <summary>
        /// Handles form invokes
        /// </summary>
        /// <param name="action"></param>
        private void HandleInvoke(Action action)
        {
            if (InvokeRequired)
            {
                Invoke(action);
                return;
            }
            action.Invoke();
        }

        /// <summary>
        /// Hooks the hans audio player events
        /// </summary>
        private void HookHansAudioPlayerEvents()
        {
            _hansAudioPlayer.SearchFinished += _hansAudioPlayer_SearchFinished;
            _hansAudioPlayer.SongQueueChanged += _hansAudioPlayer_SongQueueChanged;
            _hansAudioPlayer.NewSong += _hansAudioPlayer_NewSong;
            _hansAudioPlayer.Library.NewSong += Library_NewSong;
            _hansAudioPlayer.Library.SearchFinished += Library_SearchFinished;
        }

        /// <summary>
        /// Initializes the form refresher timer
        /// </summary>
        private void InitFormRefresher()
        {
            _formRefresher = new Timer { Interval = 100 };
            _formRefresher.Elapsed += _formRefresher_Elapsed;
            _formRefresher.Start();
        }

        /// <summary>
        /// Initializes the service combo box and adds all services
        /// </summary>
        private void InitServiceComboBox()
        {
            ComboBoxService.DisplayMemberPath = "Name";
            ComboBoxService.Items.Add(typeof(Services.SoundCloud.SoundCloud));
            ComboBoxService.Items.Add(typeof(YouTube));
            ComboBoxService.Items.Add(typeof(LinkCrawl));
            ComboBoxService.SelectedIndex = 0;
        }

        /// <summary>
        /// Invokes an action with the form thread
        /// </summary>
        /// <param name="act"></param>
        private void Invoke(Action act)
        {
            Dispatcher.Invoke(act);
        }

        /// <summary>
        /// Determines whether the library listview has a selected item
        /// </summary>
        /// <returns></returns>
        private bool IsLibrarySelectionEmpty()
        {
            return ListViewLibrarySearch.SelectedIndex == -1;
        }

        /// <summary>
        /// Determines whether the selected item from the search listview is an online service track
        /// </summary>
        /// <returns></returns>
        private bool IsNoServiceTrack()
        {
            return !(ListViewSearch.SelectedValue is IOnlineServiceTrack);
        }

        /// <summary>
        /// Determines whether the online search listview ha a selection
        /// </summary>
        /// <returns></returns>
        private bool IsOnlineSearchListViewSelectionEmpty()
        {
            return ListViewSearch.SelectedIndex == -1;
        }

        /// <summary>
        /// Determines whether the songQueue listview is empty
        /// </summary>
        /// <returns></returns>
        private bool IsSongQueueSelectionEmpty()
        {
            return ListViewSongQueue.SelectedIndex != -1;
        }

        /// <summary>
        /// Gets calle dhwen the library has found a new song
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Library_NewSong(object sender, NewLibrarySongEventArgs e)
        {
            HandleInvoke(() => ListViewLibrarySearch.Items.Add(e.Song));
        }

        /// <summary>
        /// Gets called when the library search finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Library_SearchFinished(object sender, LibrarySearchFinishedEventArgs e)
        {
            HandleInvoke(() =>
            {
                ListViewLibrarySearch.Items.Clear();
                foreach (var song in e.Tracks)
                {
                    ListViewLibrarySearch.Items.Add(song);
                }
            });
        }

        /// <summary>
        /// Gets called when the sonqueue listview got double clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewSongQueue_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!IsSongQueueSelectionEmpty())
            {
                return;
            }
            _hansAudioPlayer.SetPlayingIndex(ListViewSongQueue.SelectedIndex);
        }

        /// <summary>
        /// Gets called when the mainwindow closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _formRefresher.Stop();
            _exitAppTrigger.Trigger();
            Environment.Exit(0);
        }

        /// <summary>
        /// Gets called when the menu item add from directory got clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemAddFromDirectory_OnClick(object sender, RoutedEventArgs e)
        {
            var fbd = new FolderBrowserDialog
            {
                Description = Settings.Default.Form_Text_AddFromLibrary,
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic),
                ShowNewFolderButton = true
            };
            HandleDialogResultToLoadFolder(fbd.ShowDialog(), fbd.SelectedPath);
        }

        /// <summary>
        /// gets called when the menu item add to playlist in the online search listview got clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// gets called when the menu item add to playlist in the library listview got clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Gets called when the menu item downloads got clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItemDownloads_OnClick(object sender, RoutedEventArgs e)
        {
            _downloaderWindow.Show();
        }

        /// <summary>
        /// Refreshes the the songqueue listview
        /// </summary>
        private void RefreshSongQueueListView()
        {
            ListViewSongQueue.ItemsSource = SongQueueListViewItems;
        }

        /// <summary>
        /// Gets called when the song progress slider get changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliderSongProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_hansAudioPlayer == null || _progressChangeIgnoreIndicator)
            {
                return;
            }
            _hansAudioPlayer.CurrentSongPosition = (long)e.NewValue;
        }

        /// <summary>
        /// gets called when the volume slider value changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_hansAudioPlayer == null || _volumeChangeIgnoreIndicator)
            {
                return;
            }
            _hansAudioPlayer.Volume = (float)e.NewValue;
        }

        /// <summary>
        /// Gets called when the library query textbox recognizes a key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxLibraryQuery_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ButtonLibrarySearch_Click(null, null);
            }
        }

        /// <summary>
        /// Gets called when library query textbox changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxLibraryQuery_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonLibrarySearch_Click(null, null);
        }

        /// <summary>
        /// Gets called when the online search query textbox recognizes a key down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBoxQueryOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ButtonSearch_Click(null, null);
            }
        }
    }
}