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
            new Thread((delegate ()
            {
                while (true)
                {
                    lock (ProtectStaticThread) // Защитный замок
                        if (GetOpenClipboardWindow() == IntPtr.Zero)
                        {
                            try
                            {
                                Clipboard.SetText(Text);
                            }
                            catch
                            {
                                CloseClipboard();
                            }
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                            return;
                        }
                        else
                        {
                            Thread.Sleep(1000);
                        }
                }
            }))
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
