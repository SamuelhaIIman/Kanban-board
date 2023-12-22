using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class MemberCreate : Form
    {

        readonly OleDbConnection connection;
        
        public MemberCreate(OleDbConnection connection)
        {
            InitializeComponent();
            this.connection = connection;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void TeamComboBoxAdd() // Method for populating comboboxes
        {
            string query = $"SELECT TeamID FROM Teams";

            OleDbCommand cmd = new OleDbCommand(query, connection);
            OleDbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int teamId = reader.GetInt32(0);

                Team_Combobox.Items.Add($"{teamId}");
            }
        }

        private void button1_Click(object sender, EventArgs e) // Creates new person for the team
        {
            try
            {
                string FirstName = Firstname_Box.Text;
                string LastName = Lastname_Textbox.Text;
                string Email = Email_textbox.Text;
                int Phonenumber = Convert.ToInt32(Phonenumber_box.Text);
                string Role = Role_Combobox.Text;
                int Team = Convert.ToInt32(Team_Combobox.Text);

                string query = $"INSERT INTO Persons(FirstName, LastName, Email, Role, PhoneNumber, FK_TeamID) VALUES ('{FirstName}', '{LastName}', '{Email}', '{Role}', {Phonenumber}, {Team})";

                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();
                MessageBox.Show($"Henkilön luominen onnistui", "MemberCreate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Virhe:\n\n{ex.Message}", "MemberCreate", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Sprint_Form_Load(object sender, EventArgs e)
        {
            TeamComboBoxAdd();
        }
    }
}
