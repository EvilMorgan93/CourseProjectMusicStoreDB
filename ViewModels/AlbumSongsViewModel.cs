using iTextSharp.text;
using iTextSharp.text.pdf;
using MusicStoreDB_App.Commands;
using MusicStoreDB_App.Data;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace MusicStoreDB_App.ViewModels {
    public class AlbumSongsViewModel : BaseViewModel {
        public CollectionViewSource AlbumSongs { get; }
        public CollectionViewSource Album { get; }
        public CollectionViewSource Song { get; }
        public string Name => "Альбомные композиции";
        private Album_Songs selectedAlbumSongItem;
        public Album_Songs SelectedAlbumSongItem {
            get => selectedAlbumSongItem;
            set
            {
                SetProperty(ref selectedAlbumSongItem, value);
                SelectedAlbumItem = selectedAlbumSongItem.Album;
                SelectedSongItem = selectedAlbumSongItem.Song;
            }
        }
        private Album selectedAlbumItem;
        public Album SelectedAlbumItem {
            get => selectedAlbumItem;
            set => SetProperty(ref selectedAlbumItem, value);
        }
        private Song selectedSongItem;
        public Song SelectedSongItem {
            get => selectedSongItem;
            set => SetProperty(ref selectedSongItem, value);
        }

        public AlbumSongsViewModel() {
            AlbumSongs = new CollectionViewSource();
            Album = new CollectionViewSource();
            Song = new CollectionViewSource();
            RefreshData();
            SaveEvent = new SaveCommand(this);
            AddEvent = new AddCommand(this);
            RefreshEvent = new RefreshCommand(this);
            DeleteEvent = new DeleteCommand(this);
            ExportEvent = new ExportCommand(this);
        }

        public void RefreshData() {
            using (var dbContext = new MusicStoreDBEntities()) {
                AlbumSongs.Source = dbContext.Album_Songs
                    .Include(a => a.Album)
                    .Include(s => s.Song)
                    .ToList();
                Song.Source = dbContext.Songs.ToList();
                Album.Source = dbContext.Albums.ToList();
            }
        }
        public void ExportAlbumSongsToPdf() {
            try {
                var document = new Document();
                var writer = PdfWriter.GetInstance(document, new FileStream("Отчёт по песням.pdf", FileMode.Create));
                document.Open();
                using (var dbContext = new MusicStoreDBEntities()) {
                    var ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
                    var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
                    string[] nameColumns = {
                        "Название альбома",
                        "Название композиции",
                        "Номер трека"
                    };
                    var table = new PdfPTable(nameColumns.Length) {
                        WidthPercentage = 100
                    };
                    var cell = new PdfPCell(new Phrase("Отчёт по песням", font)) {
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
                MessageBox.Show("Отчёт сформирован!", "Информация об отчёте", MessageBoxButton.OK, MessageBoxImage.Information);
                Process.Start("Отчёт по песням.pdf");
                writer.Close();
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
                    dbContext.SaveChanges();
                }
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public void AddAlbumSongData(MusicStoreDBEntities dbContext) {
            SelectedAlbumSongItem.id_album = SelectedAlbumItem.id_album;
            SelectedAlbumSongItem.id_song = SelectedSongItem.id_song;
            dbContext.Album_Songs.Add(SelectedAlbumSongItem);
        }
        public void EditAlbumSongData(MusicStoreDBEntities dbContext) {
            dbContext.Albums.Attach(SelectedAlbumItem);
            dbContext.Songs.Attach(SelectedSongItem);
            SelectedAlbumSongItem.id_album = SelectedAlbumItem.id_album;
            SelectedAlbumSongItem.id_song = SelectedSongItem.id_song;
            dbContext.Entry(SelectedAlbumSongItem).State = EntityState.Modified;
        }
        public void DeleteAlbumSongData() {
            try {
                using (var dbContext = new MusicStoreDBEntities()) {
                    dbContext.Entry(SelectedAlbumSongItem).State = EntityState.Deleted;
                    dbContext.SaveChanges();
                    RefreshData();
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
