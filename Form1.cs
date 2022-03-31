using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq;

namespace AddressBook_WinForm
{
    public partial class Form1 : Form
    {
        SqlConnection connection;

        public Form1()
        {
            InitializeComponent();
            string DB = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\User\source\repos\Address_Book\Database.mdf;Integrated Security=True";
            connection = new SqlConnection(DB);
            connection.Open();
        }

        private void ShowAllContacts(object sender, EventArgs e)
        {
            Func();
        }

        private void Func()
        {
            SqlDataReader sqlReader = null;
            SqlCommand command = new SqlCommand("SELECT * FROM [Contacts]", connection);
            sqlReader = command.ExecuteReader();
            dataGridView1.Rows.Clear();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            while (sqlReader.Read())
            {
                dataGridView1.Rows.Add((string)sqlReader["Name"], (string)sqlReader["Number"], (string)sqlReader["Email"], (string)sqlReader["Address"], sqlReader["Id"]);
            }
            sqlReader.Close();
        }

        private void ViewSingleContact(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            SqlCommand command = new SqlCommand("SELECT * FROM [Contacts] WHERE Name LIKE '%"+textBox1.Text+"%'", connection);
            command.Parameters.AddWithValue("Name", textBox1.Text);
            SqlDataReader sqlReader = command.ExecuteReader();
            while (sqlReader.Read())
            {
                dataGridView1.Rows.Add((string)sqlReader["Name"], (string)sqlReader["Number"], (string)sqlReader["Email"], (string)sqlReader["Address"], sqlReader["Id"]);
            }
            sqlReader.Close();
        }

        private void CreateContact(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(textBox1.Text) && !textBox1.Text.Any(char.IsNumber) && !string.IsNullOrEmpty(textBox2.Text) && !textBox2.Text.Any(c => (c < '0' || c > '9') && c != ' '))
            {
                SqlCommand command = new SqlCommand("INSERT INTO [Contacts] (Name, Number, Email, Address) VALUES(@Name, @Number, @Email, @Address)", connection);
                command.Parameters.AddWithValue("Name", textBox1.Text);
                command.Parameters.AddWithValue("Number", textBox2.Text);
                command.Parameters.AddWithValue("Email", textBox3.Text);
                command.Parameters.AddWithValue("Address", textBox4.Text);
                command.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("Invalid Value");
                return;
            }
            Func();
        }

        private void EditContact(object sender, EventArgs e)
        {
            var id = dataGridView1.SelectedCells[4].Value;
            if (!string.IsNullOrEmpty(textBox1.Text) && !textBox1.Text.Any(char.IsNumber) && !string.IsNullOrEmpty(textBox2.Text) && !textBox2.Text.Any(c => (c < '0' || c > '9') && c != ' '))
            {
                SqlCommand command = new SqlCommand("UPDATE [Contacts] SET [Name]=@Name, [Number]=@Number, [Email]=@Email, [Address]=@Address WHERE [Id]=@Id", connection);
                command.Parameters.AddWithValue("Name", textBox1.Text);
                command.Parameters.AddWithValue("Number", textBox2.Text);
                command.Parameters.AddWithValue("Email", textBox3.Text);
                command.Parameters.AddWithValue("Address", textBox4.Text);
                command.Parameters.AddWithValue("Id", id);
                command.ExecuteNonQuery();
            }
            else
            {
                MessageBox.Show("Invalid Value");
                return;
            }

            Func();
        }

        private void DeleteContact(object sender, EventArgs e)
        {
            string name = (string)dataGridView1.SelectedCells[0].Value;
            SqlCommand command = new SqlCommand("DELETE FROM [Contacts] WHERE [Name]=@Name", connection);
            command.Parameters.AddWithValue("Name", name);
            command.ExecuteNonQuery();

            Func();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = (string)dataGridView1.SelectedCells[0].Value;
            textBox2.Text = (string)dataGridView1.SelectedCells[1].Value;
            textBox3.Text = (string)dataGridView1.SelectedCells[2].Value;
            textBox4.Text = (string)dataGridView1.SelectedCells[3].Value;
        }

        private void textBox1_FullName(object sender, EventArgs e)
        {

        }

        private void textBox2_Email(object sender, EventArgs e)
        {

        }

        private void textBox3_Number(object sender, EventArgs e)
        {

        }

        private void textBox4_Address(object sender, EventArgs e)
        {

        }
    }
}
