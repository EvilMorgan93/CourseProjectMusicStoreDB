using MusicStoreDB_App.ViewModels;
using System;
using System.Windows.Input;

namespace MusicStoreDB_App.Commands {
    public class SaveCommand : ICommand {
        private readonly dynamic baseViewModel;

        public SaveCommand(BaseViewModel baseViewModel) {
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
                    baseViewModel.SaveChanges();
                    break;
                case "Альбомы":
                    baseViewModel.SaveChanges();
                    break;
                case "Альбомные композиции":
                    baseViewModel.SaveChanges();
                    break;
            }
        }
    }
}