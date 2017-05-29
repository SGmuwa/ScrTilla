using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ScrTilla
{
    static class Clipboard_s
    {
        [STAThread]
        public static void ToClipboard(string Text)
        {
            // Выполнить операцию требуется в отдельном потоке
            new Thread((delegate ()
            {
                // Эту операцию требуется делать, пока не будет результат
                while (true)
                {
                    lock (ProtectStaticThread) // Защитный замок
                        if (GetOpenClipboardWindow() == IntPtr.Zero) // Если свободен буфер обмена
                        {
                            try
                            {
                                // То отправляем текст в буфер обмена
                                Clipboard.SetText(Text);
                            }
                            catch
                            {
                                // В случае ошибки, пробуем закрыть буфер обмена с нашей стороны
                                CloseClipboard();
                                Thread.Sleep(1000);
                                continue;
                            }
                            // Очищаем память после работы с буфером обмена
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            GC.Collect();
                            return;
                        }
                        else
                        {
                            // Если буфер обмена занят, ожидаем секундочку
                            Thread.Sleep(1000);
                        }
                }
            }))
            // Запускать поток требуется в STA режиме
            { ApartmentState = ApartmentState.STA }.Start();
        }

        /// <summary>
        /// Организует защитный ключ для однопоточного обращения к статическому методу
        /// </summary>
        private static object ProtectStaticThread = new object();

        static Clipboard_s()
        {

        }

        private static string buffer = "";
        public static Thread UpdaterClipboard()
        {
            Thread th = new Thread(delegate ()
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    if (buffer != "")
                    {
                        //try
                        {
                            Clipboard.SetText(buffer);
                        }
                        //catch { }
                        buffer = "";
                    }
                }
            });
            th.SetApartmentState(ApartmentState.STA); th.Start();
            return th;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetOpenClipboardWindow();

        [DllImport("user32.dll")]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);
    }
}
