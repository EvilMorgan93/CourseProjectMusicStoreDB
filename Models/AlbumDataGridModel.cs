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
    class AlbumDataGridModel {    
        public void CreateAlbumDataGrid(DataGrid dataGrid) {
            dataGrid.ItemsSource = null;
            dataGrid.Items.Clear();
            dataGrid.Columns.Clear();
            var albumId = new DataGridTextColumn {
                Binding = new Binding("id_album"),
                Header = "ID_Album",
                Width = 80,
                IsReadOnly = true
            };
            dataGrid.Columns.Add(albumId);
            var albumName = new DataGridTextColumn {
                Binding = new Binding("album_name"),
                Header = "Название альбома",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };
            dataGrid.Columns.Add(albumName);
            var albumYear = new DataGridTextColumn {
                Binding = new Binding("album_year"),
                Header = "Год выпуска",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };
            dataGrid.Columns.Add(albumYear);
            var artistId = new DataGridTextColumn {
                Binding = new Binding("id_artist"),
                Header = "ID_Artist",
                Width = 80,
                IsReadOnly = true
            };
            dataGrid.Columns.Add(artistId);
            var albumSongId = new DataGridTextColumn {
                Binding = new Binding("id_album_songs"),
                Header = "ID_Album_Song",
                Width = 110,
                IsReadOnly = true
            };
            dataGrid.Columns.Add(albumSongId);
        }
        public void AddAlbumData(DataGridView window,TextBox[] textBoxNames) {
            using (var db = new MusicStoreDBEntities()) {
                var album = new Album() {
                    album_name = textBoxNames[0].Text,
                    album_year = DateTime.Parse(textBoxNames[1].Text),
                    id_artist = int.Parse(textBoxNames[2].Text),
                    id_album_songs = int.Parse(textBoxNames[3].Text)
                };
                db.Albums.Add(album);
                db.SaveChanges();
            }            
        }
    }
}
