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
        public void CreateAlbumGridView() {
            //Album album = new Album();
            DataTableView DataTableView = new DataTableView();
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
            GridViewColumn albumId = new GridViewColumn {
                DisplayMemberBinding = new Binding("id_album"),
                Header = "ID_Album",
                Width = 50
            };
            gridView.Columns.Add(albumId);
            GridViewColumn artistId = new GridViewColumn {
                DisplayMemberBinding = new Binding("id_artist"),
                Header = "ID_Artist",
                Width = 50
            };
            gridView.Columns.Add(artistId);
            GridViewColumn albumSongId = new GridViewColumn {
                DisplayMemberBinding = new Binding("id_album_songs"),
                Header = "ID_Album_Song",
                Width = 50
            };
            gridView.Columns.Add(albumSongId);
            DataTableView.listView.View = gridView;
        }
    }
}
