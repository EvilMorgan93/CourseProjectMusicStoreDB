using MusicStoreDB_App.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MusicStoreDB_App.ViewModels {
    public abstract class BaseViewModel : INotifyPropertyChanged {
        public SaveCommand SaveEvent { get; set; }
        public AddCommand AddEvent { get; set; }
        public ExportCommand ExportEvent { get; set; }
        public RefreshCommand RefreshEvent { get; set; }
        public DeleteCommand DeleteEvent { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string property="") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public BaseViewModel() {
            ButtonAddContent = "Добавить";
        }
        private string buttonAddContent;
        public string ButtonAddContent {
            get => buttonAddContent;
            set {
                buttonAddContent = value;
                OnPropertyChanged("ButtonAddContent");
            }
        }
    }
}