namespace WindowsFormsApp2
{
    partial class MemberCreate
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.Firstname_Box = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.Lastname_Textbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Email_textbox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Role_Combobox = new System.Windows.Forms.ComboBox();
            this.Phonenumber_box = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Team_Combobox = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label1.Location = new System.Drawing.Point(79, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "First name";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // Firstname_Box
            // 
            this.Firstname_Box.Location = new System.Drawing.Point(71, 98);
            this.Firstname_Box.Name = "Firstname_Box";
            this.Firstname_Box.Size = new System.Drawing.Size(100, 20);
            this.Firstname_Box.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(347, 279);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Create";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Lastname_Textbox
            // 
            this.Lastname_Textbox.Location = new System.Drawing.Point(196, 98);
            this.Lastname_Textbox.Name = "Lastname_Textbox";
            this.Lastname_Textbox.Size = new System.Drawing.Size(100, 20);
            this.Lastname_Textbox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label2.Location = new System.Drawing.Point(204, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Last name";
            // 
            // Email_textbox
            // 
            this.Email_textbox.Location = new System.Drawing.Point(313, 98);
            this.Email_textbox.Name = "Email_textbox";
            this.Email_textbox.Size = new System.Drawing.Size(100, 20);
            this.Email_textbox.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label3.Location = new System.Drawing.Point(335, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 18);
            this.label3.TabIndex = 5;
            this.label3.Text = "Email";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label4.Location = new System.Drawing.Point(449, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 18);
            this.label4.TabIndex = 7;
            this.label4.Text = "Role";
            // 
            // Role_Combobox
            // 
            this.Role_Combobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Role_Combobox.FormattingEnabled = true;
            this.Role_Combobox.Items.AddRange(new object[] {
            "Coder",
            "UI Designer",
            "Scrum Master",
            "Product Owner"});
            this.Role_Combobox.Location = new System.Drawing.Point(429, 98);
            this.Role_Combobox.Name = "Role_Combobox";
            this.Role_Combobox.Size = new System.Drawing.Size(81, 21);
            this.Role_Combobox.TabIndex = 8;
            // 
            // Phonenumber_box
            // 
            this.Phonenumber_box.Location = new System.Drawing.Point(523, 98);
            this.Phonenumber_box.Name = "Phonenumber_box";
            this.Phonenumber_box.Size = new System.Drawing.Size(100, 20);
            this.Phonenumber_box.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label5.Location = new System.Drawing.Point(520, 77);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 18);
            this.label5.TabIndex = 9;
            this.label5.Text = "Phone number";
            // 
            // Team_Combobox
            // 
            this.Team_Combobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Team_Combobox.FormattingEnabled = true;
            this.Team_Combobox.Location = new System.Drawing.Point(640, 98);
            this.Team_Combobox.Name = "Team_Combobox";
            this.Team_Combobox.Size = new System.Drawing.Size(81, 21);
            this.Team_Combobox.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.label6.Location = new System.Drawing.Point(655, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 18);
            this.label6.TabIndex = 11;
            this.label6.Text = "Team";
            // 
            // MemberCreate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Team_Combobox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Phonenumber_box);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Role_Combobox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Email_textbox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Lastname_Textbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Firstname_Box);
            this.Controls.Add(this.label1);
            this.Name = "MemberCreate";
            this.Text = "Person Creator";
            this.Load += new System.EventHandler(this.Sprint_Form_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Firstname_Box;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox Lastname_Textbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Email_textbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox Role_Combobox;
        private System.Windows.Forms.TextBox Phonenumber_box;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox Team_Combobox;
        private System.Windows.Forms.Label label6;
    }
}