using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace Tiny2Tools
{
    public partial class Form1 : Form
    {
        const byte RedVal = 0x00;
        const byte GreenVal = 0x01;
        const byte BlueVal = 0x02;
        const byte YellowVal = 0x03;

        Byte[] buf;

        Bitmap bitmap;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            Color[] colorList = new Color[4];

            colorList[(int)RedVal] = Color.FromArgb(255, 255, 0, 0);
            colorList[(int)GreenVal] = Color.FromArgb(255, 0, 255, 0);
            colorList[(int)BlueVal] = Color.FromArgb(255, 0, 0, 255);
            colorList[(int)YellowVal] = Color.FromArgb(255, 255, 255, 0);

            /*
            // stream load
            FileStream file = new FileStream("test.bin", FileMode.Open, FileAccess.Read);
            Byte[] data = new Byte[file.Length];
            */

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = saveFileDialog1.ShowDialog();

            bitmap.Save(saveFileDialog1.FileName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                FileStream file = File.OpenRead(openFileDialog1.FileName);
                int count = (int)file.Length;
                buf = new Byte[count];
                file.Read(buf, 0, count);
                file.Dispose();
            }

            CreateBitmap();

            pictureBox1.Image = bitmap;
            this.Invalidate();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void CreateBitmap()
        {
            bitmap = new Bitmap(256, 256);

            /*
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    Color color = Color.FromArgb(255, i, j, 0);

                    bitmap.SetPixel(i, j, color);
                }
            }
             * */

            int x = 0, y = 0;

            for (int code = 0; code < 64; code++ )
            {
                y = 0;
                int offset = 0x2e00;

                for (int i = 0; i < 8; i++)
                {
                    int val = (int)buf[code * 8 + i + offset];
                    Debug.WriteLine(val.ToString("x"));
                    x = 0;

                    for (int j = 0; j < 8; j++)
                    {
                        if ((val & 0x80) != 0)
                        {
                            if (y < 4)
                            {
                                bitmap.SetPixel((code % 32) * 8 + x, (code / 32) * 8 + y, Color.White);
                            }
                            else
                            {
                                bitmap.SetPixel((code % 32) * 8 + x, (code / 32) * 8 + y, Color.LightGreen);
                            }
                        }
                        else
                        {
                            bitmap.SetPixel((code % 32) * 8 + x, (code / 32) * 8 + y, Color.Black);
                        }
                        val = val << 1;
                        x++;
                    }
                    y++;
                }
            }

        }
    }

}
