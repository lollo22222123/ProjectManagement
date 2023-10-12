using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectManagement
{
    public partial class DeleteConfirmationForm : Form
    {
        // Property to hold the selected IDs
        public List<int> SelectedIDs { get; private set; }

        private SQLiteConnection connection;

        public DeleteConfirmationForm(SQLiteConnection connection)
        {
            InitializeComponent();

            this.connection = connection; // Set the connection

            checkedListBox1.DrawItem += checkedListBox1_DrawItem;
            // Add event handler for DrawItem
            checkedListBox1.DrawItem += new DrawItemEventHandler(checkedListBox1_DrawItem);

            // Call AddHeaders
            AddHeaders();

            // Style the CheckedListBox
            checkedListBox1.BorderStyle = BorderStyle.None;
            checkedListBox1.Font = new Font("Raleway", 12, FontStyle.Regular);

            List<KeyValuePair<int, string>> data = GetDataFromDatabase();
            checkedListBox1.DataSource = new BindingList<KeyValuePair<int, string>>(data);

            // Make the form responsive
            this.Resize += (sender, e) => AdjustControlPositions();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // Retrieve the selected IDs
            SelectedIDs = new List<int>();
            foreach (KeyValuePair<int, string> item in checkedListBox1.CheckedItems)
            {
                int id = item.Key;
                SelectedIDs.Add(id);
            }

            // Close the form with DialogResult.OK
            DialogResult = DialogResult.OK;
            Close();
        }

        private void DeleteConfirmationForm_Load(object sender, EventArgs e)
        {
            List<KeyValuePair<int, string>> data = GetDataFromDatabase();
            checkedListBox1.DataSource = new BindingList<KeyValuePair<int, string>>(data);
            checkedListBox1.DisplayMember = "Value";
            checkedListBox1.ValueMember = "Key";
        }

        private List<KeyValuePair<int, string>> GetDataFromDatabase()
        {
            List<KeyValuePair<int, string>> data = new List<KeyValuePair<int, string>>();

            string selectDataQuery = "SELECT ID, MachineName, Data, ProjectNumber, Pieces FROM YourTableName";

            using (SQLiteCommand cmd = new SQLiteCommand(selectDataQuery, connection))
            {
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string value = reader.GetString(1);
                        data.Add(new KeyValuePair<int, string>(id, value));
                    }
                }
            }

            return data;
        }

        private void AdjustControlPositions()
        {
            // Adjust the position of controls on form resize
            checkedListBox1.Width = this.ClientSize.Width - 40;
            btnConfirm.Location = new Point((this.ClientSize.Width - btnConfirm.Width) / 2, this.ClientSize.Height - btnConfirm.Height - 20);
        }

        private void checkedListBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            if (e.Index >= 0)
            {
                KeyValuePair<int, string> item = (KeyValuePair<int, string>)checkedListBox1.Items[e.Index];
                string text = item.Value;

                using (Brush brush = new SolidBrush(e.ForeColor))
                {
                    e.Graphics.DrawString(text, e.Font, brush, e.Bounds);
                }
            }

            e.DrawFocusRectangle();
        }

        private void AddHeaders()
        {
            checkedListBox1.Items.Add("Machine Name");
            checkedListBox1.Items.Add("Data");
            checkedListBox1.Items.Add("Project Number");
            checkedListBox1.Items.Add("Pieces");
        }

        private void DeleteHeaders()
        {
            while (checkedListBox1.Items.Count > 0)
            {
                checkedListBox1.Items.RemoveAt(0);
            }
        }
    }
}
