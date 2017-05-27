namespace ScrTilla
{
    class json_st
    {
        public class ResponsePost
        {
            public ResponsePost()
            {
                filename = message = "";
            }
            public string filename { get; set; }
            public int code { get; set; }
            public string message { get; set; }
        }

        public class InfoGet
        {
            public InfoGet()
            {
                version = url = upload_dir = server_name = "";
            }
            /// <summary>
            /// Версия сервера.
            /// </summary>
            public string version;
            /// <summary>
            /// url сервера для сборки изображения.
            /// </summary>
            public string url;
            /// <summary>
            /// Адрес, куда нужно делать POST для загрузки изображения.
            /// </summary>
            public string upload_dir;
            /// <summary>
            /// Человеческое имя сервера для отображения.
            /// </summary>
            public string server_name;

            public override string ToString()
            {
                return "version: " + version + "\r\nurl: " + url + "\r\nupload_dir: " + upload_dir + "\r\nserver_name: " + server_name;
            }
        }
    }
}
