using iTextSharp.text.pdf;
using MusicStoreDB_App.Data;
using MusicStoreDB_App.Models;
using MusicStoreDB_App.Views;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq; 
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MusicStoreDB_App {
    public partial class DataTableView : Window {
        private static readonly SongDataTableModel songData = new SongDataTableModel();
        private static readonly AlbumDataTableModel albumData = new AlbumDataTableModel();
        private static readonly PurchaseDataTableModel purchaseData = new PurchaseDataTableModel();
        public DataTableView() {
            InitializeComponent();        
            songData.CreateSongDataGrid(dataGrid);
        }
        public async Task RefreshListViewAsync() {
            using (var db = new MusicStoreDBEntities()) {
                //SQL Query == "SELECT * FROM TABLE"  
                switch (comboBoxTableChoose.Text) {
                    case "Песни":
                        dataGrid.ItemsSource = await db.Songs.ToListAsync();
                        break;
                    case "Альбомы":
                        dataGrid.ItemsSource = await db.Albums.ToListAsync();
                        break;
                    case "Продажи":
                        break;
                }
            }
        }
        //Нажатие кнопки "Обновить"
        private async void Refresh_ClickAsync(object sender, RoutedEventArgs e) {
            await RefreshListViewAsync();
        }
        private void Add_Click(object sender, RoutedEventArgs e) {
            var modalAdd = new AddDataView();
            modalAdd.ShowDialog();
        }
        private async void Delete_ClickAsync(object sender, RoutedEventArgs e) {
            // SQL Query == "DELETE FROM TABLE WHERE predicat"
            switch (comboBoxTableChoose.Text) {
                case "Песни":
                    songData.DeleteSongData(dataGrid);
                    await RefreshListViewAsync();
                    break;
                case "Альбомы":
                    albumData.DeleteAlbumData(dataGrid);
                    await RefreshListViewAsync();
                    break;
            }
        }
        private void Edit_Click(object sender, RoutedEventArgs e) {
            CreateEditStackPanel();
        }
        private void Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
        private void ComboBoxTableChoose_DropDownClosed(object sender, EventArgs e) {
            switch (comboBoxTableChoose.Text) {
                case "Песни":
                    songData.CreateSongDataGrid(dataGrid);
                    break;
                case "Альбомы":
                    albumData.CreateAlbumDataGrid(dataGrid);
                    break;
            }
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

        private void CreateEditStackPanel() {
            //addBorder.Visibility = Visibility.Visible;
            //using (var db = new MusicStoreDBEntities()) {
            //    var textBlock = new TextBlock() {
            //        Text = comboBoxTableChoose.Text,
            //        FontSize = 20,
            //        TextAlignment = TextAlignment.Center,
            //        Margin = new Thickness(0, 10, 0, 0)
            //    };
            //    editStackPanel.Children.Add(textBlock);
            //    var grid = new Grid();
            //    ColumnDefinition column = new ColumnDefinition() {
            //        Width = new GridLength(1, GridUnitType.Star)
            //    };
            //    grid.ColumnDefinitions.Add(column);
            //    column = new ColumnDefinition();
            //    grid.ColumnDefinitions.Add(column);

            //    for (int i = 0; i < db.Entr; i++) {
            //        RowDefinition row = new RowDefinition();
            //        grid.RowDefinitions.Add(row);
            //        textBlock = new TextBlock() {
            //            Text = array[i],
            //            Style = FindResource("textBlockStyle") as Style,
            //            HorizontalAlignment = HorizontalAlignment.Center,
            //            TextAlignment = TextAlignment.Center,
            //            Margin = new Thickness(10, 20, 5, 5)
            //        };
            //        Grid.SetRow(textBlock, i);
            //        grid.Children.Add(textBlock);
            //        TextBox textBox = new TextBox() {
            //            Margin = new Thickness(10, 20, 40, 5),
            //            HorizontalAlignment = HorizontalAlignment.Left,
            //            Width = 300,
            //            Style = FindResource("textBoxStyle") as Style,
            //            Name = "textBox" + $"{i}"
            //        };
            //        Grid.SetColumn(textBox, 1);
            //        Grid.SetRow(textBox, i);
            //        textBoxNames[i] = textBox;
            //        grid.Children.Add(textBox);
            //    }
            //    editStackPanel.Children.Add(grid);

            //    grid = new Grid() {
            //        Margin = new Thickness(0, 30, 0, 0)
            //    };
            //    for (int i = 0; i < 2; i++) {
            //        ColumnDefinition columnDefinition = new ColumnDefinition();
            //        grid.ColumnDefinitions.Add(columnDefinition);
            //    }
            //    Button acceptButton = new Button() {
            //        Content = "Добавить",
            //        Style = FindResource("AddStyle") as Style,
            //        HorizontalAlignment = HorizontalAlignment.Center,
            //    };
            //    Grid.SetColumn(acceptButton, 0);
            //    grid.Children.Add(acceptButton);
            //    Button cancelButton = new Button() {
            //        Content = "Отмена",
            //        Style = FindResource("DeleteStyle") as Style,
            //        HorizontalAlignment = HorizontalAlignment.Center
            //    };
            //    Grid.SetColumn(cancelButton, 1);
            //    grid.Children.Add(cancelButton);
            //    editStackPanel.Children.Add(grid);
            //    cancelButton.Click +=
            //    acceptButton.Click += Accept_Click;
            //}
        }
    }
}