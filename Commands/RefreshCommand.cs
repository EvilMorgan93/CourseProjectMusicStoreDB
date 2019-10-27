using MusicStoreDB_App.ViewModels;
using System;
using System.Windows.Input;

namespace MusicStoreDB_App.Commands {
    public class RefreshCommand : ICommand {
        private readonly dynamic baseViewModel;

        public RefreshCommand(BaseViewModel baseViewModel) {
            this.baseViewModel = baseViewModel;
        }

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            switch (baseViewModel.Name) {
                case "Композиции":
                    baseViewModel.RefreshData();
                    break;
                case "Альбомы":
                    baseViewModel.RefreshData();
                    break;
                case "Продажи":
                    baseViewModel.RefreshData();
                    break;
                case "Альбомные композиции":
                    baseViewModel.RefreshData();
                    break;
            }
        }
    }
}