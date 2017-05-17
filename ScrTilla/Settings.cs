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
        /// Имя директории, где лежит файл
        /// </summary>
        private static readonly string DIRECT_NAME = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\ScrTilla";
        /// <summary>
        /// Имя файла
        /// </summary>
        private static readonly string FILE_NAME = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\ScrTilla\\settings.json";
        /// <summary>
        /// Контейнер, которых хранит в оперативной памяти настройки
        /// </summary>
        private static Conteiner cont;

        /// <summary>
        /// Конструктор статического класса
        /// </summary>
        static Settings()
        {
            StreamReader fp = null;
            try
            {
                if (File.Exists(FILE_NAME))
                {
                    using (fp = new StreamReader(FILE_NAME, System.Text.Encoding.UTF8))
                    {
                        cont = JsonConvert.DeserializeObject<Conteiner>(fp.ReadToEnd());
                    }
                }
                else
                {
                    CreateNewSettingFile();
                }
            }
            catch (JsonException e)
            {
                fp?.Close();
                CreateNewSettingFile();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Нет доступа к \"" + FILE_NAME + "\"\n\n" + e.Message, "ScrTilla", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        private static void CreateNewSettingFile()
        {

            cont = new Conteiner() { URI = "http://s.mtudev.ru/upload", PNGs = "http://s.mtudev.ru/" };
            if (!Directory.Exists(DIRECT_NAME))
            {
                Directory.CreateDirectory(DIRECT_NAME);
            }
            if(File.Exists(FILE_NAME))
            {
                File.Delete(FILE_NAME);
            }
            update();
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