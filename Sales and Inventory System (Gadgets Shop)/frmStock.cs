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
namespace Sales_and_Inventory_System__Gadgets_Shop_
{
    public partial class frmStock : Form
    {
        OleDbConnection con = null;
        OleDbCommand cmd = null;
        OleDbDataReader rdr;
        String cs = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\SIS_DB.accdb;";
        public frmStock()
        {
            InitializeComponent();
        }

        private void frmStock_Load(object sender, EventArgs e)
        {
            GetData();
        }
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "123456789".ToCharArray();
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
        private void auto()
        {
            txtStockID.Text = "S-" + GetUniqueKey(6);
        }
        public void GetData()
        {
            try
            {
                con = new OleDbConnection(cs);
                con.Open();
                cmd = new OleDbCommand("SELECT StockId as [Stock ID], (productName) as [Product Name],Features,sum(Quantity) as [Quantity],Price,sum(TotalPrice) as [Total Price] from Config,Stock where Config.ConfigID=Stock.ConfigID group by Stockid, productname,features,price having sum(Quantity > 0)  order by Productname", con);
                OleDbDataAdapter myDA = new OleDbDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Stock");
                myDA.Fill(myDataSet, "Config");
                dataGridView1.DataSource = myDataSet.Tables["Stock"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Config"].DefaultView;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmConfigRecord frm = new frmConfigRecord();
            frm.label1.Text = label8.Text;
            frm.Show();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtProductname.Text == "")
            {
                MessageBox.Show("Please retrieve product name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtProductname.Focus();
                return;
            }
            if (txtQty.Text == "")
            {
                MessageBox.Show("Please enter quantity", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtQty.Focus();
                return;
            }

            try
            {
            con = new OleDbConnection(cs);
            con.Open();
            String ct = "select ConfigID  from stock where ConfigID=" + txtConfigID.Text +"";
            cmd = new OleDbCommand(ct);
            cmd.Connection = con;
            rdr = cmd.ExecuteReader();

            if (rdr.Read()==true) 
                {
                MessageBox.Show("Record already exists" + "\n" + "please update the stock of product", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if ((rdr != null))
                {
                    rdr.Close();
                }
                return;
                }
                auto();
                con = new OleDbConnection(cs);
                con.Open();
                string cb = "insert into Stock(StockID,ConfigID,Quantity,Totalprice,StockDate) VALUES ('" + txtStockID.Text + "'," + txtConfigID.Text + "," + txtQty.Text + "," + txtTotalPrice.Text +",#" + dtpStockDate.Value + "#)";
                cmd = new OleDbCommand(cb);
                cmd.Connection = con;
                cmd.ExecuteReader();
                con.Close();
                MessageBox.Show("Successfully saved", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
                GetData();
                frmStockin frm = new frmStockin();
                frm.GetData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            int val1 = 0;
            int val2 = 0;
            int.TryParse(txtPrice.Text, out val1);
            int.TryParse(txtQty.Text, out val2);
            int I = (val1 * val2);
            txtTotalPrice.Text = I.ToString();
        }
        private void Reset()
        {
            txtPrice.Text = "";
            txtFeatures.Text = "";
            txtProductname.Text = "";
            txtQty.Text = "";
            txtTotalPrice.Text = "";
            txtStockID.Text = "";
            dtpStockDate.Text = DateTime.Today.ToString();
            txtProduct.Text = "";
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnDelete_Click(object sender, EventArgs e)
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
                string cq = "delete from Stock where StockID='" + txtStockID.Text + "'";
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

        private void frmStock_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            frmMainMenu frm = new frmMainMenu();
            frm.lblUser.Text = label8.Text;
            frm.Show();
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmStockRecord1 frm = new frmStockRecord1();
            frm.label1.Text = label8.Text;
            frm.Show();
            frm.GetData();
        }

        private void txtProduct_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con = new OleDbConnection(cs);
                con.Open();
                cmd = new OleDbCommand("SELECT StockId as [Stock ID], (productName) as [Product Name],Features,sum(Quantity) as [Quantity],Price,sum(TotalPrice) as [Total Price] from Config,Stock where Config.ConfigID=Stock.ConfigID and productname like '" + txtProduct.Text + "%' group by Stockid, productname,features,price having sum(quantity > 0)  order by Productname", con);
                OleDbDataAdapter myDA = new OleDbDataAdapter(cmd);
                DataSet myDataSet = new DataSet();
                myDA.Fill(myDataSet, "Stock");
                myDA.Fill(myDataSet, "Config");
                dataGridView1.DataSource = myDataSet.Tables["Stock"].DefaultView;
                dataGridView1.DataSource = myDataSet.Tables["Config"].DefaultView;
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
         
                con = new OleDbConnection(cs);
                con.Open();
                string cb = "Update Stock set ConfigID=" + txtConfigID.Text + ",Quantity=" + txtQty.Text + ",Totalprice=" + txtTotalPrice.Text + ",StockDate= #" + dtpStockDate.Value + "# where StockID='" + txtStockID.Text + "'";
                cmd = new OleDbCommand(cb);
                cmd.Connection = con;
                cmd.ExecuteReader();
                con.Close();
                MessageBox.Show("Successfully updated", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnUpdate.Enabled = false;
                GetData();
                frmStockin frm = new frmStockin();
                frm.GetData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmStockRecord1 frm = new frmStockRecord1();
            frm.label1.Text = label8.Text;
            frm.Show();
            frm.GetData();
        }

       
    }
}
