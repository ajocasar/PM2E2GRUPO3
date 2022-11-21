using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace PM2E2GRUPO3.Models
{
    public class ConfiguracionApi
    {
        public static String ipaddress = "192.168.0.28";  //IP public o DNS 
        public static String webapi = "Api_clase";

        // Routing
        public static String getRoute = "Obtener.php";  //IP public o DNS 
        public static String postRoute = "Create.php";  //IP public o DNS 
        // public static String getRoute = "Eliminar.php";  //IP public o DNS 
        //public static String postRoute = "Actualizar.php";  //IP public o DNS 

        //Format URL 
        public static String WebUrlApi = "http://{0}/{1}/{2}";


        //EndPoint
        public static String ApiGetListAlumn = string.Format(WebUrlApi, ipaddress, webapi, getRoute);
        public static String ApiPostAlumn = string.Format(WebUrlApi, ipaddress, webapi, postRoute);

    }
}

