using System;
using System.Windows.Input;
using MusicStoreDB_App.Views;
using MusicStoreDB_App.Commands;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using Unity;

namespace MusicStoreDB_App {
    public class MainModule : IModule {
        public void OnInitialized(IContainerProvider containerProvider) {
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(SongView));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(PurchaseView));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(AlbumSongsView));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(AlbumView));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry) {
        }

        readonly IRegionManager regionManager;

        public MainModule(RegionManager regionManager) {
            this.regionManager = regionManager;
        }
    }
}