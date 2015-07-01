using BrightIdeasSoftware;
using Hans.Components.General;
using Hans.Components.Services.SoundCloud;
using Hans.Core.General;
using Hans.Core.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Hans.Gui.WinForms
{
    public partial class FormMain : Form
    {
        private readonly HansAudioPlayer _hmp;
        private readonly ExitAppTrigger _exitAppTrigger;

        public FormMain(HansAudioPlayer hmp, ExitAppTrigger exitAppTrigger)
        {
            _hmp = hmp;
            _exitAppTrigger = exitAppTrigger;
            InitializeComponent();
            UpdateForm();
            _hmp.SearchFinished += _hmp_SearchFinished;
            _hmp.NewSong += _hmp_NewSong;
            BuildSearchListView();
        }

        private void _hmp_NewSong(object sender, EventArgs e)
        {
            olvPlaylist.SetObjects(_hmp.SongQueue);
        }

        private void BuildSearchListView()
        {
            olvSearch.Columns.Add(new OLVColumn("Title", "Title")
            {
                Groupable = false,
                Width = 250
            });
            olvSearch.Columns.Add(new OLVColumn("Artist", "Artist")
            {
                Groupable = false,
                Width = 150
            });
        }

        private void _hmp_SearchFinished(object sender, SearchFinishedEventArgs e)
        {
            HandleFormInvoke(() =>
            {
                olvSearch.SetObjects(e.Tracks);
            });
        }

        private void UpdateForm()
        {
            tsslSongsInLibrary.Text = String.Format("Songs In Library: {0}", _hmp.Library.Songs.Count());
            BindLibraryListView();
            olvLibrary.CustomSorter = (column, order) =>
            {
                //olvLibrary.ListViewItemSorter = new ColumnComparer();
            };
        }

        private void BindLibraryListView()
        {
            olvLibrary.SetObjects(_hmp.Library.Songs.Take(10));
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _exitAppTrigger.Trigger();
        }

        private void lvLibrary_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void ForEachSelectedItem(ListView listView, Action<object> action)
        {
            foreach (var item in listView.SelectedItems)
            {
                var copiedItem = item;
                HandleFormInvoke(() => action(copiedItem));
            }
        }

        private void HandleFormInvoke(Action action)
        {
            if (InvokeRequired)
                Invoke(new MethodInvoker(action));
            else
                action.Invoke();
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            _hmp.Search(new SearchRequest
            {
                OnlineService = new SoundCloud(),
                Query = tbSearch.Text
            });
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var track in olvSearch.SelectedObjects)
                _hmp.Download((IOnlineServiceTrack)track);
        }
    }
}