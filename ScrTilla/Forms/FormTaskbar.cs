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
    public partial class FormTaskbar : Form
    {
        public FormTaskbar()
        {
            InitializeComponent();
            notifyIcon1.ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Настройки", n_OpenForm),
                new MenuItem("Закрыть", n_Close)
            });
            notifyIcon1.ShowBalloonTip(0, "ScrTilla", "Запущен", ToolTipIcon.Info);
        }

        private void n_Close(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FormTaskbar_Load(object sender, EventArgs e)
        {
            this.Hide();
        }

        private bool PerefencesOpen = false;
        private Action ToShowPreferences;

        private void n_OpenForm(object sender, EventArgs e)
        {
            if (!PerefencesOpen)
            {
                PerefencesOpen = true;
                new ScrTilla.FormPreferences(PrefencesClosed, ref ToShowPreferences).ShowDialog();
            }
            else
            {
                ToShowPreferences.Invoke();
            }
        }

        private void PrefencesClosed(object sender, FormClosedEventArgs e)
        {
            PerefencesOpen = false;
        }

        public void ClipboardSet(object sender, KeyEventArgs e)
        {
            notifyIcon1.ShowBalloonTip(0, "ScrTilla", "Ссылка скопирована", ToolTipIcon.Info);
        }
    }
}
