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
    public class OrdenesVentasController : ApiController
    {
        Conexion g = new Conexion();
        G G = new G();
        ModelCliente db = new ModelCliente();

        object resp;
        [Route("api/OrdenesVentas/Insertar")]
        public async Task<HttpResponseMessage> Post([FromUri] string Usuario, string Clave, [FromBody] OrdenVenta orden)
        {
            try
            {
                if (G.VerificaDatosLogin(Usuario, Clave))
                {
                    Parametros param = db.Parametros.FirstOrDefault();
                    var client = (Documents)Conexion.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                    client.DocObjectCode = BoObjectTypes.oOrders;
                    client.CardCode = orden.CardCode;
                    client.DocCurrency = (orden.Currency == "CRC" ? "COL" : orden.Currency);
                    client.DocDate = orden.CreationDate;
                    client.DocDueDate = orden.DocDueDate;


                    if (orden.Items)
                    {
                        client.DocType = BoDocumentTypes.dDocument_Items;
                    }
                    else
                    {
                        client.DocType = BoDocumentTypes.dDocument_Service;
                    }


                    client.HandWritten = BoYesNoEnum.tNO;
                    client.NumAtCard = orden.CardCode; //orderid 
                    client.ReserveInvoice = BoYesNoEnum.tNO;
                    client.Series = param.SerieOV;
                    client.TaxDate = orden.CreationDate; //CreationDate 
                    client.Comments = orden.Comments;

                    if (orden.SalesPerson != 0)
                    {
                        client.SalesPersonCode = orden.SalesPerson;

                    }

                    int i = 0;
                    foreach (var item in orden.detalle)
                    {
                        client.Lines.SetCurrentLine(i);
                        client.Lines.Currency = orden.Currency;
                        client.Lines.DiscountPercent = item.discountPercent;
                        client.Lines.ItemCode = item.itemCode;
                        client.Lines.Quantity = Convert.ToDouble(item.quantity);
                        client.Lines.TaxCode = item.taxCode;
                        client.Lines.TaxOnly = BoYesNoEnum.tNO;
                        client.Lines.UnitPrice = Convert.ToDouble(item.unitPrice);
                        client.Lines.WarehouseCode = item.wareHouseCode;

                        foreach (var lot in item.lotes)
                        {

                            client.Lines.BatchNumbers.ItemCode = item.itemCode;
                            client.Lines.BatchNumbers.Quantity = Convert.ToDouble(lot.Quantity);
                            client.Lines.BatchNumbers.BatchNumber = lot.BatchNumber;
                            client.Lines.BatchNumbers.Add();
                        }

                        client.Lines.Add();
                        i++;
                    }

                    var respuesta = client.Add();
                    if (respuesta == 0)
                    {
                        resp = new
                        {

                            Type = "Orden de Venta",
                            Status = "Exitoso",
                            Message = "Orden creada exitosamente en SAP",
                            User = Conexion.Company.UserName,
                            DocEntry = Conexion.Company.GetNewObjectKey()
                        };
                    }
                    else
                    {
                        resp = new
                        {

                            Type = "Orden de Venta",
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
                        bitacora.Descripcion = "Se activa el api de Ordenes Venta con la informacion " + JsonConvert.SerializeObject(orden);
                        bitacora.IP = HttpContext.Current.Request.UserHostAddress;
                        bitacora.Metodo = "INSERTAR ORDEN VENTA";
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
            catch(Exception ex)
            {
                 

                BitacoraErrores be = new BitacoraErrores();
                be.Descripcion = ex.Message;
                be.Metodo = "INSERTAR ORDEN VENTA";
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