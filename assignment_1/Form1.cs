using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace assignment_1
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;


        }

        private Bitmap imgDefault = null;
        public Bitmap image;




        //import image
        private void openBtn(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "bitmap (*.bmp)|*.bmp";
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (imgDefault != null)
                        imgDefault.Dispose();
                    imgDefault = (Bitmap)Bitmap.FromFile(openFileDialog1.FileName, false);

                }
                catch (Exception)
                {
                    MessageBox.Show("Can not open file", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

            image = new Bitmap(imgDefault.Width, imgDefault.Height);
            //image pixel

            for (int i = 0; i < imgDefault.Width; i++)
            {
                for (int j = 0; j < imgDefault.Height; j++)
                {
                    Color PixelColor = imgDefault.GetPixel(i, j);
                    int gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    image.SetPixel(i, j, Color.FromArgb(gray, gray, gray));
                }
            }

            pictureBox1.Image = imgDefault;
            pictureBox2.Image = image;

            textBox1.Text = "Default image";
            

            //histogram
            chart1.Series[0].Points.Clear();
         

            int[] hist = new int[256];
            for (int i = 0; i < imgDefault.Width; i++)
            {
                for (int j = 0; j < imgDefault.Height; j++)
                {
                    Color PixelColor = imgDefault.GetPixel(i, j);
                    int gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    hist[gray]++;

                    
                    chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    chart1.ChartAreas[0].AxisX.Title = "Gray Level";
                    chart1.ChartAreas[0].AxisY.Title = "No. of Pixels";
                    //chart1.ChartAreas[0].AxisX.Maximum = 255;
                    chart1.ChartAreas[0].AxisX.Minimum = 0;
                    chart1.Series[0].Points.AddXY(gray, hist[gray]);
                }
            }

            

          
            


        }





        //threshold
        private void thresholdBtn(object sender, EventArgs e)
        {
            int threshold = 0;
            int meanGrayLevel = 0;

            for (int i = 0; i < imgDefault.Width; i++)
            {
                for (int j = 0; j < imgDefault.Height; j++)
                {
                    Color PixelColor = imgDefault.GetPixel(i, j);
                    int C_gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    meanGrayLevel += C_gray;
                    
                }
            }
            meanGrayLevel = meanGrayLevel / (imgDefault.Width * imgDefault.Height);
            threshold = meanGrayLevel;



            Bitmap imgOutput = new Bitmap(imgDefault.Width, imgDefault.Height);

            for (int i = 0; i < imgDefault.Width; i++)
            {
                for (int j = 0; j < imgDefault.Height; j++)
                {
                    Color PixelColor = imgDefault.GetPixel(i, j);
                    int gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    if (gray >= threshold)
                    {
                        imgOutput.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                    }
                    else
                    {
                        imgOutput.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                    }
                }
            }
            pictureBox2.Image = imgOutput;
            textBox1.Text = "Threshold";


            //histogram
            chart2.Series[0].Points.Clear();


            int[] hist = new int[256];
            for (int i = 0; i < imgOutput.Width; i++)
            {
                for (int j = 0; j < imgOutput.Height; j++)
                {
                    Color PixelColor = imgOutput.GetPixel(i, j);
                    int gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    hist[gray]++;


                    chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    chart2.ChartAreas[0].AxisX.Title = "Gray Level";
                    chart2.ChartAreas[0].AxisY.Title = "No. of Pixels";
                    //chart2.ChartAreas[0].AxisX.Maximum = 255;
                    chart2.ChartAreas[0].AxisX.Minimum = 0;
                    chart2.Series[0].Points.AddXY(gray, hist[gray]);
                }
            }



        }

        //quantization level 4
        private void quantizationLevel4Btn(object sender, EventArgs e)
        {
            Bitmap imgOutput = new Bitmap(imgDefault.Width, imgDefault.Height);

            for (int i = 0; i < imgDefault.Width; i++)
            {
                for (int j = 0; j < imgDefault.Height; j++)
                {
                    Color PixelColor = imgDefault.GetPixel(i, j);
                    int C_gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    if (C_gray <= 64)
                        imgOutput.SetPixel(i, j, Color.FromArgb(32, 32, 32));
                    else if (C_gray <= 128)
                        imgOutput.SetPixel(i, j, Color.FromArgb(96, 96, 96));
                    else if (C_gray <= 192)
                        imgOutput.SetPixel(i, j, Color.FromArgb(160, 160, 160));
                    else
                        imgOutput.SetPixel(i, j, Color.FromArgb(224, 224, 224));
                }
            }
            pictureBox2.Image = imgOutput;
            textBox1.Text = "Quantization 4 levels";

            //histogram

            chart2.Series[0].Points.Clear();
            int[] hist = new int[256];

            for (int i = 0; i < imgOutput.Width; i++)
            {
                for (int j = 0; j < imgOutput.Height; j++)
                {
                    Color PixelColor = imgOutput.GetPixel(i, j);
                    int gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    hist[gray]++;

                    chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    chart2.ChartAreas[0].AxisX.Title = "Gray Level";
                    chart2.ChartAreas[0].AxisY.Title = "No. of Pixels";
                    //chart2.ChartAreas[0].AxisX.Maximum = 255;
                    chart2.ChartAreas[0].AxisX.Minimum = 0;
                    chart2.Series[0].Points.AddXY(gray, hist[gray]);
                }
            }

        }

        //quantization level 8

        private void quantizationLevel8Btn(object sender, EventArgs e)
        {
            Bitmap imgOutput = new Bitmap(imgDefault.Width, imgDefault.Height);




            for (int i = 0; i < imgDefault.Width; i++)
            {
                for (int j = 0; j < imgDefault.Height; j++)
                {
                    Color PixelColor = imgDefault.GetPixel(i, j);
                    int C_gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3; 
                    if (C_gray <= 32)
                        imgOutput.SetPixel(i, j, Color.FromArgb(16, 16, 16));
                    else if (C_gray <= 64)
                        imgOutput.SetPixel(i, j, Color.FromArgb(48, 48, 48));
                    else if (C_gray <= 96)
                        imgOutput.SetPixel(i, j, Color.FromArgb(80, 80, 80));
                    else if (C_gray <= 128)
                        imgOutput.SetPixel(i, j, Color.FromArgb(112, 112, 112));
                    else if (C_gray <= 160)
                        imgOutput.SetPixel(i, j, Color.FromArgb(144, 144, 144));
                    else if (C_gray <= 192)
                        imgOutput.SetPixel(i, j, Color.FromArgb(176, 176, 176));
                    else if (C_gray <= 224)
                        imgOutput.SetPixel(i, j, Color.FromArgb(208, 208, 208));
                    else
                        imgOutput.SetPixel(i, j, Color.FromArgb(240, 240, 240));
                }
            }
            pictureBox2.Image = imgOutput;
            textBox1.Text = "Quantization 8 levels";


            //histogram

            chart2.Series[0].Points.Clear();
            int[] hist = new int[256];

            for (int i = 0; i < imgOutput.Width; i++)
            {
                for (int j = 0; j < imgOutput.Height; j++)
                {
                    Color PixelColor = imgOutput.GetPixel(i, j);
                    int gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    hist[gray]++;

                    chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    chart2.ChartAreas[0].AxisX.Title = "Gray Level";
                    chart2.ChartAreas[0].AxisY.Title = "No. of Pixels";
                    //chart2.ChartAreas[0].AxisX.Maximum = 255;
                    chart2.ChartAreas[0].AxisX.Minimum = 0;
                    chart2.Series[0].Points.AddXY(gray, hist[gray]);
                }
            }

        }

        //histogram equalization
        private void histogramEqualizationBtn(object sender, EventArgs e)
        {

            int[] hist = new int[256];

            for (int i = 0; i < imgDefault.Width; i++)
            {
                for (int j = 0; j < imgDefault.Height; j++)
                {
                    Color PixelColor = imgDefault.GetPixel(i, j);
                    int C_gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    hist[C_gray]++;
                }

            }
            int[] hist2 = new int[256];
            hist2[0] = hist[0];
            for (int i = 1; i < 256; i++)
            {
                hist2[i] = hist2[i - 1] + hist[i];

            }

            Bitmap imgOutput = new Bitmap(imgDefault.Width, imgDefault.Height);

            //set pixel
            for (int i = 0; i < imgDefault.Width; i++)
            {


                for (int j = 0; j < imgDefault.Height; j++)
                {
                    Color PixelColor = imgDefault.GetPixel(i, j);
                    int gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    int gray2 = (int)(hist2[gray] * 255 / (imgDefault.Width * imgDefault.Height));

                    imgOutput.SetPixel(i, j, Color.FromArgb(gray2, gray2, gray2));

                }


            }

            pictureBox2.Image = imgOutput;
            textBox1.Text = "Histrogram Equalization";


            //histogram
            chart2.Series[0].Points.Clear();

            for (int i = 0; i < imgOutput.Width; i++)
            {
                for (int j = 0; j < imgOutput.Height; j++)
                {
                    Color PixelColor = imgOutput.GetPixel(i, j);
                    int gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    hist[gray]++;

                    chart2.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                    chart2.ChartAreas[0].AxisX.Title = "Gray Level";
                    chart2.ChartAreas[0].AxisY.Title = "No. of Pixels";
                    //chart2.ChartAreas[0].AxisX.Maximum = 255;
                    chart2.ChartAreas[0].AxisX.Minimum = 0;
                    chart2.Series[0].Points.AddXY(gray, hist[gray]);
                }
            }

        }

        private void imgDiffBtn(object sender, EventArgs e)
        {
            Bitmap imgDiff = new Bitmap(imgDefault.Width, imgDefault.Height);
            Bitmap imgOutput = (Bitmap)pictureBox2.Image;

            for (int i = 0; i < imgDefault.Width; i++)
            {
                for (int j = 0; j < imgDefault.Height; j++)
                {
                    Color PixelColor = imgDefault.GetPixel(i, j);
                    Color PixelColor2 = imgOutput.GetPixel(i, j);
                    int gray = (int)(PixelColor.R + PixelColor.G + PixelColor.B) / 3;
                    int gray2 = (int)(PixelColor2.R + PixelColor2.G + PixelColor2.B) / 3;
                    int gray3 = Math.Abs(gray - gray2);
                    imgDiff.SetPixel(i, j, Color.FromArgb(255-gray3, 255-gray3, 255-gray3));
                }
            }
            pictureBox5.Image = imgDiff;
            

        }


        
        



    }
}
