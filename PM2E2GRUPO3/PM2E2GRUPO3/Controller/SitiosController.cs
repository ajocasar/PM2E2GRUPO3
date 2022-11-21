using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PM2E2GRUPO3.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;

namespace PM2E2GRUPO3.Controller
{
    public class SitiosController
    {
        public async static Task<List<apiSitios.SitioC>> ObtenerSitios()
        {
            List<apiSitios.SitioC> listapaises = new List<apiSitios.SitioC>();
            using (HttpClient client = new HttpClient())
            {


                // *********** OBTENER LOS DATOS DE LA DB
                
                var response = await client.GetAsync("http://192.168.28.10/Api_clase/Obtener.php");
                if (response.IsSuccessStatusCode)
                {
                    var contenido = response.Content.ReadAsStringAsync().Result;
                    listapaises = JsonConvert.DeserializeObject<List<apiSitios.SitioC>>(contenido);
                }
            }
            return listapaises;
        }
    }
}