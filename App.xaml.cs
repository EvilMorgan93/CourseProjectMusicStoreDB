using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using MusicStoreDB_App.Views;
using Prism.Commands;
using Prism.Ioc;
using Prism.Modularity;

namespace MusicStoreDB_App {

    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry) {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainModule>();
        }
    }
}
