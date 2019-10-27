using MusicStoreDB_App.Commands;
using MusicStoreDB_App.Data;
using System;
using System.Collections.ObjectModel;
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
            set => SetProperty(ref selectedAlbumItem, value);
        }

        public AlbumViewModel() {
            Albums = new CollectionViewSource();
            Groups = new CollectionViewSource();
            RefreshData();          
            SaveEvent = new SaveCommand(this);
            AddEvent = new AddCommand(this);
            RefreshEvent = new RefreshCommand(this);
            DeleteEvent = new DeleteCommand(this);           
        }
        public void RefreshData() {            
            using (var dbContext = new MusicStoreDBEntities()) {
                Albums.Source = dbContext.Albums
                    .Include(a => a.Group)
                    .Include(pr => pr.Price_List)
                    .ToList();
                var query = (from g in dbContext.Groups
                             join c in dbContext.Countries on g.id_country equals c.id_country
                             select new {
                                 g.id_artist,
                                 g.group_name,
                                 c.country_name
                             }).ToList();
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
                    dbContext.SaveChanges();
                }
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }            
        }
        public void AddAlbumData(MusicStoreDBEntities dbContext) {
            dbContext.Albums.Add(SelectedAlbumItem);
        }
        public void EditAlbumData(MusicStoreDBEntities dbContext) {
            dbContext.Entry(SelectedAlbumItem).State = EntityState.Modified;
        }
        public void DeleteAlbumData() {
            try {
                using (var dbContext = new MusicStoreDBEntities()) {
                    dbContext.Entry(SelectedAlbumItem).State = EntityState.Deleted;
                    dbContext.SaveChanges();
                    RefreshData();
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}