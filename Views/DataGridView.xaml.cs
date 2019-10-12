using iTextSharp.text.pdf;
using MusicStoreDB_App.Data;
using MusicStoreDB_App.Models;
using MusicStoreDB_App.Views;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq; 
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MusicStoreDB_App {
    public partial class DataGridView : Window {
        private static readonly SongDataGridModel songData = new SongDataGridModel();
        private static readonly AlbumDataGridModel albumData = new AlbumDataGridModel();
        private static readonly PurchaseDataGridModel purchaseData = new PurchaseDataGridModel();
        public DataGridView() {
            InitializeComponent();                       
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e) {
            await RefreshListViewAsync();
        }
        //Асинхронное обновление базы данных в дата грид
        public async Task RefreshListViewAsync() {
            using (var db = new MusicStoreDBEntities()) {
                //SQL Query == "SELECT * FROM TABLE"  
                switch (comboBoxTableChoose.Text) {
                    case "Песни":
                        songData.CreateSongDataGrid(dataGrid);
                        dataGrid.ItemsSource = await db.Songs.ToListAsync();
                        break;
                    case "Альбомы":
                        albumData.CreateAlbumDataGrid(dataGrid);
                        dataGrid.ItemsSource = await db.Albums.ToListAsync();
                        break;
                    case "Продажи":
                        break;
                }
            }
        }
        private void Add_Click(object sender, RoutedEventArgs e) {
            var modalAdd = new AddDataView();
            modalAdd.ShowDialog();
        }
        //Удаление строки из базы данных
        private async void Delete_ClickAsync(object sender, RoutedEventArgs e) {
            // SQL Query == "DELETE FROM TABLE WHERE predicat"
            DeleteData();
            await RefreshListViewAsync();
        }
        public void DeleteData() {
            using (var db = new MusicStoreDBEntities()) {
                if (dataGrid.SelectedIndex == -1) { return; } else {
                    var entity = dataGrid.SelectedItem;
                    db.Entry(entity).State = EntityState.Deleted;
                    db.SaveChanges();
                }
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e) {
        }
        private void Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
        private async void ComboBoxTableChoose_DropDownClosed(object sender, EventArgs e) {
            await RefreshListViewAsync();
        }
        private async void Export_Songs_To_PDF_ClickAsync(object sender, RoutedEventArgs e) {
            var document = new iTextSharp.text.Document();
            var writer = PdfWriter.GetInstance(document, new FileStream("Отчёт по песням.pdf", FileMode.Create));
            document.Open();
            using (var db = new MusicStoreDBEntities()) {
                string[] nameColumns = new string[] {
                    "ID",
                    "Song Title",
                    "Song Duration"
                };
                var table = new PdfPTable(nameColumns.Length);
                var song = await db.Songs.ToListAsync();
                for (int i = 0; i < nameColumns.Length; i++) {
                    PdfPCell cell = new PdfPCell(new iTextSharp.text.Phrase(nameColumns[i])) {
                        BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY,
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
                    };
                    table.AddCell(cell);
                }
                for (int k = 0; k < db.Songs.Count(); k++) {
                    table.AddCell(new iTextSharp.text.Phrase(song[k].id_song.ToString()));
                    table.AddCell(new iTextSharp.text.Phrase(song[k].song_title));
                    table.AddCell(new iTextSharp.text.Phrase(song[k].song_duration.ToString()));         
                }
                document.Add(table);
            }
            document.Close();
            writer.Close();
            MessageBox.Show("Отчёт сформирован!", "Информация об отчёте", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e) {
            using (var db = new MusicStoreDBEntities()) {
                var item = dataGrid.SelectedCells as Song;
                var value = e.EditingElement as TextBox;
                item.song_title = value.Text;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}