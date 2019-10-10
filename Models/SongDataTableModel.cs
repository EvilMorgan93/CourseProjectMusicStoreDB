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
    class SongDataTableModel {       
        public void CreateSongDataGrid(DataGrid dataGrid) {
            dataGrid.ItemsSource = null;
            dataGrid.Columns.Clear();
            dataGrid.Items.Clear();
            var idColumn = new DataGridTextColumn {
                Binding = new Binding("id_song"),
                Header = "ID",
                Width = 100
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
        public void DeleteSongData(DataGrid dataGrid) {
            using (var db = new MusicStoreDBEntities()) {
                if (dataGrid.SelectedIndex == -1) { return; } else {
                    var song = new Song();
                    song = dataGrid.SelectedItem as Song;
                    db.Entry(song).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
        }
    }
}
