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
        /// Получает изображение экранов
        /// </summary>
        /// <returns>Фото экранов</returns>
        public static byte[] GetScreen()
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(bitmap as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            graphics.Dispose(); graphics = null;
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
            var requ = new MultipartFormDataContent();
            var ImContent = new ByteArrayContent(image);
            ImContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/png");
            requ.Add(ImContent, "up_image", "image.png");

            HttpResponseMessage response = await cl.PostAsync(Settings.URI, requ);
            requ.Dispose();
            ImContent.Dispose();
            return await Newtonsoft.Json.JsonConvert.DeserializeObjectAsync<json_st.ResponsePost>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Получение сведений от сервера.
        /// </summary>
        /// <param name="image">Изображение, которое следует от</param>
        /// <returns>Ответ от сервера</returns>
        public static async Task<json_st.InfoGet> GetInfo()
        {
            var requ = new MultipartFormDataContent();
            HttpResponseMessage response = await cl.GetAsync(Settings.URI);
            requ.Dispose();
            return await Newtonsoft.Json.JsonConvert.DeserializeObjectAsync<json_st.InfoGet>(await response.Content.ReadAsStringAsync());
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
