using MusicStoreDB_App.Data;
using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MusicStoreDB_App.Models {
    class SongDataGridModel {
        public void CreateSongDataGrid(DataGrid dataGrid) {
            dataGrid.ItemsSource = null;
            dataGrid.Columns.Clear();
            dataGrid.Items.Clear();
            var idColumn = new DataGridTextColumn {
                Binding = new Binding("id_song"),
                Header = "ID",
                Width = 100,
                IsReadOnly = true
            };
            dataGrid.Columns.Add(idColumn);
            var songTitle = new DataGridTextColumn {
                Binding = new Binding("song_title"),
                Header = "Название песни",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };
            dataGrid.Columns.Add(songTitle);
            var durationColumn = new DataGridTextColumn {
                Binding = new Binding("song_duration"),
                Header = "Длительность",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };
            dataGrid.Columns.Add(durationColumn);
        }
        public void AddSongData(DataGridView window, TextBox[] textBoxNames) {
            using (var db = new MusicStoreDBEntities()) {
                var song = new Song() {
                    song_title = textBoxNames[0].Text,
                    song_duration = TimeSpan.Parse(textBoxNames[1].Text)
                };
                db.Songs.Add(song);
                db.SaveChanges();
            }
        }
        public void EditSongData() {
            using (var db = new MusicStoreDBEntities()) {
                var song = new Song() {

                };
                db.Entry(song).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
