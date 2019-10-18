using MusicStoreDB_App.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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