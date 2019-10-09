using MusicStoreDB_App.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MusicStoreDB_App.Models {
    class AlbumDataTableModel {    
        public void CreateAlbumGridView(ListView listView) {
            listView.ItemsSource = null;
            listView.Items.Clear();
            GridView gridView = new GridView();
            GridViewColumn albumId = new GridViewColumn {
                DisplayMemberBinding = new Binding("id_album"),
                Header = "ID_Album",
                Width = 50
            };
            gridView.Columns.Add(albumId);
            GridViewColumn albumName = new GridViewColumn {
                DisplayMemberBinding = new Binding("album_name"),
                Header = "Название альбома",
                Width = 130
            };
            gridView.Columns.Add(albumName);
            GridViewColumn albumYear = new GridViewColumn {
                DisplayMemberBinding = new Binding("album_year"),
                Header = "Год выпуска",
                Width = 100
            };
            gridView.Columns.Add(albumYear);
            GridViewColumn artistId = new GridViewColumn {
                DisplayMemberBinding = new Binding("id_artist"),
                Header = "ID_Artist",
                Width = 60
            };
            gridView.Columns.Add(artistId);
            GridViewColumn albumSongId = new GridViewColumn {
                DisplayMemberBinding = new Binding("id_album_songs"),
                Header = "ID_Album_Song",
                Width = 110
            };
            gridView.Columns.Add(albumSongId);
            listView.View = gridView;
        }
        public void DeleteAlbumData(ListView listView) {
            using (var db = new MusicStoreDBEntities()) {
                if (listView.SelectedIndex == -1) { return; } else {
                    var album = new Album();
                    album = listView.SelectedItem as Album;
                    db.Entry(album).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
        }
    }
}
