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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace LaRubica
{
    public partial class FrmPrincipale : Form
    {
       
        public FrmPrincipale()
        {
            InitializeComponent();
            LoadRubicaData();
        }

        public void LoadRubicaData()
        {
            string username = DatabaseConfig.GetDbUsername();
            string password = DatabaseConfig.GetDbPassword();

          //  string connectionString = $"Server=KEVIN;Database=Rubica;User Id={username};Password={password};";

            string query = "SELECT ID,Nome,Cognome,Telefono FROM Persone";

            using (SqlConnection connection = new SqlConnection(DatabaseConfig.GetConnectionString()))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable); // Fill the DataTable with data from the database

                    
                    dataGridView1.DataSource = dataTable; // Bind the DataTable to the DataGridView
                    dataGridView1.Columns["ID"].Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errore durante loading data: " + ex.Message);
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnNuova_Click(object sender, EventArgs e)
        {
            frmEditorPersona frmPrn = new frmEditorPersona(this);
            frmPrn.Show();
            
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void EliminaPersona(int id)
        {


            // string query = "DELETE FROM Persone WHERE ID = @ID";

            //using (SqlConnection connection = new SqlConnection(connectionString))
            //{
            //    SqlCommand command = new SqlCommand(query, connection);
            //    command.Parameters.AddWithValue("@ID", id);

            //    try
            //    {
            //        connection.Open();
            //        command.ExecuteNonQuery();
            //        MessageBox.Show("Persona è eliminata!");
            //        this.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show("Errore durante eliminare la persona: " + ex.Message);
            //    }
            //}

            string username = DatabaseConfig.GetDbUsername();
            string password = DatabaseConfig.GetDbPassword();

            //string connectionString = $"Server=KEVIN;Database=Rubica;User Id={username};Password={password};";

            using (SqlConnection connection = new SqlConnection(DatabaseConfig.GetConnectionString()))
            {
                connection.Open();

                // SQL query to delete the record
                string query = "DELETE FROM Persone WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Set the parameter value
                    command.Parameters.AddWithValue("@ID", id);

                    // Execute the query
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Persona eliminato!", "Successo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Eliminazione persona non è andata buon fine !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }


        }
    

        private void btnElimina_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the ID of the selected row
                int selectedId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

                // Confirm deletion
                var result = MessageBox.Show("Voule eliminare persona scelta?", "Conferma eliminazione",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Delete from the database
                    EliminaPersona(selectedId);

                    // Reload the data in the DataGridView
                    LoadRubicaData();
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

        private void btnModifica_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                string query = "SELECT * FROM persone WHERE ID = @Id";
                string username = DatabaseConfig.GetDbUsername();
                string password = DatabaseConfig.GetDbPassword();

               // string connectionString = $"Server=KEVIN;Database=Rubica;User Id={username};Password={password};";

                using (SqlConnection connection = new SqlConnection(DatabaseConfig.GetConnectionString()))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    int id = Convert.ToInt32(selectedRow.Cells["ID"].Value.ToString());
                    command.Parameters.AddWithValue("@Id",id);


                    try
                    {
                        connection.Open();
                        var result = command.ExecuteReader();

                        if (result.HasRows)
                        {
                            while (result.Read())
                            {
                                string nome = result.GetString(result.GetOrdinal("Nome"));
                                string cognome = result.GetString(result.GetOrdinal("Cognome"));
                                string indirizzo = result.GetString(result.GetOrdinal("Indirizzo"));
                                string telefono = result.GetString(result.GetOrdinal("Telefono"));

                                int eta = result.IsDBNull(result.GetOrdinal("Eta"))
                          ? 0 
                          : result.GetInt32(result.GetOrdinal("Eta"));
                                
                                Persona persona = new Persona(nome, cognome, indirizzo, telefono, eta);

                                frmEditorPersona editForm = new frmEditorPersona(this, persona, id);
                                editForm.ShowDialog(); // Show the edit form as a modal dialog
                            }

                            


                           
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Errore durante login: " + ex.Message);
                    }
                    LoadRubicaData();
                }

            }
            else
            {
                MessageBox.Show("Seleziona una persona da modificare.");
            }
        }

        private void FrmPrincipale_Load(object sender, EventArgs e)
        {

        }

        private void FrmPrincipale_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void FrmPrincipale_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {

                e.Cancel = true;

                // Hide the MainForm
                this.Hide();

                // Create and show the LoginForm again
                Login loginForm = new Login();
                loginForm.Show();
            }
        }
    }
}
