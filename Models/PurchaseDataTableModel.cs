using MusicStoreDB_App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace MusicStoreDB_App.Models {
    class PurchaseDataTableModel {
        private void CreatePurchaseGridView(ListView listView) {
            GridView gridView = new GridView();
            GridViewColumn albumName = new GridViewColumn {
                DisplayMemberBinding = new Binding("album_name"),
                Header = "Название альбома",
                Width = 80
            };
            gridView.Columns.Add(albumName);
            GridViewColumn albumYear = new GridViewColumn {
                DisplayMemberBinding = new Binding("album_year"),
                Header = "Год выпуска",
                Width = 100
            };
            gridView.Columns.Add(albumYear);
            GridViewColumn durationColumn = new GridViewColumn {
                DisplayMemberBinding = new Binding("song_duration"),
                Header = "Длительность",
                Width = 80
            };
            gridView.Columns.Add(durationColumn);
            listView.View = gridView;
        }
    }
}
