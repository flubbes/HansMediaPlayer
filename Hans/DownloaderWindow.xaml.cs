using Hans.Core.Web;
using Hans.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;

namespace Hans
{
    /// <summary>
    /// Interaction logic for DownloaderWindow.xaml
    /// </summary>
    public partial class DownloaderWindow : Window, IDisposable
    {
        private readonly SongDownloads _songDownloads;
        private Timer _formTimer;

        /// <summary>
        /// Initializes a new downloader window
        /// </summary>
        /// <param name="songDownloads"></param>
        public DownloaderWindow(SongDownloads songDownloads)
        {
            _songDownloads = songDownloads;
            InitializeComponent();
            _formTimer = new Timer();
            _formTimer.Interval = 100;
            _formTimer.Elapsed += _formTimer_Elapsed;
            _formTimer.Start();
        }

        /// <summary>
        /// If an invoke is required
        /// </summary>
        private bool InvokeRequired
        {
            get { return !Dispatcher.CheckAccess(); }
        }

        /// <summary>
        /// Disposes the form
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Disposes all managed and native objects
        /// </summary>
        /// <param name="cleanAll"></param>
        protected virtual void Dispose(bool cleanAll)
        {
            if (cleanAll)
            {
                _formTimer.Dispose();
            }
        }

        /// <summary>
        /// Gets called when the form timer is elapsed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _formTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            HandleInvoke(() =>
            {
                ListViewDownloads.ItemsSource = _songDownloads.ActiveDownloads.Select(a => new DownloaderListViewItem
                {
                    Artist = a.OnlineServiceTrack.Artist,
                    Progress = a.Downloader.Progress,
                    ServiceName = a.ServiceName,
                    Title = a.OnlineServiceTrack.Title
                });
            });
        }

        /// <summary>
        /// Gets calles when the downloader windows closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloaderWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        /// <summary>
        /// Handles all form invokes
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
        /// Invokes an action with the form thread
        /// </summary>
        /// <param name="act"></param>
        private void Invoke(Action act)
        {
            Dispatcher.Invoke(act);
        }
    }
}