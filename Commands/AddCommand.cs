using MusicStoreDB_App.Data;
using MusicStoreDB_App.ViewModels;
using System;
using System.Windows.Input;

namespace MusicStoreDB_App.Commands {
    public class AddCommand : ICommand {    
        private readonly dynamic baseViewModel;

        public AddCommand(BaseViewModel baseViewModel) {
            this.baseViewModel = baseViewModel;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            if (baseViewModel.ButtonAddContent == "Add") {
                switch (baseViewModel.Name) {
                    case "Композиции":
                        var song = new Song();
                        baseViewModel.SelectedItem = song;
                        break;
                    case "Альбомы":
                        var album = new Album() {
                            album_year = DateTime.Now
                        };
                        baseViewModel.SelectedItem = album;
                        break;
                    case "Продажи":
                        var purchase = new Purchase {
                            purchase_date = DateTime.Now
                        };
                        baseViewModel.SelectedItem = purchase;
                        break;
                    case "Альбомные композиции":
                        var albumSong = new Album_Songs();
                        baseViewModel.SelectedItem = albumSong;
                        break;
                }
                baseViewModel.ButtonAddContent = "Cancel";
            } else {
                switch (baseViewModel.Name) {
                    case "Композиции":
                        baseViewModel.SelectedItem = baseViewModel.ListSongs.View.CurrentItem as Song;
                        break;
                    case "Альбомы":
                        baseViewModel.SelectedItem = baseViewModel.Albums.View.CurrentItem as Album;
                        break;
                    case "Продажи":
                        baseViewModel.SelectedItem = baseViewModel.Purchase.View.CurrentItem as Purchase;
                        break;
                    case "Альбомные композиции":
                        baseViewModel.SelectedItem = baseViewModel.AlbumSongs.View.CurrentItem as Album_Songs;
                        break;
                }
            }
        }
    }
}