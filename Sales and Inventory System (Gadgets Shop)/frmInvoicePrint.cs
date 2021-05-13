using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sales_and_Inventory_System__Gadgets_Shop_
{
	public partial class frmInvoicePrint : Form
	{
		Invoice _factura ;
		public frmInvoicePrint(Invoice factura)
		{
			InitializeComponent();
			_factura = factura;
			

			
		}

		private void frmInvoicePrint_Load(object sender, EventArgs e)
		{
			InvoiceBindingSource.DataSource = _factura;
			InvoiceDetailBindingSource.DataSource = _factura.Details;
			this.reportViewer1.RefreshReport();
		}

		private void reportViewer1_Load(object sender, EventArgs e)
		{

		}
	}
}
