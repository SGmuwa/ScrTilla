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
            
        }

        public void ToMaximizame()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}
