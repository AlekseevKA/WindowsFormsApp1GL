using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Firebase.Database;
using Firebase.Database.Query;

namespace WindowsFormsApp1
{
    public partial class RecordsForm : Form
    {
        private FirebaseClient firebaseClient;

        public RecordsForm()
        {
            InitializeComponent();
            firebaseClient = new FirebaseClient("https://dbgigalivesey-default-rtdb.europe-west1.firebasedatabase.app/");
            dataGridView.Columns.AddRange(
                new DataGridViewTextBoxColumn { Name = "PlayerNameColumn", HeaderText = "Player Name" },
                new DataGridViewTextBoxColumn { Name = "ScoreColumn", HeaderText = "Score" });
        }

        private async void RecordsForm_Load(object sender, EventArgs e)
        {
            await LoadPlayerScores();
        }

        private async void refreshButton_Click(object sender, EventArgs e)
        {
            await LoadPlayerScores();
        }

        private async Task LoadPlayerScores()
        {
            try
            {
                // Запрос на получение данных из базы данных
                var records = await firebaseClient.Child("records").OnceAsync<Dictionary<string, Record>>();

                // Очистка DataGridView перед загрузкой новых данных
                dataGridView.Rows.Clear();

                // Заполнение DataGridView данными об очках игроков
                foreach (var recordSet in records)
                {
                    foreach (var record in recordSet.Object)
                    {
                        // Извлечение значений PlayerName и Score из объекта record
                        string playerName = record.Value.PlayerName;
                        int score = record.Value.Score;

                        // Добавление записи в DataGridView
                        dataGridView.Rows.Add(playerName, score);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке данных: " + ex.Message);
            }
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void refreshButton_Click_1(object sender, EventArgs e)
        {
            LoadPlayerScores().Wait();
        }
    }
}
