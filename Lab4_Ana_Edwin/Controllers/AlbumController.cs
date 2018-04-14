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
        public static Dictionary<string, Paises> Diccionario1 = new Dictionary<string, Paises>();
        public static Dictionary<string, Calcomanias> Diccionario2 = new Dictionary<string, Calcomanias>();
        // GET: Album
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase Archivo)
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
                    Diccionario1.Add(llave,pais);
                }
                
            }
            return View();
        }

        // GET: Album/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Album/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Album/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Album/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Album/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Album/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Album/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
