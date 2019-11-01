using iTextSharp.text;
using iTextSharp.text.pdf;
using MusicStoreDB_App.Commands;
using MusicStoreDB_App.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Prism.Commands;

namespace MusicStoreDB_App.ViewModels {
    public class PurchaseViewModel : BaseViewModel {
        public CollectionViewSource Purchase { get; }
        public CollectionViewSource Album { get; }
        public CollectionViewSource Employee { get; }
        public ICommand AddPurchaseEvent { get; set; }
        private List<Purchase> purchases;
        private long nextUniquePurchaseNumber;
        private DateTime timeNow;

        public string Name => "Продажи";
        private Purchase selectedPurchaseItem;
        public Purchase SelectedPurchaseItem {
            get => selectedPurchaseItem;
            set {
                SetProperty(ref selectedPurchaseItem, value);
                if (selectedPurchaseItem == null) {
                    SelectedAlbumItem = Album.View.CurrentItem as Album;
                    SelectedEmployeeItem = Employee.View.CurrentItem as Employee;
                } else {
                    SelectedAlbumItem = selectedPurchaseItem.Album;
                    SelectedEmployeeItem = selectedPurchaseItem.Employee;
                }
            }
        }
        private Album selectedAlbumItem;
        public Album SelectedAlbumItem {
            get => selectedAlbumItem;
            set => SetProperty(ref selectedAlbumItem, value);
        }
        private Employee selectedEmployeeItem;
        public Employee SelectedEmployeeItem {
            get => selectedEmployeeItem;
            set => SetProperty(ref selectedEmployeeItem, value);
        }
        private string filterString;
        public string FilterString {
            get => filterString;
            set {
                SetProperty(ref filterString, value);
                Purchase.View.Refresh();
            }
        }
        private string buttonStartPurchaseContent;
        public string ButtonStartPurchaseContent {
            get => buttonStartPurchaseContent;
            set => SetProperty(ref buttonStartPurchaseContent, value);
        }

