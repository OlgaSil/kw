using Npgsql;
using System;
using System.Timers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Phones
{
    public partial class Call : ContentPage
    {
        private System.Timers.Timer atimer;
        private string currentImage;
        private TimeSpan callDuration;
        int id_abonent;

        public Call(string number, int id)
        {
            InitializeComponent();
            id_abonent = id;
            numcall.Text = number;
            callDuration = TimeSpan.Zero;
            TimeUpdate();
        }

        private void TimeUpdate()
        {
            atimer = new System.Timers.Timer(1000);
            atimer.Elapsed += OnTimedEvent;
            atimer.AutoReset = true;
            atimer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            callDuration = callDuration.Add(TimeSpan.FromSeconds(1));
            MainThread.BeginInvokeOnMainThread(() =>
            {
                UpdateCallTime();
            });
        }

        private void UpdateCallTime()
        {
            timeCall.Text = callDuration.ToString(@"hh\:mm\:ss");
        }

        private async void Abort(object sender, EventArgs e)
        {
            string  timeCall="";
            int city = 1;
            string date = DateTime.Now.ToString(@"dd.MM.yyyy");
            int day = 1;

            timeCall = callDuration.ToString(@"hh\:mm\:ss");
            await Navigation.PushAsync(new Abonent(id_abonent));

            string connectionString = "Host=localhost;Username=postgres;Password=123;Database=phones";

            try
            {
                numcall.Text = timeCall;
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync(); // Асинхронное открытие соединения

                    // Подготовка команды для вставки данных
                    using (var cmd = new NpgsqlCommand("INSERT INTO conversation (id_abonent, city, date, time, day) VALUES (@id_abonent, @city, @date, @timeCall, @day)", conn))
                    {
                        cmd.Parameters.AddWithValue("@id_abonent", id_abonent);
                        cmd.Parameters.AddWithValue("@city", city);
                        cmd.Parameters.AddWithValue("@date", date);
                        cmd.Parameters.AddWithValue("@timeCall", timeCall);
                        cmd.Parameters.AddWithValue("@day", day);
                        numcall.Text = "123";
                        // Выполнение команды
                        int rowsAffected = await cmd.ExecuteNonQueryAsync(); // Асинхронное выполнение команды
                        numcall.Text = "8";

                    }
                }
                
            }
            catch (Exception ex)
            {

            }
            /*await Navigation.PushAsync(new Abonent(id_abonent));*/

        }
    }
}