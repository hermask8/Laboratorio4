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
        public static List<Mostrar> mostrar = new List<Mostrar>();
        public static List<Mostrar> mostrarFaltantes = new List<Mostrar>();
        public static List<Mostrar> mostrarColeccionadas = new List<Mostrar>();
        public static List<Mostrar> mostrarCambios = new List<Mostrar>();
        public static List<Mostrar> mostrarDiccionario = new List<Mostrar>();
       
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

                for (int i = 0; i < lista.ElementAt(0).Count(); i++)
                {
                    var atributos = lista.ElementAt(0).ElementAt(i).Key.Split('_');
                    var valor = lista.ElementAt(0).ElementAt(i).Value;
                    var llave = lista.ElementAt(0).ElementAt(i).Key;

                    Calcomanias miCalcomania = new Calcomanias
                    {
                        Contenidos = valor,
                        Numero = int.Parse(atributos[1]),
                        Pais = atributos[0]
                    };
                    Diccionario2.Add(llave, miCalcomania);

                }

            }
            Refrescar();

            ReturnFaltantes();
            ReturnColeccionadas();
            ReturnCambios();
            ReturnDiccionario();
            return RedirectToAction("RetornarDiccionario");
        }
        public void Refrescar()
        {
            foreach (var item in Diccionario1)
            {
                foreach (var item2 in Diccionario2)
                {
                    if (item.Key == item2.Value.Pais)
                    {
                        if (item2.Value.Contenidos == true)
                        {
                            item.Value.cambios.Add(item2.Value.Numero);
                        }
                        else
                        {
                            item.Value.coleccionadas.Add(item2.Value.Numero);
                            item.Value.faltantes.Remove(item2.Value.Numero);
                        }
                    }
                }
            }
        }
        public ActionResult Busqueda()
        {
            mostrar.Clear();
            return View();
        }
        [HttpPost]
        public ActionResult Busqueda(int estampa)
        {

            Mostrar objmostrar = new Mostrar();
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
            return RedirectToAction("listadoBusqueda");
        }
        public ActionResult listadoBusqueda()
        {
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
                ReturnFaltantes();
                ReturnColeccionadas();
                ReturnCambios();
                ReturnDiccionario();
            }
            return RedirectToAction("RetornarDiccionario");
        }
        
        
        public ActionResult RetornarFaltantes()
        {
            return View( mostrarFaltantes);
        }
        public void ReturnFaltantes()
        {
            mostrarFaltantes.Clear();
            for (int i = 0; i < Diccionario1.Count; i++)
            {
                foreach (var item in Diccionario1.ElementAt(i).Value.faltantes)
                {
                    Mostrar objFaltantes = new Mostrar();
                    objFaltantes.listaUbicación = "Faltantes";
                    objFaltantes.estampita = item;
                    mostrarFaltantes.Add(objFaltantes);
                }
            }
        }
        
        public ActionResult RetornarColeccionadas()
        {
            return View(mostrarColeccionadas);
        }
        public void ReturnColeccionadas()
        {
            mostrarColeccionadas.Clear();
            for (int i = 0; i < Diccionario1.Count; i++)
            {
                foreach (var item in Diccionario1.ElementAt(i).Value.coleccionadas)
                {
                    Mostrar objColeccionadas = new Mostrar();
                    objColeccionadas.listaUbicación = "Coleccionadas";
                    objColeccionadas.estampita = item;
                    mostrarColeccionadas.Add(objColeccionadas);
                }
            }
        }
        
        public ActionResult RetornarCambios()
        { 
            return View(mostrarCambios);
        }
        public void ReturnCambios()
        {
            mostrarCambios.Clear();
            for (int i = 0; i < Diccionario1.Count; i++)
            {
                foreach (var item in Diccionario1.ElementAt(i).Value.cambios)
                {
                    Mostrar objCambios = new Mostrar();
                    objCambios.listaUbicación = "Cambios";
                    objCambios.estampita = item;
                    mostrarCambios.Add(objCambios);
                }
            }
        }
        public ActionResult RetornarDiccionario()
        {
            return View(mostrarDiccionario);
        }
        
        public void ReturnDiccionario()
        {
            mostrarDiccionario.Clear();
            foreach (var item in mostrarFaltantes)
            {
                mostrarDiccionario.Add(item); 
            }
            foreach(var item in mostrarColeccionadas)
            {
                mostrarDiccionario.Add(item);
            }
            foreach (var item in mostrarCambios)
            {
                mostrarDiccionario.Add(item);
            }
        }
        
        public ActionResult Index()
        {
            return View();
        }
    }
}
