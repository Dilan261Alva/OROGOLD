using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebExamenFrontNovaTech.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Formulario(string Nombre, string Correo, string Telefono)
        {
            try
            {
                ValidarCamposVacios(Nombre, Correo, Telefono);
                ValidarCitasRepetidas(Correo, Nombre);
                BusinessAgregarCita(Nombre, Correo, Telefono);
                TempData["exito"] = $"Hola {Nombre}, tu cita se ha agregado correctamente";
                return View("Index");
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("Index");
            }
        }


        public void BusinessAgregarCita(string Nombre, string Correo, string Telefono)
        {
            if (DataAgregarCita(Nombre, Correo, Telefono) != 1)
            {
                throw new ApplicationException("Error al agregar cliente");
            }
        }


        public int DataAgregarCita(string Nombre, string Correo, string Telefono)
        {
                int filas = 0;
                SqlConnection con = new SqlConnection("data source=DESKTOP-GQQG16F; database=dilan; user id=dilan; password=12345;");
                SqlCommand com = new SqlCommand($"insert into Clientes values('{Nombre}' , '{Correo}' , '{Telefono}')", con);
            try
            {
                con.Open();
                filas = com.ExecuteNonQuery();
                con.Close();
                return filas;
            }
            catch (Exception)
            {
                con.Close();
                throw;
            }
        }


        public void ValidarCitasRepetidas(string Correo, string Nombre)
        {
            DataTable dt = BuscarCitasRepetidas(Correo);
            if (dt.Rows.Count != 0)
            {
                throw new ApplicationException($"Hola {Nombre}, Ya existe una cita con tu información");
            }
        }


        public DataTable BuscarCitasRepetidas(string Correo)
        {
            SqlConnection con = new SqlConnection("data source=DESKTOP-GQQG16F; database=dilan; user id=dilan; password=12345;");
            SqlDataAdapter da = new SqlDataAdapter($"Select * from Clientes where Correo = '{Correo}'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


        public void ValidarCamposVacios(string Nombre, string Correo, string Telefono)
        {
            string aux = "";
            if (Nombre == "")
            {
                aux += "Debes ingresar tu nombre <br />";
            }
            if (Correo == "")
            {
                aux += "Debes ingresar tu correo electronico <br />";
            }
            if (Telefono == "")
            {
                aux += "Debes ingresar tu teléfono";
            }
            if (aux != "")
            {
                throw new ApplicationException(aux);
            }
        }


    }
}