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
            if (baseViewModel.ButtonAddContent == "Добавить") {
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
                        baseViewModel.SelectedPurchaseItem = purchase;
                        baseViewModel.SelectedAlbumItem = baseViewModel.Album.View.CurrentItem as Album;
                        baseViewModel.SelectedEmployeeItem = baseViewModel.Employee.View.CurrentItem as Employee;
                        break;
                    case "Альбомные композиции":
                        var albumSong = new Album_Songs();
                        baseViewModel.SelectedAlbumSongItem = albumSong;
                        baseViewModel.SelectedAlbumItem = baseViewModel.Album.View.CurrentItem as Album;
                        baseViewModel.SelectedSongItem = baseViewModel.Song.View.CurrentItem as Song;
                        break;
                }
                baseViewModel.ButtonAddContent = "Отмена";
            } else {
                baseViewModel.ButtonAddContent = "Добавить";
            }
        }
    }
}