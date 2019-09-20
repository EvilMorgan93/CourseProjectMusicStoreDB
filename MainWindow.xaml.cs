using MusicStoreDB_App.data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicStoreDB_App
{
    public partial class MainWindow : Window
    {
        string connectionString = @"Data Source=.\DESKTOP-B4CMOIR;Initial Catalog=MusicStoreDB;Integrated Security=True";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            list_view.Items.Clear();
            List<string> songs = new List<string>();
            using(var db = new MusicStoreDBEntities())
            {
                songs = (from sg in db.Songs select sg.song_title).ToList();
            }
            foreach (string str in songs)
                list_view.Items.Add(str);   
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new MusicStoreDBEntities())
            {
                Song song = new Song()
                {
                    id_song = db.Songs.Count() + 2,
                    song_title = text_box.Text,
                    song_duration = TimeSpan.Parse(text_box1.Text)
                };
                db.Songs.Add(song);
                db.SaveChanges();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new MusicStoreDBEntities())
            {
                /* EntityConnection entityConnection = new EntityConnection("name=MusicStoreDBEntities");
                 entityConnection.Open();
                 EntityCommand command = new EntityCommand($"DELETE Songs WHERE id_song = {list_view.SelectedIndex + 2}",entityConnection);
                 command.ExecuteNonQuery();
                 entityConnection.Close();*/
                if (list_view.SelectedIndex == -1)
                    MessageBox.Show("Ошибка!");
                else
                {
                    db.Songs.Remove(db.Songs.Single(a => a.id_song == list_view.SelectedIndex + 2));
                    db.SaveChanges();
                }
            }
        }
    }
}