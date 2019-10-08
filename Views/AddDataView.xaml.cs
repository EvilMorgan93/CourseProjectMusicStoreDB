using MusicStoreDB_App.Data;
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
        public AddDataView() {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e) {
            using (var db = new MusicStoreDBEntities()) {
                if (textBoxTitle.Text == "" || textBoxDuration.Text == "") {
                    MessageBox.Show("Введите данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                } else {
                    var song = new Song() {
                        song_title = textBoxTitle.Text,
                        song_duration = TimeSpan.Parse(textBoxDuration.Text)
                    };
                    db.Songs.Add(song);
                    db.SaveChanges();
                    textBoxTitle.Text = "";
                    textBoxDuration.Text = "";
                    Close();
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
