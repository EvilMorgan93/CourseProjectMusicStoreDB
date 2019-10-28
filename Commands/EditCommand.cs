using System;
using System.Windows.Input;
using MusicStoreDB_App.Data;
using MusicStoreDB_App.ViewModels;

namespace MusicStoreDB_App.Commands
{
    public class EditCommand : ICommand {
        private readonly dynamic currentViewModel;

        public EditCommand(BaseViewModel currentViewModel) {
            this.currentViewModel = currentViewModel;
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
            if (currentViewModel.Name == "Композиции") {
                currentViewModel.EditSongData();
            }
            else if (currentViewModel.Name == "Альбомы") {

            }
            else if (currentViewModel.Name == "Продажи") {
                currentViewModel.EditPurchaseData();
            }
            else if (currentViewModel.Name == "Альбомные композиции")
            {
                currentViewModel.EditAlbumSongsData();
            }
        }
    }
}