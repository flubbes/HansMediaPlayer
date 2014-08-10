using System.Windows.Forms;
using FluentAssertions;
using Hans.Core.Database.Songs;
using NUnit.Framework;

namespace Hans.Gui.WinForms.Tests
{
    [TestFixture]
    public class SongListViewFillerTests
    {
        [Test]
        public void CanFillListView()
        {
            var filler = new SongListViewFiller();
            var listView = new ListView();
            var item = new HansSong("test");
            filler.Fill(new SongListViewFillRequest
            {
                ListView = listView,
                Songs = new[] { item}
            });
            listView.Items.Count.Should().Be(1);
            var firstItem = listView.Items[0];
            firstItem.SubItems[0].Text.Should().Be(item.Artist);
            firstItem.SubItems[1].Text.Should().Be(item.Title);
        }
    }

    public struct SongListViewFillRequest
    {
        public ListView ListView { get; set; }
        public HansSong[] Songs { get; set; }
    }

    public class SongListViewFiller
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
