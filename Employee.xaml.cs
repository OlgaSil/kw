
using System.Timers;



namespace Phones
{
    public partial class Employee : ContentPage
    {
        private System.Timers.Timer atimer;
        int id_abonent;
        public Employee(int id)
        {
            this.id_abonent = id;
            InitializeComponent();
            timeset();
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
            MainThread.BeginInvokeOnMainThread(() =>
            {
                timeset();
            });
        }

        private void timeset()
        {
            time.Text = DateTime.Now.ToString("HH:mm:ss");
            data.Text = DateTime.Now.ToString("dd.MM.yyyy");

        }

        private async void CitiesClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Cities(id_abonent));
        }
        private async void AbonentsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AbonentsList());
        }
    }
}