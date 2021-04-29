using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
namespace Sales_and_Inventory_System__Gadgets_Shop_
{
    public partial class frmLogin : Form
    {
        
        String cs = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\SIS_DB.accdb;";

        public frmLogin()
        {
            InitializeComponent();
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cbRole.Text == "")
            {
                MessageBox.Show("Please select User Role?", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbRole.Focus();
                return;
            }


            if (txtUserName.Text == "")
            {
                MessageBox.Show("Please enter user name", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUserName.Focus();
                return;
            }
            if (txtPassword.Text == "")
            {
                MessageBox.Show("Please enter password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Focus();
                return;
            }


            try
            {
                OleDbConnection myConnection = default(OleDbConnection);
                myConnection = new OleDbConnection(cs);

                OleDbCommand myCommand = default(OleDbCommand);

                myCommand = new OleDbCommand("SELECT Role, Username,User_password FROM Users WHERE Role = @userRole AND Username = @username AND User_password = @UserPassword", myConnection);
                OleDbParameter uRole = new OleDbParameter("@userRole", OleDbType.VarChar);
                OleDbParameter uName = new OleDbParameter("@username", OleDbType.VarChar);
                OleDbParameter uPassword = new OleDbParameter("@UserPassword", OleDbType.VarChar);

                uRole.Value = cbRole.Text;
                uName.Value = txtUserName.Text;
                uPassword.Value = txtPassword.Text;
                myCommand.Parameters.Add(uRole);
                myCommand.Parameters.Add(uName);
                myCommand.Parameters.Add(uPassword);
                Program.usuario = txtUserName.Text;
                Program.Role = cbRole.Text;
                myCommand.Connection.Open();

                OleDbDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);


                #region
                if (myReader.Read() == true)
                {
                    int i;
                    ProgressBar1.Visible = true;
                    ProgressBar1.Maximum = 5000;
                    ProgressBar1.Minimum = 0;
                    ProgressBar1.Value = 4;
                    ProgressBar1.Step = 1;

                    for (i = 0; i <= 5000; i++)
                    {
                        ProgressBar1.PerformStep();
                    }

                    this.Hide();
                    frmMainMenu frm = new frmMainMenu();
                    //Logged in as (Role)
                    frm.Show();
                    frm.lblUser.Text = cbRole.Text;
                   
                    //The following is user defined call for method disable 
                    //ignore the green highlight

                    if (cbRole.SelectedItem == "Manager") frm.disable();

                    if (cbRole.SelectedItem == "Employee") frm.disable1();

                }



                #endregion


                else
                {

                    MessageBox.Show("Login is Failed..Countercheck Role & User/Password !", "Login Denied", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    txtUserName.Clear();
                    txtPassword.Clear();

                    cbRole.Enabled = true;
                    cbRole.Focus();

                }
                if (myConnection.State == ConnectionState.Open)
                {
                    myConnection.Dispose();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ProgressBar1.Visible = false;
            txtUserName.Focus();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            frmChangePassword frm = new frmChangePassword();
            frm.Show();
            frm.txtUserName.Text = "";
            frm.txtNewPassword.Text = "";
            frm.txtOldPassword.Text = "";
            frm.txtConfirmPassword.Text = "";
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            frmRecoveryPassword frm = new frmRecoveryPassword();
            frm.txtEmail.Focus();
            frm.Show();
        }

    }
}
