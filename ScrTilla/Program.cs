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
            PrtScr_Hook.StopHook(PrtHooked); // Во время обработки приостанавливаем перехват
            json_st.ResponsePost Resp = new json_st.ResponsePost(); // Хранилище ответа
            try
            {
                byte[] Scr = Combine.GetScreen(); // Шаг 2: получаем изображение
                if (Settings.Save) SaveScr.Save(Scr); // Сохраняем при надобности на диск
                Resp = await Combine.SendScreen(Scr); // Шаг 3: отправка изображения
                Scr = null;
            }
            catch
            {
                Resp.message = "Error"; // Если не удалось отправить изображение
            }
            if (Resp.filename.Length > 4 && Resp.code != 415 && Resp.code != 0)
            { // Шаг 4: Если результат положительный, то отправить его в буфер обмена
                Clipboard_s.ToClipboard(Settings.HTTP_ADDRESS + "/" + Resp.filename);
            }
            GC.Collect(); // Вызываем сборщик мусора дважды
            PrtScr_Hook.StartHook(PrtHooked); // Возобновление шага 1
            GC.Collect();
        }
    }
}
