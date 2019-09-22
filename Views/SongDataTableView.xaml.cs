using MusicStoreDB_App.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MusicStoreDB_App
{
    public partial class SongDataTableView : Window
    {
        private Song song = new Song();

        public SongDataTableView()
        {
            InitializeComponent();
            comboBoxTableChoose.SelectedIndex = 0;
        }


        public async Task Refresh_Songs_List_View()
        {
            using (var db = new MusicStoreDBEntities())
            {
                songsListView.ItemsSource = await db.Songs.ToListAsync();
            }
        }

        //Нажатие кнопки "Обновить"
        private async void Refresh_ClickAsync(object sender, RoutedEventArgs e)
        {
            await Refresh_Songs_List_View();
        }

        private async void Add_ClickAsync(object sender, RoutedEventArgs e)
        {
            using (var db = new MusicStoreDBEntities())
            {
                if (textBoxTitle.Text == "" || textBoxDuration.Text == "")
                    MessageBox.Show("Введите данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                else
                {
                    var song = new Song()
                    {
                        song_title = textBoxTitle.Text,
                        song_duration = TimeSpan.Parse(textBoxDuration.Text)
                    };
                    db.Songs.Add(song);
                    db.SaveChanges();
                    textBoxTitle.Text = "";
                    textBoxDuration.Text = "";
                    await Refresh_Songs_List_View();
                }
            }          
        }

        private async void Delete_ClickAsync(object sender, RoutedEventArgs e)
        {
            using (var db = new MusicStoreDBEntities())
            {
                if (songsListView.SelectedIndex == -1)
                    return;
                else
                {
                    song = songsListView.SelectedItem as Song;
                    db.Entry(song).State = EntityState.Deleted;
                    db.SaveChanges();
                    await Refresh_Songs_List_View();
                }
            }           
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}