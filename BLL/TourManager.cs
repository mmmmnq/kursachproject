using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using DomainModel;
using DTO;
using BLL.Services;

namespace BLL.Managers
{
    public class TourManager
    {
        private readonly TourService _tourService;

        public TourManager(TourService tourService)
        {
            _tourService = tourService ?? throw new ArgumentNullException(nameof(tourService));
        }

        public async Task<(bool success, string message)> CreateTourAsync(TourDTO tourData)
        {
            try
            {
                // Валидация данных
                var validationResult = ValidateTourData(tourData);
                if (!validationResult.isValid)
                {
                    return (false, validationResult.message);
                }

                // Преобразование в доменную модель
                var tour = new tours
                {
                    name = tourData.Name,
                    description = tourData.Description,
                    price = tourData.Price,
                    startdate = tourData.StartDate,
                    enddate = tourData.EndDate,
                    destination = tourData.Destination,
                    transportid = tourData.TransportId,
                    availableseats = tourData.AvailableSeats,
                    status = tourData.Status
                };

                // Сохранение тура
                var result = await _tourService.AddTourAsync(tourData);
                return result ?
                    (true, "Тур успешно создан") :
                    (false, "Не удалось создать тур");
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка при создании тура: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> UpdateTourAsync(TourDTO tourData)
        {
            try
            {
                var validationResult = ValidateTourData(tourData);
                if (!validationResult.isValid)
                {
                    return (false, validationResult.message);
                }

                var result = await _tourService.UpdateTourAsync(tourData);
                return result ?
                    (true, "Тур успешно обновлен") :
                    (false, "Не удалось обновить тур");
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка при обновлении тура: {ex.Message}");
            }
        }

        public async Task<(bool success, string message)> DeleteTourAsync(int tourId)
        {
            try
            {
                var result = await _tourService.DeleteTourAsync(tourId);
                return result ?
                    (true, "Тур успешно удален") :
                    (false, "Не удалось удалить тур");
            }
            catch (Exception ex)
            {
                return (false, $"Ошибка при удалении тура: {ex.Message}");
            }
        }

        private (bool isValid, string message) ValidateTourData(TourDTO tour)
        {
            if (tour == null)
                return (false, "Данные тура не предоставлены");

            if (string.IsNullOrWhiteSpace(tour.Name))
                return (false, "Название тура обязательно для заполнения");

            if (string.IsNullOrWhiteSpace(tour.Destination))
                return (false, "Место назначения обязательно для заполнения");

            if (tour.Price <= 0)
                return (false, "Цена должна быть больше нуля");

            if (tour.StartDate >= tour.EndDate)
                return (false, "Дата начала должна быть раньше даты окончания");

            if (tour.AvailableSeats < 0)
                return (false, "Количество мест не может быть отрицательным");

            if (string.IsNullOrWhiteSpace(tour.Status))
                return (false, "Статус тура обязателен для заполнения");

            return (true, string.Empty);
        }
    }
}