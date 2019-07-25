﻿using Clinic.Clases;
using Clinic.Models;
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
using XF.Material.Forms.UI;
using XF.Material.Forms.UI.Dialogs;
using XF.Material.Forms.UI.Dialogs.Configurations;

namespace Clinic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Pending_Quotes : ContentPage
    {
        MaterialControls control = new MaterialControls();
        BaseUrl get = new BaseUrl();
        private string baseurl;
        User name = new User();
        public Pending_Quotes()
        {
            InitializeComponent();
            getQuotes();
        }

        private async void getQuotes()
        {
            try
            {

                control.ShowLoading("Obteniendo lista");
                baseurl = get.url;
                string username = name.getName();
                string url2 = baseurl + "/Api/usuario/read_id.php?username=" + username;

                HttpClient client2 = new HttpClient();
                HttpResponseMessage connect2 = await client2.GetAsync(url2);

                if (connect2.StatusCode == HttpStatusCode.OK)
                {
                    var response2 = await client2.GetStringAsync(url2);
                    var info = JsonConvert.DeserializeObject<Usuario>(response2);
                    var id = info.reference;

                    string url = baseurl + "/Api/pending_quotes/read.php?idempleado=" + id;

                    HttpClient client = new HttpClient();
                    HttpResponseMessage connect = await client.GetAsync(url);

                    if (connect.StatusCode == HttpStatusCode.OK)
                    {
                        var response = await client.GetStringAsync(url);
                        var quotes = JsonConvert.DeserializeObject<List<Pending>>(response);
                        mylist.ItemsSource = quotes;
                    }
                    else
                    {
                        mylist.IsVisible = false;
                        message.IsVisible = true;
                    }
                }

            }
            catch (HttpRequestException e)
            {
                await DisplayAlert("error", "" + e, "Ok");
            }
        }

        private async void Mylist_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                if (e != null)
                {
                    MaterialControls control = new MaterialControls();
                    var config = new MaterialInputDialogConfiguration()
                    {
                        InputType = MaterialTextFieldInputType.Numeric,
                        CornerRadius = 8,
                        BackgroundColor = Color.FromHex("#2c3e50"),
                        InputTextColor = Color.White,
                        InputPlaceholderColor = Color.White.MultiplyAlpha(0.6),
                        TintColor = Color.White,
                        TitleTextColor = Color.White,
                        MessageTextColor = Color.FromHex("#DEFFFFFF")
                    };

                    var res = await MaterialDialog.Instance.InputAsync(title: "Aceptar cita",
                                                                         message: "Para aceptar la cita ingrese el numero de consultorio",
                                                                         inputPlaceholder: "Consultorio",
                                                                         confirmingText: "Aceptar",
                                                                         configuration: config);

                    if (res != "")
                    {
 
                        control.ShowLoading("Aceptando cita");
                        var list = (ListView)sender;
                        var selection = list.SelectedItem as Pending;

                        string username = name.getName();
                        string url2 = baseurl + "/Api/usuario/read_id.php?username=" + username;

                        HttpClient client2 = new HttpClient();
                        HttpResponseMessage connect2 = await client2.GetAsync(url2);

                        if (connect2.StatusCode == HttpStatusCode.OK)
                        {
                            var response2 = await client2.GetStringAsync(url2);
                            var info = JsonConvert.DeserializeObject<Usuario>(response2);
                            var id = info.reference;

                            string url4 = baseurl + "/Api/empleado/read_one.php?idempleado=" + id;
                            HttpClient client3 = new HttpClient();
                            HttpResponseMessage connect3 = await client3.GetAsync(url2);

                            if (connect3.StatusCode == HttpStatusCode.OK)
                            {
                                var response3 = await client3.GetStringAsync(url4);
                                var personal = JsonConvert.DeserializeObject<Empleados>(response3);
                               var nombres = personal.nombres;

                                string url = baseurl + "/Api/pending_quotes/delete.php?idpending=" + selection.idpending;

                                HttpClient client = new HttpClient();
                                HttpResponseMessage connect = await client.GetAsync(url);
                                if (connect.StatusCode == HttpStatusCode.OK)
                                {

                                    Citas citas = new Citas
                                    {
                                        fecha_Cita = selection.fecha,
                                        hora_Cita = selection.hora,
                                        nombre_Paciente = selection.nombre,
                                        apellido_Paciente = selection.apellido,
                                        num_Consultorio = Convert.ToInt32(res),
                                        nombre_Doctor = nombres,
                                        idpaciente = selection.idpaciente,
                                        idempleado = id
                                    };



                                    HttpClient cliente = new HttpClient();
                                    BaseUrl baseurl = new BaseUrl();
                                    string url3 = baseurl.url;
                                    string controlador = "/Api/citas/create.php";
                                    cliente.BaseAddress = new Uri(url);

                                    string json = JsonConvert.SerializeObject(citas);
                                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                                    var result = await cliente.PostAsync(controlador, content);

                                    if (result.IsSuccessStatusCode)
                                    {

                                        control.ShowAlert("Registrado!!", "Exito", "Ok");
                                    }
                                    else
                                    {
                                        control.ShowAlert("Ocurrio un error al registrar", "Error", "Ok");
                                    }
                                }
                                else
                                {
                                    control.ShowAlert("Ocurrio un error al eliminar", "Error", "Ok");
                                }
                            }
                            else
                            {
                                control.ShowSnackBar("Se cancelo");
                            }
                        }
                        getQuotes();
                    }    
                }
            }
    catch (Exception ex)
            {

              await  DisplayAlert("Error", "Error: " + ex, "ok");
            }
        }
    }
}