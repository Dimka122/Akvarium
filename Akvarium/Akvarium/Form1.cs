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

        Graphics g;
        int dx, dy; // Скорости по X и Y
        Random random = new Random();
        Rectangle rct;
        Boolean demo = true;

        public Form1()
        {
            InitializeComponent();

            fish.MakeTransparent();
            this.ClientSize = new System.Drawing.Size(new Point(akvarium.Width, akvarium.Height));
            g = Graphics.FromImage(akvarium);

            // Инициализация случайной позиции рыбы в пределах аквариума
            InitializeFishPosition();

            // Случайные начальные скорости
            dx = random.Next(2, 6) * (random.Next(2) == 0 ? -1 : 1); // -5 до -2 или 2 до 5
            dy = random.Next(1, 4) * (random.Next(2) == 0 ? -1 : 1); // -3 до -1 или 1 до 3

            rct.Width = fish.Width;
            rct.Height = fish.Height;

            timer1.Interval = 20;
            timer1.Enabled = true;
        }

        private void InitializeFishPosition()
        {
            // Размещаем рыбу в случайной позиции внутри аквариума
            rct.X = random.Next(0, akvarium.Width - fish.Width);
            rct.Y = random.Next(0, akvarium.Height - fish.Height);
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            // Очищаем и перерисовываем фон
            g.DrawImage(akvarium, new Point(0, 0));

            // Обновляем позицию рыбы
            rct.X += dx;
            rct.Y += dy;

            // Проверка границ и отскок
            CheckBoundaries();

            // Случайное изменение направления для более естественного движения
            if (random.Next(100) < 2) // 2% шанс изменения направления
            {
                ChangeDirectionRandomly();
            }

            // Рисуем рыбу на новой позиции
            g.DrawImage(fish, rct.X, rct.Y);

            // Отображаем обновляемую область
            if (!demo)
            {
                this.Invalidate(rct);
            }
            else
            {
                Rectangle reg = new Rectangle(20, 20, akvarium.Width - 40, akvarium.Height - 40);
                g.DrawRectangle(Pens.Black, reg.X, reg.Y, reg.Width - 1, reg.Height - 1);
                this.Invalidate(reg);
            }
        }

        private void CheckBoundaries()
        {
            // Отскок от левой границы
            if (rct.X <= 0)
            {
                rct.X = 0;
                dx = Math.Abs(dx); // Меняем направление на положительное
            }

            // Отскок от правой границы
            if (rct.X + rct.Width >= akvarium.Width)
            {
                rct.X = akvarium.Width - rct.Width;
                dx = -Math.Abs(dx); // Меняем направление на отрицательное
            }

            // Отскок от верхней границы
            if (rct.Y <= 0)
            {
                rct.Y = 0;
                dy = Math.Abs(dy); // Меняем направление на положительное
            }

            // Отскок от нижней границы
            if (rct.Y + rct.Height >= akvarium.Height)
            {
                rct.Y = akvarium.Height - rct.Height;
                dy = -Math.Abs(dy); // Меняем направление на отрицательное
            }
        }

        private void ChangeDirectionRandomly()
        {
            // Случайно меняем скорость для более естественного движения
            dx = random.Next(2, 6) * (dx > 0 ? 1 : -1);
            dy = random.Next(1, 4) * (random.Next(2) == 0 ? -1 : 1);
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            // Инициализация при загрузке формы
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            // Отрисовка происходит в таймере
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            // Дополнительный таймер для будущей функциональности
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Выбор скорости или режима анимации
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            // При клике можно добавить новую рыбу или изменить поведение
            InitializeFishPosition();
            ChangeDirectionRandomly();
        }
    }
}