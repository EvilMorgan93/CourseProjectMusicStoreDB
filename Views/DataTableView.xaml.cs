using iTextSharp.text.pdf;
using MusicStoreDB_App.Data;
using MusicStoreDB_App.Models;
using MusicStoreDB_App.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MusicStoreDB_App {
    public partial class DataTableView : Window {
        private readonly SongDataTableModel songData = new SongDataTableModel();
        private readonly AlbumDataTableModel albumData = new AlbumDataTableModel();
        private readonly PurchaseDataTableModel purchaseData = new PurchaseDataTableModel();
        public DataTableView() {
            InitializeComponent();        
            songData.CreateSongGridView(listView);
        }
        public async Task RefreshListView() {
            using (var db = new MusicStoreDBEntities()) {
                //SQL Query == "SELECT * FROM TABLE"  
                switch (comboBoxTableChoose.Text) {
                    case "Песни":
                        listView.ItemsSource = await db.Songs.ToListAsync();
                        break;
                    case "Альбомы":
                        listView.ItemsSource = await db.Albums.ToListAsync();
                        break;
                }
            }
        }
        //Нажатие кнопки "Обновить"
        private async void Refresh_ClickAsync(object sender, RoutedEventArgs e) {
            await RefreshListView();
        }
        private void Add_Click(object sender, RoutedEventArgs e) {
            var modalAdd = new AddDataView();
            modalAdd.ShowDialog();
        }
        private async void Delete_ClickAsync(object sender, RoutedEventArgs e) {
            // SQL Query == "DELETE FROM TABLE WHERE predicat"
            switch (comboBoxTableChoose.Text) {
                case "Песни":
                    songData.DeleteSongData(listView);
                    await RefreshListView();
                    break;
                case "Альбомы":
                    albumData.DeleteAlbumData(listView);
                    await RefreshListView();
                    break;
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e) {

        }
        private void Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
        private void ComboBoxTableChoose_DropDownClosed(object sender, EventArgs e) {
            switch (comboBoxTableChoose.Text) {
                case "Песни":
                    songData.CreateSongGridView(listView);
                    break;
                case "Альбомы":
                    albumData.CreateAlbumGridView(listView);
                    break;
            }
        }
        private void Safe_Songs_To_PDF_Click(object sender, RoutedEventArgs e) {
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
                var song = db.Songs.ToList();
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
    }
}