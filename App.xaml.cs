﻿namespace Phones
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = base.CreateWindow(activationState);

            const int newWidth = 800;
            const int newHeight = 650;

            window.Width = newWidth;
            window.Height = newHeight;

            return window;
        }
    }
}