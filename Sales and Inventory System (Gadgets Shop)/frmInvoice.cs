using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Security.Cryptography;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
namespace Sales_and_Inventory_System__Gadgets_Shop_
{
    public partial class frmInvoice : Form
    {
        Invoice invoice;
        OleDbCommand cmd;
        OleDbConnection con;
        OleDbDataReader rdr;
        String cs = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\SIS_DB.accdb;";
        

        public frmInvoice()
        {
            InitializeComponent();
        }
        private void auto()
        {
            label17.Text = "INV-" + GetUniqueKey(8);

        }
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars = "123456789".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCustomerID.Text == "")
                {
                    MessageBox.Show("Please retrieve Customer ID", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtCustomerID.Focus();
                    return;
                }

                if (txtTaxPer.Text == "")
                {
                    MessageBox.Show("Please enter tax percentage", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTaxPer.Focus();
                    return;
                }

                if (txtTotalPayment.Text == "")
                {
                    MessageBox.Show("Please enter total payment", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtTotalPayment.Focus();
                    return;
                }
                if (ListView1.Items.Count == 0)
                {
                    MessageBox.Show("sorry no product added", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                auto();
                con = new OleDbConnection(cs);
                con.Open();
                string ct = "select invoiceno from Sales where invoiceno=@find";
                cmd = new OleDbCommand(ct);
                cmd.Connection = con;
                cmd.Parameters.Add(new OleDbParameter("@find", System.Data.OleDb.OleDbType.VarChar, 20, "invoiceno"));
                cmd.Parameters["@find"].Value = label17.Text;
                rdr = cmd.ExecuteReader();
                if (rdr.Read() == true)
                {
                    MessageBox.Show("Invoice No. Already Exists", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if ((rdr != null))
                    {
                        rdr.Close();
                    }
                    return;
                }

                con = new OleDbConnection(cs);
                con.Open();

                string cb = "insert Into Sales(InvoiceNo,InvoiceDate,CustomerID,SubTotal,VATPercentage,VATAmount,GrandTotal,TotalPayment,PaymentDue,Remarks) VALUES ('" + label17.Text + "',#" + "#,'" + txtCustomerID.Text + "'," + txtSubTotal.Text + "," + txtTaxPer.Text + "," + txtTaxAmt.Text + "," + txtTotal.Text + "," + txtTotalPayment.Text + "," + txtPaymentDue.Text + ",'" + txtRemarks.Text + "')";
                cmd = new OleDbCommand(cb);
                cmd.Connection = con;
                cmd.ExecuteReader();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                con.Close();


                for (int i = 0; i <= ListView1.Items.Count - 1; i++)
                {
                    con = new OleDbConnection(cs);

                    string cd = "insert Into ProductSold(InvoiceNo,ConfigID,Quantity,Price,TotalAmount) VALUES (@InvoiceNo,@ConfigID,@Quantity,@Price,@Totalamount)";
                    cmd = new OleDbCommand(cd);
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("InvoiceNo", label17.Text);
                    cmd.Parameters.AddWithValue("ConfigID", ListView1.Items[i].SubItems[1].Text);
                    cmd.Parameters.AddWithValue("Quantity", ListView1.Items[i].SubItems[4].Text);
                    cmd.Parameters.AddWithValue("Price", ListView1.Items[i].SubItems[3].Text);
                    cmd.Parameters.AddWithValue("TotalAmount", ListView1.Items[i].SubItems[5].Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                for (int i = 0; i <= ListView1.Items.Count - 1; i++)
                {
                    con = new OleDbConnection(cs);
                    con.Open();
                    string cb1 = "update stock set Quantity = Quantity - " + ListView1.Items[i].SubItems[4].Text + " where ConfigID= " + ListView1.Items[i].SubItems[1].Text + "";
                    cmd = new OleDbCommand(cb1);
                    cmd.Connection = con;
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                for (int i = 0; i <= ListView1.Items.Count - 1; i++)
                {
                    con = new OleDbConnection(cs);
                    con.Open();

                    string cb2 = "update stock set TotalPrice = Totalprice - '" + ListView1.Items[i].SubItems[5].Text + "' where ConfigID= " + ListView1.Items[i].SubItems[1].Text + "";
                    cmd = new OleDbCommand(cb2);
                    cmd.Connection = con;
                    cmd.ExecuteReader();
                    con.Close();
                }

                Save.Enabled = false;
                btnPrint.Enabled = true;
                GetData();
                MessageBox.Show("Successfully saved", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmInvoice_Load(object sender, EventArgs e)
        {
            GetData();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmCustomersRecord1 frm = new frmCustomersRecord1();
            frm.label1.Text = label6.Text;
            frm.Visible=true;
        }

      
        private void txtSaleQty_TextChanged(object sender, EventArgs e)
        {
            int val1 = 0;
            int val2 = 0;
            int.TryParse(txtPrice.Text, out val1);
            int.TryParse(txtSaleQty.Text, out val2);
            int I = (val1 * val2);
            txtTotalAmount.Text = I.ToString();
        }

        public double subtot()
        {
            int i = 0;
            int j = 0;
            int k = 0;
            i = 0;
            j = 0;
            k = 0;


            try
            {
           
                j = ListView1.Items.Count;
                for (i = 0; i <= j - 1; i++)
                {
                    k = k + Convert.ToInt32(ListView1.Items[i].SubItems[5].Text);
                }
               
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return k;

        }

        private void Button7_Click(object sender, EventArgs e)
        {
            try
            {
                //if (txtCustomerID.Text == "")
                //{
                //    MessageBox.Show("Please retrieve Customer ID", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    txtCustomerID.Focus();
                //    return;
                //}

                //if (txtProductName.Text=="")
                //{
                //    MessageBox.Show("Please retrieve product name", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                //if (txtSaleQty.Text=="")
                //{
                //    MessageBox.Show("Please enter no. of sale quantity", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    txtSaleQty.Focus();
                //    return;
                //}
                //int SaleQty = Convert.ToInt32(txtSaleQty.Text);
                //if (SaleQty == 0)
                //{
                //    MessageBox.Show("no. of sale quantity can not be zero", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    txtSaleQty.Focus();
                //    return;
                //}
              
                if (ListView1.Items.Count==0)
                {
                   
                    ListViewItem lst = new ListViewItem();
                    lst.SubItems.Add(txtConfigID.Text);
                    lst.SubItems.Add(txtProductName.Text);
                    lst.SubItems.Add(txtPrice.Text);
                    lst.SubItems.Add(txtSaleQty.Text);
                    lst.SubItems.Add(txtTotalAmount.Text);
                    ListView1.Items.Add(lst);
                    txtSubTotal.Text = subtot().ToString();
                    if (txtTaxPer.Text != "")
                    {
                        txtTaxAmt.Text = Convert.ToInt32((Convert.ToInt32(txtSubTotal.Text) * Convert.ToDouble(txtTaxPer.Text) / 100)).ToString();
                        txtTotal.Text = (Convert.ToInt32(txtSubTotal.Text) + Convert.ToInt32(txtTaxAmt.Text)).ToString();
                    }
                    int val1 = 0;
                    int val2 = 0;
                    int.TryParse(txtTotal.Text, out val1);
                    int.TryParse(txtTotalPayment.Text, out val2);
                    int I = (val1 - val2);
                    txtPaymentDue.Text = I.ToString();
                    txtProductName.Text = "";
                    txtConfigID.Text = "";
                    txtPrice.Text = "";
                    txtAvailableQty.Text = "";
                    txtSaleQty.Text = "";
                    txtTotalAmount.Text = "";
                    textBox1.Text = "";
                    return;
                }

                //for (int j = 0; j <= ListView1.Items.Count - 1; j++)
                //{
                //    if (ListView1.Items[j].SubItems[1].Text == txtConfigID.Text)
                //    {
                //        ListView1.Items[j].SubItems[1].Text = txtConfigID.Text;
                //        ListView1.Items[j].SubItems[2].Text = txtProductName.Text;
                //        ListView1.Items[j].SubItems[3].Text = txtPrice.Text;
                //        ListView1.Items[j].SubItems[4].Text = (Convert.ToInt32(ListView1.Items[j].SubItems[4].Text) + Convert.ToInt32(txtSaleQty.Text)).ToString();
                //        ListView1.Items[j].SubItems[5].Text = (Convert.ToInt32(ListView1.Items[j].SubItems[5].Text) + Convert.ToInt32(txtTotalAmount.Text)).ToString();
                //        txtSubTotal.Text = subtot().ToString();
                //        if (txtTaxPer.Text != "")
                //        {
                //            txtTaxAmt.Text = Convert.ToInt32((Convert.ToInt32(txtSubTotal.Text) * Convert.ToDouble(txtTaxPer.Text) / 100)).ToString();
                //            txtTotal.Text = (Convert.ToInt32(txtSubTotal.Text) + Convert.ToInt32(txtTaxAmt.Text)).ToString();
                //        }
                //        int val1 = 0;
                //        int val2 = 0;
                //        int.TryParse(txtTotal.Text, out val1);
                //        int.TryParse(txtTotalPayment.Text, out val2);
                //        int I = (val1 - val2);
                //        txtPaymentDue.Text = I.ToString();
                //        txtProductName.Text = "";
                //        txtConfigID.Text = "";
                //        txtPrice.Text = "";
                //        txtAvailableQty.Text = "";
                //        txtSaleQty.Text = "";
                //        txtTotalAmount.Text = "";
                //        return;

                    //}
                //}
                   
                    ListViewItem lst1 = new ListViewItem();

                    lst1.SubItems.Add(txtConfigID.Text);
                    lst1.SubItems.Add(txtProductName.Text);
                    lst1.SubItems.Add(txtPrice.Text);
                    lst1.SubItems.Add(txtSaleQty.Text);
                    lst1.SubItems.Add(txtTotalAmount.Text);
                    ListView1.Items.Add(lst1);
                    txtSubTotal.Text = subtot().ToString();
                    if (txtTaxPer.Text != "")
                    {
                        txtTaxAmt.Text = Convert.ToInt32((Convert.ToInt32(txtSubTotal.Text) * Convert.ToDouble(txtTaxPer.Text) / 100)).ToString();
                        txtTotal.Text = (Convert.ToInt32(txtSubTotal.Text) + Convert.ToInt32(txtTaxAmt.Text)).ToString();
                    }
                    int val3 = 0;
                    int val4 = 0;
                    int.TryParse(txtTotal.Text, out val3);
                    int.TryParse(txtTotalPayment.Text, out val4);
                    int I1 = (val3 - val4);
                    txtPaymentDue.Text = I1.ToString();
                    txtProductName.Text = "";
                    txtConfigID.Text = "";
                    txtPrice.Text = "";
                    txtAvailableQty.Text = "";
                    txtSaleQty.Text = "";
                    txtTotalAmount.Text = "";
                    return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (ListView1.Items.Count == 0)
                {
                    MessageBox.Show("No items to remove", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int itmCnt = 0;
                    int i = 0;
                    int t = 0;

                    ListView1.FocusedItem.Remove();
                    itmCnt = ListView1.Items.Count;
                    t = 1;

                    for (i = 1; i <= itmCnt + 1; i++)
                    {
                        //Dim lst1 As New ListViewItem(i)
                        //ListView1.Items(i).SubItems(0).Text = t
                        t = t + 1;

                    }
                    txtSubTotal.Text = subtot().ToString();
                    if (txtTaxPer.Text != "")
                    {
                        txtTaxAmt.Text = Convert.ToInt32((Convert.ToInt32(txtSubTotal.Text) * Convert.ToDouble(txtTaxPer.Text) / 100)).ToString();
                        txtTotal.Text = (Convert.ToInt32(txtSubTotal.Text) + Convert.ToInt32(txtTaxAmt.Text)).ToString();
                    }
                    int val1 = 0;
                    int val2 = 0;
                    int.TryParse(txtTotal.Text, out val1);
                    int.TryParse(txtTotalPayment.Text, out val2);
                    int I = (val1 - val2);
                    txtPaymentDue.Text = I.ToString();
                }

                btnRemove.Enabled = false;
                if (ListView1.Items.Count == 0)
                {
                    txtSubTotal.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtTaxPer_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTaxPer.Text))
                {
                    txtTaxAmt.Text = "";
                    txtTotal.Text = "";
                    return;
                }
                txtTaxAmt.Text = Convert.ToInt32((Convert.ToInt32(txtSubTotal.Text) * Convert.ToDouble(txtTaxPer.Text) / 100)).ToString() ;
                txtTotal.Text = (Convert.ToInt32(txtSubTotal.Text) + Convert.ToInt32(txtTaxAmt.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnRemove.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con = new OleDbConnection(cs);
                con.Open();
                String sql = "SELECT StockID,Config.ConfigID,ProductName,Features,Price,sum(Quantity) from Stock,Config where Stock.ConfigID=Config.ConfigID and Productname like '" + textBox1.Text + "%' group by StockID,productname,Price,Features,Config.ConfigID having sum(quantity > 0) order by ProductName";
                cmd = new OleDbCommand(sql, con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3], rdr[4], rdr[5]);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string strRowNumber = (e.RowIndex + 1).ToString();
            SizeF size = e.Graphics.MeasureString(strRowNumber, this.Font);
            if (dataGridView1.RowHeadersWidth < Convert.ToInt32((size.Width + 20)))
            {
                dataGridView1.RowHeadersWidth = Convert.ToInt32((size.Width + 20));
            }
            Brush b = SystemBrushes.ControlText;
            e.Graphics.DrawString(strRowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));
     
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                DataGridViewRow dr = dataGridView1.SelectedRows[0];
                txtConfigID.Text = dr.Cells[3].Value.ToString();
                txtProductName.Text = dr.Cells[1].Value.ToString();
                txtPrice.Text = dr.Cells[2].Value.ToString();
                //txtAvailableQty.Text = dr.Cells[5].Value.ToString();
                txtSaleQty.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void GetData()
        {
            try
            {
                label17.Text = Program.usuario;
                label18.Text = DateTime.Today.ToString("D");
                con = new OleDbConnection(cs);
                con.Open();
                String sql = "SELECT * from Productos";
                cmd = new OleDbCommand(sql, con);
                rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dataGridView1.Rows.Clear();
                while (rdr.Read() == true)
                {
                    dataGridView1.Rows.Add(rdr[0], rdr[1], rdr[2], rdr[3]);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Reset()
        {
            label17.Text = "";
            //dtpInvoiceDate.Text = DateTime.Today.ToString();
            txtCustomerID.Text = "";
            txtCustomerName.Text = "";
            txtProductName.Text = "";
            txtConfigID.Text = "";
            txtPrice.Text = "";
            txtAvailableQty.Text = "";
            txtSaleQty.Text = "";
            txtTotalAmount.Text = "";
            ListView1.Items.Clear();
            txtSubTotal.Text = "";
            txtTaxPer.Text = "";
            txtTaxAmt.Text = "";
            txtTotal.Text = "";
            txtTotalPayment.Text = "";
            txtPaymentDue.Text = "";
            textBox1.Text = "";
            txtRemarks.Text = "";
            Save.Enabled = true;
            Delete.Enabled = false;
            btnUpdate.Enabled = false;
            btnRemove.Enabled = false;
            //btnPrint.Enabled = false;
            ListView1.Enabled = true;
            Button7.Enabled = true;

        }

        private void NewRecord_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you really want to delete this record?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                delete_records();
            }
        }
        private void delete_records()
        {

            try
            {

                int RowsAffected = 0;
                con = new OleDbConnection(cs);
                con.Open();
                string cq1 = "delete from productSold where InvoiceNo='" + label17.Text + "'";
                cmd = new OleDbCommand(cq1);
                cmd.Connection = con;
                RowsAffected = cmd.ExecuteNonQuery();
                con.Close();
                con = new OleDbConnection(cs);
                con.Open();
                string cq = "delete from Sales where InvoiceNo='" + label17.Text + "'";
                cmd = new OleDbCommand(cq);
                cmd.Connection = con;
                RowsAffected = cmd.ExecuteNonQuery();
                if (RowsAffected > 0)
                {
                    MessageBox.Show("Successfully deleted", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                }
                else
                {
                    MessageBox.Show("No Record found", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                }
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmInvoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            frmMainMenu frm = new frmMainMenu();
            if (label6.Text == "Manager") frm.disable();
            if (label6.Text == "Employee") frm.disable1();
            frm.lblUser.Text = label6.Text;
            frm.Show();
        }

        private void txtTotalPayment_TextChanged(object sender, EventArgs e)
        {
            int val1 = 0;
            int val2 = 0;
            int.TryParse(txtTotal.Text, out val1);
            int.TryParse(txtTotalPayment.Text, out val2);
            int I = (val1 - val2);
            txtPaymentDue.Text = I.ToString();
        }

        private void txtTotalPayment_Validating(object sender, CancelEventArgs e)
        {
            int val1 = 0;
            int val2 = 0;
            int.TryParse(txtTotal.Text, out val1);
            int.TryParse(txtTotalPayment.Text, out val2);
            if (val2 > val1)
            {
                MessageBox.Show("Total Payment can't be more than grand total", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTotalPayment.Text = "";
                txtPaymentDue.Text = "";
                txtTotalPayment.Focus();
                return;
            }
        }

        private void txtSaleQty_Validating(object sender, CancelEventArgs e)
        {

            //int val1 = 0;
            //int val2 = 0;
            //int.TryParse(txtAvailableQty.Text, out val1);
            //int.TryParse(txtSaleQty.Text, out val2);
            //if (val2 > val1)
            //{
            //    MessageBox.Show("Selling quantities are more than available quantities", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    txtSaleQty.Text = "";
            //    txtTotalAmount.Text = "";
            //    txtSaleQty.Focus();
            //    return;
            //}
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            //MessageBox.Show(ListView1.Items[1].SubItems[5].Text, "contenido");
            invoice = new Invoice();
            con = new OleDbConnection(cs);
            con.Open();
            for (int i = 0; i < ListView1.Items.Count; i++)
            {


                String sql = "UPDATE Stock SET Quantity = Quantity -" + int.Parse(ListView1.Items[i].SubItems[4].Text) + " where StockID = " + int.Parse(ListView1.Items[i].SubItems[1].Text);
                cmd = new OleDbCommand(sql, con);
                cmd.ExecuteNonQuery();
            }
            for (int i = 0; i<ListView1.Items.Count; i++)
            {
                invoice.Details.Add(
                    new InvoiceDetail
                    {
                        ProductCode = ListView1.Items[i].SubItems[2].Text,
                        UnitPrice = ListView1.Items[i].SubItems[3].Text,
                        Qty =ListView1.Items[i].SubItems[4].Text,
                        LineTotal = ListView1.Items[i].SubItems[5].Text

                }) ;

                //ListView1.Items.RemoveAt(ListView1.Items.Count - 1);
            }
            invoice.InvoiceTotal = txtSubTotal.Text;
            invoice.DateCreated = DateTime.Today;

            using (frmInvoicePrint frm = new frmInvoicePrint(invoice))
			{
                frm.ShowDialog();
			}

                try
                {

                    Cursor = Cursors.WaitCursor;
                    timer1.Enabled = true;

                    rptInvoice rpt = new rptInvoice();
                    //The report you created.
                    cmd = new OleDbCommand();
                    OleDbDataAdapter myDA = new OleDbDataAdapter();
                    DataSet myDS = new DataSet();
                    //The DataSet you created.
                    con = new OleDbConnection(cs);
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT Config.ConfigID, Config.ProductName, Config.Features, Config.Price, Sales.InvoiceNo, Sales.InvoiceDate, Sales.CustomerID, Sales.SubTotal,Sales.VATPercentage, Sales.VATAmount, Sales.GrandTotal, Sales.TotalPayment, Sales.PaymentDue, Sales.Remarks, ProductSold.ID,ProductSold.InvoiceNo AS Expr1, ProductSold.ConfigID AS Expr2, ProductSold.Quantity, ProductSold.Price AS Expr3, ProductSold.TotalAmount,Customer.CustomerID AS Expr4, Customer.CustomerName, Customer.Address, Customer.Landmark, Customer.City, Customer.State, Customer.ZipCode,Customer.Phone, Customer.MobileNo, Customer.FaxNo, Customer.Email, Customer.Notes FROM (((Customer INNER JOIN Sales ON Customer.CustomerID = Sales.CustomerID) INNER JOIN ProductSold ON Sales.InvoiceNo = ProductSold.InvoiceNo) INNER JOIN Config ON ProductSold.ConfigID = Config.ConfigID) where Sales.invoiceNo='" + label17.Text + "'";
                    cmd.CommandType = CommandType.Text;
                    //myDA.SelectCommand = cmd;
                    //myDA.Fill(myDS, "Config");
                    //myDA.Fill(myDS, "Sales");
                    //myDA.Fill(myDS, "ProductSold");
                    //myDA.Fill(myDS, "Customer");
                    //rpt.SetDataSource(myDS);
                    //frmInvoiceReport frm = new frmInvoiceReport();
                    //frm.crystalReportViewer1.ReportSource = rpt;
                    //frm.crystalReportViewer1.PrintReport();
                    //frm.Visible=true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            timer1.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
            con = new OleDbConnection(cs);
            con.Open();
            String cb = "update Sales set GrandTotal= " + txtTotal.Text + ",TotalPayment= " + txtTotalPayment.Text + ",PaymentDue= " + txtPaymentDue.Text + ",Remarks='" + txtRemarks.Text + "' where Invoiceno= '" + label17.Text + "'";
            cmd = new OleDbCommand(cb);
            cmd.Connection = con;
            cmd.ExecuteReader();
            con.Close();
            for (int i = 0; i <= ListView1.Items.Count - 1; i++)
            {
                con = new OleDbConnection(cs);
                string cd = "update ProductSold set Quantity=" + ListView1.Items[i].SubItems[4].Text + ",Price= " + ListView1.Items[i].SubItems[3].Text + ",TotalAmount= " + ListView1.Items[i].SubItems[5].Text + " where InvoiceNo='" + label17.Text + "' and ConfigID= " + ListView1.Items[i].SubItems[1].Text + "";
                cmd = new OleDbCommand(cd);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            for (int i = 0; i <= ListView1.Items.Count - 1; i++)
            {
                con = new OleDbConnection(cs);
                con.Open();
                string cb1 = "update stock set Quantity = Quantity - " + ListView1.Items[i].SubItems[4].Text + " where ConfigID= " + ListView1.Items[i].SubItems[1].Text + "";
                cmd = new OleDbCommand(cb1);
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            for (int i = 0; i <= ListView1.Items.Count - 1; i++)
            {
                con = new OleDbConnection(cs);
                con.Open();

                string cb2 = "update stock set TotalPrice = Totalprice - '" + ListView1.Items[i].SubItems[5].Text + "' where ConfigID= " + ListView1.Items[i].SubItems[1].Text + "";
                cmd = new OleDbCommand(cb2);
                cmd.Connection = con;
                cmd.ExecuteReader();
                con.Close();
            }
            GetData();
            btnUpdate.Enabled = false;
            MessageBox.Show("Successfully updated", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        catch (Exception ex)
            {
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmSalesRecord1 frm = new frmSalesRecord1();
            frm.DataGridView1.DataSource = null;
            frm.dtpInvoiceDateFrom.Text = DateTime.Today.ToString();
            frm.dtpInvoiceDateTo.Text = DateTime.Today.ToString();
            frm.GroupBox3.Visible = false;
            frm.DataGridView3.DataSource = null;
            frm.cmbCustomerName.Text = "";
            frm.GroupBox4.Visible = false;
            frm.DateTimePicker1.Text = DateTime.Today.ToString();
            frm.DateTimePicker2.Text = DateTime.Today.ToString();
            frm.DataGridView2.DataSource = null;
            frm.GroupBox10.Visible = false;
            frm.FillCombo();
            frm.label9.Text = label6.Text;
            frm.Show();
        }

		private void label17_TextChanged(object sender, EventArgs e)
		{

		}

		private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{

		}

		private void txtCustomerID_TextChanged(object sender, EventArgs e)
		{

		}

		private void txtTotalAmount_TextChanged(object sender, EventArgs e)
		{

		}

		private void GroupBox1_Enter(object sender, EventArgs e)
		{

		}

		private void txtSubTotal_TextChanged(object sender, EventArgs e)
		{

		}
	}
}