using MusicStoreDB_App.Data;
using MusicStoreDB_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MusicStoreDB_App.Views {
    /// <summary>
    /// Логика взаимодействия для AddSongView.xaml
    /// </summary>
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
        private readonly DataTableView window = (DataTableView)Application.Current.MainWindow;
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
                FontSize = 16
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
                    Width = 150,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    TextAlignment = TextAlignment.Center,
                    Margin = new Thickness(40, 20, 5, 5)
                };
                Grid.SetRow(textBlock, i);
                grid.Children.Add(textBlock);
                TextBox textBox = new TextBox() {
                    Width = 300,
                    FontSize = 17,
                    Margin = new Thickness(50, 20, 40, 5),
                    HorizontalAlignment = HorizontalAlignment.Right
                };
                Grid.SetColumn(textBox, 1);
                Grid.SetRow(textBox, i);
                grid.Children.Add(textBox);
            }         
            stackPanel.Children.Add(grid);

            Button acceptButton = new Button() {
                Content = "Добавить",
                Width = 100,
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0,20,0,10)
            };
            Button cancelButton = new Button() {
                Content = "Отмена",
                Width = 100,
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 20, 0, 10)
            };           
            stackPanel.Children.Add(acceptButton);
            stackPanel.Children.Add(cancelButton);
            cancelButton.Click += Cancel_Click;
            acceptButton.Click += Accept_Click;
        }
        private void Accept_Click(object sender, RoutedEventArgs e) {
            //using (var db = new MusicStoreDBEntities()) {
            //    if (textBoxTitle.Text == "" || textBoxDuration.Text == "") {
            //        MessageBox.Show("Введите данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    } else {
            //        var song = new Song() {
            //            song_title = textBoxTitle.Text,
            //            song_duration = TimeSpan.Parse(textBoxDuration.Text)
            //        };
            //        db.Songs.Add(song);
            //        db.SaveChanges();
            //        Close();
            //    }
            //}
        }
        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
