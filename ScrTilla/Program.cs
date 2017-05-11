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
            PrtScr_Hook.StartHook(PrtHooked);
            Application.Run();
            PrtScr_Hook.StopHook(PrtHooked);
        }

        private static void PrtHooked(object sender, KeyEventArgs e)
        {
            Combine.GetScreen();
        }
    }
}
