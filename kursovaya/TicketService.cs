using System;
using System.IO;
using kursovaya.Helpers;

public class PdfTicketService : IPdfTicketService
{
    private const string TicketFolderPath = @"D:\vsprojects\kursaAAch\Tickets";

    public string GenerateTicket(TicketInfo ticketInfo)
    {
        // Создаем директорию для билетов, если она не существует
        Directory.CreateDirectory(TicketFolderPath);

        // Генерируем уникальное имя файла
        string fileName = $"Ticket_{ticketInfo.UserName}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
        string filePath = Path.Combine(TicketFolderPath, fileName);

        try
        {
            // Используем статический метод TicketGenerator для создания PDF
            TicketGenerator.GenerateTicket(
                filePath,
                ticketInfo.UserName,
                ticketInfo.UserEmail,
                ticketInfo.TourName,
                ticketInfo.Destination,
                ticketInfo.TourStartDate,
                
                ticketInfo.TourStartDate.AddDays(7),
                ticketInfo.NumberOfSeats,
                ticketInfo.TotalPrice
            );

            return filePath;
        }
        catch (Exception ex)
        {
            // В реальном приложении лучше использовать logging
            throw new Exception($"Ошибка при создании PDF-билета: {ex.Message}", ex);
        }
    }
}