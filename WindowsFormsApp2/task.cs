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
using WindowsFormsApp2.Models;

namespace WindowsFormsApp2
{
    public partial class task : Form
    {
        readonly OleDbConnection connection;
        readonly TaskButton button;
        readonly Form parentForm;
        int storyId;

        public bool WindowIsRaised { get; set; }

        private int parentProjectID;


        public task(TaskButton button, OleDbConnection connection, int parentProjectID, Form parentform, int storyid)
        {
            InitializeComponent();

            this.button = button;
            this.connection = connection;
            this.parentProjectID = parentProjectID;
            this.parentForm = parentform;
            this.storyId = storyid;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) // Creates teamview window
        {
            if (!WindowIsRaised)
            {
                Teamview TeamviewForm = new Teamview(connection, parentProjectID);
                TeamviewForm.Show(this);
            }
        }
        private void textBox1_leave(object sender, EventArgs e) // Just logic for input box behaviour
        {
            TextBox textBox = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.ForeColor = Color.Gray;
                textBox.Text = "Enter topic here...";
            }
        }

        private void Story_Textbox_Enter(object sender, EventArgs e) // Just logic for input box behaviour
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == "Enter topic here...")
            {
                textBox.ForeColor = Color.Black;
                textBox.Text = "";
            }
        }

        private void Description_leave(object sender, EventArgs e) // Just logic for input box behaviour
        {
            TextBox textBox = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.ForeColor = Color.Gray;
                textBox.Text = "Enter description here...";
            }
        }

        private void Description_Enter(object sender, EventArgs e) // Just logic for input box behaviour
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == "Enter description here...")
            {
                textBox.ForeColor = Color.Black;
                textBox.Text = "";
            }
        }

        private void Task_Create_Button_Click(object sender, EventArgs e) // Creates new task and inserts into the Tasks Database with required parameters
        {
            string contentTopic = textBox2.Text;
            string contentDescription = textBox1.Text;
            int priorityStatus = Convert.ToInt32(comboBox1.Text);
            string difficulty = comboBox1.Text;

            DateTime startDate = dateTimePicker2.Value;
            DateTime endDate = dateTimePicker1.Value;
            
            string query = $"INSERT INTO Tasks(Task_Name, TaskDescription, Status, Priority, Difficulty, StartDate, EndDate, FK_StoryID) VALUES ('{contentTopic}', '{contentDescription}', {button.Task.Status}, {priorityStatus}, {difficulty}, '{startDate.ToString("d")}', '{endDate.ToString("d")}', {storyId} )";
            try
            {
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();
                MessageBox.Show($"Taskin lisääminen onnistui", "Task", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ((FlowlayoutButtonForm)parentForm).PopulateStories(storyId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Virhe:\n\n{ex.Message}", "Task", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void task_Load(object sender, EventArgs e) // Loads label information on the left side of the window
        {
            string text;
            switch (button.Task.Status)
            {
                case 1:
                    text = $"{button.Task.Description} \n\nTo Be Done ";
                    break;
                case 2:
                    text = $"{button.Task.Description} \n\nIn process";
                    break;
                case 3:
                    text = $"{button.Task.Description} \n\nDone";
                    break;
                default:
                    text = "Task";
                    break;
            }
            
            label2.Text = text;

            TeamComboBoxAdd();
            RemovePersonData();
        }

        private void button3_Click(object sender, EventArgs e) // Changes the priority of the task
        {
            int priorityStatus = Convert.ToInt32(comboBox1.Text);
            string query = $"UPDATE Tasks SET Priority = {priorityStatus} WHERE TaskID = {button.Task.TaskID}";
            try
            {
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();
                MessageBox.Show($"Priorityn vaihtaminen onnistui", "Task", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ((FlowlayoutButtonForm)parentForm).PopulateStories(storyId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Virhe:\n\n{ex.Message}", "Task", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Deletetask_Button_Click(object sender, EventArgs e) // Deletes task from the database
        {
            try
            {
                string query = $"DELETE FROM Tasks WHERE TaskID = {button.Task.TaskID}"; // Poistetaan Userstoryjen relaatiot taskista
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();

                MessageBox.Show($"Taskin poistaminen onnistui", "Task", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ((FlowlayoutButtonForm)parentForm).PopulateStories(storyId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Virhe:\n\n{ex.Message}", "Task delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void TeamComboBoxAdd() // Populates all neccesary comboboxes
        {
            string query = $"SELECT FirstName, LastName, PersonID FROM Persons";



            OleDbCommand cmd = new OleDbCommand(query, connection);
            OleDbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string FirstName = reader.GetString(0);
                string LastName = reader.GetString(1);
                int PersonId = reader.GetInt32(2);

                Setperson_Combobox.Items.Add($"{FirstName} {LastName} ({PersonId})");
            }
        }
        private void RemovePersonData() // Method for populatin person removing combobox
        {
            string query = $"SELECT PersonID, FK_PersonID, FirstName, LastName FROM Individual_Tasks, Persons WHERE Individual_Tasks.FK_PersonID = Persons.PersonID";

            OleDbCommand cmd = new OleDbCommand(query, connection);
            OleDbDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string FirstName = reader.GetString(2);
                string LastName = reader.GetString(3);
                int PersonID = reader.GetInt32(0);

                Remove_Combobox.Items.Add($"{FirstName} {LastName} ({PersonID})");
            }
        }

        private void Setperson_Button_Click(object sender, EventArgs e) // Method for setting a person for the project
        {
            try
            {
                string selectedProject = (string)Setperson_Combobox.SelectedItem;

                int ProjectId = Helpers.ParseProjectId(selectedProject);

                string query = $"INSERT INTO Individual_Tasks(FK_PersonID, FK_TaskID) VALUES ({ProjectId}, {button.Task.TaskID})";

                OleDbCommand cmd = new OleDbCommand(query, connection);
                OleDbDataReader reader = cmd.ExecuteReader();

                MessageBox.Show($"Käyttäjän lisääminen onnistui", "Task", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RemovePersonData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Virhe:\n\n{ex.Message}", "Task", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button4_Click(object sender, EventArgs e) // Removes person from the task
        {
            try
            {
                string selectedProject = (string)Remove_Combobox.SelectedItem;

                int ProjectId = Helpers.ParseProjectId(selectedProject);

                string query = $"DELETE FROM Individual_Tasks WHERE {ProjectId} = FK_PersonID";

                OleDbCommand cmd = new OleDbCommand(query, connection);
                OleDbDataReader reader = cmd.ExecuteReader();

                MessageBox.Show($"Käyttäjän poistaminen onnistui", "Task", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RemovePersonData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Virhe:\n\n{ex.Message}", "Task", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
