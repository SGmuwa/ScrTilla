using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ScrTilla
{
    class Program
    {
        static void Main(string[] args)
        {
            PrtScr_Hook.PrintScreen += PrtScr_Hook_PrintScreen;
            PrtScr_Hook.StartHook();
            Application.Run();
            PrtScr_Hook.StopHook();
        }

        private static void PrtScr_Hook_PrintScreen(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Print Screen");
        }
    }


}
