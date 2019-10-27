using MusicStoreDB_App.Commands;
using MusicStoreDB_App.Data;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MusicStoreDB_App.ViewModels {
    public class SongViewModel : BaseViewModel {
        public CollectionViewSource Songs { get; set; }      
        public CollectionViewSource ListSongs { get; set; }

        public string Name => "Композиции";
        private Song selectedSongItem;
        public Song SelectedSongItem {
            get => selectedSongItem;
            set => SetProperty(ref selectedSongItem, value);
        }

        public SongViewModel() {
            Songs = new CollectionViewSource();
            ListSongs = new CollectionViewSource();
            RefreshData();       
            SaveEvent = new SaveCommand(this);
            AddEvent = new AddCommand(this);
            RefreshEvent = new RefreshCommand(this);
            DeleteEvent = new DeleteCommand(this);
        }
        public void RefreshData() {
            using (var dbContext = new MusicStoreDBEntities()) {
                Songs.Source = dbContext.Songs
                    .ToList();
            }
        }
        public void SaveChanges() {
            try {
                using (var dbContext = new MusicStoreDBEntities()) {
                    if (ButtonAddContent == "Отмена") {
                        AddSongData(dbContext);
                        ButtonAddContent = "Добавить";
                    } else {
                        EditSongData(dbContext);
                    }
                    dbContext.SaveChanges();
                }
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public void AddSongData(MusicStoreDBEntities dbContext) {
            dbContext.Songs.Add(SelectedSongItem);
        }
        public void EditSongData(MusicStoreDBEntities dbContext) {
            dbContext.Entry(SelectedSongItem).State = EntityState.Modified;
        }
        public void DeleteSongData() {
            try {
                using (var dbContext = new MusicStoreDBEntities()) {
                    var entity = SelectedSongItem;
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