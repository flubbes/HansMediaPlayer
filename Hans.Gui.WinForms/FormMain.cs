using System.Linq;
using System.Windows.Forms;
using Hans.Components.General;
using Hans.Core.General;
using Hans.Gui.WinForms.Data;

namespace Hans.Gui.WinForms
{
    public partial class FormMain : Form
    {
        private readonly HansAudioPlayer _hansMediaPlayer;
        private readonly ExitAppTrigger _exitAppTrigger;
        private readonly ISongListViewFiller _songListViewFiller;

        public FormMain(HansAudioPlayer hansMediaPlayer, ExitAppTrigger exitAppTrigger, ISongListViewFiller songListViewFiller)
        {
            _hansMediaPlayer = hansMediaPlayer;
            _exitAppTrigger = exitAppTrigger;
            _songListViewFiller = songListViewFiller;
            InitializeComponent();
            SetUpLibraryListView();
            FillLibraryListView();
        }

        private void SetUpLibraryListView()
        {
            lvLibrary.Columns.Add("Artist");
            lvLibrary.Columns.Add("Title");
        }

        private void FillLibraryListView()
        {
            _songListViewFiller.Fill(new SongListViewFillRequest
            {
                ListView = lvLibrary,
                Songs = _hansMediaPlayer.Library.Songs.ToArray()
            });
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _exitAppTrigger.Trigger();
        }
    }
}
