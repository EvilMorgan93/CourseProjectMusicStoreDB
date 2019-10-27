using MusicStoreDB_App.Commands;
using Prism.Mvvm;

namespace MusicStoreDB_App.ViewModels {
    public abstract class BaseViewModel : BindableBase {
        public SaveCommand SaveEvent { get; set; }
        public AddCommand AddEvent { get; set; }
        public ExportCommand ExportEvent { get; set; }
        public RefreshCommand RefreshEvent { get; set; }
        public DeleteCommand DeleteEvent { get; set; }

        protected BaseViewModel() {
            ButtonAddContent = "Добавить";
        }
        private string buttonAddContent;
        public string ButtonAddContent {
            get => buttonAddContent;
            set => SetProperty(ref buttonAddContent, value);
        }
    }
}