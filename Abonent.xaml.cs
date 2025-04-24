
using System.Timers;



namespace Phones
{
    public partial class Abonent : ContentPage
    {
        private System.Timers.Timer atimer;
        private string currentImage;
        int id_abonent;
        public Abonent(int id)
        {
            this.id_abonent = id;
            InitializeComponent();
            picture();
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
                picture();
            });
        }

        private void picture()
        {
            if ((DateTime.Now.Hour >= 20 || DateTime.Now.Hour < 8) && currentImage != "night.jpeg")
            {
                currentImage = "night.jpeg";
                timeImage.Source = "night.jpeg";
            }
            else if ((DateTime.Now.Hour >= 8 && DateTime.Now.Hour < 20) && currentImage != "day.jpeg")
            {
                currentImage = "day.jpeg";
                timeImage.Source = "day.jpeg";
            }
            timeLabel.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private async void EnterClicked(object sender, EventArgs e)
        {
            string number;
            
            number = num.Text;

            if (!string.IsNullOrWhiteSpace(number))
            {
                await Navigation.PushAsync(new Call(number, id_abonent));
            }
            else
            {
                errornum.Text = "Введите номер!";
            }
        }
        private async void EnterClicked1(object sender, EventArgs e)
        {
            string number;
            string c;

            number = num.Text;
            c = a.Text;
            number += c;
            num.Text = number;
        }
        private async void EnterClicked2(object sender, EventArgs e)
        {
            string number;
            string c;

            number = num.Text;
            c =b.Text;
            number += c;
            num.Text = number;
        }
        private async void EnterClicked3(object sender, EventArgs e)
        {
            string number;
            string c1;

            number = num.Text;
            c1 = c.Text;
            number += c1;
            num.Text = number;
        }
        private async void EnterClicked4(object sender, EventArgs e)
        {
            string number;
            string c;

            number = num.Text;
            c = d.Text;
            number += c;
            num.Text = number;
        }
        private async void EnterClicked5(object sender, EventArgs e)
        {
            string number;
            string c;

            number = num.Text;
            c = o.Text;
            number += c;
            num.Text = number;
        }

        private async void EnterClicked6(object sender, EventArgs e)
        {
            string number;
            string c;

            number = num.Text;
            c = f.Text;
            number += c;
            num.Text = number;
        }

        private async void EnterClicked7(object sender, EventArgs e)
        {
            string number;
            string c;

            number = num.Text;
            c = g.Text;
            number += c;
            num.Text = number;
        }
        private async void EnterClicked8(object sender, EventArgs e)
        {
            string number;
            string c;

            number = num.Text;
            c = q.Text;
            number += c;
            num.Text = number;
        }
        private async void EnterClicked9(object sender, EventArgs e)
        {
            string number;
            string c;

            number = num.Text;
            c = r.Text;
            number += c;
            num.Text = number;
        }
        private async void EnterClicked10(object sender, EventArgs e)
        {
            string number;
            string c;

            number = num.Text;
            c = n.Text;
            number += c;
            num.Text = number;
        }
        private async void Clear(object sender, EventArgs e)
        {
            string number;

            number = num.Text;
            num.Text = number.Remove(number.Length - 1); ;
        }

    }
}