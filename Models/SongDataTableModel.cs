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
        public void CreateSongGridView(ListView listView) {
            listView.ItemsSource = null;
            listView.Items.Clear();
            GridView gridView = new GridView();
            GridViewColumn idColumn = new GridViewColumn {
                DisplayMemberBinding = new Binding("id_song"),
                Header = "ID",
                Width = 50
            };
            gridView.Columns.Add(idColumn);
            GridViewColumn songTitle = new GridViewColumn {
                DisplayMemberBinding = new Binding("song_title"),
                Header = "Название песни",
                Width = 150
            };
            gridView.Columns.Add(songTitle);
            GridViewColumn durationColumn = new GridViewColumn {
                DisplayMemberBinding = new Binding("song_duration"),
                Header = "Длительность",
                Width = 90
            };
            gridView.Columns.Add(durationColumn);
            listView.View = gridView;
        }
        public void DeleteSongData(ListView listView) {
            using (var db = new MusicStoreDBEntities()) {
                if (listView.SelectedIndex == -1) { return; } else {
                    var song = new Song();
                    song = listView.SelectedItem as Song;
                    db.Entry(song).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
        }
    }
}
