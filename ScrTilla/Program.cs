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
        /// <summary>
        /// Запускает диагностику по работе с другими процессами.
        /// </summary>
        /// <returns>True - требуется закрыть приложение. False - программа может продолжать работу.</returns>
        private static bool ProcessRepair()
        {
            try
            {
                // Приложение "ScrTilla.exe" было найдено в списке выполняемых процессов. Новый экземпляр приложения запускать опасно из-за повторного перехвата "Print Screen"; запускать опасно из-за файла настроек Settings.json, который используется всеми копиями процессов "ScrTilla.exe". Вы можете продолжить запуск нового экземпляра для подключения к иному серверу, но данная функция не предусмотрена данным приложением из-за статического экземпляра настроек.
                switch (MessageBox.Show("The application is already running.\n\nThe application \"" + Process.GetCurrentProcess().ProcessName + "\" was found in the list of running processes. A new instance of the application is dangerous to run because of the repeated interception of the \"Print Screen\"; It's dangerous to run because of Settings.json settings file, which is used by all copies of the processes \"" + Process.GetCurrentProcess().ProcessName + "\". You can continue to start a new instance to connect to a different server, but this function is not provided by this application because of a static instance of the settings.\nContinue?",
                    Process.GetCurrentProcess().ProcessName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:
                        {
                            switch (MessageBox.Show("Close other processes with the name \"" + Process.GetCurrentProcess().ProcessName + "\"?\nYes - Close other.\nNo - Don't close processes.",
                                "ScrTilla.exe", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                            {
                                case DialogResult.Yes:
                                    {
                                        foreach (var p in Process.GetProcessesByName("ScrTilla.exe"))
                                        {
                                            if (p.Id != Process.GetCurrentProcess().Id) p.Close();
                                        }
                                        return false;
                                    }
                                default: return false;
                            }
                        }
                    default: return true;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), Process.GetCurrentProcess().ProcessName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }

        static void Main(string[] args)
        {
            if (Process.GetProcesses().Count(p => p.ProcessName == Process.GetCurrentProcess().ProcessName) > 1)
            { // Найден иной процесс с данным названием
                if(ProcessRepair()) return;
            }
            if (!System.IO.File.Exists("Newtonsoft.Json.dll"))
            { // Не найдена важнейшая библиотека для чтения и записи в стандарте Json
                switch(MessageBox.Show("Newtonsoft.Json.dll not found!\nContinue?", "ScrTilla", MessageBoxButtons.YesNo, MessageBoxIcon.Error))
                {
                    case DialogResult.No: return;
                    case DialogResult.None: return;
                    case DialogResult.Cancel: return;
                }
            }
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
