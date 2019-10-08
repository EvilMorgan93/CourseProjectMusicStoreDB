using MusicStoreDB_App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MusicStoreDB_App.Models {
    class SongDataTableModel {       
        public void CreateSongGridView() {
            GridView gridView = new GridView();
            DataTableView dataTableView = new DataTableView();
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
                Width = 80
            };
            gridView.Columns.Add(durationColumn);
            dataTableView.listView.View = gridView;
            using (var db = new MusicStoreDBEntities()) {
                dataTableView.listView.ItemsSource = db.Songs.ToList();
            }
        }

    }
}
