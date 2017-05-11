using System.Drawing;
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

        private static async void PrtHooked(object sender, KeyEventArgs e)
        {
            Image im = Combine.GetScreen();
            try
            {
                MessageBox.Show(await Combine.SendScreen(im));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
