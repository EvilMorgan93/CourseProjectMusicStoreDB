using MusicStoreDB_App.Data;
using MusicStoreDB_App.Models;
using MusicStoreDB_App.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MusicStoreDB_App {
    public partial class DataTableView : Window {
        public DataTableView() {            
            InitializeComponent();
            comboBoxTableChoose.SelectedIndex = 0;
            CreateSongGridView();
        }
        public async Task Refresh_Songs_List_View() {
            using (var db = new MusicStoreDBEntities()) {
                var songs = await db.Songs.ToListAsync();
                var albums = await db.Albums.ToListAsync();
                listView.ItemsSource = songs;
            }
        }
        //Нажатие кнопки "Обновить"
        private async void Refresh_ClickAsync(object sender, RoutedEventArgs e) {
            await Refresh_Songs_List_View();
        }
        private void Add_Click(object sender, RoutedEventArgs e) {
            var modalAddSong = new AddDataView();
            modalAddSong.ShowDialog();
        }
        private async void Delete_ClickAsync(object sender, RoutedEventArgs e) {
            using (var db = new MusicStoreDBEntities()) {
                if (listView.SelectedIndex == -1) { return; }
                else {
                    Song song = new Song();
                    song = listView.SelectedItem as Song;
                    db.Entry(song).State = EntityState.Deleted;
                    db.SaveChanges();
                    await Refresh_Songs_List_View();
                }
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
        private void ComboBoxTableChoose_DropDownClosed(object sender, EventArgs e) {
            switch (comboBoxTableChoose.Text) {
                case "Песни":
                    CreateSongGridView();
                    break;
                case "Альбомы":
                    CreateAlbumGridView();
                    break;
            }
        }
        public void CreateAlbumGridView() {
            listView.ItemsSource = null;
            listView.Items.Clear();
            GridView gridView = new GridView();
            GridViewColumn albumName = new GridViewColumn {
                DisplayMemberBinding = new Binding("album_name"),
                Header = "Название альбома",
                Width = 120
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
                Header = "ID Album",
                Width = 80
            };
            gridView.Columns.Add(albumId);
            GridViewColumn artistId = new GridViewColumn {
                DisplayMemberBinding = new Binding("id_artist"),
                Header = "ID Artist",
                Width = 80
            };
            gridView.Columns.Add(artistId);
            GridViewColumn albumSongId = new GridViewColumn {
                DisplayMemberBinding = new Binding("id_album_songs"),
                Header = "ID Album Song",
                Width = 100
            };
            gridView.Columns.Add(albumSongId);
            listView.View = gridView;
        }
        public void CreateSongGridView() {
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
                Width = 80
            };
            gridView.Columns.Add(durationColumn);
            listView.View = gridView;
        }
    }
}