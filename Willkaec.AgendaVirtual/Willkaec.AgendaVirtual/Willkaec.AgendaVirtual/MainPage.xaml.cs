using Plugin.Connectivity;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Willkaec.AgendaVirtual
{
    public partial class MainPage : ContentPage
    {

        protected string Host = "willkaec.com";
        protected string SourceSite = "https://agenda.willkaec.com/Web";


        public MainPage()
        {
            InitializeComponent();
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Error", "Error Connected", "OK");
            }
            else
            {

                webView.Source = SourceSite;
            }
        }


        private async void Current_ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (!e.IsConnected)
            {
                await DisplayAlert("Error Title", "Error Msg. Current_ConnectivityChanged", "OK");
            }
            else
            {
                webView.Source = SourceSite;
            }

        }

        protected override bool OnBackButtonPressed()
        {

            base.OnBackButtonPressed();

            if (webView.CanGoBack)
            {
                webView.GoBack();
                return true;
            }
            else
            {
                base.OnBackButtonPressed();
                return true;
            }

        }

        async void webviewNavigating(object sender, WebNavigatingEventArgs e)
        {
            var isConnected = CrossConnectivity.Current.IsConnected;
            if (!isConnected)
            {
                await DisplayAlert("Error Title", "Error Msg- webviewNavigating", "OK");
            }

            //Interna
            if (e.Url.CaseInsensitiveContains(Host))
                return;


            if (await Launcher.CanOpenAsync(e.Url))
            {
                e.Cancel = true;
                await Launcher.OpenAsync(e.Url);
            }

            e.Cancel = true;
            _ = Browser.OpenAsync(e.Url, this.LaunchOptions);

        }

        async void webviewNavigated(object sender, WebNavigatedEventArgs e)
        {

            var isConnected = CrossConnectivity.Current.IsConnected;
            if (!isConnected)
            {
                await DisplayAlert("Error Title", "Error Msg- webviewNavigated", "OK");
            }

        }

        async void OnBackButtonClicked(object sender, EventArgs e)
        {
            if (webView.CanGoBack)
            {
                webView.GoBack();
            }
            else
            {
                await Navigation.PopAsync();
            }
        }

        void OnForwardButtonClicked(object sender, EventArgs e)
        {
            if (webView.CanGoForward)
            {
                webView.GoForward();
            }
        }


        public BrowserLaunchOptions LaunchOptions { get; set; } = new BrowserLaunchOptions
        {
            LaunchMode = BrowserLaunchMode.External, //.SystemPreferred,
            TitleMode = BrowserTitleMode.Default,
            PreferredControlColor = Color.FromHex("#f3f3f3"),
            PreferredToolbarColor = Color.FromHex("#212121")
        };

    }

    public static class Extensions
    {
        public static bool CaseInsensitiveContains(this string text, string value,
            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return text.IndexOf(value, stringComparison) >= 0;
        }
    }
}
