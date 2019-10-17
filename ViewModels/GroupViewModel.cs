using MusicStoreDB_App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MusicStoreDB_App.ViewModels {
    public class GroupViewModel : BaseViewModel{
        public CollectionViewSource Group { get; private set; }
        private Group selectedItem;
        public Group SelectedItem {
            get { return selectedItem; }
            set {
                selectedItem = value;
                OnPropertyChanged("SelectedItem");
            }
        }

        public GroupViewModel() {
            Group = new CollectionViewSource();
        }
    }
}
