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
        private void CreatePurchaseDataGrid(DataGrid dataGrid) {
            var albumName = new DataGridTextColumn {
                Binding = new Binding("album_name"),
                Header = "Название альбома",
                Width = 80
            };
            dataGrid.Columns.Add(albumName);
            var albumYear = new DataGridTextColumn {
                Binding = new Binding("album_year"),
                Header = "Год выпуска",
                Width = 100
            };
            dataGrid.Columns.Add(albumYear);
            var durationColumn = new DataGridTextColumn {
                Binding = new Binding("song_duration"),
                Header = "Длительность",
                Width = 80
            };
            dataGrid.Columns.Add(durationColumn);
        }
    }
}
