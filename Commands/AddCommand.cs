using MusicStoreDB_App.Data;
using MusicStoreDB_App.ViewModels;
using System;
using System.Windows.Input;

namespace MusicStoreDB_App.Commands {
    public class AddCommand : ICommand {    
        private SongViewModel songViewModel;
        private AlbumViewModel albumViewModel;
        private PurchaseViewModel purchaseViewModel;
        private AlbumSongsViewModel albumSongsViewModel;

        public AddCommand(AlbumSongsViewModel albumSongsViewModel) {
            this.albumSongsViewModel = albumSongsViewModel;
        }

        public AddCommand(SongViewModel songViewModel) {
            this.songViewModel = songViewModel;
        }
        public AddCommand(AlbumViewModel albumViewModel) {
            this.albumViewModel = albumViewModel;
        }
        public AddCommand(PurchaseViewModel purchaseViewModel) {
            this.purchaseViewModel = purchaseViewModel;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            if (songViewModel != null) {
                if (songViewModel.ButtonAddContent == "Добавить") {
                    var song = new Song();
                    songViewModel.SelectedItem = song;
                    songViewModel.ButtonAddContent = "Отмена";
                } else {
                    songViewModel.SelectedItem = songViewModel.ListSongs.View.CurrentItem as Song;
                }
            }
            if (albumViewModel != null) {
                if (albumViewModel.ButtonAddContent == "Добавить") {
                    var album = new Album();
                    albumViewModel.SelectedItem = album;
                    albumViewModel.ButtonAddContent = "Отмена";
                } else {
                    albumViewModel.SelectedItem = albumViewModel.Albums.View.CurrentItem as Album;
                }
            }
            if (purchaseViewModel != null) {
                if (purchaseViewModel.ButtonAddContent == "Добавить") {
                    var purchase = new Purchase {
                        purchase_date = DateTime.Now
                    };
                    purchaseViewModel.SelectedItem = purchase;
                    purchaseViewModel.ButtonAddContent = "Отмена";
                } else {
                    purchaseViewModel.SelectedItem = purchaseViewModel.Purchase.View.CurrentItem as Purchase;
                }
            }
            if (albumSongsViewModel != null) {
                if (albumSongsViewModel.ButtonAddContent == "Добавить") {
                    var albumSong = new Album_Songs();
                    albumSongsViewModel.SelectedItem = albumSong;
                    albumSongsViewModel.ButtonAddContent = "Отмена";
                } else {
                    albumSongsViewModel.SelectedItem = albumSongsViewModel.AlbumSongs.View.CurrentItem as Album_Songs;
                }
            }
        }
    }
}