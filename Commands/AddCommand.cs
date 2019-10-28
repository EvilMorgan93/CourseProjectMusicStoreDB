using MusicStoreDB_App.Data;
using MusicStoreDB_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace MusicStoreDB_App.Commands {
    public class AddCommand :  ICommand
    {
        private readonly dynamic currentViewModel;

        public AddCommand(BaseViewModel currentViewModel) {
            this.currentViewModel = currentViewModel;
        }

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            if (currentViewModel.ButtonAddContent == "Добавить") {
                switch (currentViewModel.Name) {
                    case "Композиции":
                        var song = new Song();
                        currentViewModel.SelectedSongItem = song;
                        break;
                    case "Альбомы":
                        var album = new Album() {
                            album_year = DateTime.Now
                        };
                        currentViewModel.SelectedAlbumItem = album;
                        break;
                    case "Альбомные композиции":
                        var albumSong = new Album_Songs();
                        currentViewModel.SelectedAlbumSongItem = albumSong;
                        currentViewModel.SelectedAlbumItem = currentViewModel.Album.View.CurrentItem as Album;
                        currentViewModel.SelectedSongItem = currentViewModel.Song.View.CurrentItem as Song;
                        break;
                }
                currentViewModel.ButtonAddContent = "Отмена";
            } else {
                currentViewModel.ButtonAddContent = "Добавить";
            }
        }
    }
}