        public PurchaseViewModel() {
            Purchase = new CollectionViewSource();
            Album = new CollectionViewSource();
            Employee = new CollectionViewSource();
            ButtonStartPurchaseContent = "Начать заказ";
            ButtonAddContent = "Добавить";
            RefreshData();
            PurchaseEvent = new DelegateCommand(Execute, () => true);
            AddPurchaseEvent = new DelegateCommand(AddCommandExecute, () => true);
            EditEvent = new EditCommand(this);
            RefreshEvent = new RefreshCommand(this);
            DeleteEvent = new DeleteCommand(this);
            ExportEvent = new ExportCommand(this);
        }
        private void AddCommandExecute() {
            if (ButtonStartPurchaseContent != "Сохранить") return;
            try {
                SelectedPurchaseItem.id_album = SelectedAlbumItem.id_album;
                SelectedPurchaseItem.id_employee = SelectedEmployeeItem.id_employee;
                SelectedPurchaseItem.purchase_number = nextUniquePurchaseNumber;
                SelectedPurchaseItem.purchase_date = timeNow;
                purchases.Add(selectedPurchaseItem);
                RestartItemPosition();
                MessageBox.Show($"Альбом добавлен в заказ\nВсего в заказе: {purchases.Count}", "Заказ",
                    MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            catch (Exception) {
                MessageBox.Show("Необходимо нажать 'Начать заказ'");
            }
        }

        private void Execute() {
            if (buttonStartPurchaseContent == "Сохранить") {
                if (purchases.Count == 0) {
                    ButtonStartPurchaseContent = "Начать заказ";
                    return;
                }
                AddPurchaseData();
                MessageBox.Show($"Добавлено {purchases.Count} в итоговый заказ\nНомер заказа: {nextUniquePurchaseNumber}\nСумма заказа: {TotalPriceQuery()}");
                ButtonStartPurchaseContent = "Начать заказ";
            } else {
                RestartItemPosition();
                nextUniquePurchaseNumber = GenerateUniquePurchaseNumber();
                purchases = new List<Purchase>();
                timeNow = DateTime.Now;
                ButtonStartPurchaseContent = "Сохранить";
            }
        }

        private int TotalPriceQuery() {
            using (var dbContext = new MusicStoreDBEntities()) {
                var totalPrice = (from a in dbContext.Albums
                                  join p in dbContext.Purchases on a.id_album equals p.id_album
                                  join pr in dbContext.Price_List on a.id_price equals pr.id_price
                                  select new {
                                      p.purchase_number,
                                      pr.purchase_price,
                                      p.purchase_amount
                                  }).Where(p => p.purchase_number == nextUniquePurchaseNumber)
                    .Sum(pr => pr.purchase_price * pr.purchase_amount);
                return totalPrice;
            }
        }

        private void RestartItemPosition() {
            var purchase = new Purchase();
            SelectedPurchaseItem = purchase;
        }

        public bool Filter(object obj) {
            if (!(obj is Purchase data)) return false;
            if (!string.IsNullOrEmpty(filterString)) {
                return data.Album.album_name.Contains(filterString) || data.purchase_date.ToString(CultureInfo.CurrentCulture).Contains(filterString);
            }
            return true;
        }
        public void ExportPurchasesToPdf() {
            try {
                var document = new Document();
                var writer = PdfWriter.GetInstance(document, new FileStream("Отчёт по продажам.pdf", FileMode.Create));
                document.Open();
                using (var dbContext = new MusicStoreDBEntities()) {
                    var ttf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF");
                    var baseFont = BaseFont.CreateFont(ttf, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    var font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
                    string[] nameColumns = {
                        "№ заказа",
                        "Имя продавца",
                        "Название альбома",
                        "Название группы",
                        "Количество копий",
                        "Цена",
                        "Дата покупки",
                        "Сумма заказа"
                    };
                    var table = new PdfPTable(nameColumns.Length) {
                        WidthPercentage = 100
                    };
                    var cell = new PdfPCell(new Phrase("Отчёт по продажам", font)) {
                        Colspan = nameColumns.Length,
                        HorizontalAlignment = 1,
                        Border = 0,
                        PaddingBottom = 10
                    };
                    table.AddCell(cell);
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    var query = (from a in dbContext.Albums
                                 join g in dbContext.Groups on a.id_artist equals g.id_artist
                                 join p in dbContext.Purchases on a.id_album equals p.id_album
                                 join pr in dbContext.Price_List on a.id_price equals pr.id_price
                                 join emp in dbContext.Employees.AsParallel() on p.id_employee equals emp.id_employee orderby p.purchase_date
                                 select new {
                                     emp.employee_name,
                                     p.purchase_number,
                                     a.album_name,
                                     g.group_name,
                                     p.purchase_date,
                                     pr.purchase_price,
                                     p.purchase_amount
                                 }).ToList();
                    sw.Stop();
                    var kek = sw.ElapsedMilliseconds;
                    var totalPrice = query
                        .GroupBy(x => new {
                            x.purchase_number,
                            x.purchase_date
                        })
                        .Select(x => new {
                            x.Key.purchase_number,
                            x.Key.purchase_date,
                            purchase_prise = x.Sum(z => z.purchase_amount * z.purchase_price)
                        }).ToArray();

                    foreach (var t in nameColumns)
                    {
                        cell = new PdfPCell(new Phrase(t, font)) {
                            BackgroundColor = BaseColor.LIGHT_GRAY,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            Padding = 3
                        };
                        table.AddCell(cell);
                    }
                    var isOnlyOne = true;
                    for (int i = 0, j = 0; i < query.Count; i++) {
                        table.AddCell(new PdfPCell(new Phrase(query[i].purchase_number.ToString())) {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BackgroundColor = BaseColor.LIGHT_GRAY
                        });
                        table.AddCell(new PdfPCell(new Phrase(query[i].employee_name, font)) {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        table.AddCell(new PdfPCell(new Phrase(query[i].album_name, font)) {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        table.AddCell(new PdfPCell(new Phrase(query[i].group_name, font)) {
                            HorizontalAlignment = Element.ALIGN_CENTER
                        });
                        table.AddCell(new PdfPCell(new Phrase(query[i].purchase_amount.ToString(), font)) {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                        });
                        table.AddCell(new PdfPCell(new Phrase((query[i].purchase_price * query[i].purchase_amount).ToString(), font)) {
                            HorizontalAlignment = Element.ALIGN_CENTER,
                        });
                        if (query[i].purchase_number == totalPrice[j].purchase_number && isOnlyOne) {
                            table.AddCell(new PdfPCell(new Phrase(totalPrice[j].purchase_date.ToString(CultureInfo.InvariantCulture), font)) {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                BackgroundColor = BaseColor.RED
                            });
                            table.AddCell(new PdfPCell(new Phrase(totalPrice[j].purchase_prise.ToString(), font)) {
                                HorizontalAlignment = Element.ALIGN_CENTER,
                                BackgroundColor = BaseColor.RED
                            });
                            isOnlyOne = false;
                            if (totalPrice.Length - 1 != j) {
                                j++;
                                isOnlyOne = true;
                            }
                        } else {
                            table.AddCell(new PdfPCell());
                            table.AddCell(new PdfPCell());
                        }
                    }

                    document.Add(table);
                }
                document.Close();
                writer.Close();
                MessageBox.Show("Отчёт сформирован!", "Информация об отчёте", MessageBoxButton.OK, MessageBoxImage.Information);
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Информация об отчёте", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        public void RefreshData() {
            using (var dbContext = new MusicStoreDBEntities()) {
                Purchase.Source = dbContext.Purchases
                    .Include(a => a.Album)
                    .Include(emp => emp.Employee)
                    .ToList();
                Employee.Source = dbContext.Employees.ToList();
                Album.Source = dbContext.Albums.ToList();
                Purchase.View.Filter = Filter;
            }
        }

        private long GenerateUniquePurchaseNumber() {
            using (var dbContext = new MusicStoreDBEntities()) {
                var numbers = dbContext.Purchases
                    .Select(num => num.purchase_number)
                    .ToList();
                nextUniquePurchaseNumber = numbers.Count;
                for (int i = 0; i < numbers.Count; i++) {
                    if (nextUniquePurchaseNumber == numbers[i]) {
                        nextUniquePurchaseNumber++;
                        i = 0;
                    }
                }
            }
            return nextUniquePurchaseNumber;
        }

        public void AddPurchaseData() {
            try {
                using (var dbContext = new MusicStoreDBEntities()) {
                    dbContext.Purchases.AddRange(purchases);
                    dbContext.SaveChanges();
                }
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public void EditPurchaseData() {
            try {
                using (var dbContext = new MusicStoreDBEntities()) {
                    dbContext.Albums.Attach(SelectedAlbumItem);
                    dbContext.Employees.Attach(SelectedEmployeeItem);
                    SelectedPurchaseItem.id_album = SelectedAlbumItem.id_album;
                    SelectedPurchaseItem.id_employee = SelectedEmployeeItem.id_employee;
                    dbContext.Entry(SelectedPurchaseItem).State = EntityState.Modified;
                    dbContext.SaveChanges();
                }
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        public void DeletePurchaseData() {
            try {
                using (var dbContext = new MusicStoreDBEntities()) {
                    dbContext.Entry(SelectedPurchaseItem).State = EntityState.Deleted;
                    dbContext.SaveChanges();
                }
                RefreshData();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
    }
}