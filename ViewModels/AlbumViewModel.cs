using MusicStoreDB_App.Commands;
using MusicStoreDB_App.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MusicStoreDB_App.ViewModels {
    public class AlbumViewModel : BaseViewModel{
        public CollectionViewSource Albums { get; private set; }
        public CollectionViewSource Groups { get; private set; }

        private Album selectedItem;
        public Album SelectedItem {
            get { return selectedItem; }
            set {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
                ButtonAddContent = "Добавить";
            }
        }
        public AlbumViewModel() {

            Albums = new CollectionViewSource();
            Groups = new CollectionViewSource();

            RefreshData();
            SelectedItem = Albums.View.CurrentItem as Album;
            ButtonAddContent = "Добавить";
            SaveEvent = new SaveCommand(this);
            AddEvent = new AddCommand(this);
            RefreshEvent = new RefreshCommand(this);
            DeleteEvent = new DeleteCommand(this);           
        }
        public void RefreshData() {
            using (var dbContext = new MusicStoreDBEntities()) {
                Albums.Source = dbContext.Albums.ToList();
            }
        }
        public void SaveChanges() {
            try {
                using (var dbContext = new MusicStoreDBEntities()) {
                    if (ButtonAddContent == "Отмена") {
                        AddAlbumData(dbContext);
                        ButtonAddContent = "Добавить";
                    } else {
                        EditAlbumData(dbContext);
                    }
                }
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }            
        }
        public void AddAlbumData(MusicStoreDBEntities dbContext) {
            dbContext.Albums.Add(SelectedItem as Album);
            dbContext.SaveChanges();
        }
        public void EditAlbumData(MusicStoreDBEntities dbContext) {
            dbContext.Entry(SelectedItem).State = EntityState.Modified;
            dbContext.SaveChanges();
        }
        public void DeleteAlbumData() {
            try {
                using (var dbContext = new MusicStoreDBEntities()) {
                    var entity = Albums.View.CurrentItem as Album;
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