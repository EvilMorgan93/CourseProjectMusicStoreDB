using MusicStoreDB_App.ViewModels;
using System;
using System.Windows.Input;

namespace MusicStoreDB_App.Commands {
    public class ExportCommand : ICommand {
        private readonly PurchaseViewModel purchaseViewModel;
        private readonly AlbumSongsViewModel albumSongsViewModel;

        public ExportCommand(PurchaseViewModel purchaseViewModel) {
            this.purchaseViewModel = purchaseViewModel;
        }
        public ExportCommand(AlbumSongsViewModel albumSongsViewModel) {
            this.albumSongsViewModel = albumSongsViewModel;
        }

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter)
        {
            purchaseViewModel?.ExportPurchasesToPdf();
            albumSongsViewModel?.ExportAlbumSongsToPdf();
        }
    }
}
