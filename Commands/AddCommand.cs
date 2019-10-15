using MusicStoreDB_App.Data;
using MusicStoreDB_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicStoreDB_App.Commands {
    public class AddCommand : ICommand {    
        private SongViewModel songViewModel;
        private AlbumViewModel albumViewModel;

        public AddCommand(SongViewModel songViewModel) {
            this.songViewModel = songViewModel;
        }
        public AddCommand(AlbumViewModel albumViewModel) {
            this.albumViewModel = albumViewModel;
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
            } else {
                if (albumViewModel.ButtonAddContent == "Добавить") {
                    var album = new Album();
                    albumViewModel.SelectedItem = album;
                    albumViewModel.ButtonAddContent = "Отмена";
                } else {
                    albumViewModel.SelectedItem = albumViewModel.Albums.View.CurrentItem as Album;
                }
            }
        }
    }
}
