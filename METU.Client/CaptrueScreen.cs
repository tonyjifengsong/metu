using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace METU.Client
{
  public   class CaptrueScreen
    {
        private readonly HttpClient client = new HttpClient();

        private async void Monitor(object para)
        {
            client.BaseAddress = new Uri("http://localhost:5000");
            try
            {
                //截屏，并且将截屏数据序列化为byte数组发送给后台
                var v = await client.GetAsync("/Images");
                var result = await v.Content.ReadAsStringAsync();
                if (!Convert.ToBoolean(result))
                    return;
                Bitmap img = new Bitmap(Screen.AllScreens[0].Bounds.Width, Screen.AllScreens[0].Bounds.Height);
                Graphics g = Graphics.FromImage(img);
                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), Screen.AllScreens[0].Bounds.Size);
                using (MemoryStream ms = new MemoryStream())
                {
                    img.Save(ms, ImageFormat.Jpeg);
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    ms.Close();
                    await client.PostAsync("/Images", new ByteArrayContent(arr));
                }
            }
            catch (Exception ex)
            { }
        }
}
