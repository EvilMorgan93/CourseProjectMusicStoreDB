using MusicStoreDB_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MusicStoreDB_App.Commands {
    public class ExportCommand : ICommand {
        private PurchaseViewModel purchaseViewModel;
        private AlbumSongsViewModel albumSongsViewModel;

        public ExportCommand(PurchaseViewModel purchaseViewModel) {
            this.purchaseViewModel = purchaseViewModel;
        }
        public ExportCommand(AlbumSongsViewModel albumSongsViewModel) {
            this.albumSongsViewModel = albumSongsViewModel;
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            if (purchaseViewModel != null) {
                purchaseViewModel.ExportPucrhasesToPDF();
            }
            if (albumSongsViewModel != null) {
                albumSongsViewModel.ExportAlbumSongsToPDF();
            }
        }

    }
}
