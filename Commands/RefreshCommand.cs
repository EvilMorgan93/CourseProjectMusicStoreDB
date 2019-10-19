using MusicStoreDB_App.ViewModels;
using System;
using System.Windows.Input;

namespace MusicStoreDB_App.Commands {
    public class RefreshCommand : ICommand {
        private SongViewModel songViewModel;
        private AlbumViewModel albumViewModel;
        private PurchaseViewModel purchaseViewModel;
        private AlbumSongsViewModel albumSongsViewModel;

        public RefreshCommand(AlbumSongsViewModel albumSongsViewModel) {
            this.albumSongsViewModel = albumSongsViewModel;
        }

        public RefreshCommand(SongViewModel songViewModel) {
            this.songViewModel = songViewModel;
        }
        public RefreshCommand(AlbumViewModel albumViewModel) {
            this.albumViewModel = albumViewModel;
        }
        public RefreshCommand(PurchaseViewModel purchaseViewModel) {
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
                songViewModel.RefreshData();
            } 
            if (albumViewModel != null) { 
                albumViewModel.RefreshData(); 
            } 
            if (purchaseViewModel != null) {
                purchaseViewModel.RefreshData();
            }
            if (albumSongsViewModel != null) {
                albumSongsViewModel.RefreshData();
            }
        }
    }
}
