using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using WAAOPOS.Models.ModelCliente;

namespace WAAOPOS.Controllers
{
    public class G
    {
        ModelCliente db = new ModelCliente();

        public bool VerificaDatosLogin(string Usuario, string Clave)
        {
            try
            {
                var Datos = db.Login.FirstOrDefault();
                if(Datos != null)
                {
                    if (Datos.Usuario == Usuario && Datos.Clave == Clave)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {

                return false;
            }
        }
        public void GuardarTxt(string nombreArchivo, string texto)
        {
            try
            {
                texto = (DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " " + texto + Environment.NewLine + "------------------------------------------" + Environment.NewLine);
                System.IO.File.AppendAllText(HttpContext.Current.Server.MapPath("~") + @"\Bitacora\" + nombreArchivo, texto);


            }
            catch { }
        }

        public static string ObtenerConfig(string v)
        {
            try
            {
                return WebConfigurationManager.AppSettings[v];
            }
            catch
            {
                return "";
            }
        }

        public static decimal Redondeo(decimal valor)
        {
            try
            {
                return Math.Round(valor, 2);
            }
            catch (Exception)
            {

                return valor;
            }
        }
    }
}