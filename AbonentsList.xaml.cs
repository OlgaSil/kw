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
    public partial class AbonentsList : ContentPage
    {
        private int currentRow;
        public AbonentsList()
        {
            InitializeComponent();
            citybase();
            currentRow = 1;
        }

        private async void AddAbonent(object sender, EventArgs e)
        {
            currentRow = grid.RowDefinitions.Count;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            var surnameEntry = new Entry
            {
                Placeholder = "Фамилия",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 100
            };

            var nameEntry = new Entry
            {
                Placeholder = "Имя",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 100
            };

            var patronymicEntry = new Entry
            {
                Placeholder = "Отчество",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 100
            };

            var innEntry = new Entry
            {
                Placeholder = "ИНН",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 100
            };
            var accEntry = new Entry
            {
                Placeholder = "Лиц.счёт",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 100
            };
            var numEntry = new Entry
            {
                Placeholder = "Номер",
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 70
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
                Content = surnameEntry
            };

            grid.Children.Add(border);
            Grid.SetRow(border, currentRow);
            Grid.SetColumn(border, 1);

            border = new Border
            {
                StrokeThickness = 1,
                Content = nameEntry
            };

            grid.Children.Add(border);
            Grid.SetRow(border, currentRow);
            Grid.SetColumn(border, 2);

            border = new Border
            {
                StrokeThickness = 1,
                Content = patronymicEntry
            };

            grid.Children.Add(border);
            Grid.SetRow(border, currentRow);
            Grid.SetColumn(border, 3);

            border = new Border
            {
                StrokeThickness = 1,
                Content = innEntry
            };

            grid.Children.Add(border);
            Grid.SetRow(border, currentRow);
            Grid.SetColumn(border, 4);

            border = new Border
            {
                StrokeThickness = 1,
                Content = accEntry
            };

            grid.Children.Add(border);
            Grid.SetRow(border, currentRow);
            Grid.SetColumn(border, 5);


            var layout = new Grid
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = GridLength.Auto }
                }
            };


            layout.Children.Add(numEntry);
            layout.Children.Add(saveButton);
            Grid.SetColumn(numEntry, 0);
            Grid.SetColumn(saveButton, 1);

            border = new Border
            {
                StrokeThickness = 1,
                Content = layout
            };

            grid.Children.Add(border);
            Grid.SetRow(border, currentRow);
            Grid.SetColumn(border, 6);



            // Обработчик сохранения
            saveButton.Clicked += async (s, e) =>
            {
                SaveNewAbonent(surnameEntry.Text, nameEntry.Text, patronymicEntry.Text, innEntry.Text, accEntry.Text, numEntry.Text);
                grid.Clear();
            };
        }

        private async void SaveNewAbonent(string surname, string name, string patronymic, string inn, string account, string number)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=123;Database=phones";
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new NpgsqlCommand("INSERT INTO abonent (surname, name, patronymic, inn, account, phone_number) VALUES (@surname, @name, @patronymic, @inn, @account, @phone_number)", conn))
                    {
                        cmd.Parameters.AddWithValue("@surname", surname);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@patronymic", patronymic);
                        cmd.Parameters.AddWithValue("@inn", inn);
                        cmd.Parameters.AddWithValue("@account", account);
                        cmd.Parameters.AddWithValue("@phone_number", number);
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
                    using (var cmd = new NpgsqlCommand("SELECT * FROM abonent", conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            List<Abonent> abonents = new List<Abonent>();
                            while (await reader.ReadAsync())
                            {
                                abonents.Add(new Abonent
                                {
                                    Id = reader.GetInt32(0),
                                    Surname = reader.GetString(1),
                                    Name = reader.GetString(2),
                                    Patronymic = reader.GetString(3),
                                    Inn = reader.GetString(4),
                                    Account = reader.GetString(5),
                                    Number = reader.GetString(6)
                                });
                            }

                            await UpdateUI(abonents);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка загрузки: " + ex.Message);
            }
        }

        private async Task UpdateUI(List<Abonent> abonents)
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
                    Text = "Фамилия",
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
                    Text = "Имя",
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
                    Text = "Отчество",
                    TextColor = Colors.Green,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };

            grid.Children.Add(border);
            Grid.SetRow(border, 0);
            Grid.SetColumn(border, 3);

            border = new Border
            {
                StrokeThickness = 1,
                Content = new Label
                {
                    Text = "ИНН",
                    TextColor = Colors.Green,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };

            grid.Children.Add(border);
            Grid.SetRow(border, 0);
            Grid.SetColumn(border, 4);

            border = new Border
            {
                StrokeThickness = 1,
                Content = new Label
                {
                    Text = "Лиц.счёт",
                    TextColor = Colors.Green,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };

            grid.Children.Add(border);
            Grid.SetRow(border, 0);
            Grid.SetColumn(border, 5);

            border = new Border
            {
                StrokeThickness = 1,
                Content = new Label
                {
                    Text = "Номер",
                    TextColor = Colors.Green,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                }
            };

            grid.Children.Add(border);
            Grid.SetRow(border, 0);
            Grid.SetColumn(border, 6);

            int row = 1;
            foreach (var abonent in abonents)
            {
                AddDataRow(abonent, row);
                row = row + 1;
            }
        }

        private void AddDataRow(Abonent abonent, int row)
        {
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
            AddSimpleCell(row.ToString(), row, 0);
            AddSimpleCell(abonent.Surname, row, 1);
            AddSimpleCell(abonent.Name, row, 2);
            AddSimpleCell(abonent.Patronymic, row, 3);
            AddSimpleCell(abonent.Inn, row, 4);
            AddSimpleCell(abonent.Account, row, 5);
            AddCellWithDelete(abonent.Number, abonent.Id, row, 6); // Только в последней колонке будет ✕
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
                    using (var cmd = new NpgsqlCommand("DELETE FROM abonent WHERE id = @id", conn))
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

        public class Abonent
        {
            public int Id { get; set; }
            public string Surname { get; set; }
            public string Name { get; set; }
            public string Patronymic { get; set; }
            public string Inn { get; set; }
            public string Account { get; set; }
            public string Number { get; set; }

        }
    }
}