using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace fifteens
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public bool isGameStart;

        private int CountClick;

        private int Time;

        private Point Point_16 = new Point();

        private int[,] model = new int[4,4];

        private Button[,] button = new Button[4, 4];

        private bool isGameWin()
        {
            for (int i=0; i < 16; i++)
            {
                if (model[i / 4, i % 4] != i + 1) return false;
            }
            return true;
        }

        private bool IsGameCanFinish()
        {
            int sum = 0;
            for (int i = 0; i < 16; i++)
            {
                if (model[i / 4, i % 4] != 16)
                {
                    for (int j = i + 1; j < 16; j++)
                    {
                        if (model[j / 4, j % 4] < model[i / 4, i % 4]) sum++;
                    }
                }
                else sum += i / 4 + 1;
                                
            }
            return sum%2 == 0;
        }

        Label labPause = new Label();

        private void InitializeOtherGameComponent()
        {
            isGameStart = false;
            labPause.Font = new Font("",20);
            labPause.AutoSize = true;
            labPause.Enabled = true;
            labPause.Location = new Point(100,100);
            labPause.Text = "Пауза";
            CountClick = 0;
            Time = 0;
            timer1.Enabled = false;
            TimeToolStripStatusLabel.Text = "00:00:00";
            CountClickToolStripStatusLabel2.Text = "0";
        }

        private void InitializeModel()
        {
            timer1.Enabled = false;
            PauseToolStripMenuItem.Enabled = false;
            HashSet<int> Set_number = new HashSet<int>();
            Random random = new Random();            
            do
            {                
                Set_number.Clear();
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        int number;
                        do
                        {
                            number = random.Next(1, 17);
                        }
                        while (Set_number.Contains(number));
                        model[i, j] = number;
                        if (number == 16) Point_16 = new Point(i, j);
                        Set_number.Add(number);
                    }
                }
            }
            while (!IsGameCanFinish());            
        }


        private void InitializeButtons()
        {
            for (int i=0;i<4;i++)
            {
                for (int j = 0; j < 4; j++)
                {                    
                    button[i, j] = new Button();
                    button[i, j].Enabled = true;
                    button[i, j].Font = new Font("",28);
                    button[i, j].Location = new Point(73*(j), 27+73*(i));
                    button[i, j].Size = new Size(70, 70);
                    button[i, j].Visible = true;
                    button[i, j].BackColor = SystemColors.ControlDark;
                    button[i, j].Tag = new Point(i,j);
                    button[i,j].Click += new EventHandler(Buttons_Click);
                    Controls.Add(button[i,j]);
                }
            }            
        }

        private void UpdateButtons()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    button[i, j].Text = model[i, j].ToString();
                    if (model[i, j] == 16) button[i, j].Visible = false;
                    else button[i, j].Visible = true;
                }
            }
            CountClickToolStripStatusLabel2.Text = CountClick.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.InitializeModel();
            this.InitializeButtons();
            this.UpdateButtons();
            this.InitializeOtherGameComponent();
        }
        private void NewGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                button[i / 4, i % 4].Enabled = true;
            }
            this.InitializeModel();
            this.UpdateButtons();
            this.InitializeOtherGameComponent();
        }

        private void Swap(ref int a, ref int b)
        {
            int buf = a;
            a = b;
            b = buf;
        }

        private void Buttons_Click(object sender, EventArgs e)
        {                     
            Button BufButton = (sender as Button);
            Point BufPoint = (Point)BufButton.Tag;     
            if ((Math.Abs(Point_16.X - BufPoint.X) + Math.Abs(Point_16.Y - BufPoint.Y)) == 1)
            {
                isGameStart = true;
                PauseToolStripMenuItem.Enabled = true;
                Swap(ref model[Point_16.X, Point_16.Y], ref model[BufPoint.X, BufPoint.Y]);
                Point_16 = BufPoint;
                CountClick++;
                timer1.Enabled = true;
                UpdateButtons();
                if (this.isGameWin())
                {
                    timer1.Enabled = false;
                    PauseToolStripMenuItem.Enabled = false;
                    int hour = Time / 3600;
                    int min = (Time - hour * 3600) / 60;
                    int sek = Time % 60;
                    string hour_ = (hour / 10).ToString() + (hour % 10).ToString() + ":";
                    string min_ = (min / 10).ToString() + (min % 10).ToString() + ":";
                    string sek_ = (sek / 10).ToString() + (sek % 10).ToString();
                    string Time_ = hour_ + min_ + sek_;
                    MessageBox.Show("Время: "+Time_ + "\n" + "\n" + "  Ходы: "+CountClick.ToString(),"Победа");
                    this.InitializeModel();
                    this.UpdateButtons();
                    this.InitializeOtherGameComponent();
                }
            }            
        }

        private void RuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Rule_Form form = new Rule_Form(this);
            timer1.Enabled = false;
            form.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Time++;
            int hour = Time / 3600;
            int min = (Time - hour * 3600) / 60;
            int sek = Time % 60;
            string hour_ = (hour / 10).ToString() + (hour % 10).ToString() + ":";
            string min_ =  (min / 10).ToString() + (min % 10).ToString() + ":";
            string sek_ = (sek / 10).ToString() + (sek % 10).ToString();
            TimeToolStripStatusLabel.Text = hour_+min_+sek_;
        }

        private void PauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                if (model[i / 4, i % 4] != 16)
                    button[i / 4, i % 4].Visible = !button[i / 4, i % 4].Visible;
            }
            timer1.Enabled = !timer1.Enabled;
            if (!timer1.Enabled)
            {
                isGameStart = false;
                PauseToolStripMenuItem.Text = "пуск";
                Controls.Add(labPause);
            }
            else
            {
                isGameStart = true;
                Controls.Remove(labPause);
                PauseToolStripMenuItem.Text = "пауза";
            }
        }

        private void опрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Зелянов" + "\n" + "Петренко" + "\n" + "Старостин","Создатели");
        }
    }
}
