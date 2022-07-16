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

namespace Akvarium
{
    public partial class Form1 : Form
    {
        Bitmap akvarium = new Bitmap(@"C:\Users\Дима\source\repos\Akvarium\Akvarium\Akvarium\Resources\akvarium.jpg");
        Bitmap fish = new Bitmap(@"C:\Users\Дима\source\repos\Akvarium\Akvarium\Akvarium\Resources\img5.jpg");
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
            dx = 2; // скорость полета - 2 пикселя/тик_таймера
            timer1.Interval = 20;
            timer1.Enabled = true;


        }

        private void MoveCircle()
        {
            //устанавливаем блокировку метода
            lock (this)
            {
                //создаем контекст устройства
                Graphics g = this.CreateGraphics();
                //создаем красную кисть - для круга
                //Brush b1 = Brushes.Red;
                //и кисть цвета формы - для стирания круга
                //Brush b2 = SystemBrushes.Control;
                Bitmap akvarium = new Bitmap(@"C:\Users\Дима\source\repos\Akvarium\Akvarium\Akvarium\Resources\akvarium.jpg");
                Bitmap fish = new Bitmap(@"C:\Users\Дима\source\repos\Akvarium\Akvarium\Akvarium\Resources\img5.jpg");
                int x;
                //координата x круга будет зависеть от имени потока
                if (Thread.CurrentThread.Name == "First")
                    x = Width / 2 - 30;
                else
                    x = Width / 2 + 30;
                //цикл от верхнего до нижнего края формы
                for (int y = 10; y < Height - 40; y++)
                {
                    //рисуем круг
                    g.DrawImage(fish, x - 10, y - 10, 20, 20);
                    //"усыпляем" поток на 30 миллисекунд
                    Thread.Sleep(30);
                    //если круг или его часть - внутри прямоугольника,
                    if (y + 10 > rct.Y && y - 10 < rct.Y + rct.Height)
                        //то перерисовываем прямоугольник
                        Invalidate(rct);
                    else
                    {
                        //разрешаем выполнение другого потока
                        Monitor.Pulse(this);
                        //ждем приостановки/завершения другого потока
                        Monitor.Wait(this);
                    }
                    //стираем круг
                    g.DrawImage(fish, x - 10, y - 10, 20, 20);
                }
                //выводим поток из режима ожидания
                Monitor.Pulse(this);
            }
            //проверяем корректность завершения потока
            MessageBox.Show("Поток " + Thread.CurrentThread.Name + "завершен!");

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(new ThreadStart(MoveCircle));
                thread1.Name = "First";
            Thread thread2 = new Thread(new ThreadStart(MoveCircle));
            thread2.Name = "Second";
            //запускаем оба потока
            thread1.Start();
            thread2.Start();
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            g.DrawImage(akvarium, new Point(0, 0));
            if (rct.X < this.ClientRectangle.Width) rct.X += dx;
            else
            {
                // если граница, задаем заново положение самолета
                rct.X = -40;
                rct.Y = 20;

            }
            // рисуем самолет на рабочей поверхности
            g.DrawImage(fish, rct.X, rct.Y);
            // Метод Invalidate(rct) - перерисовка области rct
            if (!demo) this.Invalidate(rct); // обновить область, где самолет
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
            //получаем контекст устройства
            Graphics g = e.Graphics;
            //создаем черную кисть
            Brush b = Brushes.Black;
            //рисуем прямоугольник
            g.FillRectangle(b, rct);
        }
    }
}
