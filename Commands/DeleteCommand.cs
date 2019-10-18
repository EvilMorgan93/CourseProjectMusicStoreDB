using MusicStoreDB_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicStoreDB_App.Commands {
    public class DeleteCommand : ICommand {
        private SongViewModel songViewModel;
        private AlbumViewModel albumViewModel;
        private PurchaseViewModel purchaseViewModel;

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
        }
    }
}
