using MusicStoreDB_App.Commands;
using MusicStoreDB_App.Data;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MusicStoreDB_App.ViewModels {
    public class SongViewModel : BaseViewModel, IPageViewModel {
        public CollectionViewSource Songs { get; private set; }      
        public CollectionViewSource ListSongs { get; set; }
        private Song selectedItem;
        public Song SelectedItem {
            get { return selectedItem; }
            set {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
                ButtonAddContent = "Добавить";
            }
        }
        public string Name {
            get => "Композиции";
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
            dbContext.Songs.Add(SelectedItem as Song);
        }
        public void EditSongData(MusicStoreDBEntities dbContext) {
            dbContext.Entry(SelectedItem).State = EntityState.Modified;
        }
        public void DeleteSongData() {
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