using MusicStoreDB_App.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace MusicStoreDB_App.ViewModels {
    public class ApplicationViewModel : INotifyPropertyChanged {
        private ICommand changePageCommand;
        private IPageViewModel currentPageViewModel;
        private List<IPageViewModel> pageViewModels;

        public ApplicationViewModel() {
            // Add available pages
            PageViewModels.Add(new SongViewModel());
            PageViewModels.Add(new AlbumSongsViewModel());
            PageViewModels.Add(new AlbumViewModel());
            PageViewModels.Add(new PurchaseViewModel());           

            // Set starting page
            CurrentPageViewModel = PageViewModels[0];
        }

        public ICommand ChangePageCommand {
            get {
                if (changePageCommand == null) {
                    changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);
                }
                return changePageCommand;
            }
        }
        public List<IPageViewModel> PageViewModels {
            get {
                if (pageViewModels == null)
                    pageViewModels = new List<IPageViewModel>();
                return pageViewModels;
            }
        }
        public IPageViewModel CurrentPageViewModel {
            get => currentPageViewModel;
            set {
                if (currentPageViewModel != value) {
                    currentPageViewModel = value;
                    OnPropertyChanged("CurrentPageViewModel");
                }
            }
        }
        protected void OnPropertyChanged(string property) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void ChangeViewModel(IPageViewModel viewModel) {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);
            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }
    }
}
