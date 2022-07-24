using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Imaging;


namespace Akvarium
{
    public partial class Form1 : Form
    {
        Bitmap akvarium = new Bitmap(@"C:\Users\Дима\source\repos\Akvarium\Akvarium\Akvarium\Resources\akvarium.jpg");
        Bitmap fish = new Bitmap(@"C:\Users\Дима\source\repos\Akvarium\Akvarium\Akvarium\Resources\img33.png");
        //Bitmap fish1 = new Bitmap(@"C:\Users\Дима\source\repos\Akvarium\Akvarium\Akvarium\Resources\img4.jpg");
        Graphics g;
        int dx;
        Rectangle rct;
        Boolean demo = true;
        public Form1()
        {

            InitializeComponent();

            fish.MakeTransparent();
            this.ClientSize = new System.Drawing.Size(new Point(BackgroundImage.Width, BackgroundImage.Height));
            g = Graphics.FromImage(BackgroundImage);
            rct.X = -40;
            rct.Y = 20;
            rct.Width = fish.Width;
            rct.Height = fish.Height;
            dx = 3; // скорость полета - 2 пикселя/тик_таймера
            timer1.Interval = 20;
            timer1.Enabled = true;
            //timer2.Interval = 20;
            //timer2.Enabled = true;



        }
        private void Form1_Load_1(object sender, EventArgs e)
        {
            //g = CreateGraphics();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            g.DrawImage(akvarium, new Point(0, 0));
            if (rct.X < this.ClientRectangle.Width) rct.X += dx;
            else
            {
                // если граница, задаем заново положение 

                rct.X = -40;
                rct.Y = 40;


            }
            // рисуем  рабочей поверхности
            g.DrawImage(fish, rct.X, rct.Y);
            // Метод Invalidate(rct) - перерисовка области rct
            if (!demo) this.Invalidate(rct); // обновить область
            else
            {
                // если объект вне области rct, он не виден
                Rectangle reg = new Rectangle(20, 20, akvarium.Width - 40, akvarium.Height - 40);

                // показать обновляемую область
                g.DrawRectangle(Pens.Black, reg.X, reg.Y, reg.Width - 1, reg.Height - 1);

                this.Invalidate(reg); // обновить область
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            //Point position = new Point(0, 90);
            //g.DrawImage(akvarium, new Point(0, 0));
            //if (rct.X < this.ClientRectangle.Width) rct.X += dx;
            //else
            //{

            //    rct.X = 90;

            //    rct.Y = 40;
            //}
            //g.DrawImage(fish, rct.X, rct.Y);
            //// Метод Invalidate(rct) - перерисовка области rct
            //if (!demo) this.Invalidate(rct);
            //else
            //{
            //    // если объект вне области rct, он не виден
            //    Rectangle reg = new Rectangle(20, 20, akvarium.Width - 40, akvarium.Height - 40);

            //    // показать обновляемую область
            //    g.DrawRectangle(Pens.Black, reg.X, reg.Y, reg.Width - 1, reg.Height - 1);

            //    this.Invalidate(reg); // обновить область
        }
    

        private void timer2_Tick(object sender, EventArgs e)
        {
            


        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            
        }
    }
}


