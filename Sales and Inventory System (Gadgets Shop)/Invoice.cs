using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sales_and_Inventory_System__Gadgets_Shop_
{
	public class Invoice
	{

        
        public DateTime DateCreated { get; set; }
        
        public string InvoiceTotal { get; set; }
        
        public List<InvoiceDetail> Details { get; set; } = new List<InvoiceDetail>();

    }

    public class InvoiceDetail
    {
        
        public string ProductCode { get; set; }
        public string UnitPrice { get; set; }
        public string Qty { get; set; }
        public string LineTotal { get; set; }
    }


}
