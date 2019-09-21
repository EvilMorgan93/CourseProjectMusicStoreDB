using MusicStoreDB_App.data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;

namespace MusicStoreDB_App
{
    public partial class MainWindow : Window
    {
        private Song song = new Song();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Refresh_Songs_List_View()
        {
            using (var db = new MusicStoreDBEntities())
            {
                list_view.ItemsSource = db.Songs.ToList();
                list_view.DataContext = this;
            }
        }

        //Нажатие кнопки "Обновить"
        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            Refresh_Songs_List_View();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new MusicStoreDBEntities())
            {
                Song song = new Song()
                {                  
                    song_title = text_box.Text,
                    song_duration = TimeSpan.Parse(text_box1.Text)
                };
                db.Songs.Add(song);
                db.SaveChanges();
            }
            Refresh_Songs_List_View();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new MusicStoreDBEntities())
            {
                if (list_view.SelectedIndex == -1)
                    MessageBox.Show("Выберите строку, которую хотите удалить", "Ошибка", MessageBoxButton.OK);
                else
                {                   
                    song = list_view.SelectedItem as Song;
                    db.Entry(song).State = EntityState.Deleted;
                    db.SaveChanges();
                    Refresh_Songs_List_View();
                }
            }           
        }

    }
}