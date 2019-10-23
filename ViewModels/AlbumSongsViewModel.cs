using iTextSharp.text;
using iTextSharp.text.pdf;
using MusicStoreDB_App.Commands;
using MusicStoreDB_App.Data;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MusicStoreDB_App.ViewModels {
    public class AlbumSongsViewModel : BaseViewModel, IPageViewModel {
        public CollectionViewSource AlbumSongs { get; private set; }
        public CollectionViewSource Album { get; private set; }
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
            Song = new CollectionViewSource();
            RefreshData();
            SelectedItem = AlbumSongs.View.CurrentItem as Album_Songs;
            SaveEvent = new SaveCommand(this);
            AddEvent = new AddCommand(this);
            RefreshEvent = new RefreshCommand(this);
            DeleteEvent = new DeleteCommand(this);
            ExportEvent = new ExportCommand(this);
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
            }
        }
        public void ExportAlbumSongsToPDF() {
            try {
                var document = new Document();
                var writer = PdfWriter.GetInstance(document, new FileStream("Отчёт по песням.pdf", FileMode.Create));
                document.Open();
                using (var dbContext = new MusicStoreDBEntities()) {
                    string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
                    var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
                    string[] nameColumns = new string[] {
                        "Название альбома",
                        "Название композиции",
                        "Номер трека"
                    };
                    var table = new PdfPTable(nameColumns.Length) {
                        WidthPercentage = 100
                    };
                    PdfPCell cell = new PdfPCell(new Phrase("Отчёт по песням", font)) {
                        Colspan = nameColumns.Length,
                        HorizontalAlignment = 1,
                        Border = 0,
                        PaddingBottom = 10
                    };
                    table.AddCell(cell);
                    var query = (from albs in dbContext.Album_Songs
                                 join a in dbContext.Albums on albs.id_album equals a.id_album
                                 join s in dbContext.Songs on albs.id_song equals s.id_song
                                 orderby a.album_name, albs.track_number
                                 select new {
                                     a.album_name,
                                     s.song_title,
                                     albs.track_number
                                 }).ToList();
                    for (int i = 0; i < nameColumns.Length; i++) {
                        cell = new PdfPCell(new Phrase(nameColumns[i], font)) {
                            BackgroundColor = BaseColor.LIGHT_GRAY,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 3,
                        };
                        table.AddCell(cell);
                    }
                    for (int k = 0; k < query.Count; k++) {
                        table.AddCell(new PdfPCell(new Phrase(query[k].album_name, font)) {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        table.AddCell(new PdfPCell(new Phrase(query[k].song_title, font)) {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        table.AddCell(new PdfPCell(new Phrase(query[k].track_number.ToString(), font)) {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                    }
                    document.Add(table);
                }
                document.Close();
                writer.Close();
                MessageBox.Show("Отчёт сформирован!", "Информация об отчёте", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Информация об отчёте", MessageBoxButton.OK, MessageBoxImage.Error);
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
