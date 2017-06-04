using System;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ScrTilla
{
    static class Settings // Статический, так как одно приложение имеет один файл настроек
    {
        private struct Conteiner
        {
            /// <summary>
            /// Место загрузки изображений
            /// </summary>
            public string UPLOAD;
            /// <summary>
            /// Адрес сервера с конкетенацией: http://ADDRESS
            /// </summary>
            public string HTTP_ADDRESS;
            /// <summary>
            /// Место хранений изображений (не используется?)
            /// </summary>
            public string PNGs;
            /// <summary>
            /// Адрес сервера DNS или IP или полное местоположение обработчика на сервере
            /// Пример: s.mtudev.ru
            /// </summary>
            public string ADDRESS;
            /// <summary>
            /// Пользователь хочет сохранять изображения на компьютере
            /// </summary>
            public bool Save;
            /// <summary>
            /// Пользователь хочет получать уведомления
            /// </summary>
            public bool Notifications;
            /// <summary>
            /// Создаёт экземпляр с настройками по-умолчанию.
            /// </summary>
            public static Conteiner Default
            {
                get
                {
                    return
                        new Conteiner()
                        {
                            UPLOAD = "http://s.mtudev.ru/upload",
                            HTTP_ADDRESS = "http://s.mtudev.ru",
                            PNGs = "http://s.mtudev.ru",
                            ADDRESS = "s.mtudev.ru",
                            Save = false,
                            Notifications = false
                        }
                    ;
                }
            }
        }

        /// <summary>
        /// Установить PNGs и URI в зависимости от address.
        /// </summary>
        /// <param name="address">DNS или IP сервера.</param>
        /// <returns>true, если настройки применены успешно.</returns>
        public static async Task<bool> SetSettingsByAddress(string address)
        {
            if (address == null) return false;//UPLOAD = HTTP_ADDRESS = ADDRESS = PNGs = "";
            json_st.InfoGet info = await Combine.GetInfo("http://" + address);
            if (info.server_name == null || info.server_name.Length == 0 || info.upload_dir == null || info.upload_dir.Length == 0) return false;
            cont.ADDRESS = address;
            cont.HTTP_ADDRESS = "http://" + address;
            cont.UPLOAD = HTTP_ADDRESS + info.upload_dir;
            cont.PNGs = HTTP_ADDRESS;
            update();
            return true;
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
        /// Сбрасывает настройки по-умлочанию.
        /// </summary>
        public static void ToDefault()
        {
            cont = Conteiner.Default;
            update();
        }

        /// <summary>
        /// Принудительно вызвать конструктор класса
        /// </summary>
        public static void RefreshClass()
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
                    // если что-то не нашлось в настройках, создаём это сами:
                    // При таком подходе вполне возможно, что будет ADDRESS указывать на одно место, а HTTP_ADDRESS на default
                    // Чтобы это исправить, пользователю надо будет поприменить настройки в форме.
                    Conteiner Def = Conteiner.Default;
                    cont.ADDRESS = cont.ADDRESS == null ? Def.ADDRESS : cont.ADDRESS;
                    cont.HTTP_ADDRESS = cont.HTTP_ADDRESS == null ? Def.HTTP_ADDRESS : cont.HTTP_ADDRESS;
                    cont.PNGs = cont.PNGs == null ? Def.PNGs : cont.PNGs;
                    cont.UPLOAD = cont.UPLOAD == null ? Def.UPLOAD : cont.UPLOAD;
                }
                else
                {
                    CreateNewSettingFile();
                }
            }
            catch (JsonException)
            {
                fp?.Close();
                CreateNewSettingFile();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Нет доступа к \"" + FILE_NAME + "\"\n\n" + e.Message, "ScrTilla", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Конструктор статического класса
        /// </summary>
        static Settings()
        {
            cont = new Conteiner();
            RefreshClass();
        }

        /// <summary>
        /// Создаёт новый файл настроек
        /// </summary>
        private static void CreateNewSettingFile()
        {
            cont = Conteiner.Default;
            if (!Directory.Exists(DIRECT_NAME))
            {
                Directory.CreateDirectory(DIRECT_NAME);
            }
            if (File.Exists(FILE_NAME))
            {
                File.Delete(FILE_NAME);
            }
            update();
        }

        #region Свойства

        /// <summary>
        /// Свойство представляет собой адрес до загрузки фотографий
        /// </summary>
        public static string UPLOAD
        {
            get
            {
                return cont.UPLOAD;
            }
            set
            {
                cont.UPLOAD = value;
                update();
            }
        }

        /// <summary>
        /// Свойство представляет собой конкетенацию http:// и ADDRESS
        /// </summary>
        public static string HTTP_ADDRESS
        {
            get
            {
                return cont.HTTP_ADDRESS;
            }
            set
            {
                cont.HTTP_ADDRESS = value;
                update();
            }
        }

        /// <summary>
        /// Путь до места хранения PNGs (не используется?)
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
        /// Адрес как DNS или IP сервера
        /// </summary>
        public static string ADDRESS
        {
            get
            {
                return cont.ADDRESS;
            }
            set
            {
                cont.ADDRESS = value;
                update();
            }
        }

        /// <summary>
        /// Получает или задаёт: нужно ли сохранять изображение на компьютере?
        /// </summary>
        public static bool Save
        {
            get
            {
                return cont.Save;
            }
            set
            {
                cont.Save = value;
                update();
            }
        }

        /// <summary>
        /// Получает или задаёт: Нужно ли присылать пользователю уведомления?
        /// </summary>
        public static bool Notifications
        {
            get
            {
                return cont.Notifications;
            }
            set
            {
                cont.Notifications = value;
                update();
            }
        }

        #endregion Свойства

        /// <summary>
        /// Записать изменения в файл настроек
        /// </summary>
        private static void update()
        {
            StreamWriter fp = null;
            try
            {
                using (fp = new StreamWriter(FILE_NAME, false, System.Text.Encoding.UTF8))
                { // Записать в файл настроек все настройки программы в JSON формате
                    fp.Write(JsonConvert.SerializeObject(cont));
                }
            }
            catch { fp?.Close(); }
        }
    }
}

