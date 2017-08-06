using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;

namespace printWin
{
    class CommonTool
    {
        public static void Log(String msg)
        {
            string currentDirectory = Environment.CurrentDirectory;
            string path = currentDirectory + "/log.txt";
            if (System.IO.File.Exists(path))
            {
                using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
                {
                    sw.WriteLine(new DateTime().ToString("yyyy-MM-dd HH:mm:ss -")+msg);
                    sw.Flush();
                }
            }
        }

        /// <summary>
        /// base64字符串转图片
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static Image getImageByString(String base64Str)

        {
            if (base64Str!=null)
            {
                int headIndex = base64Str.IndexOf("base64", StringComparison.Ordinal);
                if (headIndex >= 0)
                {
                    base64Str = base64Str.Substring(headIndex + 7);
                }

                MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(base64Str));
                Image fromStream = Image.FromStream(memoryStream);
                return fromStream;
            }
            return null;
        }

        public static Image DealImage(Image bitmap)
        {
            if (bitmap != null)
            {
                Bitmap newbitmap = bitmap.Clone() as Bitmap;
                Rectangle rect = new Rectangle(0, 0, newbitmap.Width, newbitmap.Height);
                System.Drawing.Imaging.BitmapData bmpdata = newbitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, newbitmap.PixelFormat);
                IntPtr ptr = bmpdata.Scan0;

                int bytes = newbitmap.Width * newbitmap.Height * 3;
                byte[] rgbvalues = new byte[bytes];

                System.Runtime.InteropServices.Marshal.Copy(ptr, rgbvalues, 0, bytes);

                double colortemp = 0;
                for (int i = 0; i < rgbvalues.Length; i += 3)
                {
                    colortemp = (rgbvalues[i + 2] + rgbvalues[i + 1] + rgbvalues[i])/3;
                    if (colortemp > 170)
                    {
                        rgbvalues[i] = rgbvalues[i + 1] = rgbvalues[i + 2] = 255;
                    }
                    else
                    {
                        rgbvalues[i] = rgbvalues[i + 1] = rgbvalues[i + 2] = 0;
                    }
                    
                }

                System.Runtime.InteropServices.Marshal.Copy(rgbvalues, 0, ptr, bytes);

                newbitmap.UnlockBits(bmpdata);
//                pictureBox1.Image = newbitmap.Clone() as Image;
                return newbitmap;
            }
            return null;
        }



        /// <summary>  
        /// 使用GDI+缩放图像。  
        /// </summary>  
        /// <param name="original">要缩放的图像。</param>  
        /// <param name="newWidth">新宽度。</param>  
        /// <param name="newHeight">新高度。</param>  
        /// <returns>缩放后的图像。</returns>  
        public static Bitmap ResizeUsingGDIPlus(Bitmap original, int newWidth, int newHeight)
        {
            try
            {
                Bitmap bitmap = new Bitmap(newWidth, newHeight);
                Graphics graphics = Graphics.FromImage(bitmap);

                // 插值算法的质量  
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(original, new Rectangle(0, 0, newWidth, newHeight),
                    new Rectangle(0, 0, original.Width, original.Height), GraphicsUnit.Pixel);
                graphics.Dispose();

                return bitmap;
            }
            catch
            {
                return null;
            }
        }
        private static double[,] GaussianBlur;//声明私有的高斯模糊卷积核函数  

        /// <summary>  
        /// 构造卷积（Convolution）类函数  
        /// </summary>  
         static CommonTool()
        {
            //初始化高斯模糊卷积核  
            int k = 273;
            GaussianBlur = new double[5, 5]{{(double)1/k,(double)4/k,(double)7/k,(double)4/k,(double)1/k},
                {(double)4/k,(double)16/k,(double)26/k,(double)16/k,(double)4/k},
                {(double)7/k,(double)26/k,(double)41/k,(double)26/k,(double)7/k},
                {(double)4/k,(double)16/k,(double)26/k,(double)16/k,(double)4/k},
                {(double)1/k,(double)4/k,(double)7/k,(double)4/k,(double)1/k}};
        }


        /// <summary>  
        /// 对图像进行平滑处理（利用高斯平滑Gaussian Blur）  
        /// </summary>  
        /// <param name="bitmap">要处理的位图</param>  
        /// <returns>返回平滑处理后的位图</returns>  
        public static Bitmap Smooth(Bitmap bitmap)
        {
            int[,,] InputPicture = new int[3, bitmap.Width, bitmap.Height];//以GRB以及位图的长宽建立整数输入的位图的数组  

            Color color = new Color();//储存某一像素的颜色  
            //循环使得InputPicture数组得到位图的RGB  
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    color = bitmap.GetPixel(i, j);
                    InputPicture[0, i, j] = color.R;
                    InputPicture[1, i, j] = color.G;
                    InputPicture[2, i, j] = color.B;
                }
            }

            int[,,] OutputPicture = new int[3, bitmap.Width, bitmap.Height];//以GRB以及位图的长宽建立整数输出的位图的数组  
            Bitmap smooth = new Bitmap(bitmap.Width, bitmap.Height);//创建新位图  
            //循环计算使得OutputPicture数组得到计算后位图的RGB  
            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    int R = 0;
                    int G = 0;
                    int B = 0;

                    //每一个像素计算使用高斯模糊卷积核进行计算  
                    for (int r = 0; r < 5; r++)//循环卷积核的每一行  
                    {
                        for (int f = 0; f < 5; f++)//循环卷积核的每一列  
                        {
                            //控制与卷积核相乘的元素  
                            int row = i - 2 + r;
                            int index = j - 2 + f;

                            //当超出位图的大小范围时，选择最边缘的像素值作为该点的像素值  
                            row = row < 0 ? 0 : row;
                            index = index < 0 ? 0 : index;
                            row = row >= bitmap.Width ? bitmap.Width - 1 : row;
                            index = index >= bitmap.Height ? bitmap.Height - 1 : index;

                            //输出得到像素的RGB值  
                            R += (int)(GaussianBlur[r, f] * InputPicture[0, row, index]);
                            G += (int)(GaussianBlur[r, f] * InputPicture[1, row, index]);
                            B += (int)(GaussianBlur[r, f] * InputPicture[2, row, index]);
                        }
                    }
                    color = Color.FromArgb(R, G, B);//颜色结构储存该点RGB  
                    smooth.SetPixel(i, j, color);//位图存储该点像素值  
                }
            }
            return smooth;
        }
    }

     

}
