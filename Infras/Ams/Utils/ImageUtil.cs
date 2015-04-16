using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Ams.Utils
{
    public static class ImageUtil
    {
        public static Byte[] GetBytes(string imagePath)
        {
            if (!File.Exists(imagePath)) return null;

            using (FileStream fs = new FileStream(imagePath, FileMode.Open))
            {
                byte[] ary = new byte[fs.Length];
                fs.Read(ary, 0, ary.Length);
                fs.Close();
                return ary;
            }
        }

        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static Byte[] GetCompress(string imagePath)
        {
            byte[] ary = GetBytes(imagePath);
            if (ary == null || ary.Length == 0) return null;

            //压缩图片
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream(ary))
            {
                ImageCompress ic = new ImageCompress(System.Drawing.Image.FromStream(memory));
                ic.FixedSize = new System.Drawing.Size(300, 410);

                if (ic.ImageSource.Width > 300 || ic.ImageSource.Height > 410)
                {
                    System.Drawing.Size size = ic.GetZoomSize(ic.ImageSource.Width, ic.ImageSource.Height);
                    return ic.CompressedToBytes(size, 75);
                }

                ic.Dispose();
                return ary;
            }
        }

        /// <summary>
        /// 获取缩略图
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        public static byte[] GetZoomSize(string imagePath)
        {
            //根据文件类型获取 

            if (!File.Exists(imagePath)) return null;

            using (System.Drawing.Image img = System.Drawing.Image.FromFile(imagePath))
            {
                ImageCompress ic = new ImageCompress(img);
                System.Drawing.Size size = new System.Drawing.Size(400, 600);
                if (img.Width <= size.Width && img.Height <= size.Height)
                {
                    //不需要压缩 
                    using (MemoryStream stream = new MemoryStream())
                    {
                        img.Save(stream, img.RawFormat);
                        //return stream.ToArray();
                    }
                }
                else
                {
                    //图像压缩 
                    ic.ImageSource = img;
                    ic.FixedSize = size;
                    size = ic.GetZoomSize(img.Width, img.Height);
                    //obj.Photo = ic.CompressedToBytes(size, 75);
                }

                //生成缩略图 
                ic.FixedSize = new System.Drawing.Size(110, 145);
                size = ic.GetZoomSize(img.Width, img.Height);
                return ic.CompressedToBytes(size, 85);
            }
        }

        /// <summary>
        /// 图片地址转图片
        /// </summary>
        /// <param name="imagePath">图片地址</param>
        /// <returns></returns>
        public static Image Path2Image(string imagePath)
        {
            byte[] ary = GetBytes(imagePath);
            if (ary == null) return null;

            using (MemoryStream ms = new MemoryStream(ary))
            {
                var img = Image.FromStream(ms);
                return img;
            }
        }

    }
}
