﻿using Newtonsoft.Json;
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
using IronPdf;
using Syncfusion.Pdf.Parsing;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        readonly OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\Users\Riki\Documents\GitHub\3002ryhma3\WindowsFormsApp2\ketterakanta.accdb");
        // Connection query
        public int ProjectID { get; private set;}
        public bool WindowIsRaised { get; set; }

        Timer timer = new Timer() // Creates timer so we know if panel was moved
        {
            Interval = 200
        };

        public Form1() // Sovelluksen ensimmäinen ikkuna ja yhteys databaseen tehdään
        {
            InitializeComponent();
            connection.Open();

            ProjectID = -1;
            Project_Display.Text = "Project: ";

            timer.Tick += Timer_Tick;
        }

        Point previousMouseLocation = Point.Empty;
        UserstoryButton pressedButton = null;

        private void Timer_Tick(object sender, EventArgs e) // Timer so we know if panel was moved
        {
            if (previousMouseLocation != Point.Empty && pressedButton != null)
            {
                if (previousMouseLocation != MousePosition)
                {
                    string jsonString = JsonConvert.SerializeObject(new DraggableUserStoryData() // Sets data what we want when dragging
                    {
                        StoryId = pressedButton.Userstory.StoryID.ToString(),
                        Status = pressedButton.Userstory.Status.ToString()
                    });

                    pressedButton.DoDragDrop(jsonString, DragDropEffects.Move); // Checks if dragged
                }
                else if (!WindowIsRaised)
                {
                    FlowlayoutButtonForm flowlayoutButtonForm = new FlowlayoutButtonForm(this, connection, pressedButton, ProjectID);
                    flowlayoutButtonForm.Show(this);
                }

                previousMouseLocation = Point.Empty;
                pressedButton = null;
            }

            timer.Stop();
        }

        private void Form1_Load(object sender, EventArgs e) // Ikkuna ladataan ja haetaan paneeleihin valuet
        {
            PopulateStories(ProjectID);
        }

        public void PopulateStories(int projectid) // Täydennetään paneelit tiedolla
        {
            Projects_List.Items.Clear();

            foreach (UserstoryButton button in ToDoPaneeli.Controls)
            {
                button.Enabled = false;
                button.Dispose();
            }
            ToDoPaneeli.Controls.Clear();
            
            foreach (UserstoryButton button in InProgressPaneeli.Controls)
            {
                button.Enabled = false;
                button.Dispose();
            }
            InProgressPaneeli.Controls.Clear();

            foreach (UserstoryButton button in DonePaneeli.Controls)
            {
                button.Enabled = false;
                button.Dispose();
            }
            DonePaneeli.Controls.Clear();

            string query = $"SELECT Project_name, ProjectID FROM Projects"; // Haetaan Projektit tietokannasta projektien nimet

            OleDbCommand cmd = new OleDbCommand(query, connection);
            OleDbDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                int projectID = reader.GetInt32(1);
                string projectName = reader.GetString(0);

                Projects_List.Items.Add($"{projectName} ({projectID})"); // Lisätään ne dropdown listaan

            }
            reader.Close();

            if (projectid < 0)
            {
                return;
            }

            //(string)Projects_List.SelectedItem
            // Creating panels with propper information, query shows what information is wanted
            query = $"SELECT StoryID, StoryDescription, Status, Priority, SprintID, StoryName FROM (User_Stories INNER JOIN Sprints ON Sprints.SprintID = User_Stories.FK_SprintID) INNER JOIN Projects ON Projects.ProjectID = Sprints.FK_ProjectID WHERE User_Stories.FK_SprintID = Sprints.SprintID AND Sprints.FK_ProjectID = {projectid}"; // Haetaan UserStoryt tietokannasta projektin IDllä
            cmd = new OleDbCommand(query, connection);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int storyID = reader.GetInt32(0);
                string userStory = reader.GetString(1);
                int status = reader.GetInt32(2);
                int storyPriority = reader.GetInt32(3);
                int sprintID = reader.GetInt32(4);
                string storyName = reader.GetString(5);

                switch (status)
                {
                    case 1:
                        UserstoryButton button = CreateFlowLayoutButton(new User_Stories() { StoryID = storyID, Description = userStory, Status = 1, Priority = storyPriority, Name = storyName, FK_SprintID = sprintID }, storyPriority); // Add information to the panels
                        ToDoPaneeli.Controls.Add(button);
                        break;
                    case 2:
                        UserstoryButton button2 = CreateFlowLayoutButton(new User_Stories() { StoryID = storyID, Description = userStory, Status = 2, Priority = storyPriority, Name = storyName, FK_SprintID = sprintID }, storyPriority); // Add information to the panels
                        InProgressPaneeli.Controls.Add(button2);
                        break;
                    case 3:
                        UserstoryButton button3 = CreateFlowLayoutButton(new User_Stories() { StoryID = storyID, Description = userStory, Status = 3, Priority = storyPriority, Name = storyName, FK_SprintID = sprintID }, storyPriority); // Add information to the panels
                        DonePaneeli.Controls.Add(button3);
                        break;
                }
            }
            reader.Close();
        }

        public UserstoryButton CreateFlowLayoutButton(User_Stories Userstory, int priority = 0) //Nappien luonti tauluihin, sisältää muotoilun
        {
            int ButtonPadding = 5; //Napin 'padding' arvo

            UserstoryButton button = new UserstoryButton
            {
                Margin = new Padding(ButtonPadding, ButtonPadding, ButtonPadding, 0),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(ToDoPaneeli.Size.Width - ButtonPadding * 2, 40),
                Anchor = AnchorStyles.None,
                Userstory = Userstory
            };
            switch (priority)
            {
                case 1:
                    button.BackColor = Color.Red;
                    break;
                case 2:
                    button.BackColor = Color.Orange;
                    break;
                case 3:
                    button.BackColor = Color.Yellow;
                    break;
                default:
                    button.BackColor = Color.LightGray; 
                    break;
            }
            button.Font = new Font(button.Font.FontFamily, button.Font.Size + 1, FontStyle.Bold);
            button.FlatAppearance.BorderSize = 0;
            button.Text = button.Userstory.Description;
            button.MouseDoubleClick += new MouseEventHandler(FlowlayoutButton_MouseDoubleClick);
            button.MouseDown += new MouseEventHandler(flowLayoutButton_MouseDown);

            return button;
        }
        
        private void FlowlayoutButton_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!WindowIsRaised)
            {
                FlowlayoutButtonForm flowlayoutButtonForm = new FlowlayoutButtonForm(this, connection, (UserstoryButton)sender, ProjectID);
                flowlayoutButtonForm.Show(this);
            }
        }
        
        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            button.SetButtonActive(Color.Red);
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            button.SetButtonInactive(Color.Transparent);
        }

        private void ToDo1_Click(object sender, EventArgs e) // Creates ToDo empty panels
        {
            Button button = CreateFlowLayoutButton(new User_Stories() { Description = "", Status = 1, FK_SprintID = GetProjectSprintId(ProjectID) }); // Saves needed data to the panel

            ToDoPaneeli.Controls.Add(button);
        }

        private void InProgress1_Click(object sender, EventArgs e)// Creates InProgress empty panels
        {
            Button button = CreateFlowLayoutButton(new User_Stories() { Description = "", Status = 2, FK_SprintID = GetProjectSprintId(ProjectID) }); // Saves needed data to the panel

            InProgressPaneeli.Controls.Add(button);
        }

        private void Done1_Click(object sender, EventArgs e)// Creates Done empty panels
        {
            Button button = CreateFlowLayoutButton(new User_Stories() { Description = "", Status = 3, FK_SprintID = GetProjectSprintId(ProjectID) }); // Saves needed data to the panel

            DonePaneeli.Controls.Add(button);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) // what happens when form is closed
        {
            connection.Dispose();
            connection.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
        private void TeamAdd_Click(object sender, EventArgs e) // Creates new window 
        {
            FlowlayoutButtonForm flowlayoutButtonForm = new FlowlayoutButtonForm(this, connection, (UserstoryButton)sender, ProjectID);
            flowlayoutButtonForm.Show(this);
        }

        private void ToDoPaneeli_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e) // When entered removes enter topic here
        {
            TextBox textBox = (TextBox)sender;

            if (textBox.Text == "Enter topic here...")
            {
                textBox.ForeColor = Color.Black;
                textBox.Text = "";
            }
        }

        private void textBox1_Leave(object sender, EventArgs e) // When left adds enter topic here if empty
        {
            TextBox textBox = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.ForeColor = Color.Gray;
                textBox.Text = "Enter topic here...";
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e) // when Enter is pressed executes query
        {
            if (e.KeyCode == Keys.Enter)
            {
                string contentTopic = textBox1.Text;
                string query = $"INSERT INTO Projects(Project_name, FK_TeamID) VALUES ('{contentTopic}', 1)"; // Lisättävän datan query 

                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();
                e.Handled = true;
                PopulateStories(ProjectID);
            }
        }

        private void Projects_List_SelectedIndexChanged(object sender, EventArgs e) // Populates combobox with projects
        {
            string selectedProject = (string)Projects_List.SelectedItem;

            ProjectID = Helpers.ParseProjectId(selectedProject);

            PopulateStories(ProjectID);

            Project_Display.Text = $"Project: {selectedProject}";
        }

        private void ProjectCreate_Button_Click(object sender, EventArgs e)
        {
            string contentTopic = textBox1.Text;
            string query = $"INSERT INTO Projects(Project_name, FK_TeamID) VALUES ('{contentTopic}', 1)"; // Lisättävän datan query 

            OleDbCommand cmd = new OleDbCommand(query, connection);
            cmd.ExecuteNonQuery();
            PopulateStories(ProjectID);
        }

        private void sprint_Button_Click(object sender, EventArgs e) // Creates new sprint and assigns it to the project
        {
            DateTime startDate = dateTimePicker2.Value;
            DateTime endDate = dateTimePicker1.Value;
            string query = $"INSERT INTO Sprints(StartDate, EndDate, FK_ProjectID) VALUES ('{startDate.ToString("d")}', '{endDate.ToString("d")}', {ProjectID})"; // Lisättävän datan query 

            try
            {
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();
                PopulateStories(ProjectID);
                MessageBox.Show($"Sprintin lisääminen onnistui", "Project", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Virhe:\n\n{ex.Message}", "Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void flowLayoutButton_MouseDown(object sender, MouseEventArgs e) // Moves the panels
        {
            if (e.Button == MouseButtons.Left)
            {
                previousMouseLocation = MousePosition;
                pressedButton = (UserstoryButton)sender;

                timer.Start();
            }
        }

        private void flowLayoutPanel_DragEnter(object sender, DragEventArgs e) // Starts the dragging effect
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void flowLayoutPanel_DragDrop(object sender, DragEventArgs e) // Logic when panel is dropped into the parent panel
        {
            DraggableUserStoryData draggableObjectData = JsonConvert.DeserializeObject<DraggableUserStoryData>(e.Data.GetData(DataFormats.Text).ToString());

            int storyId = Convert.ToInt32(draggableObjectData.StoryId);
            //int status = Convert.ToInt32(draggableObjectData.Status);

            string query = $"UPDATE User_Stories SET Status = (((id))) WHERE StoryID = {storyId}";

            switch (Convert.ToInt32(((FlowLayoutPanel)sender).Tag))
            {
                case 1:
                    query = query.Replace("(((id)))", "1");
                    break;
                case 2:
                    query = query.Replace("(((id)))", "2");
                    break;
                case 3:
                    query = query.Replace("(((id)))", "3");
                    break;
            }

            if (!query.Contains("(((id)))"))
            {
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();
            }

            PopulateStories(ProjectID);
        }

        private int GetProjectSprintId(int projectid) // Method to parse the sprint id
        {
            int sprintid = -1;
            string query = $"SELECT SprintID FROM (User_Stories INNER JOIN Sprints ON Sprints.SprintID = User_Stories.FK_SprintID) INNER JOIN Projects ON Projects.ProjectID = Sprints.FK_ProjectID WHERE User_Stories.FK_SprintID = Sprints.SprintID AND Sprints.FK_ProjectID = {projectid}"; // Haetaan UserStoryt tietokannasta projektin IDllä
            OleDbCommand cmd = new OleDbCommand(query, connection);
            OleDbDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                sprintid = reader.GetInt32(0);    
            }
            reader.Close();
            return sprintid;
        }

        private void TeamCreate_Button_Click(object sender, EventArgs e) // Creates teamview window
        {
            if (!WindowIsRaised)
            {
                Teamview TeamviewForm = new Teamview(connection, ProjectID);
                TeamviewForm.Show(this);
            }
        }

        private void Report_Button_Click(object sender, EventArgs e)
        {

        }

        private void Delete_Project_Button_Click(object sender, EventArgs e) // Deletes project from database also its relations
        {
            try
            {
                string query = $"DELETE FROM Projects WHERE ProjectID = {ProjectID}"; // Poistetaan Userstoryjen relaatiot taskista
                OleDbCommand cmd = new OleDbCommand(query, connection);
                cmd.ExecuteNonQuery();

                MessageBox.Show($"Projektin poistaminen onnistui", "Project", MessageBoxButtons.OK, MessageBoxIcon.Information);
                PopulateStories(ProjectID);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Virhe:\n\n{ex.Message}", "Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    class DraggableUserStoryData
    {
        public string StoryId { get; set; }
        public string Status { get; set; }
    }

    public static class ButtonExtensions
    {
        public static void SetButtonActive(this Button button, Color color)
        {
            button.BackColor = color;
        }

        public static void SetButtonInactive(this Button button, Color color)
        {
            button.BackColor = color;
        }
    }
    
}
