using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using System.Data.SQLite;



namespace ProjectManagement
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        // Define a panel for the welcome message and "create new project" button
        private Panel welcomePanel;

        // Define a label for Welcome Message
        private Label lblWelcomeMessage;
        // Define a label for logging out
        private Label lblLogout;
        // Define a button for Create new project button
        private Button btnCreateNewProject;
        // Add these three buttons to the welcome panel
        private Button btnModificaProgetto;
        private Button btnEliminaProgetto;
        private Button btnGestisciProgetti;

        private Label lblClock;
        private CheckBox chkShowPassword;
        private bool isPasswordVisible = false;
        private SQLiteConnection connection;

        private string rememberedUsername;
        private string rememberedPassword;

        private void Form1_Load(object sender, EventArgs e)
        {
            // Set the size of the form
            this.Size = new Size(1280, 720);

            // Set the minimum size of the form
            this.MinimumSize = new Size(800, 600);

            // Make the form resizable
            this.FormBorderStyle = FormBorderStyle.Sizable;

            txtPassword.PasswordChar = '•'; // Set default password character

            // Initialize welcome panel
            welcomePanel = new Panel();
            welcomePanel.Size = this.ClientSize; // Set size to match form
            welcomePanel.Location = new Point(0, 0); // Set location to top-left corner
            welcomePanel.BackColor = Color.Transparent; // Make it transparent

            // Create a label for the application title
            lblTitle.Text = "Project Management";
            lblTitle.Font = new Font("Raleway", 20, FontStyle.Bold);
            lblTitle.Location = new Point(10, 10);
            lblTitle.ForeColor = Color.FromArgb(101, 67, 33); // Dark brown
            lblTitle.BackColor = Color.Transparent;
            lblTitle.AutoSize = true;
            this.Controls.Add(lblTitle);

            lblWelcomeMessage = new Label();
            lblWelcomeMessage.Text = "Benvenuto, [Username]!";
            lblWelcomeMessage.Font = new Font("Raleway", 16, FontStyle.Bold);
            lblWelcomeMessage.ForeColor = Color.FromArgb(101, 67, 33); // Dark brown
            lblWelcomeMessage.BackColor = Color.Transparent;
            lblWelcomeMessage.AutoSize = true;
            lblWelcomeMessage.Anchor = AnchorStyles.Top | AnchorStyles.Right; // Anchor to top-right
            lblWelcomeMessage.Location = new Point(welcomePanel.Width - lblWelcomeMessage.Width - 10, 10); // Adjusted location
            welcomePanel.Controls.Add(lblWelcomeMessage);

            btnCreateNewProject = new Button();
            btnCreateNewProject.Text = "Crea Nuovo Progetto";
            btnCreateNewProject.Size = new Size(200, 30);
            btnCreateNewProject.Font = new Font("Raleway", 12, FontStyle.Bold);
            btnCreateNewProject.BackColor = Color.FromArgb(216, 177, 140); // Un colore beige
            btnCreateNewProject.ForeColor = Color.White;
            btnCreateNewProject.Location = new Point(10, lblTitle.Bottom + 10);
            btnCreateNewProject.Click += BtnCreateNewProject_Click;
            welcomePanel.Controls.Add(btnCreateNewProject);

            btnModificaProgetto = new Button();
            btnModificaProgetto.Text = "Modifica Progetto";
            btnModificaProgetto.Size = new Size(200, 30);
            btnModificaProgetto.Font = new Font("Raleway", 12, FontStyle.Bold);
            btnModificaProgetto.BackColor = Color.FromArgb(216, 177, 140); // Beige color
            btnModificaProgetto.ForeColor = Color.White;
            btnModificaProgetto.Location = new Point(btnCreateNewProject.Right + 10, btnCreateNewProject.Top);
            welcomePanel.Controls.Add(btnModificaProgetto);

            btnEliminaProgetto = new Button();
            btnEliminaProgetto.Text = "Elimina Progetto";
            btnEliminaProgetto.Size = new Size(200, 30);
            btnEliminaProgetto.Font = new Font("Raleway", 12, FontStyle.Bold);
            btnEliminaProgetto.BackColor = Color.FromArgb(216, 177, 140); // Beige color
            btnEliminaProgetto.ForeColor = Color.White;
            btnEliminaProgetto.Location = new Point(btnModificaProgetto.Right + 10, btnModificaProgetto.Top);
            btnEliminaProgetto.Click += btnEliminaProgetto_Click;
            welcomePanel.Controls.Add(btnEliminaProgetto);

            btnGestisciProgetti = new Button();
            btnGestisciProgetti.Text = "Gestisci Progetti";
            btnGestisciProgetti.Size = new Size(200, 30);
            btnGestisciProgetti.Font = new Font("Raleway", 12, FontStyle.Bold);
            btnGestisciProgetti.BackColor = Color.FromArgb(216, 177, 140); // Beige color
            btnGestisciProgetti.ForeColor = Color.White;
            btnGestisciProgetti.Location = new Point(btnEliminaProgetto.Right + 10, btnEliminaProgetto.Top);
            welcomePanel.Controls.Add(btnGestisciProgetti);

            dataGridView.Location = new Point(10, 120);
            dataGridView.Size = new Size(welcomePanel.Width - 40, welcomePanel.Height - 190);
            dataGridView.BackgroundColor = Color.White;
            dataGridView.GridColor = Color.FromArgb(224, 224, 224); // Light gray
            dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(216, 177, 140); // Beige color
            dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.ReadOnly = false;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Adjust column widths
            dataGridView.RowTemplate.Height = 35;
            dataGridView.Font = new Font("Raleway", 12, FontStyle.Bold); // Set the font
            dataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView.RowsDefaultCellStyle.BackColor = Color.White;
            dataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245); // Light gray
            dataGridView.DefaultCellStyle.Padding = new Padding(5, 0, 5, 0); // Adjust as needed
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.FromArgb(216, 177, 140); // Beige color
            dataGridView.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Raleway", 12, FontStyle.Bold); // Adjust font and size
            dataGridView.ScrollBars = ScrollBars.Vertical; // Only vertical scrollbar
            dataGridView.ColumnHeadersDefaultCellStyle.Padding = new Padding(5, 0, 5, 0); // Adjust as needed
            dataGridView.ColumnHeadersHeight = 40; // Adjust as needed
            dataGridView.CellBeginEdit += dataGridView_CellBeginEdit;
            // Add CheckBox Column
            DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
            checkboxColumn.Name = "checkBoxColumn";
            checkboxColumn.HeaderText = "checkBoxColumn";
            dataGridView.Columns.Add(checkboxColumn);

            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                column.Width = dataGridView.Width / dataGridView.Columns.Count;
            }


            // Add data (you can add rows of data here)

            // Add the DataGridView to the welcomePanel
            welcomePanel.Controls.Add(dataGridView);

            string connectionString = "Data Source=yourDatabaseFile.sqlite;Version=3;";
            connection = new SQLiteConnection(connectionString);
            connection.Open();

            string createTableQuery = "CREATE TABLE IF NOT EXISTS YourTableName (" +
                          "ID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                          "MachineName TEXT, " +
                          "Data TEXT, " +
                          "ProjectNumber INT, " +
                          "Pieces INT)";

            using (SQLiteCommand cmd = new SQLiteCommand(createTableQuery, connection))
            {
                cmd.ExecuteNonQuery();
            }

            // Initialize logout label
            lblLogout = new Label();
            lblLogout.Text = "Logout";
            lblLogout.AutoSize = true; // Set the size
            lblLogout.Font = new Font("Raleway", 10, FontStyle.Underline);
            lblLogout.ForeColor = Color.FromArgb(101, 67, 33); // Dark brown
            lblLogout.Location = new Point(welcomePanel.Width - lblLogout.Width - 20, 120);
            lblLogout.Click += LblLogout_Click;
            welcomePanel.Controls.Add(lblLogout);

            // Nasconde i controlli per la registrazione inizialmente
            txtNewUsername.Visible = false;
            txtNewPassword.Visible = false;
            btnRegister.Visible = false;

            // Imposta colori e proprietà
            this.BackColor = Color.FromArgb(245, 223, 186); // Un colore tono sabbia

            // Imposta colori per i pulsanti
            btnLogin.BackColor = Color.FromArgb(216, 177, 140); // Un colore beige
            btnRegister.BackColor = Color.FromArgb(216, 177, 140); // Un colore beige

            // Imposta colori e bordi per le caselle di testo
            txtUsername.BackColor = Color.FromArgb(255, 248, 220); // Un colore panna
            txtPassword.BackColor = Color.FromArgb(255, 248, 220); // Un colore panna
            txtNewUsername.BackColor = Color.FromArgb(255, 248, 220); // Un colore panna
            txtNewPassword.BackColor = Color.FromArgb(255, 248, 220); // Un colore panna

            // Imposta il colore del testo per le caselle di testo
            txtUsername.ForeColor = Color.FromArgb(101, 67, 33); // Un colore marrone scuro
            txtPassword.ForeColor = Color.FromArgb(101, 67, 33); // Un colore marrone scuro
            txtNewUsername.ForeColor = Color.FromArgb(101, 67, 33); // Un colore marrone scuro
            txtNewPassword.ForeColor = Color.FromArgb(101, 67, 33); // Un colore marrone scuro

            // Imposta il font per le caselle di testo
            txtUsername.Font = new Font("Raleway", 12, FontStyle.Regular);
            txtPassword.Font = new Font("Raleway", 12, FontStyle.Regular);
            txtNewUsername.Font = new Font("Raleway", 12, FontStyle.Regular);
            txtNewPassword.Font = new Font("Raleway", 12, FontStyle.Regular);

            // Imposta il font e il colore del testo per i pulsanti
            btnLogin.Font = new Font("Raleway", 12, FontStyle.Bold);
            btnRegister.Font = new Font("Raleway", 12, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnRegister.ForeColor = Color.White;


            // Imposta grandezza e posizione dei controlli
            txtUsername.Size = new Size(200, 30);
            txtUsername.Location = new Point((this.Width - txtUsername.Width) / 2, 150);

            txtPassword.Size = new Size(200, 30);
            txtPassword.Location = new Point((this.Width - txtPassword.Width) / 2, 200);

            btnLogin.Size = new Size(100, 30);
            btnLogin.Location = new Point((this.Width - btnLogin.Width) / 2 , 250);

            btnRegister.Size = new Size(100, 30);
            btnRegister.Location = new Point((this.Width - btnRegister.Width) / 2 , 250);

            txtNewUsername.Size = new Size(200, 30);
            txtNewUsername.Location = new Point((this.Width - txtNewUsername.Width) / 2, 150);

            txtNewPassword.Size = new Size(200, 30);
            txtNewPassword.Location = new Point((this.Width - txtNewPassword.Width) / 2, 200);

            // Add placeholder text for Username
            txtUsername.ForeColor = Color.Gray;


            // Add placeholder text for Password
            txtPassword.ForeColor = Color.Gray;

            // Create a clickable label for login
            lblToggleLogin.Text = "Already have an account? Click here to login.";
            lblToggleLogin.Font = new Font("Raleway", 12, FontStyle.Underline);
            lblToggleLogin.ForeColor = Color.FromArgb(101, 67, 33); // Dark brown
            lblToggleLogin.Location = new Point((this.Width - lblToggleLogin.Width) / 2, 300);
            lblToggleLogin.Click += (s, ev) =>
            {
                ToggleLoginFields();
            };
            lblToggleLogin.Visible = false; // Initially hidden
            this.Controls.Add(lblToggleLogin);

            // Create a clickable label for registration
            lblToggleRegistration.Text = "Don't have an account? Click here to register.";
            lblToggleRegistration.Font = new Font("Raleway", 12, FontStyle.Underline);
            lblToggleRegistration.ForeColor = Color.FromArgb(101, 67, 33); // Dark brown
            lblToggleRegistration.Location = new Point((this.Width - lblToggleRegistration.Width) / 2, 300);
            lblToggleRegistration.Click += (s, ev) =>
            {
                ToggleRegistrationFields();
            };
            this.Controls.Add(lblToggleRegistration);

                // Initialize checkbox
            chkRememberDetails.Text = "Remember login details";
            chkRememberDetails.Font = new Font("Raleway", 10, FontStyle.Regular);
            chkRememberDetails.ForeColor = Color.FromArgb(101, 67, 33); // Dark brown
            chkRememberDetails.Location = new Point((this.Width - chkRememberDetails.Width) / 2, 350);
            this.Controls.Add(chkRememberDetails);

            // Subscribe to events
            chkRememberDetails.CheckedChanged += ChkRememberDetails_CheckedChanged;
    
            // Load remembered details (if any)
            rememberedUsername = Properties.Settings.Default.RememberedUsername;
            rememberedPassword = Properties.Settings.Default.RememberedPassword;
            if (!string.IsNullOrWhiteSpace(rememberedUsername) && !string.IsNullOrWhiteSpace(rememberedPassword))
            {
                txtUsername.Text = rememberedUsername;
                txtPassword.Text = rememberedPassword;
                ((System.Windows.Forms.CheckBox)chkRememberDetails).Checked = true;
            }

            chkShowPassword = new CheckBox();
            chkShowPassword.Appearance = Appearance.Button; // Make it look like a button
            chkShowPassword.Size = new Size(30, 30); // Set the size of the checkbox
            chkShowPassword.FlatAppearance.BorderSize = 0; // Remove border
            chkShowPassword.FlatAppearance.CheckedBackColor = Color.Transparent; // Make it transparent when checked
            chkShowPassword.FlatAppearance.MouseDownBackColor = Color.Transparent; // Make it transparent on mouse down
            chkShowPassword.FlatAppearance.MouseOverBackColor = Color.Transparent; // Make it transparent on mouse over
            chkShowPassword.FlatStyle = FlatStyle.Flat; // Make it flat

            UpdateShowPasswordImage(); // Set initial image

            chkShowPassword.Location = new Point(txtPassword.Right + 10, txtPassword.Top);
            chkShowPassword.CheckedChanged += ChkShowPassword_CheckedChanged;
            this.Controls.Add(chkShowPassword);

            // Create a label for the clock
            lblClock = new Label();
            lblClock.Font = new Font("Raleway", 12, FontStyle.Regular);
            lblClock.ForeColor = Color.FromArgb(101, 67, 33); // Dark brown
            lblClock.Size = new Size(200, 30);
            lblClock.Location = new Point(welcomePanel.Width - lblClock.Width - 10, welcomePanel.Height - lblClock.Height - 10); // Position at bottom left
            welcomePanel.Controls.Add(lblClock);

            // Create a Timer to update the clock every second
            Timer clockTimer = new Timer();
            clockTimer.Interval = 1000; // 1 second
            clockTimer.Tick += UpdateClock;
            clockTimer.Start(); // Start the timer

            // Set initial control positions
            AdjustControlPositions();
            LoadData();

            this.Resize += (s, ev) =>
            {
                // Update positions for other controls
                AdjustControlPositions();
            };

            this.Controls.Add(welcomePanel);
            welcomePanel.Visible = false; // Initially hidden
        }

        private void BtnCreateNewProject_Click(object sender, EventArgs e)
        {
          string insertQuery = "INSERT INTO YourTableName (MachineName, Data, ProjectNumber, Pieces) " +
         "VALUES (@MachineName, @Data, @ProjectNumber, @Pieces)";

            using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, connection))
            {
                cmd.Parameters.AddWithValue("@MachineName", "Macchina1");
                cmd.Parameters.AddWithValue("@Data", "15/07/2023");
                cmd.Parameters.AddWithValue("@ProjectNumber", "C0.000708");
                cmd.Parameters.AddWithValue("@Pieces", "1pz");

                cmd.ExecuteNonQuery();
            }

            LoadData();
        }

        private void AdjustControlPositions()
        {
            // Adjust Welcome Panel Size
            welcomePanel.Size = new Size(this.Width, this.Height);

            // Set position for controls based on form size
            lblTitle.Location = new Point(10, 10);

            // Login Controls
            txtUsername.Location = new Point((this.Width - txtUsername.Width) / 2, 150);
            txtPassword.Location = new Point((this.Width - txtPassword.Width) / 2, 200);
            btnLogin.Location = new Point((this.Width - btnLogin.Width) / 2, 250);

            // Registration Controls
            txtNewUsername.Location = new Point((this.Width - txtNewUsername.Width) / 2, 150);
            txtNewPassword.Location = new Point((this.Width - txtNewPassword.Width) / 2, 200);
            btnRegister.Location = new Point((this.Width - btnRegister.Width) / 2, 250);

            // Toggle Labels
            lblToggleLogin.Location = new Point((this.Width - lblToggleLogin.Width) / 2, 300);
            lblToggleRegistration.Location = new Point((this.Width - lblToggleRegistration.Width) / 2, 300);

            // Welcome Panel and Buttons
            btnCreateNewProject.Location = new Point(10, lblTitle.Bottom + 10);
            lblWelcomeMessage.Location = new Point(welcomePanel.Width - lblWelcomeMessage.Width - 15, 10);
            lblLogout.Location = new Point(welcomePanel.Width - lblLogout.Width - 20, 50);
            // Remember Details Checkbox
            chkRememberDetails.Location = new Point((this.Width - chkRememberDetails.Width) / 2, 350);

            chkShowPassword.Location = new Point(txtPassword.Right + 10, txtPassword.Top);

            lblClock.Location = new Point(welcomePanel.Width - lblClock.Width - 10, welcomePanel.Height - lblClock.Height - 35);

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            connection.Close();
        }

        private void LblLogout_Click(object sender, EventArgs e)
        {
            // Reset login controls
            ToggleLoginFields();

            // Hide welcome panel and logout label
            welcomePanel.Visible = false;
            lblLogout.Visible = false;
            chkRememberDetails.Visible = true;
            chkShowPassword.Visible = true;
            txtPassword.PasswordChar = '•'; // Set default password character

            // Restore remembered details if checkbox is checked
            if (chkRememberDetails.Checked)
            {
                rememberedUsername = Properties.Settings.Default.RememberedUsername;
                rememberedPassword = Properties.Settings.Default.RememberedPassword;
                txtUsername.Text = rememberedUsername;
                txtPassword.Text = rememberedPassword; // Decrypt the password

                // Add Debugging Output
                Console.WriteLine($"Remembered Username: {rememberedUsername}");
                Console.WriteLine($"Remembered Password: {rememberedPassword}");

            }
            else
            {
                txtUsername.Text = "";
                txtPassword.Text = "";
            }

            this.Controls.Add(lblTitle);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            // Check if the checkbox is checked
            // MessageBox.Show("Checkbox Checked: " + chkRememberDetails.Checked.ToString());

            // Add this code to handle the checkbox
            if (chkRememberDetails.Checked)
            {
                Properties.Settings.Default.RememberedUsername = username;
                Properties.Settings.Default.RememberedPassword = password;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.RememberedUsername = "";
                Properties.Settings.Default.RememberedPassword = "";
                Properties.Settings.Default.Save();
            }

            // Verifica l'autenticazione (potresti implementare questa logica)

            // Esempio di autenticazione semplice
            if (username == "Lorenzo" && password == "password")
            {

                // Show welcome panel
                txtUsername.Visible = false;
                txtPassword.Visible = false;
                btnLogin.Visible = false;
                lblToggleLogin.Visible = false;
                lblToggleRegistration.Visible = false;
                chkShowPassword.Visible = false;

                // Show welcome message
                welcomePanel.Controls[0].Text = "Benvenuto, " + username + "!";
                lblLogout.Visible = true;
                welcomePanel.Visible = true;
                chkRememberDetails.Visible = false;

                // Adjust the size of lblWelcomeMessage
                lblWelcomeMessage.AutoSize = true; // Automatically adjust the size based on content

                // Recalculate the location of lblWelcomeMessage
                lblWelcomeMessage.Location = new Point(welcomePanel.Width - lblWelcomeMessage.Width - 15, 10);

                // Adjust the size of lblClock
                lblClock.Size = new Size(200, 30);

                // Recalculate the location of lblClock
                lblClock.Location = new Point(welcomePanel.Width - lblClock.Width -10, welcomePanel.Height - lblClock.Height - 35);

                welcomePanel.Controls.Add(lblTitle);
                // Make lblTitle visible
                lblTitle.Visible = true;
                LoadData();
            }
            else
            {
                MessageBox.Show("Credenziali non valide. Riprova.", "Errore di Autenticazione", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string newUsername = txtNewUsername.Text;
            string newPassword = txtNewPassword.Text;

            if (!string.IsNullOrWhiteSpace(newUsername) && !string.IsNullOrWhiteSpace(newPassword))
            {
                // Salva i dati del nuovo account (es. in un database)

                // Mostra un messaggio di conferma
                MessageBox.Show("Account registrato con successo!", "Registrazione Completata", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Per favore, inserisci un nome utente e una password.", "Campi Vuoti", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void ToggleRegistrationFields()
        {
            // Toggle the visibility of registration fields
            txtUsername.Visible = false;
            txtPassword.Visible = false;
            btnLogin.Visible = false;

            // Show the toggle label for login
            lblToggleLogin.Visible = true;

            // Hide the toggle label for registration
            lblToggleRegistration.Visible = false;

            // Show registration fields
            txtNewUsername.Visible = true;
            txtNewPassword.Visible = true;
            btnRegister.Visible = true;
        }

        private void ToggleLoginFields()
        {
            // Toggle the visibility of login fields
            txtUsername.Visible = true;
            txtPassword.Visible = true;
            btnLogin.Visible = true;

            // Show the toggle label for registration
            lblToggleRegistration.Visible = true;

            // Hide the toggle label for login
            lblToggleLogin.Visible = false;

            // Hide registration fields
            txtNewUsername.Visible = false;
            txtNewPassword.Visible = false;
            btnRegister.Visible = false;
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (btnLogin.Visible)
                {
                    btnLogin.PerformClick();
                }
                else if (btnRegister.Visible)
                {
                    btnRegister.PerformClick();
                }
            }
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (btnLogin.Visible)
                {
                    btnLogin.PerformClick();
                }
                else if (btnRegister.Visible)
                {
                    btnRegister.PerformClick();
                }
            }
        }

        private void txtNewUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (btnRegister.Visible)
                {
                    btnRegister.PerformClick();
                }
            }
        }

        private void txtNewPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (btnRegister.Visible)
                {
                    btnRegister.PerformClick();
                }
            }
        }

        private void ChkRememberDetails_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRememberDetails.Checked)
            {
                rememberedUsername = txtUsername.Text;
                rememberedPassword = txtPassword.Text; // Encrypt the password
                Properties.Settings.Default.RememberedUsername = rememberedUsername;
                Properties.Settings.Default.RememberedPassword = rememberedPassword;
                Properties.Settings.Default.Save();

                Console.WriteLine($"Checkbox Checked: {chkRememberDetails.Checked}");
                Console.WriteLine($"Remembered Username: {rememberedUsername}");
                Console.WriteLine($"Remembered Password: {rememberedPassword}");
            }
            else
            {
                rememberedUsername = null;
                rememberedPassword = null;
                Properties.Settings.Default.RememberedUsername = "";
                Properties.Settings.Default.RememberedPassword = "";
                Properties.Settings.Default.Save();

                Console.WriteLine($"Checkbox Checked: {chkRememberDetails.Checked}");
            }
        }

        private void UpdateClock(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            string formattedDate = now.ToString("dd MMMM yyyy HH:mm:ss");
            lblClock.Text = formattedDate;
        }

        private void ChkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            isPasswordVisible = !isPasswordVisible;
            UpdateShowPasswordImage();
            txtPassword.PasswordChar = isPasswordVisible ? '\0' : '•';
        }

        private void UpdateShowPasswordImage()
        {
            string imageName = isPasswordVisible ? "eye_open" : "eye_closed";
            Bitmap image = (Bitmap)Properties.Resources.ResourceManager.GetObject(imageName);

            // Resize the image
            int newWidth = 30; // Set the new width
            int newHeight = 30; // Set the new height
            Bitmap resizedImage = new Bitmap(image, newWidth, newHeight);

            chkShowPassword.Image = resizedImage;
        }
        private void LoadData()
        {
            string selectQuery = "SELECT * FROM YourTableName";
            using (SQLiteCommand cmd = new SQLiteCommand(selectQuery, connection))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    dataGridView.Columns.Clear(); // Clear existing columns

                    // Create a new DataGridViewCheckBoxColumn
                    DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
                    checkboxColumn.Name = "checkBoxColumn"; // You can give it any name you like
                    checkboxColumn.HeaderText = "checkBoxColumn"; // Header text for the column
                    checkboxColumn.ReadOnly = false; // Make sure it's not read-only

                    // Add the checkbox column to the DataGridView
                    dataGridView.Columns.Add(checkboxColumn);

                    if (dt.Rows.Count > 0)
                    {
                        dataGridView.DataSource = dt;
                    }
                    else
                    {
                        // If no rows, create columns with column names
                        foreach (DataColumn column in dt.Columns)
                        {
                            dataGridView.Columns.Add(column.ColumnName, column.ColumnName);
                        }
                    }
                }
            }
        }

        private void btnEliminaProgetto_Click(object sender, EventArgs e)
        {
            // Create a list to store the IDs of selected rows
            List<int> selectedIDs = new List<int>();

            // Iterate through the rows in the DataGridView
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                // Check if the checkbox is selected in the current row
                DataGridViewCheckBoxCell checkBoxCell = (DataGridViewCheckBoxCell)row.Cells["checkBoxColumn"];
                if (checkBoxCell != null && (bool)checkBoxCell.Value)
                {
                    // If checkbox is selected, retrieve the ID and add to the list
                    if (row.Cells["ID"] != null && row.Cells["ID"].Value != null)
                    {
                        if (int.TryParse(row.Cells["ID"].Value.ToString(), out int id))
                        {
                            selectedIDs.Add(id);
                        }
                        else
                        {
                            Console.WriteLine("Failed to parse ID value");
                        }
                    }
                    else
                    {
                        Console.WriteLine("row.Cells['ID'] is null or its value is null");
                    }
                }
                else
                {
                    Console.WriteLine("checkBoxCell is null or its value is false");
                }
            }

            // Delete the selected rows based on their IDs
            foreach (int selectedID in selectedIDs)
            {
                string deleteQuery = "DELETE FROM YourTableName WHERE ID = @ID";

                using (SQLiteCommand cmd = new SQLiteCommand(deleteQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", selectedID);
                    cmd.ExecuteNonQuery();
                }
            }

            // Update the DataGridView
            LoadData();

            // Clear the selection in case the deleted items were selected
            dataGridView.ClearSelection();
        }

        private void dataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // Check if the current cell is not in the checkbox column
            if (dataGridView.Columns[e.ColumnIndex] != dataGridView.Columns["checkBoxColumn"])
            {
                // Cancel the edit operation
                e.Cancel = true;
            }
        }

    }


}

