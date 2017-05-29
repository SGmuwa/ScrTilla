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

        /// <summary>
        /// Сохранение изображения на диск.
        /// </summary>
        /// <param name="input">Данные, которые следует сохранить.</param>
        public static async void Save(byte[] input)
        {
            // Если папка, в которой должен храниться файл не существует,
            if (!Directory.Exists(DIRECT_NAME))
                // То создать папку
                Directory.CreateDirectory(DIRECT_NAME);
            FileStream fp = null;
            try
            {
                // Открытие потока записи файла
                using (
                    fp = new FileStream
                    (DIRECT_NAME + DateTime.Now.ToString("\\\\yyyy-MM-dd hh-mm-ss.pn\\g"), FileMode.CreateNew)
                    )
                {
                    // Асихронная запись инофрмации в файл.
                    await fp.WriteAsync(input, 0, input.Length);
                }
            }
            catch (IOException)
            {
                // Если с файлом что-то случилось, и ссылка на поток существует, то поток следует закрыть
                fp?.Close();
            }
            catch { }
        }

        internal static void Clear()
        {
            if(Directory.Exists(DIRECT_NAME)) Directory.Delete(DIRECT_NAME, true);
            Directory.CreateDirectory(DIRECT_NAME);
        }
    }
}
