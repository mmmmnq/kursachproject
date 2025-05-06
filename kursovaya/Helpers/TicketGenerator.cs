using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;

namespace kursovaya.Helpers
{
   

    public class TicketGenerator
    {
        public static void GenerateTicket(string filePath, string userName, string userEmail, string tourName,
            string destination, DateTime startDate, DateTime endDate, int seats, decimal totalPrice)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));

                    // Шапка
                    page.Header().Height(50).Background(Colors.Grey.Lighten3).AlignMiddle().Text("Ваш билет").FontSize(18).Bold().AlignCenter();

                    // Основной контент
                    page.Content().PaddingVertical(10).Column(content =>
                    {
                        // Информация о пользователе
                        content.Item().Text($"Пассажир: {userName}").FontSize(14).Bold();
                        content.Item().Text($"Email: {userEmail}").FontSize(12);

                        content.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        // Информация о туре
                        content.Item().Text($"Тур: {tourName}").FontSize(14).Bold();
                        content.Item().Text($"Место назначения: {destination}").FontSize(12);
                        content.Item().Text($"Даты: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}").FontSize(12);

                        content.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);

                        // Детали бронирования
                        content.Item().Text($"Количество мест: {seats}").FontSize(12);
                        content.Item().Text($"Итоговая стоимость: {totalPrice:C}").FontSize(12);

                        content.Item().PaddingTop(20).AlignCenter().Text("Спасибо за выбор нашего турагентства!").FontSize(14).Bold().Italic();
                    });

                    // Нижний колонтитул
                    page.Footer().AlignCenter().Text("Турагентство 'Лучшие путешествия' - Все права защищены").FontSize(10).Italic();
                });
            });

            // Сохранение PDF
            document.GeneratePdf(filePath);
        }
    }
}