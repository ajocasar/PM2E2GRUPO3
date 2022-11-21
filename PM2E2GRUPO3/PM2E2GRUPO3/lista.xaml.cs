using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//Carpeta Controller 
using PM2E2GRUPO3.Controller;
using System.IO;
using System.Net.Http;
using System.Diagnostics;
using System.Reflection;


namespace PM2E2GRUPO3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class lista : ContentPage
    {
        public lista()
        {
            InitializeComponent();
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Listar.ItemsSource = await SitiosController.ObtenerSitios();
        }

        private async void Listar_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

            String sexResult = await DisplayActionSheet("Seleccione una opción ", "Cancelar", null, "Actualizar", "Mapa", "Eliminar", "Ver");
            var d = e.SelectedItem as Models.apiSitios.SitioC;

            if (sexResult == "Ver")
            {
                // img
                var byteArray = Convert.FromBase64String(d.imgT);
                System.IO.Stream stream = new MemoryStream(byteArray);
                var imageSource = ImageSource.FromStream(() => stream);
                imgFoto.Source = imageSource;
            }

            if (sexResult == "Eliminar")
            {


                //************MANDAR ELIMINAR SIIOS DE LA BASE DE DATOS *************

                //String direccion = "https://cr10.000webhostapp.com/examen/eliminarSitios.php";
                String direccion = "http://192.168.28.10:8080/Api_clase/Eliminar.php";
                // cliente.UploadValues(direccion, "POST", parametros);
                MultipartFormDataContent parametros = new MultipartFormDataContent();
                StringContent id = new StringContent(d.id);

                parametros.Add(id, "id");


                using (HttpClient client = new HttpClient())
                {
                    var respuesta = await client.PostAsync(direccion, parametros);

                    Debug.WriteLine(respuesta.Content.ReadAsStringAsync().Result);
                    await DisplayAlert("Proceso Terminado", "Datos Eliminados", "OK");
                }

            }

            if (sexResult == "Actualizar")
            {

               // String direccion = "http://192.168.28.10:8080/Api_clase/Actualizar.php";
                //var newpage = new vista(d.id, d.descripcion, d.latitud, d.longitud, d.imgT, d.audioT);
                var newpage = new vista(d.id,d.descripcion, d.latitud, d.longitud, d.imgT, d.audioT);
                await Navigation.PushAsync(newpage);
            }

            if (sexResult == "Mapa")
            {
                var newpage = new mapa(d.latitud, d.longitud, d.descripcion);
                await Navigation.PushAsync(newpage);
            }



            /* audio*/
            /*
            String b64 = d.audioT;


            //System.IO.File.WriteAllBytes("th.wav", bytes);
            
            var audio = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
            audio.Load("PM2E2GRUPO3.th.wav");
            audio.Play();


            var assembly = typeof(App).GetTypeInfo().Assembly;
            Stream audioStream = assembly.GetManifestResourceStream("PM2E2GRUPO3.th.wav");
            audio.Load(audioStream);

            
            Play(decodedString);
               await DisplayAlert("Proceso Terminado", d.sitiosA.descripcion, "OK");*/
        }
    }
}