using System;
using System.Drawing;
using System.IO;

namespace ScrTilla
{
    static class SaveScr
    {
        /// <summary>
        /// Имя директории, где лежит файл
        /// </summary>
        private static readonly string DIRECT_NAME = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\ScrTilla\\Cashe";

        static SaveScr()
        {
            if (!Directory.Exists(DIRECT_NAME)) Directory.CreateDirectory(DIRECT_NAME);
        }

        public static async void Save(byte[] input)
        {
            if (!Directory.Exists(DIRECT_NAME)) Directory.CreateDirectory(DIRECT_NAME);
            FileStream fp = null;
            //try
            {
                using (fp = new FileStream(DIRECT_NAME + DateTime.Now.ToString("\\\\yyyy-MM-dd hh-mm-ss.pn\\g"), FileMode.CreateNew))
                {
                    await fp.WriteAsync(input, 0, input.Length);
                }
            }
            //catch
            {
            //    fp?.Close();
            }
        }

        internal static void Clear()
        {
            if(Directory.Exists(DIRECT_NAME)) Directory.Delete(DIRECT_NAME, true);
            Directory.CreateDirectory(DIRECT_NAME);
        }
    }
}
