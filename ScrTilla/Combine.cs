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
        public static Bitmap GetScreen()
        {
            Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics graphics = Graphics.FromImage(bitmap as Image);
            graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
            graphics.Dispose(); graphics = null;
            return bitmap;
        }

        private static HttpClient cl = new HttpClient();

        public static async Task<string> SendScreen(Image image)
        {
            var requ = new MultipartFormDataContent();
            requ.Add(new ByteArrayContent(ImageToByte(image)), "up_image" /*, "image.png"*/); // image.png - отсутсвует в данном проекте

            HttpResponseMessage response = await cl.PostAsync("", requ);
            return await response.Content.ReadAsStringAsync();
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
