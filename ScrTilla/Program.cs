using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;


namespace ScrTilla
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (Process.GetProcesses().Count(p => p.ProcessName == Process.GetCurrentProcess().ProcessName) > 1) return;
            PrtScr_Hook.StartHook(PrtHooked);
            using (ScrTilla.FormTaskbar f = new ScrTilla.FormTaskbar())
            {
                PrtScr_Hook.PrintScreen += f.ClipboardSet;
                Application.Run(f);
                PrtScr_Hook.PrintScreen -= f.ClipboardSet;
            }
            PrtScr_Hook.StopHook(PrtHooked);
        }

        private static async void PrtHooked(object sender, KeyEventArgs e)
        {
            PrtScr_Hook.StopHook(PrtHooked);
            json_st.ResponsePost Resp = new json_st.ResponsePost();
            try
            {
                byte[] Scr = Combine.GetScreen();
                if (Settings.Save) SaveScr.Save(Scr);
                Resp = await Combine.SendScreen(Combine.GetScreen());
                Scr = null; // Попытка явно очистить память, отвязав адреса
            }
            catch
            {
                Resp.message = "critical error";
            }
            if (Resp.filename.Length > 4 && Resp.code != 415 && Resp.code != 0)
            {
                Clipboard_s.ToClipboard(Settings.HTTP_ADDRESS + "/" + Resp.filename);
            }
            GC.Collect();
            PrtScr_Hook.StartHook(PrtHooked);
            GC.Collect();
        }
    }
}
