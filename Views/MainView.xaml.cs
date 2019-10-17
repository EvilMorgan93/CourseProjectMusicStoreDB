using MusicStoreDB_App.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace MusicStoreDB_App {
    public partial class MainView : Window {
        public MainView() {
            InitializeComponent();
        }
       /* private void CreateSongDataGridView() {
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
        }*/
    }
}