using iTextSharp.text;
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
        public SongViewModel SongViewModel { get { return DataContext as SongViewModel; } }
        public AlbumViewModel AlbumViewModel { get { return DataContext as AlbumViewModel; } }
        public DataGridView() {
            InitializeComponent();
            DataContext = new SongViewModel();
            CreateSongDataGridView();
        }
        private void CreateSongDataGridView() {
            dataGrid.ItemsSource = null;
            dataGrid.Items.Clear();
            dataGrid.Columns.Clear();
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
            dataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = SongViewModel.ListSongs });
            SongViewModel.RefreshData();
            CreateAddSongView();
        }
        private void CreateAlbumDataGridView() {
            dataGrid.ItemsSource = null;
            dataGrid.Items.Clear();
            dataGrid.Columns.Clear();
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
                Header = "ID Исполнителя",
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                IsReadOnly = true
            };
            dataGrid.Columns.Add(artistId);
            dataGrid.SetBinding(ItemsControl.ItemsSourceProperty, new Binding { Source = AlbumViewModel.Albums });
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
                Margin = new Thickness(0, 25, 5, 5)
            };
            var durationBlock = new TextBlock() {
                Text = "Длительность",
                Style = FindResource("textBlockStyle") as Style,
                Margin = new Thickness(0, 5, 5, 5)
            };
            Grid.SetRow(titleBlock, 0);
            Grid.SetRow(durationBlock, 1);
            durationBox = new TextBox() {
                Margin = new Thickness(5, 5, 5, 5),
                Style = FindResource("textBoxStyle") as Style
            };
            titleBox = new TextBox() {
                Margin = new Thickness(5, 25, 5, 5),
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
            for (int j = 0; j < 3; j++) {
                RowDefinition rowDefinition = new RowDefinition();
                grid.RowDefinitions.Add(rowDefinition);
            }
            var titleBlock = new TextBlock() {
                Text = "Название альбома",
                Style = FindResource("textBlockStyle") as Style,
                Margin = new Thickness(0, 25, 5, 5)
            };
            var yearBlock = new TextBlock() {
                Text = "Дата создания",
                Style = FindResource("textBlockStyle") as Style,
                Margin = new Thickness(0, 5, 5, 5)
            };
            var idArtistBlock = new TextBlock() {
                Text = "Название исполнителя",
                Style = FindResource("textBlockStyle") as Style,
                Margin = new Thickness(0, 5, 5, 5)
            };
            Grid.SetRow(titleBlock, 0);
            Grid.SetRow(yearBlock, 1);
            Grid.SetRow(idArtistBlock, 2);
            grid.Children.Add(titleBlock);
            grid.Children.Add(yearBlock);
            grid.Children.Add(idArtistBlock);
            var titleBox = new TextBox() {
                Margin = new Thickness(5, 25, 5, 5),
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
                TargetNullValue = ""           
            };
            titleBox.SetBinding(TextBox.TextProperty, a);
            yearBox.SetBinding(TextBox.TextProperty, b);
            idArtistBox.SetBinding(TextBox.TextProperty, c);
            Grid.SetColumn(titleBox, 1);
            Grid.SetColumn(yearBox, 1);
            Grid.SetRow(yearBox, 1);
            Grid.SetColumn(idArtistBox, 1);
            Grid.SetRow(idArtistBox, 2);
            grid.Children.Add(titleBox);
            grid.Children.Add(yearBox);
            grid.Children.Add(idArtistBox);
            addStackPanel.Children.Add(grid);
        }
        private void Export_Pucrhases_To_PDF_Click(object sender, RoutedEventArgs e) {
            try {
                var document = new Document();
                var writer = PdfWriter.GetInstance(document, new FileStream("Отчёт по продажам.pdf", FileMode.Create));
                document.Open();
                using (var dbContext = new MusicStoreDBEntities()) {
                    string ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
                    var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
                    string[] nameColumns = new string[] {
                        "Имя продавца",
                        "Название альбома",
                        "Название группы",
                        "Дата покупки",
                        "Цена"
                    };
                    var table = new PdfPTable(nameColumns.Length);
                    PdfPCell cell = new PdfPCell(new Phrase("Отчёт по продажам", font)) {
                        Colspan = nameColumns.Length,
                        HorizontalAlignment = 1,
                        Border = 0,
                        PaddingBottom = 10
                    };
                    table.AddCell(cell);
                    var query = (from a in dbContext.Albums
                                 join g in dbContext.Groups on a.id_artist equals g.id_artist
                                 join p in dbContext.Purchases on a.id_album equals p.id_album
                                 join pr in dbContext.Price_List on p.id_price equals pr.id_price
                                 join emp in dbContext.Employees on p.id_employee equals emp.id_employee into ps
                                  from emp in ps.DefaultIfEmpty()
                                 select new {
                                     emp.employee_name,
                                     a.album_name,
                                     g.group_name,
                                     p.purchase_date,
                                     pr.purchase_price
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
                        table.AddCell(new PdfPCell(new Phrase(query[k].employee_name, font)) {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        table.AddCell(new PdfPCell(new Phrase(query[k].album_name, font)) {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        table.AddCell(new PdfPCell(new Phrase(query[k].group_name, font)) {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });                       
                        table.AddCell(new PdfPCell(new Phrase(query[k].purchase_date.ToString("G"), font)) {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        table.AddCell(new PdfPCell(new Phrase(query[k].purchase_price.ToString(), font)) {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                    }
                    document.Add(table);
                }
                document.Close();
                writer.Close();
                MessageBox.Show("Отчёт сформирован!", "Информация об отчёте", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message,"Информация об отчёте",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }
        private void ComboBoxTableChoose_DropDownClosed(object sender, EventArgs e) {
            switch (comboBoxTableChoose.Text) {
                case "Песни":
                    DataContext = new SongViewModel();
                    CreateSongDataGridView();
                    break;
                case "Альбомы":
                    DataContext = new AlbumViewModel();
                    CreateAlbumDataGridView();
                    break;
            }
        }
        private void Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
    }
}