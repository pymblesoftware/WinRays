using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using WinRays.Controllers;

namespace WinRays
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            bitmap.Lock();  
            // renderRandom();
            render();
            bitmap.Unlock(); // Lock() and Unlock() could be moved to the DrawRectangle() method. Just do some performance tests.

            var temp = BitmapFromWriteableBitmap(bitmap);
            image.Image = temp;

        }

        private void renderRandom()
        {
            int size = 1;

            Random rnd = new Random(DateTime.Now.Millisecond);

            for (int y = 0; y < 99; y++)
            {
                for (int x = 0; x < 99; x++)
                {
                    byte colR = (byte)rnd.Next(256);
                    byte colG = (byte)rnd.Next(256);
                    byte colB = (byte)rnd.Next(256);

                    DrawRectangle(bitmap, (size + 1) * x, (size + 1) * y, size, size, System.Windows.Media.Color.FromRgb(colR, colG, colB));
                }
            }

        }

        private void render()
        {
            Renderer renderer = new Renderer(bitmap);
            //            renderer.render(bitmap.Width, bitmap.Height);
            renderer.render(image.Width, image.Height);
        }

        private WriteableBitmap bitmap = new WriteableBitmap(1100, 1100, 96d, 96d, PixelFormats.Bgr24, null);

        private System.Drawing.Bitmap BitmapFromWriteableBitmap(WriteableBitmap writeBmp)
        {
            System.Drawing.Bitmap bmp;
            using (System.IO.MemoryStream outStream = new System.IO.MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create((BitmapSource)writeBmp));
                enc.Save(outStream);
                bmp = new System.Drawing.Bitmap(outStream);
            }
            return bmp;
        }

        public void DrawRectangle(WriteableBitmap writeableBitmap, int left, int top, int width, int height, System.Windows.Media.Color color)
        {
            // Compute the pixel's color
            int colorData = color.R << 16; // R
            colorData |= color.G << 8; // G
            colorData |= color.B << 0; // B
            int bpp = writeableBitmap.Format.BitsPerPixel / 8;

            unsafe
            {
                for (int y = 0; y < height; y++)
                {
                    // Get a pointer to the back buffer
                    int pBackBuffer = (int)writeableBitmap.BackBuffer;

                    // Find the address of the pixel to draw
                    pBackBuffer += (top + y) * writeableBitmap.BackBufferStride;
                    pBackBuffer += left * bpp;

                    for (int x = 0; x < width; x++)
                    {
                        // Assign the color data to the pixel
                        *((int*)pBackBuffer) = colorData;

                        // Increment the address of the pixel to draw
                        pBackBuffer += bpp;
                    }
                }
            }

            writeableBitmap.AddDirtyRect(new Int32Rect(left, top, width, height));
        }


        public BitmapSource ImageToBitmapSource(System.Drawing.Image image)
        {
            var bitmap = new System.Drawing.Bitmap(image);

            var bitSrc = BitmapToBitmapSource(bitmap);

            bitmap.Dispose();
            bitmap = null;

            return bitSrc;
        }

        public BitmapSource BitmapToBitmapSource(System.Drawing.Bitmap source)
        {
            BitmapSource bitSrc = null;

            var hBitmap = source.GetHbitmap();

            try
            {
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitSrc = null;
            }

            return bitSrc;
        }
    }


}
