using System;
using System.IO;
using Newtonsoft.Json;

namespace ScrTilla
{
    static class Settings // Статический, так как одно приложение имеет один файл настроек
    {
        private struct Conteiner
        {
            /// <summary>
            /// Место загрузки изображений
            /// </summary>
            public string URI;
            /// <summary>
            /// Место хранения изображений
            /// </summary>
            public string PNGs;
        }
        /// <summary>
        /// Имя файла
        /// </summary>
        private const string FILE_NAME = "settings.json";
        /// <summary>
        /// Контейнер, которых хранит в оперативной памяти настройки
        /// </summary>
        private static Conteiner cont;

        /// <summary>
        /// Конструктор статического класса
        /// </summary>
        static Settings()
        {
            try
            {
                if (File.Exists(FILE_NAME))
                {
                    using (StreamReader fp = new StreamReader(FILE_NAME, System.Text.Encoding.UTF8))
                    {
                        cont = JsonConvert.DeserializeObject<Conteiner>(fp.ReadToEnd());
                    }
                }
                else
                {
                    cont = new Conteiner() { URI = "http://s.mtudev.ru/upload" };
                    File.Create(FILE_NAME);
                    update();
                }
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("ScrTilla", "Нет доступа к \"" + FILE_NAME + "\"\n\n" + e.Message, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Свойство представляет собой адрес до загрузки фотографий
        /// </summary>
        public static string URI
        {
            get
            {
                return cont.URI;
            }
            set
            {
                cont.URI = value;
                update();
            }
        }

        /// <summary>
        /// Свойство представляет собой адрес до открытия фотографий
        /// </summary>
        public static string PNGs
        {
            get
            {
                return cont.PNGs;
            }
            set
            {
                cont.PNGs = value;
                update();
            }
        }

        /// <summary>
        /// Записать изменения в файл настроек
        /// </summary>
        private static void update()
        {
            try
            {
                using (StreamWriter fp = new StreamWriter(FILE_NAME, false, System.Text.Encoding.UTF8))
                {
                    fp.Write(JsonConvert.SerializeObject(cont));
                }
            }
            catch
            {
                // ??
                // Не будем спамить назойливыми сообщениями
            }
        }
    }
}