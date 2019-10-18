using MusicStoreDB_App.Data;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MusicStoreDB_App.ViewModels {
    public class AlbumSongsViewModel : BaseViewModel, IPageViewModel {
        public CollectionViewSource AlbumSongs { get; private set; }
        public CollectionViewSource Album { get; private set; }
        public CollectionViewSource Genre { get; private set; }
        public CollectionViewSource Song { get; private set; }
        public string Name {
            get => "Альбомные композиции";
        }
        private Album_Songs selectedItem;
        public Album_Songs SelectedItem {
            get => selectedItem;
            set {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
                ButtonAddContent = "Добавить";
            }
        }

        public AlbumSongsViewModel() {
            AlbumSongs = new CollectionViewSource();
            Album = new CollectionViewSource();
            Genre = new CollectionViewSource();
            Song = new CollectionViewSource();
            RefreshData();
            SelectedItem = AlbumSongs.View.CurrentItem as Album_Songs;
            ButtonAddContent = "Добавить";
            //SaveEvent = new SaveCommand(this);
            //AddEvent = new AddCommand(this);
            //RefreshEvent = new RefreshCommand(this);
            //DeleteEvent = new DeleteCommand(this);
        }

        public void RefreshData() {
            using (var dbContext = new MusicStoreDBEntities()) {
                AlbumSongs.Source = dbContext.Album_Songs.ToList();
                var songQuery = (from s in dbContext.Songs
                                 select new {
                                     s.id_song,
                                     s.song_title
                                 }).ToList();
                Song.Source = songQuery;
                var albumQuery = (from a in dbContext.Albums
                                  select new {
                                      a.id_album,
                                      a.album_name
                                  }).ToList();
                Album.Source = albumQuery;
                var genreQuery = (from g in dbContext.Genres
                                  select new {
                                      g.id_genre,
                                      g.genre1
                                  }).ToList();
                Genre.Source = genreQuery;
            }
        }
        public void SaveChanges() {
            try {
                using (var dbContext = new MusicStoreDBEntities()) {
                    if (ButtonAddContent == "Отмена") {
                        AddAlbumSongData(dbContext);
                        ButtonAddContent = "Добавить";
                    } else {
                        EditAlbumSongData(dbContext);
                    }
                }
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public void AddAlbumSongData(MusicStoreDBEntities dbContext) {
            dbContext.Album_Songs.Add(SelectedItem);
            dbContext.SaveChanges();
        }
        public void EditAlbumSongData(MusicStoreDBEntities dbContext) {
            dbContext.Entry(SelectedItem).State = EntityState.Modified;
            dbContext.SaveChanges();
        }
        public void DeleteAlbumSongData() {
            try {
                using (var dbContext = new MusicStoreDBEntities()) {
                    var entity = SelectedItem;
                    dbContext.Entry(entity).State = EntityState.Deleted;
                    dbContext.SaveChanges();
                    RefreshData();
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
