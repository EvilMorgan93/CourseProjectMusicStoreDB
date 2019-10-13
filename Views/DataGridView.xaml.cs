using iTextSharp.text.pdf;
using MusicStoreDB_App.Data;
using MusicStoreDB_App.ViewModels;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq; 
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace MusicStoreDB_App {
    public partial class DataGridView : Window {
        public SongViewModel songViewModel { get { return DataContext as SongViewModel; } }
        public AlbumViewModel albumViewModel { get { return DataContext as AlbumViewModel; } }
        public DataGridView() {
            InitializeComponent();
            DataContext = new SongViewModel();
            CreateSongView();
        }
        //Удаление строки из базы данных
        private void Delete_Click(object sender, RoutedEventArgs e) {
            using (var dbContext = new MusicStoreDBEntities()) {
                var entity = dataGrid.SelectedItem;
                dbContext.Entry(entity).State = EntityState.Deleted;
                dbContext.SaveChanges();
                songViewModel.RefreshData();
            }
        }
        private void CreateSongView() {
            dataGrid.ItemsSource = null;
            dataGrid.Items.Clear();
            dataGrid.Columns.Clear();
            var idColumn = new DataGridTextColumn {
                Binding = new Binding("id_song"),
                Header = "ID",
                Width = 100,
                IsReadOnly = true
            };
            dataGrid.Columns.Add(idColumn);
            var songTitle = new DataGridTextColumn {
                Binding = new Binding("song_title"),
                Header = "Название песни",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };
            dataGrid.Columns.Add(songTitle);
            var durationColumn = new DataGridTextColumn {
                Binding = new Binding("song_duration"),
                Header = "Длительность",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };
            dataGrid.Columns.Add(durationColumn);
            dataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = songViewModel.ListSongs });
            songViewModel.RefreshData();
            CreateAddSongView();
        }
        private void CreateAlbumView() {
            dataGrid.ItemsSource = null;
            dataGrid.Items.Clear();
            dataGrid.Columns.Clear();
            var albumId = new DataGridTextColumn {
                Binding = new Binding("id_album"),
                Header = "ID_Album",
                Width = 80,
                IsReadOnly = true
            };
            dataGrid.Columns.Add(albumId);
            var albumName = new DataGridTextColumn {
                Binding = new Binding("album_name"),
                Header = "Название альбома",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };
            dataGrid.Columns.Add(albumName);
            var albumYear = new DataGridTextColumn {
                Binding = new Binding("album_year"),
                Header = "Год выпуска",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star)
            };
            dataGrid.Columns.Add(albumYear);
            var artistId = new DataGridTextColumn {
                Binding = new Binding("id_artist"),
                Header = "ID_Artist",
                Width = 80,
                IsReadOnly = true
            };
            dataGrid.Columns.Add(artistId);
            var albumSongId = new DataGridTextColumn {
                Binding = new Binding("id_album_songs"),
                Header = "ID_Album_Song",
                Width = 110,
                IsReadOnly = true
            };
            dataGrid.Columns.Add(albumSongId);
            dataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = albumViewModel.AlbumsSongs });
            CreateAddAlbumView();
        }
        private void CreateAddSongView() {
            addStackPanel.Children.Clear();
            var grid = new Grid();
            TextBox titleBox;
            TextBox durationBox;
            for (int i = 0; i < 2; i++) {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                grid.ColumnDefinitions.Add(columnDefinition);
            }
            for (int j = 0; j < 2; j++) {
                RowDefinition rowDefinition = new RowDefinition();
                grid.RowDefinitions.Add(rowDefinition);
            }
            var titleBlock = new TextBlock() {
                Text = "Название песни",
                Style = FindResource("textBlockStyle") as Style,
                Margin = new Thickness(0, 5, 5, 5)
            };
            var durationBlock = new TextBlock() {
                Text = "Длительность",
                Style = FindResource("textBlockStyle") as Style,
                Margin = new Thickness(0, 5, 5, 5)
            };
            Grid.SetRow(titleBlock, 0);
            Grid.SetRow(durationBlock, 1);
            durationBox = new TextBox() {
                Margin = new Thickness(5,5,5,5),
                Style = FindResource("textBoxStyle") as Style
            };
            titleBox = new TextBox() {
                Margin = new Thickness(5, 5, 5, 5),
                Style = FindResource("textBoxStyle") as Style
            };
            var x = new Binding() {
                TargetNullValue = "",
                Path = new PropertyPath("SelectedItem.song_title"),
                Mode = BindingMode.TwoWay
            };
            var b = new Binding {
                Path = new PropertyPath("SelectedItem.song_duration"),
                TargetNullValue = "",
                Mode = BindingMode.TwoWay,
            };                               
            titleBox.SetBinding(TextBox.TextProperty, x);
            durationBox.SetBinding(TextBox.TextProperty, b);
            Grid.SetColumn(titleBox, 1);
            Grid.SetColumn(durationBox, 1);
            Grid.SetRow(durationBox, 1);
            grid.Children.Add(titleBlock);
            grid.Children.Add(durationBlock);
            grid.Children.Add(titleBox);
            grid.Children.Add(durationBox);
            addStackPanel.Children.Add(grid);
        }
        private void CreateAddAlbumView() {
            addStackPanel.Children.Clear();
            var grid = new Grid();
            for (int i = 0; i < 2; i++) {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                grid.ColumnDefinitions.Add(columnDefinition);
            }
            for (int j = 0; j < 4; j++) {
                RowDefinition rowDefinition = new RowDefinition();
                grid.RowDefinitions.Add(rowDefinition);
            }
            var titleBlock = new TextBlock() {
                Text = "Название альбома",
                Style = FindResource("textBlockStyle") as Style,
                Margin = new Thickness(0, 5, 5, 5)
            };
            var yearBlock = new TextBlock() {
                Text = "Дата создания",
                Style = FindResource("textBlockStyle") as Style,
                Margin = new Thickness(0, 5, 5, 5)
            };
            var idArtistBlock = new TextBlock() {
                Text = "ID Артиста",
                Style = FindResource("textBlockStyle") as Style,
                Margin = new Thickness(0, 5, 5, 5)
            };
            var idAlbumSongBlock = new TextBlock() {
                Text = "ID Альбомной песни",
                Style = FindResource("textBlockStyle") as Style,
                Margin = new Thickness(0, 5, 5, 5)
            };
            Grid.SetRow(titleBlock, 0);
            Grid.SetRow(yearBlock, 1);
            Grid.SetRow(idArtistBlock, 2);
            Grid.SetRow(idAlbumSongBlock, 3);
            grid.Children.Add(titleBlock);
            grid.Children.Add(yearBlock);
            grid.Children.Add(idArtistBlock);
            grid.Children.Add(idAlbumSongBlock);
            var titleBox = new TextBox() {
                Margin = new Thickness(5, 5, 5, 5),
                Style = FindResource("textBoxStyle") as Style
            };
            var yearBox = new TextBox() {
                Margin = new Thickness(5, 5, 5, 5),
                Style = FindResource("textBoxStyle") as Style
            };
            var idArtistBox = new TextBox() {
                Margin = new Thickness(5, 5, 5, 5),
                Style = FindResource("textBoxStyle") as Style
            };
            var idAlbumSongBox = new TextBox() {
                Margin = new Thickness(5, 5, 5, 5),
                Style = FindResource("textBoxStyle") as Style
            };
            var a = new Binding() {
                TargetNullValue = "",
                Path = new PropertyPath("SelectedItem.album_name"),
                Mode = BindingMode.TwoWay
            };
            var b = new Binding {
                Path = new PropertyPath("SelectedItem.album_year"),
                TargetNullValue = "",
                Mode = BindingMode.TwoWay,
            };
            var c = new Binding {
                Path = new PropertyPath("SelectedItem.id_artist"),
                TargetNullValue = "",
                Mode = BindingMode.TwoWay,
            };
            var d = new Binding {
                Path = new PropertyPath("SelectedItem.id_album_songs"),
                TargetNullValue = "",
                Mode = BindingMode.TwoWay,
            };
            titleBox.SetBinding(TextBox.TextProperty, a);
            yearBox.SetBinding(TextBox.TextProperty, b);
            idArtistBox.SetBinding(TextBox.TextProperty, c);
            idAlbumSongBox.SetBinding(TextBox.TextProperty, d);
            Grid.SetColumn(titleBox, 1);
            Grid.SetColumn(yearBox, 1);
            Grid.SetRow(yearBox, 1);
            Grid.SetColumn(idArtistBox, 1);
            Grid.SetRow(idArtistBox, 2);
            Grid.SetColumn(idAlbumSongBox, 1);
            Grid.SetRow(idAlbumSongBox, 3);
            grid.Children.Add(titleBox);
            grid.Children.Add(yearBox);
            grid.Children.Add(idArtistBox);
            grid.Children.Add(idAlbumSongBox);
            addStackPanel.Children.Add(grid);
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
        private void ComboBoxTableChoose_DropDownClosed(object sender, EventArgs e) {
            switch (comboBoxTableChoose.Text) {
                case "Песни":
                    DataContext = new SongViewModel();
                    CreateSongView();
                    break;
                case "Альбомы":
                    DataContext = new AlbumViewModel();
                    CreateAlbumView();
                    break;
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
    }
}