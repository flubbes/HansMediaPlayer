using System.Windows.Forms;
using Hans.Components.General;
using Hans.Core.General;

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
            lvLibrary.Columns.Add("Artist");
            lvLibrary.Columns.Add("Title");
            foreach (var song in _hansMediaPlayer.Library.Songs)
            {
                var item = new ListViewItem(song.Artist);
                item.SubItems.Add(song.Title);
                lvLibrary.Items.Add(item);
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _exitAppTrigger.Trigger();
        }
    }
}
