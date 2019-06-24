using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace App2
{
    public partial class MainPage : ContentPage
    {
        private string clientId = "2519521438109374";
        string token = null;
        string apiRequest = "";
        public MainPage()
        {
            InitializeComponent();
            apiRequest = "https://www.facebook.com/v3.3/dialog/oauth? client_id=" + clientId + "&response_type=token&redirect_uri=https://www.facebook.com/connect/login_success.html";

        }

        private async void WebViewOnNavigated(object sender, WebNavigatedEventArgs web)
        {
            if (web.Url.Contains("access_token"))
            {
                token = ExtractAccessTokenUrl(web.Url);
                await GetFacebookProfile(token);
            }
          
        }

        private string ExtractAccessTokenUrl(string url)
        {
            string at = null;
            if (url.Contains("access_token") && url.Contains("&expires_in"))
                at = url.Replace("https://www.facebook.com/connect/login_success.html#access_token=", "");
            return at;
        }

        public async Task GetFacebookProfile(string accessNo)
        {
             var requestUrl = "https://graph.facebook.com/v2.7/me?" + "fields=name,gender,picture,email,first_name,last_name,cover,birthday&access_token=" + accessNo;

            HttpClient httpClient = new HttpClient();
            var userJson = await httpClient.GetStringAsync(requestUrl);
            string data = JsonConvert.DeserializeObject(userJson).ToString();
            JObject parsed = JObject.Parse(data.ToString());

            FacebookUi facebookUi = new FacebookUi(parsed);

            JObject image = (JObject)parsed["picture"];
            JObject url = (JObject)image["data"];
            var webview = new WebView();
            webview.GoBack();
            Content = webview;
            await Navigation.PushAsync(new FacebookUi(parsed));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var webview = new WebView()
            {
                Source = apiRequest,
                HeightRequest = 1
            };
            webview.Navigated += WebViewOnNavigated;
            Content = webview;
        }
    }
}
