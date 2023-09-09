using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WAAOPOS.Models.ModelCliente;

namespace WAAOPOS.Controllers
{
    public class ProductosController: ApiController
    {
        Conexion g = new Conexion();
        G G = new G();
        ModelCliente db = new ModelCliente();

        public async Task<HttpResponseMessage> Get([FromUri] string Usuario, string Clave)
        {


            try
            {

                if(G.VerificaDatosLogin(Usuario,Clave))
                {
                    string SQL = "";


                    SQL = db.Parametros.FirstOrDefault().SQLProductos;

                    SqlConnection Cn = new SqlConnection(g.DevuelveCadena());
                    SqlCommand Cmd = new SqlCommand(SQL, Cn);
                    SqlDataAdapter Da = new SqlDataAdapter(Cmd);
                    DataSet Ds = new DataSet();
                    Cn.Open();
                    Da.Fill(Ds, "Productos");

                    Cn.Close();
                    try
                    {
                        BitacoraUso bitacora = new BitacoraUso();
                        bitacora.Descripcion = "Se activa el api de Productos";
                        bitacora.IP = HttpContext.Current.Request.UserHostAddress;
                        bitacora.Metodo = "GET PRODUCTOS";
                        bitacora.Respuesta = JsonConvert.SerializeObject(Ds);
                        bitacora.Fecha = DateTime.Now;
                        db.BitacoraUso.Add(bitacora);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {


                    }
                    return Request.CreateResponse(HttpStatusCode.OK, Ds);
                }else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);

                }



            }
            catch (Exception ex)
            {
                BitacoraErrores be = new BitacoraErrores();
                be.Descripcion = ex.Message;
                be.StackTrace = ex.StackTrace;
                be.Metodo = "GET PRODUCTOS";
                be.Fecha = DateTime.Now;
                db.BitacoraErrores.Add(be);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.InternalServerError, be);
            }


        }
    }
}