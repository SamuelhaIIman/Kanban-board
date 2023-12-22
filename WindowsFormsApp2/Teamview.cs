using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Teamview : Form
    {
        readonly OleDbConnection connection;

        public int ProjectId { get; private set; }

        public bool WindowIsRaised { get; set; }

        private int parentProjectID;

        public Teamview(OleDbConnection connection, int parentProjectID)
        {
            InitializeComponent();

            this.connection = connection;

            this.parentProjectID = parentProjectID;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) // Insert data to teams
        {
            string contentTopic = textBox1.Text;
            int priorityStatus = Convert.ToInt32(comboBox1.Text);
            string query = $"INSERT INTO Teams(Team_name, Project_Priorities) VALUES ('{contentTopic}', {priorityStatus})";

            OleDbCommand cmd = new OleDbCommand(query, connection);
            cmd.ExecuteNonQuery();
        }

        private void comboBox2_Click(object sender, EventArgs e)
        {

        }

        private void Teamview_Load(object sender, EventArgs e) // Loads the form
        {
            // TODO: This line of code loads data into the 'ketterakantaDataSet.Persons' table. You can move, or remove it, as needed.
            this.personsTableAdapter.Fill(this.ketterakantaDataSet.Persons);

            TeamComboBoxAdd();
        }

        private void button2_Click(object sender, EventArgs e) // Sets foreign key for projects
        {
            string query = $"UPDATE Projects SET FK_TeamID = {ProjectId} WHERE ProjectID = {parentProjectID}"; 
            OleDbCommand cmd = new OleDbCommand(query, connection);
            OleDbDataReader reader = cmd.ExecuteReader();
        }

        private void TeamComboBoxAdd() // Filling out combobox
        {
            string query = $"SELECT TeamID, Team_name FROM Teams"; // Query for getting all the teams



            OleDbCommand cmd = new OleDbCommand(query, connection);
            OleDbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int teamId = reader.GetInt32(0);
                string teamName = reader.GetString(1);
                
                comboBox2.Items.Add($"{teamName} ({teamId})");

            }
        }
        
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) // Selects the team
        {
            string selectedProject = (string)comboBox2.SelectedItem;

            ProjectId = Helpers.ParseProjectId(selectedProject);
        }

        private void button3_Click(object sender, EventArgs e) // Filling out Gridview with data
        {
            string query = $"SELECT FirstName, LastName, Email, PhoneNumber FROM Persons WHERE FK_TeamID = {ProjectId}";
            OleDbCommand cmd = new OleDbCommand(query, connection);
            var dataAdapter = new OleDbDataAdapter(cmd);

            var commandBuilder = new OleDbCommandBuilder(dataAdapter);
            var ds = new DataSet();
            dataAdapter.Fill(ds); // Filling out the dataset
            dataGridView1.ReadOnly = true; // Set gridview to readonly
            dataGridView1.DataSource = ds.Tables[0]; // start table
        }

        private void Createperson_Button_Click(object sender, EventArgs e) // Opens up MemberCreate window
        {
            if (!WindowIsRaised)
            {
                MemberCreate MemberCreate = new MemberCreate(connection); // Creates membercreate window
                MemberCreate.Show(this); // Shows it
            }
        }
    }
}
