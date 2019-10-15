using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace rgb盔甲架代码生成器
{
    public partial class Form1 : Form
    {
        //图片和函数文件的路径
        private string imgPath;
        private string filePath;
        public Form1()
        {
            InitializeComponent();
        }
        //选择图片
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "C:\\";
            ofd.Title = "请选择一张图片";
            ofd.Filter = "图像文件(JPeg, Gif, Bmp, etc.)|*.jpg;*.jpeg;*.gif;*.bmp;*.tif; *.tiff; *.png| JPeg 图像文件(*.jpg;*.jpeg)"
                + "|*.jpg;*.jpeg |GIF 图像文件(*.gif)|*.gif |BMP图像文件(*.bmp)|*.bmp|Tiff图像文件(*.tif;*.tiff)|*.tif;*.tiff|Png图像文件(*.png)"
                + "| *.png |所有文件(*.*)|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                imgPath = ofd.FileName;
                textBox1.Text = imgPath;
                MessageBox.Show("你选择的图片是：" + imgPath);
            }
        }
        //选择函数文件的位置
        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "请指定函数文件的位置";
            if (fbd.ShowDialog() == DialogResult.OK || fbd.ShowDialog() == DialogResult.Yes)
            {
                filePath = fbd.SelectedPath;
                textBox2.Text = filePath;
                MessageBox.Show("你选择的文件夹是：" + filePath);
            }
        }
        //解析图片像素点并生成对应的命令函数文件
        private void button3_Click(object sender, EventArgs e)
        {
            if (filePath == null || imgPath == null)
            {
                MessageBox.Show("你未选择图片或函数文件位置", "系统提示");
            }
            else 
            {

                using (Bitmap img = new Bitmap(imgPath))
                {
                    FileStream mcFunction = new FileStream(filePath + "\\rgb.mcfunction", FileMode.OpenOrCreate);
                    mcFunction.Close();
                    for (int x = 0; x < img.Width; x++)
                    {
                        for (int y = 0; y < img.Height; y++)
                        {
                            Color pixelColor = img.GetPixel(x, y);
                            StreamWriter sw = new StreamWriter(filePath + "\\rgb.mcfunction", true);
                            sw.WriteLine("summon armor_stand ~" + (double)x * 0.8 + " ~1 ~" + (double)y * 0.8 + " {NoGravity:1b,ArmorItems:[{},{},{},{id:\"leather_helmet\",Count:1b,tag:{display:{color:" +(pixelColor.R*65536+pixelColor.G*256+pixelColor.B) + "}}}],Invisible:1b,Small:0b}");
                            sw.Close();
                        }
                    }
                }
            }
        }
    }
}
