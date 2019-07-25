﻿using Clinic.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Clinic.Clases
{
   public class Create
    {

        public async void create(string nombre, string apellido, int idpaciente, int id)
        {
            MaterialControls control = new MaterialControls();
            control.ShowLoading("Registrando");
            Lista_Item_Espera empleados = new Lista_Item_Espera
            {
                nombre = nombre,
                apellido = apellido,
                idpaciente = idpaciente,
                idlista = id

            };



            HttpClient client = new HttpClient();
            BaseUrl get = new BaseUrl();
            string url = get.url;
            string controlador = "/Api/item_espera/create.php";
            client.BaseAddress = new Uri(url);

            string json = JsonConvert.SerializeObject(empleados);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(controlador, content);

            if (response.IsSuccessStatusCode)
            {
                control.ShowAlert("Agregado!", "Exito", "Ok");
            }
            else
            {
                control.ShowAlert("Ocurrio un error al agregar!!", "Error", "Ok");
            }
        }
    }
}