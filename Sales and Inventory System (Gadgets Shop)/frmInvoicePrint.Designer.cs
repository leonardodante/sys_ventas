namespace Sales_and_Inventory_System__Gadgets_Shop_
{
	partial class frmInvoicePrint
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
			Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
			this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
			this.InvoiceBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.InvoiceDetailBindingSource = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.InvoiceBindingSource)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.InvoiceDetailBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// reportViewer1
			// 
			this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
			reportDataSource1.Name = "DataSet1";
			reportDataSource1.Value = this.InvoiceBindingSource;
			reportDataSource2.Name = "DataSet2";
			reportDataSource2.Value = this.InvoiceDetailBindingSource;
			this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
			this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
			this.reportViewer1.LocalReport.ReportEmbeddedResource = "Sales_and_Inventory_System__Gadgets_Shop_.Factura.rdlc";
			this.reportViewer1.Location = new System.Drawing.Point(0, 0);
			this.reportViewer1.Name = "reportViewer1";
			this.reportViewer1.ServerReport.BearerToken = null;
			this.reportViewer1.Size = new System.Drawing.Size(800, 450);
			this.reportViewer1.TabIndex = 0;
			this.reportViewer1.Load += new System.EventHandler(this.reportViewer1_Load);
			// 
			// InvoiceBindingSource
			// 
			this.InvoiceBindingSource.DataSource = typeof(Sales_and_Inventory_System__Gadgets_Shop_.Invoice);
			// 
			// InvoiceDetailBindingSource
			// 
			this.InvoiceDetailBindingSource.DataSource = typeof(Sales_and_Inventory_System__Gadgets_Shop_.InvoiceDetail);
			// 
			// frmInvoicePrint
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.reportViewer1);
			this.Name = "frmInvoicePrint";
			this.Text = "frmInvoicePrint";
			this.Load += new System.EventHandler(this.frmInvoicePrint_Load);
			((System.ComponentModel.ISupportInitialize)(this.InvoiceBindingSource)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.InvoiceDetailBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
		private System.Windows.Forms.BindingSource InvoiceBindingSource;
		private System.Windows.Forms.BindingSource InvoiceDetailBindingSource;
	}
}