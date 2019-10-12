using MusicStoreDB_App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MusicStoreDB_App.ViewModels {
    class MainWindowViewModel {
        public CollectionViewSource Collection { get; private set; }
        private MusicStoreDBEntities dbContext;

        public MainWindowViewModel() {
            Collection = new CollectionViewSource();
            LoadData();
        }

        private void LoadData() {
            Refresh();
        }

        public void Refresh() {
            dbContext = new DatabaseEntities();
            _ctx.Schoolresultaten.Load();
            Collection.Source = _ctx.Schoolresultaten.Local;
            Collection.SortDescriptions.Add(new SortDescription("ID", ListSortDirection.Ascending)); //Orders the datagrid based on ID
        }

    }
}
