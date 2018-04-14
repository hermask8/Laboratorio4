using Lab4_Ana_Edwin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using pathArchivo2 = System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Lab4_Ana_Edwin.Controllers
{
    public class AlbumController : Controller
    {
        List<Mostrar> mostrar = new List<Mostrar>();
        Mostrar objmostrar = new Mostrar();
        public static Dictionary<string, Paises> Diccionario1 = new Dictionary<string, Paises>();
        public static Dictionary<string, Calcomanias> Diccionario2 = new Dictionary<string, Calcomanias>();
        // GET: Album
        public ActionResult LeerCambios()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LeerCambios(HttpPostedFileBase Archivo)
        {
            string pathArchivo = string.Empty;
            if (Archivo != null)
            {
                Archivo.SaveAs(Server.MapPath("~/JSONFiles" + Path.GetFileName(Archivo.FileName)));
                StreamReader sr = new StreamReader(Server.MapPath("~/JSONFiles" + Path.GetFileName(Archivo.FileName)));
                var informacion = sr.ReadToEnd();
                var lista = JsonConvert.DeserializeObject<List<Dictionary<string, bool>>>(informacion);

                for (int i = 0; i < lista.Count(); i++)
                {
                    var atributos = lista.ElementAt(i).ElementAt(0).Key.Split('_');
                    var valor = lista.ElementAt(i).ElementAt(0).Value;
                    var llave = lista.ElementAt(i).ElementAt(0).Key;

                    Calcomanias miCalcomania = new Calcomanias
                    {
                        Contenidos = valor,
                        Numero = atributos[1],
                        Pais = atributos[0]
                    };
                    Diccionario2.Add(llave, miCalcomania);

                }

            }
            return View();
        }
        public ActionResult Busqueda()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Busqueda(int estampa)
        {
            for (int i = 0; i < Diccionario1.Count; i++)
            {
               foreach(var item in Diccionario1.ElementAt(i).Value.faltantes)
               {
                    if(item  == estampa)
                    {
                        objmostrar.listaUbicación = "Faltantes";
                        objmostrar.estampita = estampa;
                        mostrar.Add(objmostrar);
                    }
               }
               foreach(var item in Diccionario1.ElementAt(i).Value.coleccionadas)
                {
                    if (item == estampa)
                    {
                        objmostrar.listaUbicación = "Coleccionadas";
                        objmostrar.estampita = estampa;
                        mostrar.Add(objmostrar);
                    }
                }
                foreach (var item in Diccionario1.ElementAt(i).Value.cambios)
                {
                    if (item == estampa)
                    {
                        objmostrar.listaUbicación = "Cambios";
                        objmostrar.estampita = estampa;
                        mostrar.Add(objmostrar);
                    }
                }
            }
            return View(mostrar);
        }
        public ActionResult Diccionario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Diccionario(HttpPostedFileBase Archivo)
        {
            string pathArchivo = string.Empty;
            if (Archivo != null)
            {
                Archivo.SaveAs(Server.MapPath("~/JSONFiles" + Path.GetFileName(Archivo.FileName)));
                StreamReader sr = new StreamReader(Server.MapPath("~/JSONFiles" + Path.GetFileName(Archivo.FileName)));
                var informacion = sr.ReadToEnd();
                var lista = JsonConvert.DeserializeObject<List<Dictionary<string, Paises>>>(informacion);

                for (int i = 0; i < lista.Count(); i++)
                {
                    var llave = lista.ElementAt(i).ElementAt(0).Key;
                    var pais = lista.ElementAt(i).ElementAt(0).Value;
                    Diccionario1.Add(llave, pais);
                }

            }
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}
