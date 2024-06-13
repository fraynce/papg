using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ConferenceManagementApp
{
    /// <summary>
    /// Логика взаимодействия для YCHWindow.xaml
    /// </summary>
    public partial class YCHWindow : Window
    {
        private string connectionString = "Data Source=localhost;Initial Catalog=PR1_1;Integrated Security=True";
        public List<Researcher> Researchers { get; set; }
        public List<Conference> Conferences { get; set; }
        public List<AnalysisResult> AnalysisResults { get; set; }


        public YCHWindow()
        {
            InitializeComponent();
            LoadData();
        }

        public class AnalysisResult
        {
            public string FullName { get; set; }
            public int NumberOfPresentations { get; set; }
        }

        private void LoadData()
        {
            Researchers = new List<Researcher>();
            Conferences = new List<Conference>();
            LoadResearchers();
            LoadConferences();

            analysisResultsDataGrid.ItemsSource = AnalysisResults;
            // Создайте список AnalysisResults и заполните его  
            AnalysisResults = new List<AnalysisResult>();

            // Загрузите данные из базы данных
            string query = "SELECT full_name, COUNT(*) AS number_of_presentations " +
                           "FROM Researchers " +
                           "INNER JOIN Participation ON Researchers.id = Participation.researcher_id " +
                           "GROUP BY full_name";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        AnalysisResults.Add(new AnalysisResult
                        {
                            FullName = reader.GetString(0),
                            NumberOfPresentations = reader.GetInt32(1)
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading analysis data: " + ex.Message);
                }
            }

            // Установите AnalysisResults в качестве источника данных для DataGrid
            analysisResultsDataGrid.ItemsSource = AnalysisResults;
        }

        private void LoadResearchers()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Researchers";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Researchers.Add(new Researcher
                        {
                            Id = reader.GetInt32(0),
                            FullName = reader.GetString(1),
                            Country = reader.GetString(2),
                            AcademicDegree = reader.GetString(3)
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading researchers: " + ex.Message);
                }
            }
        }

        private void LoadConferences()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Conferences";
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Conferences.Add(new Conference
                        {
                            ConferenceCode = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Date = reader.GetDateTime(2),
                            Location = reader.GetString(3)
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading conferences: " + ex.Message);
                }
            }
        }

       

        private void AddParticipation(int researcherId, int conferenceCode, string topic)
        {
            // Добавление записи о участии ученого в конференции
            string insertQuery = "INSERT INTO Participation (researcher_id, conference_code, topic) VALUES (@researcher_id, @conference_code, @topic)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(insertQuery, connection);
                command.Parameters.AddWithValue("@researcher_id", researcherId);
                command.Parameters.AddWithValue("@conference_code", conferenceCode);
                command.Parameters.AddWithValue("@topic", topic);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Participation information saved successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding participation: " + ex.Message);
                }
            }
        }

       

        private int GetNextConferenceCode()
        {
            int nextConferenceCode = 1; // Минимальный код конференции

            string query = "SELECT MAX(conference_code) FROM Conferences";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int maxConferenceCode = Convert.ToInt32(result);
                        nextConferenceCode = Math.Min(100000, maxConferenceCode + 1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            return nextConferenceCode;
        }

        

        private int GetNextEmployeeId()
        {
            int nextEmployeeId = 1000; // Минимальный табельный номер

            string query = "SELECT MAX(id) FROM Researchers";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int maxId = Convert.ToInt32(result);
                        nextEmployeeId = Math.Max(1000, maxId + 1); // Обновлено для корректного увеличения ID
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }

            return nextEmployeeId;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            searchErrorLabel.Content = "";
            int nextEmployeeId = GetNextEmployeeId();
            if (string.IsNullOrEmpty(fullNameSearchTextBox.Text))
            {
                searchErrorLabel.Content = "Вы не заполнили все поля";
                return;
            }
            string searchQuery = "SELECT Conferences.name, Conferences.date, Conferences.location, Participation.topic " +
                                 "FROM Researchers " +
                                 "INNER JOIN Participation ON Researchers.id = Participation.researcher_id " +
                                 "INNER JOIN Conferences ON Participation.conference_code = Conferences.conference_code " +
                                 "WHERE Researchers.full_name = @full_name " +
                                 "ORDER BY Conferences.date DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(searchQuery, connection);
                command.Parameters.AddWithValue("@full_name", fullNameSearchTextBox.Text);

                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    searchResultsDataGrid.ItemsSource = dataTable.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }

}
