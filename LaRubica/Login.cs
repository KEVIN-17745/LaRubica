using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaRubica
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUser.Text;
            string password = txtPass.Text;

            string query = "SELECT password FROM utenti WHERE username = @Username";

            using (SqlConnection connection = new SqlConnection(DatabaseConfig.GetConnectionString()))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username ?? string.Empty);


                try
                {
                    connection.Open();
                    Dominio USER = new Dominio(username, null);
                    USER.Password = (command.ExecuteScalar()?.ToString() ?? "");

                    if (string.IsNullOrEmpty(USER.Password))
                    {
                        MessageBox.Show("Utente non trovato", "Login errato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    else if (USER.Password != password)
                    {
                        MessageBox.Show("Password errata", "Login errato", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    else if (USER.Password == password)
                    {
                        FrmPrincipale frmPrn = new FrmPrincipale();
                        frmPrn.FormClosed += (s, args) => this.Close();
                        frmPrn.Show();
                        this.Hide();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errore durante login: " + ex.Message);
                }
            }

         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
