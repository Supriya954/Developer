using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    public partial class FacebookUi : ContentPage
    {
        public FacebookUi(JObject json)
        { 
            InitializeComponent();
            
            Name.Text = json["name"].ToString();
            JObject image = (JObject)json["picture"];
            JObject url = (JObject)image["data"];

            userimg.Source = url["url"].ToString();
            idlabel.Text = json["id"].ToString();
        }
    }
}