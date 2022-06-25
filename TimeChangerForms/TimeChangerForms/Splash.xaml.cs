using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimeChangerForms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Splash : ContentPage
    {
        Timer clockRefresh;
        public Splash()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            clockRefresh = new Timer(dueTime: 6000, period: Timeout.Infinite, callback: EndSplashScreen, state: null);
        }

        private async void EndSplashScreen(object state = null)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                clockRefresh.Change(Timeout.Infinite, Timeout.Infinite);
                //this.Frame.Navigate(typeof(MainPage));
                //Application.Current.MainPage = new MainPage();
                //await
                await Navigation.PushModalAsync(new MainPage());
                //this.NavigationService.Navigate("Second.xaml")
                //Navigate(new MainPage(this));
            });
        }

        //public void Navigate(Page p)
        //{
        //    MainFrame.Navigate(p);
        //}
    }
}