using MusicStoreDB_App.ViewModels;
using System;
using System.Windows.Input;

namespace MusicStoreDB_App.Commands {
    public class DeleteCommand : ICommand {
        private SongViewModel songViewModel;
        private AlbumViewModel albumViewModel;
        private PurchaseViewModel purchaseViewModel;
        private AlbumSongsViewModel albumSongsViewModel;

        public DeleteCommand(AlbumSongsViewModel albumSongsViewModel) {
            this.albumSongsViewModel = albumSongsViewModel;
        }

        public DeleteCommand(SongViewModel songViewModel) {
            this.songViewModel = songViewModel;
        }
        public DeleteCommand(AlbumViewModel albumViewModel) {
            this.albumViewModel = albumViewModel;
        }
        public DeleteCommand(PurchaseViewModel purchaseViewModel) {
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
                songViewModel.DeleteSongData();
            }
            if (albumViewModel != null) { 
                albumViewModel.DeleteAlbumData(); 
            }
            if (purchaseViewModel != null) {
                purchaseViewModel.DeletePurchaseData();
            }
            if (albumSongsViewModel != null) {
                albumSongsViewModel.DeleteAlbumSongData();
            }
        }
    }
}
