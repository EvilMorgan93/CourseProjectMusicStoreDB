using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MusicStoreDB_App {
    public static class ObservableCollectionExtension {
        public static void AddRange<T>(this ObservableCollection<T> observableCollection, IList<T> list) {
            foreach (var item in list)
                observableCollection.Add(item);
        }
    }
}
