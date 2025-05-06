using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.Windows;

namespace kursovaya.ViewModels
{
    public class ReportViewModel : INotifyPropertyChanged
    {
        private DateTime _startDate = DateTime.Today.AddMonths(-1);
        private DateTime _endDate = DateTime.Today;
        private string _selectedReportType;
        private ObservableCollection<object> _reportData;
        private readonly IEnumerable<BookingDTO> _bookings;
        private readonly IEnumerable<TourDTO> _tours;
        private ICommand _generateReportCommand;
        private ICommand _exportToPdfCommand;
        private string _savePath;
        private ICommand _browseCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public class BookingReportItem
        {
            public DateTime BookingDate { get; set; }
            public string TourName { get; set; }
            public string Destination { get; set; }
            public int NumberOfSeats { get; set; }
            public decimal TotalPrice { get; set; }
        }

        public class FinancialReportItem
        {
            public DateTime Date { get; set; }
            public int NumberOfBookings { get; set; }
            public decimal TotalRevenue { get; set; }
            public decimal AverageBookingValue { get; set; }
        }

        public ReportViewModel(IEnumerable<BookingDTO> bookings, IEnumerable<TourDTO> tours)
        {
            _bookings = bookings;
            _tours = tours;
            ReportData = new ObservableCollection<object>();
            GenerateReportCommand = new Helpers.RelayCommand(_ => GenerateReport());
            ExportToPdfCommand = new Helpers.RelayCommand(_ => ExportToPdf());
            BrowseCommand = new Helpers.RelayCommand(_ => BrowseForSavePath());
            SelectedReportType = "Отчет по бронированиям за период";
            SavePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");
            GenerateReport();
        }
        private void BrowseForSavePath()
        {
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "PDF файлы (*.pdf)|*.pdf",
                DefaultExt = ".pdf",
                FileName = $"Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
            };

