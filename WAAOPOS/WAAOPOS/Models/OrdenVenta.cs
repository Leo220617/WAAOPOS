using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WAAOPOS.Models
{
    public class OrdenVenta
    {
        public string CardCode { get; set; }
        public string Currency { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public string Comments { get; set; }
        public int SalesPerson { get; set; }
        public bool Items { get; set; }
        public Detalle[] detalle { get; set; }
    }
    public class Detalle
    {
        public string itemCode { get; set; }
        public decimal quantity { get; set; }
        public string taxCode { get; set; }
        public decimal unitPrice { get; set; }
        public string wareHouseCode { get; set; }
        public int discountPercent { get; set; }
        public Lotes[] lotes { get; set; }

    }
    public class Lotes
    {
        public string BatchNumber { get; set; }
        public decimal Quantity { get; set; }
    }
}