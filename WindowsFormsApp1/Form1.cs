﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Firebase.Database;
using Firebase.Database.Query;



namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private FirebaseClient firebaseClient;


        public Bitmap HandlerTexure = Resource1.Handler,
                    TargetTexture = Resource1.Target;
        private Point _targetPosition = new Point(300, 300);
        private Point _direction = Point.Empty;
        private int _score = 0;
        public Form1()
        {
            InitializeComponent();

            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint, true);

            UpdateStyles();

            

            BackgroundImage = Image.FromStream(new WebClient().OpenRead("https://media.tenor.com/hoUL6fIhNvcAAAAd/doctor-livesey.gif"));
            ImageAnimator.Animate(BackgroundImage, OnFrameChanged);
            

            this.BackgroundImageLayout = ImageLayout.Stretch;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            firebaseClient = new FirebaseClient("https://dbgigalivesey-default-rtdb.europe-west1.firebasedatabase.app/");


        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => OnFrameChanged(sender, e)));
                return;
            }
            ImageAnimator.UpdateFrames();
            Invalidate(false);
        }

//


        private void timer1_Tick(object sender, EventArgs e)
        {
            Refresh();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Random r = new Random();
            timer2.Interval = r.Next(25, 1000);
            _direction.X = r.Next(-1, 2);
            _direction.Y = r.Next(-1, 2);


        }

        private void score_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            var localPosition = this.PointToClient(Cursor.Position);

            _targetPosition.X += _direction.X * 8;
            _targetPosition.Y += _direction.Y * 8;

            if (_targetPosition.X < 0 || _targetPosition.X > 500)
            {
                _direction.X *= -1;

            }
            if (_targetPosition.Y < 0 || _targetPosition.Y > 500)
            {
                _direction.Y *= -1;

            }
          

            Point between = new Point(localPosition.X - _targetPosition.X, localPosition.Y - _targetPosition.Y);
            float distance = (float)Math.Sqrt((between.X * between.X) + (between.Y * between.Y));

            if (distance < 30)
            {
                AddScore(1);
            }
            if (distance < 10)
            {
                AddScore(5);
            }

            var handlerRect = new Rectangle(localPosition.X - 50, localPosition.Y - 50, 100, 100);
            var targetRect = new Rectangle(_targetPosition.X - 50, _targetPosition.Y - 50, 100, 200);
            g.DrawImage(HandlerTexure, targetRect);
            g.DrawImage(TargetTexture, handlerRect);

        }




        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Вы действительно хотите выйти?", "Предупреждение", MessageBoxButtons.YesNoCancel);
            if (res == System.Windows.Forms.DialogResult.Yes)
            {
                Form1.ActiveForm.Close();

            }
            else
            {
                return;
            }
            Form1 form1 = (Form1)Application.OpenForms["Form1"];
            if (form1 != null)
            {




                this.Close();
            }
            else
                MessageBox.Show("Error while changing data in Form1.", "Error");

        }

        private void MenuBtn_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();

            SaveRecord();


        }

        public Form1(Form2 f)
        {
            InitializeComponent();
        }




        private void AddScore(int score)
        {
            _score += score;
            scoreLabel.Text = _score.ToString();
        }






        private async void SaveRecord()
        {
            // Получите имя игрока из TextBox и количество набранных очков из scoreLabel
            string playerName = textBoxPlayerName2.Text;
            int score = Convert.ToInt32(scoreLabel.Text);

            // Создайте экземпляр класса Record
            Record record = new Record
            {
                PlayerName = playerName,
                Score = score
            };

            // Инициализируйте FirebaseClient с ссылкой на вашу базу данных
            FirebaseClient firebaseClient = new FirebaseClient("https://dbgigalivesey-default-rtdb.europe-west1.firebasedatabase.app/");

            // Запишите рекорд игрока в базу данных
            await firebaseClient.Child("records").PostAsync(record);
        }



    }





}

