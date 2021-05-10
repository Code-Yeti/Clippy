using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clippy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public Image resizeImage(Image image, int maxSize)
        {
            int width = maxSize;
            int height = maxSize;
            if (image.Width < maxSize) { 
                width = image.Width; 
            }
            if (image.Height < maxSize) { 
                height = image.Height; 
            }

            double ratioX = (double)width / (double)image.Width;
            double ratioY = (double)height / (double)image.Height;
            // use whichever multiplier is smaller
            double ratio = ratioX < ratioY ? ratioX : ratioY;

            // now we can get the new height and width
            int newHeight = Convert.ToInt32(image.Height * ratio);
            int newWidth = Convert.ToInt32(image.Width * ratio);
            Console.WriteLine("New width and height:" + newWidth.ToString() + " x " + newHeight.ToString());
            System.Drawing.Image newimage = new Bitmap(newWidth, newHeight); // changed parm names
            System.Drawing.Graphics graphic =
                         System.Drawing.Graphics.FromImage(newimage);

            graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphic.SmoothingMode = SmoothingMode.HighQuality;
            graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphic.CompositingQuality = CompositingQuality.HighQuality;

            graphic.Clear(Color.White); // white padding
            graphic.DrawImage(image, 0, 0, newWidth, newHeight);
            return newimage;
        }


        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                if (Clipboard.ContainsImage())
                {
                    System.Drawing.Image cpImage = null;
                    cpImage = Clipboard.GetImage();
                    Clipboard.SetImage(resizeImage(cpImage, 1920));

                    pictureBox1.Image = cpImage;
                    //Clipboard.SetImage(replacementImage);
                    notifyIcon1.BalloonTipText = "Image resized";
                    notifyIcon1.ShowBalloonTip(3);
                }
            }
            if (e.Button == MouseButtons.Right)
            {
                this.ShowInTaskbar = true;
                Show();
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
         
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
            Hide();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                //notifyIcon1.Visible = true;
            }
        }
    }
}
