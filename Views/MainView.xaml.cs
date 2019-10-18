using System.Windows;

namespace MusicStoreDB_App {
    public partial class MainView : Window {
        public MainView() {
            InitializeComponent();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            Application.Current.Shutdown();
        }
    }
}