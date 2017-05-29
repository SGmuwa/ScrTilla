using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScrTilla
{
    public partial class FormPreferences : Form
    {

        public FormPreferences()
        {
            InitializeComponent();
            bufferServer = Settings.ADDRESS;
            textBox1.Text = Settings.ADDRESS;
            checkBox1.Checked = Settings.Save;
            checkBox2.Checked = Settings.Notifications;
            timer1.Interval = 100;
            timer1_Tick(this, null);
        }

        /// <summary>
        /// Создаёт экземпляр формы настроек
        /// </summary>
        /// <param name="Closed">Вызывается после закрытия формы.</param>
        /// <param name="e">Сюда передастся метод, который следует вызывать в том случае, если нужно вывести форму на экран.</param>
        public FormPreferences(FormClosedEventHandler Closed, ref Action e) : this()
        {
            base.FormClosed += Closed;
            e = ToMaximizame;
        }

        private void FormPreferences_Load(object sender, EventArgs e)
        {

        }

        private void FormPreferences_FormClosing(object sender, FormClosingEventArgs e)
        {
            // save
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(!timer1.Enabled && bufferServer != textBox1.Text)
            {
                timer1.Start();
            }
        }

        public void ToMaximizame()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// true, если в данный момент сервер думает
        /// </summary>
        private bool IsServerLoad = false;
        /// <summary>
        /// То, что клиент сейчас проверяет
        /// </summary>
        private string bufferServer = "";

        private async void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (!IsServerLoad)
            {
                IsServerLoad = true;
                bufferServer = textBox1.Text;
                pictureBox1.Image = Properties.Resources.Address_test_load;
                if (await Settings.SetSettingsByAddress(bufferServer))
                {
                    pictureBox1.Image = Properties.Resources.Address_test_ok;
                }
                else pictureBox1.Image = Properties.Resources.Address_test_error;
                IsServerLoad = false;
            }
            if (bufferServer != textBox1.Text) timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Settings.ToDefault();
        }

        /// <summary>
        /// Пользователь отметил галочку "Уведомлять"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Notifications = checkBox2.Checked;
        }

        /// <summary>
        /// Пользователь отметил галочку "Сохранить"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Save = checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveScr.Clear();
        }
    }
}