            if (dialog.ShowDialog() == true)
            {
                SavePath = dialog.FileName;
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); }
        }
        public string SavePath
        {
            get => _savePath;
            set { _savePath = value; OnPropertyChanged(); }
        }

        public ICommand BrowseCommand
        {
            get => _browseCommand;
            set { _browseCommand = value; OnPropertyChanged(); }
        }
        public string SelectedReportType
        {
            get => _selectedReportType;
            set { _selectedReportType = value; OnPropertyChanged(); }
        }

        public ObservableCollection<object> ReportData
        {
            get => _reportData;
            set { _reportData = value; OnPropertyChanged(); }
        }

        public ICommand GenerateReportCommand
        {
            get => _generateReportCommand;
            set { _generateReportCommand = value; OnPropertyChanged(); }
        }

        public ICommand ExportToPdfCommand
        {
            get => _exportToPdfCommand;
            set { _exportToPdfCommand = value; OnPropertyChanged(); }
        }

        private void GenerateReport()
        {
            if (string.IsNullOrEmpty(SelectedReportType))
                return;

            ReportData.Clear();

            try
            {
                switch (SelectedReportType)
                {
                    case "Отчет по бронированиям за период":
                        GenerateBookingReport();
                        break;
                    case "Финансовый отчет":
                        GenerateFinancialReport();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при генерации отчета: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void GenerateBookingReport()
        {
            var bookingsInPeriod = _bookings
                .Where(b => b.BookingDate >= StartDate && b.BookingDate <= EndDate)
                .Join(_tours,
                    b => b.TourId,
                    t => t.TourId,
                    (b, t) => new BookingReportItem
                    {
                        BookingDate = b.BookingDate,
                        TourName = t.Name,
                        Destination = t.Destination,
                        NumberOfSeats = b.NumberOfSeats,
                        TotalPrice = b.TotalPrice
                    })
                .OrderBy(b => b.BookingDate)
                .ToList();

            foreach (var booking in bookingsInPeriod)
                ReportData.Add(booking);

            if (!bookingsInPeriod.Any())
            {
                MessageBox.Show("Нет данных за выбранный период", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void GenerateFinancialReport()
        {
            var financialData = _bookings
                .Where(b => b.BookingDate >= StartDate && b.BookingDate <= EndDate)
                .GroupBy(b => b.BookingDate.Date)
                .Select(g => new FinancialReportItem
                {
                    Date = g.Key,
                    NumberOfBookings = g.Count(),
                    TotalRevenue = g.Sum(b => b.TotalPrice),
                    AverageBookingValue = g.Average(b => b.TotalPrice)
                })
                .OrderBy(f => f.Date)
                .ToList();

            foreach (var data in financialData)
                ReportData.Add(data);

            if (!financialData.Any())
            {
                MessageBox.Show("Нет данных за выбранный период", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ExportToPdf()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SavePath))
                {
                    MessageBox.Show("Выберите путь для сохранения отчета", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                QuestPDF.Settings.License = LicenseType.Community;

                var document = Document.Create(container =>
                {
                    container.Page(page =>  
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(2, Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                        page.Header().Element(header =>
                        {
                            PdfHelper.AddText(
                                header.Height(50).Background(Colors.Grey.Lighten3).AlignMiddle(),
                                $"{SelectedReportType} ({StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy})",
                                18, true, true
                            );
                        });

                        page.Content().PaddingVertical(10).Element(content =>
                        {
                            if (SelectedReportType == "Отчет по бронированиям за период")
                                GenerateBookingReportContent(content);
                            else
                                GenerateFinancialReportContent(content);
                        });

                        page.Footer().Element(footer =>
                        {
                            PdfHelper.AddText(
                                footer.AlignCenter(),
                                $"Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm}",
                                10
                            );
                        });
                    });
                });

                document.GeneratePdf(SavePath);
                MessageBox.Show($"Отчет сохранен:\n{SavePath}", "Экспорт завершен",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при экспорте в PDF: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GenerateBookingReportContent(QuestPDF.Infrastructure.IContainer container)
        {
            var bookings = ReportData.Cast<BookingReportItem>().ToList();

            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    PdfHelper.AddText(row.RelativeItem(), "Дата", isBold: true);
                    PdfHelper.AddText(row.RelativeItem(), "Тур", isBold: true);
                    PdfHelper.AddText(row.RelativeItem(), "Направление", isBold: true);
                    PdfHelper.AddText(row.RelativeItem(), "Кол-во мест", isBold: true);
                    PdfHelper.AddText(row.RelativeItem(), "Сумма", isBold: true);
                });

                column.Item().LineHorizontal(1);

                foreach (var booking in bookings)
                {
                    column.Item().Row(row =>
                    {
                        PdfHelper.AddText(row.RelativeItem(), booking.BookingDate.ToString("dd.MM.yyyy"));
                        PdfHelper.AddText(row.RelativeItem(), booking.TourName);
                        PdfHelper.AddText(row.RelativeItem(), booking.Destination);
                        PdfHelper.AddText(row.RelativeItem(), booking.NumberOfSeats.ToString());
                        PdfHelper.AddText(row.RelativeItem(), $"{booking.TotalPrice:C}");
                    });
                }

                column.Item().LineHorizontal(1);

                var totalPrice = bookings.Sum(x => x.TotalPrice);
                PdfHelper.AddText(column.Item(), $"Всего бронирований: {bookings.Count}", isBold: true);
                PdfHelper.AddText(column.Item(), $"Общая сумма: {totalPrice:C}", isBold: true);
            });
        }

        private void GenerateFinancialReportContent(QuestPDF.Infrastructure.IContainer container)
        {
            var financialData = ReportData.Cast<FinancialReportItem>().ToList();

            container.Column(column =>
            {
                column.Item().Row(row =>
                {
                    PdfHelper.AddText(row.RelativeItem(), "Дата", isBold: true);
                    PdfHelper.AddText(row.RelativeItem(), "Кол-во бронирований", isBold: true);
                    PdfHelper.AddText(row.RelativeItem(), "Выручка", isBold: true);
                    PdfHelper.AddText(row.RelativeItem(), "Средний чек", isBold: true);
                });

                column.Item().LineHorizontal(1);

                foreach (var data in financialData)
                {
                    column.Item().Row(row =>
                    {
                        PdfHelper.AddText(row.RelativeItem(), data.Date.ToString("dd.MM.yyyy"));
                        PdfHelper.AddText(row.RelativeItem(), data.NumberOfBookings.ToString());
                        PdfHelper.AddText(row.RelativeItem(), $"{data.TotalRevenue:C}");
                        PdfHelper.AddText(row.RelativeItem(), $"{data.AverageBookingValue:C}");
                    });
                }

                column.Item().LineHorizontal(1);

                var totalRevenue = financialData.Sum(x => x.TotalRevenue);
                PdfHelper.AddText(column.Item(),
                    $"Общая выручка за период: {totalRevenue:C}",
                    isBold: true);
            });
        }
    }

    public static class PdfHelper
    {
        public static void AddText(QuestPDF.Infrastructure.IContainer container, string text,
            int fontSize = 12, bool isBold = false, bool isCenter = false)
        {
            var textContainer = container.Text(text);
            if (fontSize != 12)
                textContainer.FontSize(fontSize);
            if (isBold)
                textContainer.Bold();
            if (isCenter)
                textContainer.AlignCenter();
        }
    }
}
