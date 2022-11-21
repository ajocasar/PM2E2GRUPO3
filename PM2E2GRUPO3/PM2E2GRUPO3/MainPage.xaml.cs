using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

using Plugin.Media;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.Geolocator;
using Plugin.AudioRecorder;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Net.Http;
using System.Collections;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;


namespace PM2E2GRUPO3
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        // Plugin.Media.Abstractions.MediaFile imgFoto = null;

        string ruta = "", StringBase64Foto = "", StringBase64Audio = ""; //ruta de la imagen
        int aud = 0; // validaciones
        AudioRecorderService recorder = new AudioRecorderService();// grabadora


        public MainPage()
        {
            InitializeComponent();
            InizializatePlugins();
        }

        private async void InizializatePlugins()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    // Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    lblLatitud.Text = location.Latitude.ToString();
                    lblLongitud.Text = location.Longitude.ToString();
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        //**********TOMAR FOTO****************
        public async void tomar()
        {
            var takepic = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "PhotoApp",
                Name = "TEST.jpg",
               SaveToAlbum = true
            });

            ruta = takepic.Path;


            if (takepic != null)
            {
                imgFoto.Source = ImageSource.FromStream(() => { return takepic.GetStream(); });              
            }
            byte[] ImageBytes = null;

            using (var stream = new MemoryStream())
            {
                takepic.GetStream().CopyTo(stream);
                takepic.Dispose();
                ImageBytes = stream.ToArray();
                StringBase64Foto = Convert.ToBase64String(ImageBytes);
            }
        }


        //PARA GUARDAR LOS DATOS


        //public async void guardar()
            public async void guardar()
        {
            //if (String.IsNullOrWhiteSpace(txtDescripcion.Text) || ruta == "" || lblLatitud.Text == "Latitud" || lblLongitud.Text == "Longitud" || aud==0)
            if (String.IsNullOrWhiteSpace(txtDescripcion.Text) || ruta == "" || lblLatitud.Text == "" || lblLongitud.Text == "")
            {

               await DisplayAlert("Error", "No completó todos los campos", "OK");
             }
            else
            {



                /*  
                  WebClient cliente = new WebClient();
                  var parametros = new System.Collections.Specialized.NameValueCollection();
                  parametros.Add("descripcion", txtDescripcion.Text);
                  parametros.Add("longitud", lblLongitud.Text);
                  parametros.Add("latitud", lblLatitud.Text);
                  parametros.Add("imgT", StringBase64Foto);
                  parametros.Add("audioT", StringBase64Audio);

                 // cliente.UploadValues("http://192.168.28.10/Api_clase/Create.php", "POST", parametros);
                */



                //********** INSERTAR SITIOS A LA BASE DE DATOS ***********************8

                // String direccion = "https://dpstudent.000webhostapp.com/codigoQR/Examenparcial2/sitios.php";
                // String direccion = "http://172.20.10.14/Api_clase/Create.php";
                //String direccion = "http://192.168.28.10/Api_clase/Create.php";

                //CAMBIOS
                //cliente.UploadValues(direccion, "POST", parametros);

                /*
                MultipartFormDataContent parametros = new MultipartFormDataContent();
                StringContent descripcion = new StringContent(txtDescripcion.Text);
                StringContent longitud = new StringContent(lblLongitud.Text);
                StringContent latitud = new StringContent(lblLatitud.Text);
                StringContent imgT = new StringContent(StringBase64Foto);
                StringContent audioT = new StringContent(StringBase64Audio);
                parametros.Add(descripcion, "descripcion");
                parametros.Add(longitud,    "longitud");
                parametros.Add(latitud,     "latitud");
                parametros.Add(imgT,        "imgT");
                parametros.Add(audioT,      "audioT");

                */



                /*  //CAMBIOS
                        // cliente.UploadValues(direccion, "POST", parametros);
                   MultipartFormDataContent parametros = new MultipartFormDataContent();
                   StringContent des = new StringContent(txtDescripcion.Text);
                   StringContent lon = new StringContent(lblLongitud.Text);
                   StringContent lat = new StringContent(lblLatitud.Text);
                   StringContent img = new StringContent(StringBase64Foto);
                   StringContent aud = new StringContent(StringBase64Audio);
                           parametros.Add(des,  "descripcion");
                           parametros.Add(lon,  "longitud");
                           parametros.Add(lat,  "latitud");
                           parametros.Add(img,  "imgT");
                           parametros.Add(aud,  "audioT");
                 */

                /*
                var datos = new Models.apiSitios.SitioC
                {

                    descripcion = txtDescripcion.Text,
                    latitud = lblLatitud.Text,
                    longitud = lblLongitud.Text,
                    //imgT = StringBase64Foto(),
                    //audioT = StringBase64Audio()




                
                     var datos = new Models.apiSitios.SitioC
                                {

                                    descripcion = Convert.ToString(txtDescripcion),
                                    latitud = Convert.ToString(lblLatitud),
                                    longitud = Convert.ToString(lblLongitud),
                                    imgT = StringBase64Foto,
                                    audioT = StringBase64Audio


                                };
                };
                */


                var datos = new Models.apiSitios.SitioC
                {

                    descripcion = txtDescripcion.Text,
                    latitud = lblLatitud.Text,
                    longitud = lblLongitud.Text,
                    imgT = StringBase64Foto,
                    audioT = StringBase64Audio

                };

                String jsonObject = JsonConvert.SerializeObject(datos);
                StringContent contenido = new StringContent(jsonObject, Encoding.UTF8, "application/json");

                
                 
               using (HttpClient client = new HttpClient())
                  {
                    // var respuesta = await client.PostAsync(direccion, parametros);

                    HttpResponseMessage respuesta = await client.PostAsync("http://192.168.28.10/Api_clase/Create.php",  contenido);
                     //cliente.PostAsync("http://192.168.28.10/Api_clase/Create.php", contenido);
                    // cliente.UploadValues("http://192.168.28.10/Api_clase/Create.php", "POST", parametros);
                    //var respuesta = await client.PostAsync(direccion, parametros);
                    //Debug.WriteLine(respuesta.Content.ReadAsStringAsync().Result);
                    await DisplayAlert("Proceso Terminado", "Datos Guardados", "OK");

              //  String jsonObject = jsonObject.SerializeObject(Models.apiSitios);

                    reset();

                }
            }
        }




        private void btnFoto_Clicked(object sender, EventArgs e)
        {
            tomar();
        }

        private void reset()
        {
            lblLatitud.Text = "";
            lblLongitud.Text = "";
            txtDescripcion.Text = "";
            ruta = "";
            imgFoto.Source = "TEST.jpg";
            aud = 0;
            recorder = new AudioRecorderService();
        }

        public async void btnGrabar_Clicked(object sender, EventArgs e)
        {
            if (!recorder.IsRecording)
            {
               // recorder = new AudioRecorderService();

                lblAudio.Text = "Grabando";
                await recorder.StartRecording();

            }

        }

        public async void btnDetener_Clicked(object sender, EventArgs e)
        {
            if (recorder.IsRecording)
            {
                lblAudio.Text = "Audio Detenido";
                await recorder.StopRecording();

                //REALICE CAMBIOS
                aud = 1;

                byte[] AudioBytes = null;

                using (var stream = new MemoryStream())
                {
                    recorder.GetAudioFileStream().CopyTo(stream);
                    AudioBytes = stream.ToArray();
                    StringBase64Audio = Convert.ToBase64String(AudioBytes);
                }
            }
        }

        public void btnReproducir_Clicked(object sender, EventArgs e)
        {
            AudioPlayer player = new AudioPlayer();
            var filePath = recorder.GetAudioFilePath();
            lblAudio.Text = "Reproduciendo";
            player.Play(filePath);

            lblAudio.Text = "Sin acción";
        }

        public void btnGuardar_Clicked(object sender, EventArgs e)
       {
                guardar();
        }

        public async void btnListar_Clicked(object sender, EventArgs e)
        {
            var am = new lista();
            await Navigation.PushAsync(am);
        }
    }
}




