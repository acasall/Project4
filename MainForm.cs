using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient; //References the use of MySql
using System.Windows.Forms;
using System.IO;

namespace Project4
{
    public partial class MainForm : Form
    {
        string connstring = "Server=localhost;Database=project4db;User=root;Password=iHateminions9!";
        MySqlConnection connection; //Global connection object
        public MainForm()
        {
            InitializeComponent();
        }

        public void OpenDbConnection()
        {
            try
            {
                //creates a new database connection
                connection = new MySqlConnection(connstring);
                connection.Open(); //opens the db connection
            }
            catch (Exception ex) //gives us the exact error
            {
                MessageBox.Show("There was an error in establishing the connection: " + ex.Message); //concat. the actual message error
            }
        }

        public void CloseDbConnection()
        {
            if (connection != null)
            {
                connection.Close();
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)

        {
            //Checks if any of the required fields are empty
            if (string.IsNullOrWhiteSpace(txtID.Text) || string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtScore.Text))
            {
                MessageBox.Show("Please fill out all fields.");
                return;
            }

            OpenDbConnection();

            //gets values from the texboxes
            string studentName = txtName.Text;
            int studentId, score;

            //parses the ID and scores as integers
            int.TryParse(txtID.Text, out studentId);
            int.TryParse(txtScore.Text, out score);

            //SQL statement for inserting data into studentdata table
            string sqlStatement = "insert into studentdata(studentid, studentname, score) values ('" + studentId + "',  '" + studentName + "',  '" + score + "')";
            //executes the command
            MySqlCommand sqlcommand = new MySqlCommand(sqlStatement, connection);
            sqlcommand.ExecuteNonQuery();
            MessageBox.Show("Added data to the Student Data table");

            CloseDbConnection();
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            OpenDbConnection();

            //this creates the sql statemnt to be executed as a string
            string sqlStatement = "select * from studentdata";

            //Create a MySqlCommand option
            //Paremters are the Sql Statement and the db connection object
            MySqlCommand sqlCommand = new MySqlCommand(sqlStatement, connection);
            //Creates a MySqlDataReader object that reads data from the table
            MySqlDataReader reader = sqlCommand.ExecuteReader();
            //This creates a DataTable that stores row and column in memory
            DataTable dt = new DataTable();
            dt.Load(reader);
            gridData.DataSource = dt;


            CloseDbConnection();
        }

        private void tab2_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void lstGrade_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnExport_Click(object sender, EventArgs e)

        {
            try
            {
                // Open a StreamWriter to write to the file
                using (StreamWriter writer = new StreamWriter("studentgrades.txt"))
                {
                    OpenDbConnection();

                  //SQL statement to select allthe data from studentdata table
                    string sqlCommand = "select * from studentdata";
                    MySqlCommand command = new MySqlCommand(sqlCommand, connection);
                    MySqlDataReader reader = command.ExecuteReader();
                    DataTable dt = new DataTable();
                    dt.Load(reader);

                    foreach (DataRow row in dt.Rows)
                    {
                        // Converts objects to integers
                        int id = Convert.ToInt32(row["studentid"]);
                        string name = row["studentname"].ToString();
                        int score = Convert.ToInt32(row["score"]);
                        string grade;

                        //Determine the grade based on the score
                        if (score < 60)
                        {
                            grade = "F";
                        }
                        else if (score >= 60 && score <= 69)
                        {
                            grade = "D";
                        }
                        else if (score >= 70 && score <= 79)
                        {
                            grade = "C";
                        }
                        else if (score >= 80 && score <= 89)
                        {
                            grade = "B";
                        }
                        else // score is between 90 and 100
                        {
                            grade = "A";
                        }

                        // Write student data to the text file
                        writer.WriteLine($"{id}\t{name}\t{grade}");
                    }

                    MessageBox.Show("Data exported to studentgrades.txt");
                    CloseDbConnection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while exporting data: " + ex.Message);
            }
        }

        private void tabControl1_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {


            OpenDbConnection();

            //SQL statement to select all data from the studentdata table
            string sqlCommand = "select * from studentdata";
            MySqlCommand command = new MySqlCommand(sqlCommand, connection);
            MySqlDataReader reader = command.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);

            //puts student data into the lstGrade listbox
            foreach (DataRow row in dt.Rows)
            {
                //Converts objects to integers
                int id = Convert.ToInt32(row["studentid"]);
                string name = row["studentname"].ToString();
                int score = Convert.ToInt32(row["score"]);
                string grade;

                //Determine the grade based on the score
                if (score < 60)
                {
                    grade = "F";
                }
                else if (score >= 60 && score <= 69)
                {
                    grade = "D";
                }
                else if (score >= 70 && score <= 79)
                {
                    grade = "C";
                }
                else if (score >= 80 && score <= 89)
                {
                    grade = "B";
                }
                else // score is between 90 and 100
                {
                    grade = "A";
                }

               //Add student data to the lstGrade listbox
                lstGrade.Items.Add(string.Format("{0,20}{1,20}{2,20}", id, name, grade));


            }

            CloseDbConnection();

        }
    }
}
