using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using Hans.Components.General;
using Hans.Core.General;
using BrightIdeasSoftware;

namespace Hans.Gui.WinForms
{
    public partial class FormMain : Form
    {
        private readonly HansAudioPlayer _hansMediaPlayer;
        private readonly ExitAppTrigger _exitAppTrigger;

        public FormMain(HansAudioPlayer hansMediaPlayer, ExitAppTrigger exitAppTrigger)
        {
            _hansMediaPlayer = hansMediaPlayer;
            _exitAppTrigger = exitAppTrigger;
            InitializeComponent();
            UpdateForm();
        }

        private void UpdateForm()
        {
            tsslSongsInLibrary.Text = String.Format("Songs In Library: {0}", _hansMediaPlayer.Library.Songs.Count()); 
            BindLibraryListView();
            olvLibrary.CustomSorter = (column, order) =>
            {
                //olvLibrary.ListViewItemSorter = new ColumnComparer();
            };
        }

        private void BindLibraryListView()
        {
            olvLibrary.SetObjects(_hansMediaPlayer.Library.Songs.Take(10));
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
            var counter = 0;
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
    }
}
