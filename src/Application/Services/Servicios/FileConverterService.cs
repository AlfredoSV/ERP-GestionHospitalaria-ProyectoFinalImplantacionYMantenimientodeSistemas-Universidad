using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.IO;
using Application.IServicios;
namespace Application.Servicios
{
    public class FileConverterService : IFileConvertService
    {
        private string placeHolder = "/images/placeholder.svg";
        public string PlaceHolder
        {
            get
            {
                return this.placeHolder;
            }
            private set { }
        }

        public string ConvertirABase64(Stream file, int w = 256)
        {
            if (file.Length > 0)
            {
                var ms = new MemoryStream();
                file.CopyTo(ms);

                ms = RediImagen(ms, w);

                var fileBytes = ms.ToArray();
                return "data:image;base64," + Convert.ToBase64String(fileBytes);
            }
            else
            {
                return PlaceHolder;
            }
        }

        public MemoryStream RediImagen(MemoryStream ms, int w)
        {
            Image img = Image.FromStream(ms);
            int h = Convert.ToInt32(w * img.Height / img.Width);
            Image imgN = img.GetThumbnailImage(w, h, null, IntPtr.Zero);
            MemoryStream res = new MemoryStream();
            imgN.Save(res, img.RawFormat);
            return res;
        }

        public string ConvertirPDFABase64(IFormFile file)
        {
            string s;
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                s = Convert.ToBase64String(fileBytes);

            }
            return s;

        }
    }
}
