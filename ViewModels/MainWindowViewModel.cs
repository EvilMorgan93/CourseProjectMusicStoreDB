using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace MusicStoreDB_App.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        public DelegateCommand<string> NavigateCommand { get; set; }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string uri) {
            regionManager.RequestNavigate("ContentRegion", uri);
        }
    }
}