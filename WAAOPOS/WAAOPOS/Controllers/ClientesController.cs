using Newtonsoft.Json;
using SAPbobsCOM;
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
using WAAOPOS.Models;
using WAAOPOS.Models.ModelCliente;

namespace WAAOPOS.Controllers
{
    public class ClientesController : ApiController
    {
        Conexion g = new Conexion();
        G G = new G();
        ModelCliente db = new ModelCliente();
        object resp;

        public async Task<HttpResponseMessage> Get([FromUri] string Usuario, string Clave)
        {


            try
            {
                if (G.VerificaDatosLogin(Usuario, Clave))
                {
                    string SQL = "";



                    SQL = db.Parametros.FirstOrDefault().SQLClientes;

                    SqlConnection Cn = new SqlConnection(g.DevuelveCadena());
                    SqlCommand Cmd = new SqlCommand(SQL, Cn);
                    SqlDataAdapter Da = new SqlDataAdapter(Cmd);
                    DataSet Ds = new DataSet();
                    Cn.Open();
                    Da.Fill(Ds, "Clientes");

                    Cn.Close();

                    try
                    {
                        BitacoraUso bitacora = new BitacoraUso();
                        bitacora.Descripcion = "Se activa el api de Clientes";
                        bitacora.IP = HttpContext.Current.Request.UserHostAddress;
                        bitacora.Metodo = "GET CLIENTES";
                        bitacora.Respuesta = JsonConvert.SerializeObject(Ds);
                        bitacora.Fecha = DateTime.Now;
                        db.BitacoraUso.Add(bitacora);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {


                    }

                    return Request.CreateResponse(HttpStatusCode.OK, Ds);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);

                }


            }
            catch (Exception ex)
            {
                BitacoraErrores be = new BitacoraErrores();
                be.Descripcion = ex.Message;
                be.StackTrace = ex.StackTrace;
                be.Metodo = "GET CLIENTES";
                be.Fecha = DateTime.Now;
                db.BitacoraErrores.Add(be);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.InternalServerError, be);
            }


        }

        [Route("api/Clientes/Insertar")]
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromUri] string Usuario, string Clave, [FromBody] Clientes cliente)
        {
            try
            {
                if (G.VerificaDatosLogin(Usuario, Clave))
                {
                    Parametros param = db.Parametros.FirstOrDefault();
                    var client = (SAPbobsCOM.BusinessPartners)Conexion.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);


                    client.CardName = cliente.CardName;
                    client.EmailAddress = cliente.Email;
                    client.Series = param.SerieCliente;
                    client.CardForeignName = cliente.Identificacion;
                    client.FederalTaxID = cliente.Identificacion;
                    client.Currency = "USD";
                    client.Phone1 = cliente.Telefono;
                    client.CardType = BoCardTypes.cCustomer;
                    if (!string.IsNullOrEmpty(param.CampoTipoIdentificacion))
                    {
                        client.UserFields.Fields.Item(param.CampoTipoIdentificacion).Value = cliente.TipoIdentificacion;
                    }
                    client.Addresses.Add();
                    client.Addresses.SetCurrentLine(0);
                    client.Addresses.AddressName = "Direccion";
                    client.Addresses.Street = cliente.Ubicacion.Calle;

                    client.Addresses.County = cliente.Ubicacion.Ciudad;
                    client.Address = cliente.Ubicacion.Ciudad;


                    client.ContactPerson = cliente.PersonaContacto.Nombre;
                    client.ContactEmployees.Add();
                    client.ContactEmployees.SetCurrentLine(0);
                    client.ContactEmployees.Name = cliente.PersonaContacto.Nombre;
                    client.ContactEmployees.Phone1 = cliente.PersonaContacto.Telefono;
                    client.ContactEmployees.E_Mail = cliente.PersonaContacto.Email;

                    var respuesta = client.Add();
                    if (respuesta == 0)
                    {
                        resp = new
                        {

                            Type = "Cliente",
                            Status = "Exitoso",
                            Message = "Cliente exitosamente en SAP",
                            User = Conexion.Company.UserName,
                            DocEntry = Conexion.Company.GetNewObjectKey()
                        };
                    }
                    else
                    {
                        resp = new
                        {

                            Type = "Cliente",
                            Status = "Error",
                            Message = Conexion.Company.GetLastErrorDescription(),
                            User = Conexion.Company.UserName,
                            DocEntry = 0
                        };
                    }

                    Conexion.Desconectar();
                    try
                    {
                        BitacoraUso bitacora = new BitacoraUso();
                        bitacora.Descripcion = "Se activa el api de insertar Clientes con la informacion " + JsonConvert.SerializeObject(cliente);
                        bitacora.IP = HttpContext.Current.Request.UserHostAddress;
                        bitacora.Metodo = "INSERTAR CLIENTE";
                        bitacora.Respuesta = JsonConvert.SerializeObject(resp);
                        bitacora.Fecha = DateTime.Now;
                        db.BitacoraUso.Add(bitacora);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {


                    }
                    return Request.CreateResponse(HttpStatusCode.OK, resp);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);

                }

            }
            catch (Exception ex)
            {

                try
                {
                    BitacoraUso bitacora = new BitacoraUso();
                    bitacora.Descripcion = "Se activa el api de insertar Clientes con la informacion " + JsonConvert.SerializeObject(cliente);
                    bitacora.IP = HttpContext.Current.Request.UserHostAddress;
                    bitacora.Metodo = "INSERTAR CLIENTE";
                    bitacora.Respuesta = JsonConvert.SerializeObject(ex);
                    bitacora.Fecha = DateTime.Now;
                    db.BitacoraUso.Add(bitacora);
                    db.SaveChanges();
                }
                catch (Exception ex2) 
                {

                }

                BitacoraErrores be = new BitacoraErrores();
                be.Descripcion = ex.Message;
                be.Metodo = "INSERTAR CLIENTE";
                be.StackTrace = ex.StackTrace;
                be.Fecha = DateTime.Now;
                db.BitacoraErrores.Add(be);
                db.SaveChanges();

                Conexion.Desconectar();

                return Request.CreateResponse(HttpStatusCode.InternalServerError, be);
            }
        }

    }
}