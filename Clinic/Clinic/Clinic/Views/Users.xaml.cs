﻿using Clinic.Clases;
using Clinic.Models;
using Clinic.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Clinic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Users : ContentPage
    {
        public Users()
        {
            MaterialControls control = new MaterialControls();
            control.ShowLoading("Obteniendo lista de usuarios");
            InitializeComponent();
            BaseUrl get = new BaseUrl();
            string url = get.url;
            string server = url + "/Api";
            CheckUrlConnection test = new CheckUrlConnection();
            bool result = test.TestConnection(server);

            if (result == true)
            {
                getUsers();
            }
            else
            {
                control.ShowAlert("No se pudo conectar con el servidor", "Error", "Ok");
            }

            mylist.RefreshCommand = new Command(() =>
            {
                mylist.IsRefreshing = true;
                getUsers();
                mylist.IsRefreshing = false;
            });
        }

        private async void getUsers()
        {
            try
            {
                BaseUrl get = new BaseUrl();
                string baseurl = get.url;
                string url = baseurl + "/Api/usuario/read.php";

                HttpClient client = new HttpClient();
                HttpResponseMessage connect = await client.GetAsync(url);

                if (connect.StatusCode == HttpStatusCode.OK)
                {
                    var response = await client.GetStringAsync(url);
                    var usuarios = JsonConvert.DeserializeObject<List<Usuario>>(response);
                    mylist.ItemsSource = usuarios;
                }
                else
                {
                    mylist.IsVisible = false;
                    message.IsVisible = true;
                }

            }
            catch (HttpRequestException e)
            {
                await DisplayAlert("error", "" + e, "Ok");
            }
        }

        private void Mylist_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SearchEmployee());
        }
    }
}