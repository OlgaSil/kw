using Npgsql;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace Phones
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }


        private async void EnterClicked(object sender, EventArgs e)
        {
            string login;
            string password;
            int id, category;
            login = log.Text;
            password = pass.Text;

            string connectionString = "Host=localhost;Username=postgres;Password=123;Database=phones";

            try
            {

                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = new NpgsqlCommand("SELECT id, category FROM users WHERE login=@login AND password=@password", conn))
                    {
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@password", password);
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                id = reader.GetInt32(0);
                                category = reader.GetInt32(1);
                                error.Text = "";
                                if (category == 1)
                                {
                                    await Navigation.PushAsync(new Abonent(id));
                                }
                                else if (category == 0) {
                                    await Navigation.PushAsync(new Employee(id));
                                }
                            }
                            else
                            {
                                error.Text = "Пользователь не найден!";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }

}
