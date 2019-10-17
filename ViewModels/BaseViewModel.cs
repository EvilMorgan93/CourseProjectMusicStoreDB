using MusicStoreDB_App.Commands;
using MusicStoreDB_App.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicStoreDB_App.ViewModels {
    public abstract class BaseViewModel : INotifyPropertyChanged {
        public SaveCommand SaveEvent { get; set; }
        public AddCommand AddEvent { get; set; }
        public ExportPurchasesCommand ExportEvent { get; set; }
        public RefreshCommand RefreshEvent { get; set; }
        public DeleteCommand DeleteEvent { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string property) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        private string buttonAddContent;
        public string ButtonAddContent {
            get {
                return buttonAddContent;
            }
            set {
                buttonAddContent = value;
                OnPropertyChanged("ButtonAddContent");
            }
        }
    }
}