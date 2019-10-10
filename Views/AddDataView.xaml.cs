using MusicStoreDB_App.Data;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MusicStoreDB_App.Views {
    public partial class AddDataView : Window {
        private readonly string[] songAttributes = new string[] {
                "Название песни",
                "Длительность песни"
            };
        private readonly string[] albumAttributes = new string[] {
                "Название альбома",
                "Год выпуска",
                "ID Артиста",
                "ID Альбомной песни"
            };       
        private static readonly DataTableView window = (DataTableView)Application.Current.MainWindow;
        private readonly TextBox[] textBoxNames = new TextBox[SizeOfArray()];
    
        public AddDataView() {
            InitializeComponent();
            switch (window.comboBoxTableChoose.Text) {
                case "Песни":
                    CreateAddView(songAttributes);
                    break;
                case "Альбомы":
                    CreateAddView(albumAttributes);
                    break;
            }
        }
        private void CreateAddView(string[] array) {
            TextBlock textBlock = new TextBlock() {
                Text = window.comboBoxTableChoose.Text,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(0,15,0,0),
                FontSize = 18
            };
            stackPanel.Children.Add(textBlock);
            Grid grid = new Grid();
            ColumnDefinition column = new ColumnDefinition() {
                Width = new GridLength(0.45, GridUnitType.Star)
            };
            grid.ColumnDefinitions.Add(column);
            column = new ColumnDefinition();
            grid.ColumnDefinitions.Add(column);

            for (int i = 0; i < array.Length; i++) {
                RowDefinition row = new RowDefinition();
                grid.RowDefinitions.Add(row);
                textBlock = new TextBlock() {
                    Text = array[i],                    
                    Style = FindResource("textBlockStyle") as Style,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(40, 20, 5, 5)
                };
                Grid.SetRow(textBlock, i);
                grid.Children.Add(textBlock);
                TextBox textBox = new TextBox() {
                    Margin = new Thickness(50, 20, 40, 5),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    MaxLength = 30,
                    Name = "textBox" + $"{i}"
                };
                Grid.SetColumn(textBox, 1);
                Grid.SetRow(textBox, i);
                textBoxNames[i] = textBox;
                grid.Children.Add(textBox);
            }
            stackPanel.Children.Add(grid);

            grid = new Grid() {
                Margin = new Thickness(0, 30, 0, 0)
            };
            for (int i = 0; i < 2; i++) {
                ColumnDefinition columnDefinition = new ColumnDefinition();
                grid.ColumnDefinitions.Add(columnDefinition);
            }
            Button acceptButton = new Button() {
                Content = "Добавить",
                Style = FindResource("AddStyle") as Style,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            Grid.SetColumn(acceptButton, 0);
            grid.Children.Add(acceptButton);
            Button cancelButton = new Button() {
                Content = "Отмена",
                Style = FindResource("DeleteStyle") as Style,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            Grid.SetColumn(cancelButton, 1);            
            grid.Children.Add(cancelButton);
            stackPanel.Children.Add(grid);
            cancelButton.Click += Cancel_Click;
            acceptButton.Click += Accept_Click;
        }
        private void Accept_Click(object sender, RoutedEventArgs e) {
            using (var db = new MusicStoreDBEntities()) {
                switch (window.comboBoxTableChoose.Text) {
                    case "Песни":
                        var song = new Song() {
                            song_title = textBoxNames[0].Text,
                            song_duration = TimeSpan.Parse(textBoxNames[1].Text)
                        };
                        db.Songs.Add(song);
                        db.SaveChanges(); Close();
                        break;
                    case "Альбомы":
                        var album = new Album() {
                            album_name = textBoxNames[0].Text,
                            album_year = DateTime.Parse(textBoxNames[1].Text),
                            id_artist = int.Parse(textBoxNames[2].Text),
                            id_album_songs = int.Parse(textBoxNames[3].Text)
                        };
                        db.Albums.Add(album);
                        db.SaveChanges(); Close();
                        break;
                }
            }
        }       
        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
        static public int SizeOfArray() {
            int size = 0;
            if (window.comboBoxTableChoose.Text == "Песни") {
                size = 2;
            } else if (window.comboBoxTableChoose.Text == "Альбомы") {
                size = 4;
            }
            return size;
        }
    }
}