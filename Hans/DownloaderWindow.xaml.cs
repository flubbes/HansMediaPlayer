using Hans.General;
using Hans.Web;
using System;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;

namespace Hans
{
    public class DownloaderListViewitem
    {
        public string Artist { get; set; }

        public int Progress { get; set; }

        public string ServiceName { get; set; }

        public string Title { get; set; }
    }

    /// <summary>
    /// Interaction logic for DownloaderWindow.xaml
    /// </summary>
    public partial class DownloaderWindow : Window
    {
        private readonly SongDownloads _songDownloads;
        private Timer _formTimer;

        public DownloaderWindow(SongDownloads songDownloads)
        {
            _songDownloads = songDownloads;
            InitializeComponent();
            _formTimer = new Timer();
            _formTimer.Interval = 100;
            _formTimer.Elapsed += _formTimer_Elapsed;
            _formTimer.Start();
        }

        private bool InvokeRequired
        {
            get { return !Dispatcher.CheckAccess(); }
        }

        private void _formTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            HandleInvoke(() =>
            {
                ListViewDownloads.ItemsSource = _songDownloads.ActiveDownloads.Select(a => new DownloaderListViewitem
                {
                    Artist = a.OnlineServiceTrack.Artist,
                    Progress = a.Downloader.Progress,
                    ServiceName = a.ServiceName,
                    Title = a.OnlineServiceTrack.Title
                });
            });
        }

        private void DownloaderWindow_OnClosing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
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

        private void Invoke(Action act)
        {
            Dispatcher.Invoke(act);
        }
    }
}