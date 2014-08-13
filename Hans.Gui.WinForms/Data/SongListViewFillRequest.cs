using System.Windows.Forms;
using Hans.Core.Database.Songs;

namespace Hans.Gui.WinForms.Data
{
    public struct SongListViewFillRequest
    {
        public ListView ListView { get; set; }
        public HansSong[] Songs { get; set; }
    }
}