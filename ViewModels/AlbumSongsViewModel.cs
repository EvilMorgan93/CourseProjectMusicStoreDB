using iTextSharp.text;
using iTextSharp.text.pdf;
using MusicStoreDB_App.Commands;
using MusicStoreDB_App.Data;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MusicStoreDB_App.ViewModels {
    public class AlbumSongsViewModel : BaseViewModel {
        public CollectionViewSource AlbumSongs { get; }
        public CollectionViewSource Album { get; }
        public CollectionViewSource Song { get; }
        public string Name => "Композиции по альбомам";
        private Album_Songs selectedAlbumSongItem;
        public Album_Songs SelectedAlbumSongItem {
            get => selectedAlbumSongItem;
            set {
                SetProperty(ref selectedAlbumSongItem, value);
                if (selectedAlbumSongItem == null) {
                    SelectedAlbumItem = Album.View.CurrentItem as Album;
                    SelectedSongItem = Song.View.CurrentItem as Song;
                } else {
                    SelectedAlbumItem = selectedAlbumSongItem.Album;
                    SelectedSongItem = selectedAlbumSongItem.Song;
                }
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
        private string filterString;
        public string FilterString {
            get => filterString;
            set {
                SetProperty(ref filterString, value);
                AlbumSongs.View.Refresh();
            }
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
            EditEvent = new EditCommand(this);
            ExportEvent = new ExportCommand(this);
        }
        public void RefreshData() {
            using (var dbContext = new MusicStoreDBEntities()) {
                AlbumSongs.SortDescriptions.Add(new SortDescription("id_album", ListSortDirection.Ascending));
                AlbumSongs.Source = dbContext.Album_Songs
                    .Include(a => a.Album)
                    .Include(s => s.Song)
                    .ToList();
                Song.Source = dbContext.Songs.ToList();
                Album.Source = dbContext.Albums.ToList();
                AlbumSongs.View.Filter = Filter;
            }
        }
        private bool Filter(object obj) {
            if (!(obj is Album_Songs data)) return false;
            if (!string.IsNullOrEmpty(filterString)) {
                return data.Album.album_name.Contains(filterString) || data.Song.song_title.Contains(filterString);
            }
            return true;
        }
        public async Task ExportAlbumSongsPdfAsync() {
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
                        "Название группы",
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
                    
                    foreach (var t in nameColumns) {
                        cell = new PdfPCell(new Phrase(t, font)) {
                            BackgroundColor = BaseColor.LIGHT_GRAY,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 3,
                        };
                        table.AddCell(cell);
                    }
                    var query = await (from albs in dbContext.Album_Songs
                        join a in dbContext.Albums on albs.id_album equals a.id_album
                        join g in dbContext.Groups on a.id_artist equals g.id_artist
                        join s in dbContext.Songs on albs.id_song equals s.id_song
                        orderby albs.id_album, albs.track_number
                        select new {
                            a.album_name,
                            g.group_name,
                            s.song_title,
                            albs.track_number
                        }).ToListAsync();
                    var aggregateAlbumNameQuery = query
                        .GroupBy(x => new { x.album_name,x.group_name })
                        .Select(x => new { x.Key.album_name,x.Key.group_name })
                        .ToArray();
                    var isOnlyOne = true;
                    for (int k = 0, j = 0; k < query.Count; k++) {
                        if (query[k].album_name == aggregateAlbumNameQuery[j].album_name && isOnlyOne) {
                            table.AddCell(
                                new PdfPCell(new Phrase(aggregateAlbumNameQuery[j].album_name, font)) {
                                    HorizontalAlignment = Element.ALIGN_CENTER,
                                    BackgroundColor = BaseColor.LIGHT_GRAY,
                                    BorderColor = BaseColor.BLACK,
                                    BorderWidth = 1.6f
                                });
                            table.AddCell(
                                new PdfPCell(new Phrase(aggregateAlbumNameQuery[j].group_name, font)) {
                                    HorizontalAlignment = Element.ALIGN_CENTER
                                });
                            isOnlyOne = false;
                            if (aggregateAlbumNameQuery.Length - 1 != j) {
                                j++;
                                isOnlyOne = true;
                            }
                        } else {
                            for (int s = 0; s < 2; s++) {
                                table.AddCell(new PdfPCell());
                            }
                        }
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
                Process.Start("Отчёт по песням.pdf");
                writer.Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Информация об отчёте", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void SaveChanges() {
            if (ButtonAddContent == "Отмена") {
                AddAlbumSongData();
                ButtonAddContent = "Добавить";
            }
        }
        public void AddAlbumSongData() {
            try {
                using (var dbContext = new MusicStoreDBEntities()) {
                    SelectedAlbumSongItem.id_album = SelectedAlbumItem.id_album;
                    SelectedAlbumSongItem.id_song = SelectedSongItem.id_song;
                    dbContext.Album_Songs.Add(SelectedAlbumSongItem);
                    dbContext.SaveChanges();
                }
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public void EditAlbumSongData() {
            try {
                using (var dbContext = new MusicStoreDBEntities()) {
                    dbContext.Albums.Attach(SelectedAlbumItem);
                    dbContext.Songs.Attach(SelectedSongItem);
                    SelectedAlbumSongItem.id_album = SelectedAlbumItem.id_album;
                    SelectedAlbumSongItem.id_song = SelectedSongItem.id_song;
                    dbContext.Entry(SelectedAlbumSongItem).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public void DeleteAlbumSongData() {
            try {
                using (var dbContext = new MusicStoreDBEntities()) {
                    dbContext.Entry(SelectedAlbumSongItem).State = EntityState.Deleted;
                    dbContext.SaveChanges();
                }
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}