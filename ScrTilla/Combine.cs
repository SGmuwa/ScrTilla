using System.Drawing;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;

namespace ScrTilla
{
    static class Combine
    {
        /// <summary>
        /// Получает изображение экрана
        /// </summary>
        /// <returns>Фото экрана</returns>
        public static byte[] GetScreen()
        {
            // Получение размеров главного экрана и создание пустого изображения
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            // Создаём поверхность рисования 
            Graphics graphics = Graphics.FromImage(bitmap as Image);
            // Срисовываем то, что на экране
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            // Закрываем рисование
            graphics.Dispose(); graphics = null;
            // Отправляем результат
            return ImageToByte(bitmap);
        }

        private static HttpClient cl = new HttpClient();

        /// <summary>
        /// Отправка изображения на сервер
        /// </summary>
        /// <param name="image">Изображение, которое следует отправить.</param>
        /// <returns>Ответ от сервера</returns>
        public static async Task<json_st.ResponsePost> SendScreen(byte[] image)
        {
            // Создание контейнера для отправки
            var requ = new MultipartFormDataContent();
            // Конвектируем массив байтов в контент HTTP протокола
            var ImContent = new ByteArrayContent(image);
            // Указываем тип конента
            ImContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
            // Добавление сведений о контенте в контейнере
            requ.Add(ImContent, "up_image", "image.png");

            // Отправка изображения
            HttpResponseMessage response = await cl.PostAsync(Settings.UPLOAD, requ);
            // Уничтожение контейнера
            requ.Dispose();
            // Уничтожение контента HTTP
            ImContent.Dispose();
            // Конвектирование ответа JSON в структуру C#
            return await Newtonsoft.Json.JsonConvert.DeserializeObjectAsync<json_st.ResponsePost>
                (await response.Content.ReadAsStringAsync()); 
        }

        /// <summary>
        /// Получение сведений от сервера.
        /// </summary>
        /// <param name="HTTPaddress">Адрес севера в формате http://address. Если не задать, то значение будет взято Settings.HTTP_ADDRESS</param>
        /// <returns>Ответ от сервера</returns>
        public static async Task<json_st.InfoGet> GetInfo(string HTTPaddress = null)
        {
            try
            {
                return await Newtonsoft.Json.JsonConvert.DeserializeObjectAsync<json_st.InfoGet>(
                    await (await cl.GetAsync((HTTPaddress == null || HTTPaddress.Length == 0 ? Settings.HTTP_ADDRESS : HTTPaddress) + "/info", HttpCompletionOption.ResponseContentRead)).Content.ReadAsStringAsync());
            }
            catch
            {
                return new json_st.InfoGet();//await new Task<json_st.InfoGet>(delegate() { return new json_st.InfoGet(); });
            }
        }

        // http://stackoverflow.com/questions/7350679/convert-a-bitmap-into-a-byte-array
        private static byte[] ImageToByte(Image img)
        {
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
