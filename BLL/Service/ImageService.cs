using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using DTO;
using System.Data;

namespace BLL.Services
{
    public class ImageService
    {
        private readonly string _connectionString;

        public ImageService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<ImageDTO>> GetAllImagesAsync()
        {
            List<ImageDTO> images = new List<ImageDTO>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("SELECT * FROM images", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            images.Add(new ImageDTO
                            {
                                ImageId = reader.GetInt32(reader.GetOrdinal("imageid")),
                                TourId = reader.IsDBNull(reader.GetOrdinal("tourid")) ? null : (int?)reader.GetInt32(reader.GetOrdinal("tourid")),
                                ImagePath = reader.GetString(reader.GetOrdinal("imagepath"))
                            });
                        }
                    }
                }
            }

            return images;
        }

        public async Task<ImageDTO> AddImageAsync(ImageDTO image)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(
                    "INSERT INTO images (tourid, imagepath) VALUES (@tourid, @imagepath) RETURNING imageid",
                    connection))
                {
                    command.Parameters.AddWithValue("@tourid", (object)image.TourId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@imagepath", image.ImagePath);

                    image.ImageId = Convert.ToInt32(await command.ExecuteScalarAsync());
                    return image;
                }
            }
        }

        public async Task AddOrUpdateImageAsync(ImageDTO image)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                if (image.ImageId > 0) // Если есть ImageId, выполняем обновление
                {
                    using (var command = new NpgsqlCommand(
                        "UPDATE images SET tourid = @tourid, imagepath = @imagepath WHERE imageid = @imageid",
                        connection))
                    {
                        command.Parameters.AddWithValue("@tourid", (object)image.TourId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@imagepath", image.ImagePath);
                        command.Parameters.AddWithValue("@imageid", image.ImageId);

                        await command.ExecuteNonQueryAsync();
                    }
                }
                else // Иначе выполняем добавление
                {
                    using (var command = new NpgsqlCommand(
                        "INSERT INTO images (tourid, imagepath) VALUES (@tourid, @imagepath) RETURNING imageid",
                        connection))
                    {
                        command.Parameters.AddWithValue("@tourid", (object)image.TourId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@imagepath", image.ImagePath);

                        image.ImageId = Convert.ToInt32(await command.ExecuteScalarAsync());
                    }
                }
            }
        }
        public async Task UpdateImageAsync(ImageDTO image)
        {
            if (image.ImageId <= 0)
            {
                throw new ArgumentException("ID изображения должен быть больше 0");
            }

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(
                    "UPDATE images SET tourid = @tourid, imagepath = @imagepath WHERE imageid = @imageid",
                    connection))
                {
                    command.Parameters.AddWithValue("@tourid", (object)image.TourId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@imagepath", image.ImagePath);
                    command.Parameters.AddWithValue("@imageid", image.ImageId);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                    {
                        throw new Exception($"Изображение с ID {image.ImageId} не найдено");
                    }
                }
            }
        }

        public async Task DeleteImageAsync(int imageId)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand("DELETE FROM images WHERE imageid = @imageid", connection))
                {
                    command.Parameters.AddWithValue("@imageid", imageId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}