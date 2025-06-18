using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media;
using Npgsql;

namespace BouncingBalls.Models
{
    public class DatabaseContext
    {
        private readonly string _connectionString;

        public DatabaseContext(string connectionString = "Host=localhost;Port=5432;Database=bouncing_balls;Username=postgres;Password=password")
        {
            _connectionString = connectionString;
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string createTableQuery = @"
                        CREATE TABLE IF NOT EXISTS Balls (
                            Id SERIAL PRIMARY KEY,
                            X DOUBLE PRECISION NOT NULL,
                            Y DOUBLE PRECISION NOT NULL,
                            Vx DOUBLE PRECISION NOT NULL,
                            Vy DOUBLE PRECISION NOT NULL,
                            Radius DOUBLE PRECISION NOT NULL,
                            Color TEXT NOT NULL
                        )";
                    using (var command = new NpgsqlCommand(createTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    string checkEmptyQuery = "SELECT COUNT(*) FROM Balls";
                    using (var command = new NpgsqlCommand(checkEmptyQuery, connection))
                    {
                        long count = (long)command.ExecuteScalar();
                        if (count == 0)
                        {
                            string insertQuery = @"
                                INSERT INTO Balls (X, Y, Vx, Vy, Radius, Color) VALUES
                                (100, 100, 50, 30, 10, '#FF0000'),
                                (200, 150, -40, 60, 10, '#00FF00'),
                                (300, 200, 30, -50, 10, '#0000FF')";
                            using (var insertCommand = new NpgsqlCommand(insertQuery, connection))
                            {
                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка подключения к базе данных: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public List<Ball> GetBalls()
        {
            var balls = new List<Ball>();
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    string selectQuery = "SELECT Id, X, Y, Vx, Vy, Radius, Color FROM Balls";
                    using (var command = new NpgsqlCommand(selectQuery, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                balls.Add(new Ball
                                {
                                    Id = reader.GetInt32(0),
                                    X = Math.Round(reader.GetDouble(1), 4),
                                    Y = Math.Round(reader.GetDouble(2), 4),
                                    Vx = Math.Round(reader.GetDouble(3), 4),
                                    Vy = Math.Round(reader.GetDouble(4), 4),
                                    Radius = Math.Round(reader.GetDouble(5), 4),
                                    Color = (Color)TypeDescriptor.GetConverter(typeof(Color)).ConvertFromString(reader.GetString(6))!
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка: {ex.Message}");
                }
                finally
                {
                    connection.Close();
                }
            }
            return balls;
        }
    }
}
