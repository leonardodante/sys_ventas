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
    public partial class frmCustomers : Form
    {
        OleDbDataReader rdr = null;
        OleDbConnection con = null;
        OleDbCommand cmd = null;
        String cs = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\SIS_DB.accdb;";

        public frmCustomers()
        {
            InitializeComponent();
        }
        private void Reset()
        {
            txtAddress.Text = "";
            txtCity.Text = "";
            txtEmail.Text = "";
            txtFaxNo.Text = "";
            txtCustomerName.Text = "";
            txtLandmark.Text = "";
            txtMobileNo.Text = "";
            txtNotes.Text = "";
            txtPhone.Text = "";
            txtCustomerID.Text = "";
            txtZipCode.Text = "";
            cmbState.Text = "";
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            btnUpdate.Enabled = false;
            txtCustomerName.Focus();

        }
        private void frmCustomers_Load(object sender, EventArgs e)
        {

        }
        private void auto()
        {
            txtCustomerID.Text = "C-" + GetUniqueKey(6);
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
        private void txtZipCode_Validating(object sender, CancelEventArgs e)
        {
            if (txtZipCode.TextLength > 6)
            {
                MessageBox.Show("Only 6 digits are allowed", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtZipCode.Focus();
            }
        }

        private void txtZipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            if (txtMobileNo.TextLength > 10)
            {
                MessageBox.Show("Only 10 digits are allowed", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMobileNo.Focus();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCustomerName.Text == "")
            {
                MessageBox.Show("Please enter name", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCustomerName.Focus();
                return;
            }

            if (txtAddress.Text == "")
            {
                MessageBox.Show("Please enter address", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAddress.Focus();
                return;
            }
            if (txtCity.Text == "")
            {
                MessageBox.Show("Please enter city", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtCity.Focus();
                return;
            }
            if (cmbState.Text == "")
            {
                MessageBox.Show("Please select Province", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cmbState.Focus();
                return;
            }
            if (txtZipCode.Text == "")
            {
                MessageBox.Show("Please enter zip/post code", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtZipCode.Focus();
                return;
            }


            if (txtMobileNo.Text == "")
            {
                MessageBox.Show("Please enter mobile no.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMobileNo.Focus();
                return;
            }

            try
            {
                auto();
                con = new OleDbConnection(cs);
                con.Open();
                string ct = "select CustomerID from Customer where CustomerID=@find";

                cmd = new OleDbCommand(ct);
                cmd.Connection = con;
                cmd.Parameters.Add(new OleDbParameter("@find", System.Data.OleDb.OleDbType.VarChar, 20, "CustomerID"));
                cmd.Parameters["@find"].Value = txtCustomerID.Text;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    MessageBox.Show("Customer ID Already Exists", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    if ((rdr != null))
                    {
                        rdr.Close();
                    }


                }
                else
                {


                    con = new OleDbConnection(cs);
                    con.Open();

                    string cb = "insert into Customer(CustomerID,Customername,address,landmark,city,state,zipcode,Phone,email,mobileno,faxno,notes) VALUES (@d1,@d2,@d4,@d5,@d6,@d7,@d8,@d9,@d10,@d11,@d12,@d13)";

                    cmd = new OleDbCommand(cb);

                    cmd.Connection = con;

                    cmd.Parameters.Add(new OleDbParameter("@d1", System.Data.OleDb.OleDbType.VarChar, 20, "CustomerID"));
                    cmd.Parameters.Add(new OleDbParameter("@d2", System.Data.OleDb.OleDbType.VarChar, 100, "Customername"));
                    cmd.Parameters.Add(new OleDbParameter("@d4", System.Data.OleDb.OleDbType.VarChar, 250, "address"));
                    cmd.Parameters.Add(new OleDbParameter("@d5", System.Data.OleDb.OleDbType.VarChar, 250, "landmark"));

                    cmd.Parameters.Add(new OleDbParameter("@d6", System.Data.OleDb.OleDbType.VarChar, 50, "city"));

                    cmd.Parameters.Add(new OleDbParameter("@d7", System.Data.OleDb.OleDbType.VarChar, 50, "state"));

                    cmd.Parameters.Add(new OleDbParameter("@d8", System.Data.OleDb.OleDbType.VarChar, 10, "zipcode"));

                    cmd.Parameters.Add(new OleDbParameter("@d9", System.Data.OleDb.OleDbType.VarChar, 15, "phone"));

                    cmd.Parameters.Add(new OleDbParameter("@d10", System.Data.OleDb.OleDbType.VarChar, 150, "email"));

                    cmd.Parameters.Add(new OleDbParameter("@d11", System.Data.OleDb.OleDbType.VarChar, 15, "mobileno"));

                    cmd.Parameters.Add(new OleDbParameter("@d12", System.Data.OleDb.OleDbType.VarChar, 15, "faxno"));

                    cmd.Parameters.Add(new OleDbParameter("@d13", System.Data.OleDb.OleDbType.VarChar, 250, "Remarks"));


                    cmd.Parameters["@d1"].Value = txtCustomerID.Text;
                    cmd.Parameters["@d2"].Value = txtCustomerName.Text;
                    cmd.Parameters["@d4"].Value = txtAddress.Text;
                    cmd.Parameters["@d5"].Value = txtLandmark.Text;
                    cmd.Parameters["@d6"].Value = txtCity.Text;
                    cmd.Parameters["@d7"].Value = cmbState.Text;
                    cmd.Parameters["@d8"].Value = txtZipCode.Text;
                    cmd.Parameters["@d9"].Value = txtPhone.Text;
                    cmd.Parameters["@d10"].Value = txtEmail.Text;
                    cmd.Parameters["@d11"].Value = txtMobileNo.Text;
                    cmd.Parameters["@d12"].Value = txtFaxNo.Text;
                    cmd.Parameters["@d13"].Value = txtNotes.Text;

                    cmd.ExecuteReader();
                    MessageBox.Show("Successfully saved", "Customer Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnSave.Enabled = false;
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }

                    con.Close();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void delete_records()
        {

            try
            {

              int RowsAffected = 0;
              con = new OleDbConnection(cs);
              con.Open();
              string cq = "delete from Customer where CustomerID=@DELETE1;";
              cmd = new OleDbCommand(cq);
              cmd.Connection = con;
              cmd.Parameters.Add(new OleDbParameter("@DELETE1", System.Data.OleDb.OleDbType.VarChar, 20, "CustomerID"));
              cmd.Parameters["@DELETE1"].Value = txtCustomerID.Text;
                RowsAffected = cmd.ExecuteNonQuery();

                if (RowsAffected > 0)
                {
                    MessageBox.Show("Successfully deleted", "Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                }
                else
                {
                    MessageBox.Show("No record found", "Sorry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Reset();
                }
                    con.Close();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {


                if (MessageBox.Show("Do you really want to delete the record?", "Customer Record", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    delete_records();
                }

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

                string cb = "update Customer set Customername = '" + txtCustomerName.Text + "',address= '" + txtAddress.Text + "',landmark= '" + txtLandmark.Text + "',city= '" + txtCity.Text + "',state= '" + cmbState.Text + "',zipcode= '" + txtZipCode.Text + "',Phone= '" + txtPhone.Text + "',email= '" + txtEmail.Text + "',mobileno= '" + txtMobileNo.Text + "',faxno= '" + txtFaxNo.Text + "',notes= '" + txtNotes.Text + "' where CustomerID= '" + txtCustomerID.Text + "'";
                cmd = new OleDbCommand(cb);
                cmd.Connection = con;
                cmd.ExecuteReader();
                MessageBox.Show("Successfully updated", "Customer Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnUpdate.Enabled = false;
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtFaxNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmCustomersRecord2 frm = new frmCustomersRecord2();
            frm.Show();
            frm.GetData();
        }
    }
}
