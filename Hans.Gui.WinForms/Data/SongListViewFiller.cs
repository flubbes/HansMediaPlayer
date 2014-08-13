using System.Windows.Forms;

namespace Hans.Gui.WinForms.Data
{
    public interface ISongListViewFiller
    {
        void Fill(SongListViewFillRequest songs);
    }

    public class SongListViewFiller : ISongListViewFiller
    {
        public void Fill(SongListViewFillRequest songs)
        {
            foreach (var song in songs.Songs)
            {
                var item = new ListViewItem(song.Artist);
                item.SubItems.Add(song.Title);
                songs.ListView.Items.Add(item);
            }
        }
    }
}