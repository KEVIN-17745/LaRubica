using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LaRubica
{
    public partial class frmEditorPersona : Form
    {
        private FrmPrincipale _frmprn;
        private Persona _persona;
        private bool _isEdit = false;
        private int _ID;

        public frmEditorPersona(FrmPrincipale frmprn)
        {
            InitializeComponent();
            _frmprn = frmprn;
        }

        public frmEditorPersona(FrmPrincipale frmprn, Persona persona, int id)
        {
            InitializeComponent();
            _frmprn = frmprn;
            _persona = persona;
            frmEditorPersona_Load(null, null);
            _isEdit = true;
            _ID = id;
        }

        private void frmEditorPersona_Load(object sender, EventArgs e)
        {
            if (_persona != null)
            {
                txtNome.Text = _persona.Nome;
                txtCognome.Text = _persona.Cognome;
                txtIndirizzo.Text = _persona.Indirizzo;
                txtTelefono.Text = _persona.Telefono;
                txtEta.Text = _persona.Eta.ToString();
                _isEdit = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSalva_Click(object sender, EventArgs e)
        {
            // ValidateData();

            ValidateInputs();
            _frmprn.LoadRubicaData();
        }
        private void ValidateInputs()
        {
            // Clear previous errors
            errorProvider1.Clear();

            // my regex patterns for validation
            string namePattern = @"^[A-Za-z]+$";  // Only letters for Nome and Cognome
            string phonePattern = @"^\d{10}$";  // Exactly 10 digits for Telefono
            string etaPattern = @"^\d+$";  // Ensure Eta is a number (digits only)

            bool isValid = true;

            // Validate Nome
            if (string.IsNullOrEmpty(txtNome.Text) || !Regex.IsMatch(txtNome.Text, namePattern))
            {
                errorProvider1.SetError(txtNome, "Il Nome deve contenere solo lettere.");
                isValid = false;
            }

            // Validate Cognome
            if (string.IsNullOrEmpty(txtCognome.Text) || !Regex.IsMatch(txtCognome.Text, namePattern))
            {
                errorProvider1.SetError(txtCognome, "Il Cognome deve contenere solo lettere.");
                isValid = false;
            }

            // Validate Indirizzo
            if (string.IsNullOrEmpty(txtIndirizzo.Text))
            {
                errorProvider1.SetError(txtIndirizzo, "L'indirizzo non può essere vuoto.");
                isValid = false;
            }

            // Validate Eta
            if (string.IsNullOrEmpty(txtEta.Text) || !Regex.IsMatch(txtEta.Text, etaPattern))
            {
                errorProvider1.SetError(txtEta, "L'età deve essere un numero.");
                isValid = false;
            }
            else
            {
                int eta = int.Parse(txtEta.Text);
                if (eta < 0 || eta > 120)
                {
                    errorProvider1.SetError(txtEta, "L'età deve essere tra 0 e 120.");
                    isValid = false;
                }
            }

           
            if (string.IsNullOrEmpty(txtTelefono.Text) || !Regex.IsMatch(txtTelefono.Text, phonePattern))
            {
                errorProvider1.SetError(txtTelefono, "Il telefono deve contenere esattamente 10 numeri.");
                isValid = false;
            }

            
            if (isValid)
            {
                SavePersona();
            }
        }
    //    private void ValidateData()
    //    {
    //        string namePattern = @"^[A-Za-z]+$";  // Only letters for Nome and Cognome
    //        string phonePattern = @"^\d{10}$";  // Exactly 10 digits for Telefono
    //        string etaPattern = @"^\d+$";  // Ensure Eta is a number (digits only)

    //        if (string.IsNullOrEmpty(txtNome.Text) || !Regex.IsMatch(txtNome.Text, namePattern) ||
    //string.IsNullOrEmpty(txtCognome.Text) || !Regex.IsMatch(txtCognome.Text, namePattern) ||
    //string.IsNullOrEmpty(txtIndirizzo.Text) ||
    //string.IsNullOrEmpty(txtEta.Text) || !Regex.IsMatch(txtEta.Text, etaPattern) ||
    //int.TryParse(txtEta.Text, out int eta) && (eta < 0 || eta > 120) ||
    //string.IsNullOrEmpty(txtTelefono.Text) || !Regex.IsMatch(txtTelefono.Text, phonePattern))
    //        {
    //            MessageBox.Show("Deve inseriri tutti dati coretti", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
    //            return;
                
    //        }
    //            //if (true)
    //            //{
    //            //    SavePersona();
    //            //}
    //        else
    //            {
    //            SavePersona();
    //        }
    //    }


        private void SavePersona()
        {

          

            //string nome = txtNome.Text;
            //string cognome = txtCognome.Text;
            //string indirizzo = txtIndirizzo.Text;
            //string telefono = txtTelefono.Text;
            //string eta = txtEta.Text;
            Persona persona = new Persona(txtNome.Text, txtCognome.Text, txtIndirizzo.Text, txtTelefono.Text, Convert.ToInt32(txtEta.Text));

            if (_isEdit)
            {
                string query = "UPDATE Persone SET Nome = @Nome, Cognome = @Cognome, Indirizzo = @Indirizzo, Telefono = @Telefono, Eta = @Eta WHERE ID = @Id";

                using (SqlConnection connection = new SqlConnection(DatabaseConfig.GetConnectionString()))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Nome", persona.Nome ?? string.Empty);
                    command.Parameters.AddWithValue("@Cognome", persona.Cognome ?? string.Empty);
                    command.Parameters.AddWithValue("@Indirizzo", persona.Indirizzo ?? string.Empty);
                    command.Parameters.AddWithValue("@Telefono", persona.Telefono ?? string.Empty);
                    command.Parameters.AddWithValue("@Eta", persona.Eta);
                    command.Parameters.AddWithValue("@Id", _ID);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Persona è Aggiornata!");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Errore durante salvare la persona: " + ex.Message);
                    }
                }
            }
            else
            {
                string query = "INSERT INTO Persone (Nome, Cognome, Indirizzo, Telefono, Eta) " +
                              "VALUES (@Nome, @Cognome, @Indirizzo, @Telefono, @Eta)";

                using (SqlConnection connection = new SqlConnection(DatabaseConfig.GetConnectionString()))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Nome", persona.Nome ?? string.Empty);
                    command.Parameters.AddWithValue("@Cognome", persona.Cognome ?? string.Empty);
                    command.Parameters.AddWithValue("@Indirizzo", persona.Indirizzo ?? string.Empty);
                    command.Parameters.AddWithValue("@Telefono", persona.Telefono ?? string.Empty);
                    command.Parameters.AddWithValue("@Eta", persona.Eta);


                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        MessageBox.Show("Persona è Aggiunta!");
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Errore durante salvare la persona: " + ex.Message);
                    }
                }
            }






        }

        private void btnAnnulla_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblTopic_Click(object sender, EventArgs e)
        {

        }
    }
}
