using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Phones
{
    public partial class Cities : ContentPage
    {
        private int currentRow;
        public Cities(int id)
        {
            InitializeComponent();
            citybase();
            currentRow = 1;
        }

        private async void AddCity(object sender, EventArgs e)
        {
            currentRow = grid.RowDefinitions.Count;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            var nameEntry = new Entry
            {
                Placeholder = "Название",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 200
            };

            var codeEntry = new Entry
            {
                Placeholder = "Код",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 200
            };

            var costEntry = new Entry
            {
                Placeholder = "Стоимость",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 150
            };

            var saveButton = new Button
            {
                Text = "+",
                BackgroundColor = Colors.Transparent,
                TextColor = Colors.Green,
                Padding = new Thickness(5, 0),
                FontSize = 24,
                WidthRequest = 30,
                HeightRequest = 30,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            };

            // Добавляем элементы в сетку
            var border = new Border
            {
                StrokeThickness = 1,
                Content = new Label
                {
                    Text = "Ввод",
                    TextColor = Colors.Green,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };

            grid.Children.Add(border);
            Grid.SetRow(border, currentRow);
            Grid.SetColumn(border, 0);

            border = new Border
            {
                StrokeThickness = 1,
                Content = nameEntry
            };

            grid.Children.Add(border);
            Grid.SetRow(border, currentRow);
            Grid.SetColumn(border, 1);

            border = new Border
            {
                StrokeThickness = 1,
                Content = codeEntry
            };

            grid.Children.Add(border);
            Grid.SetRow(border, currentRow);
            Grid.SetColumn(border, 2);

            
            var layout = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };


            layout.Children.Add(costEntry);
            layout.Children.Add(saveButton);
            Grid.SetColumn(costEntry, 0);
            Grid.SetColumn(saveButton, 1);

            border = new Border
            {
                StrokeThickness = 1,
                Content = layout
            };

            grid.Children.Add(border);
            Grid.SetRow(border, currentRow);
            Grid.SetColumn(border, 3);



            // Обработчик сохранения
            saveButton.Clicked += async (s, e) =>
            {
                SaveNewCity(nameEntry.Text, codeEntry.Text, costEntry.Text);
                grid.Clear();
            };
        }

        private async void SaveNewCity(string name, string code, string cost)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=123;Database=phones";
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new NpgsqlCommand("INSERT INTO cities (name, code, cost) VALUES (@name, @code, @cost)", conn))
                    {
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@code", code);
                        cmd.Parameters.AddWithValue("@cost", cost);
                        await cmd.ExecuteNonQueryAsync();
                    }
                    citybase(); // Обновляем данные
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении: " + ex.Message);
            }
        }


        private async void citybase()
        {
            string connectionString = "Host=localhost;Username=postgres;Password=123;Database=phones";
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new NpgsqlCommand("SELECT * FROM cities", conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            List<City> cities = new List<City>();
                            while (await reader.ReadAsync())
                            {
                                cities.Add(new City
                                {
                                    Id = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    Code = reader.GetString(2),
                                    Price = reader.GetString(3)
                                });
                            }

                            await UpdateUI(cities);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка загрузки: " + ex.Message);
            }
        }

        private async Task UpdateUI(List<City> cities)
        {
            grid.Children.Clear();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });

            var border = new Border
            {
                StrokeThickness = 1,
                Content = new Label
                {
                    Text = "№",
                    TextColor = Colors.Green,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };

            grid.Children.Add(border);
            Grid.SetRow(border, 0);
            Grid.SetColumn(border, 0);

            border = new Border
            {
                StrokeThickness = 1,
                Content = new Label
                {
                    Text = "Название",
                    TextColor = Colors.Green,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };

            grid.Children.Add(border);
            Grid.SetRow(border, 0);
            Grid.SetColumn(border, 1);

            border = new Border
            {
                StrokeThickness = 1,
                Content = new Label
                {
                    Text = "Код",
                    TextColor = Colors.Green,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };

            grid.Children.Add(border);
            Grid.SetRow(border, 0);
            Grid.SetColumn(border, 2);

            border = new Border
            {
                StrokeThickness = 1,
                Content = new Label
                {
                    Text = "Стоимость руб/мин",
                    TextColor = Colors.Green,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };

            grid.Children.Add(border);
            Grid.SetRow(border, 0);
            Grid.SetColumn(border, 3);

            int row = 1;
            foreach (var city in cities)
            {
                AddDataRow(city, row);
                row = row+1;
            }
        }

        private void AddDataRow(City city, int row)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            AddSimpleCell(row.ToString(), row, 0);
            AddSimpleCell(city.Name, row, 1);
            AddSimpleCell(city.Code, row, 2);
            AddCellWithDelete(city.Price, city.Id, row, 3); // Только в последней колонке будет ✕
        }

        private void AddSimpleCell(string text, int row, int column)
        {
            var border = new Border
            {
                StrokeThickness = 1,
                Content = new Label
                {
                    Text = text,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };

            grid.Children.Add(border);
            Grid.SetRow(border, row);
            Grid.SetColumn(border, column);
        }

        private void AddCellWithDelete(string text, int cityId, int row, int column)
        {
            var layout = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = GridLength.Auto }
                },
                RowDefinitions =
        {
            new RowDefinition { Height = new GridLength(40) }
        }

            };

            var label = new Label
            {
                Text = text,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

            var deleteButton = new Button
            {
                Text = "✕",
                BackgroundColor = Colors.Transparent,
                TextColor = Colors.Red,
                Padding = new Thickness(5, 0),
                FontSize = 14,
                WidthRequest = 30,
                HeightRequest = 30,
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            };

            deleteButton.Clicked += async (s, e) =>
            {
                await DeleteCityFromDatabase(cityId);
                RemoveRowFromGrid(row);
            };

            layout.Children.Add(label);
            layout.Children.Add(deleteButton);
            Grid.SetColumn(label, 0);
            Grid.SetColumn(deleteButton, 1);

            var border = new Border
            {
                StrokeThickness = 1,
                Content = layout
            };

            grid.Children.Add(border);
            Grid.SetRow(border, row);
            Grid.SetColumn(border, column);
        }

        private async Task DeleteCityFromDatabase(int cityId)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=123;Database=phones";
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new NpgsqlCommand("DELETE FROM cities WHERE id = @id", conn))
                    {
                        cmd.Parameters.AddWithValue("@id", cityId);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при удалении: " + ex.Message);
            }
        }

        private void RemoveRowFromGrid(int rowToRemove)
        {
            var toRemove = grid.Children.Where(e => grid.GetRow(e) == rowToRemove).ToList();
            foreach (var el in toRemove)
                grid.Children.Remove(el);

            foreach (var el in grid.Children)
            {
                int r = grid.GetRow(el);
                if (r > rowToRemove)
                    grid.SetRow(el, r - 1);
            }
        }

        public class City
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public string Price { get; set; }
        }
    }
}