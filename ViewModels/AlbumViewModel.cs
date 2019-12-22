using MusicStoreDB_App.Commands;
using MusicStoreDB_App.Data;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MusicStoreDB_App.ViewModels {
    public class AlbumViewModel : BaseViewModel {
        public CollectionViewSource Albums { get; }
        public CollectionViewSource Groups { get; }
        public string Name => "Альбомы";
        private Album selectedAlbumItem;
        public Album SelectedAlbumItem {
            get => selectedAlbumItem;
            set {
                SetProperty(ref selectedAlbumItem, value);
                if (selectedAlbumItem == null) {
                    SelectedGroupItem = Groups.View.CurrentItem as Group;
                } else {
                    SelectedGroupItem = selectedAlbumItem.Group;
                }
            }
        }

        private Group selectedGroupItem;
        public Group SelectedGroupItem {
            get => selectedGroupItem;
            set => SetProperty(ref selectedGroupItem, value);
        }


        public AlbumViewModel() {
            Albums = new CollectionViewSource();
            Groups = new CollectionViewSource();
            RefreshData();
            SaveEvent = new SaveCommand(this);
            AddEvent = new AddCommand(this);
            RefreshEvent = new RefreshCommand(this);
            DeleteEvent = new DeleteCommand(this);
            EditEvent = new EditCommand(this);
        }
        public void RefreshData() {
            using var dbContext = new MusicStoreDBEntities();
            Albums.Source = dbContext.Albums
                .Include(a => a.Group)
                .ToList();
            Groups.Source = dbContext.Groups.ToList();
        }
        public void SaveChanges() {
            if (ButtonAddContent == "Отмена") {
                AddAlbumData();
                ButtonAddContent = "Добавить";
            }
        }
        public void AddAlbumData() {
            try {
                using var dbContext = new MusicStoreDBEntities();
                SelectedAlbumItem.id_artist = SelectedGroupItem.id_artist;
                dbContext.Albums.Add(SelectedAlbumItem);
                dbContext.SaveChanges();
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public void EditAlbumData() {
            try {
                using var dbContext = new MusicStoreDBEntities();
                dbContext.Groups.Attach(SelectedGroupItem);
                SelectedAlbumItem.id_artist = SelectedGroupItem.id_artist;
                dbContext.Entry(SelectedAlbumItem).State = EntityState.Modified;
                dbContext.SaveChanges();
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public void DeleteAlbumData() {
            try {
                using var dbContext = new MusicStoreDBEntities();
                dbContext.Entry(SelectedAlbumItem).State = EntityState.Deleted;
                dbContext.SaveChanges();
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}