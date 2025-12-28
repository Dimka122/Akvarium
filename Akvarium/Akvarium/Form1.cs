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
        Bitmap fishImage = new Bitmap(@"C:\Users\Дима\source\repos\Akvarium\Akvarium\Akvarium\Resources\img33.png");

        Graphics g;
        Random random = new Random();
        List<Fish> fishes = new List<Fish>();
        Boolean demo = true;

        // Цвета для разных рыб
        Color[] fishColors =
        {
            Color.FromArgb(255, 255, 100, 100),   // Светло-красный
            Color.FromArgb(255, 100, 255, 100),   // Светло-зеленый
            Color.FromArgb(255, 100, 100, 255),   // Светло-синий
            Color.FromArgb(255, 255, 255, 100),   // Желтый
            Color.FromArgb(255, 255, 100, 255),   // Розовый
            Color.FromArgb(255, 100, 255, 255),   // Голубой
            Color.FromArgb(255, 255, 150, 50),    // Оранжевый
        };

        public Form1()
        {
            InitializeComponent();

            fishImage.MakeTransparent();
            this.ClientSize = new System.Drawing.Size(new Point(akvarium.Width, akvarium.Height));
            g = Graphics.FromImage(akvarium);

            // Создаем несколько рыб
            InitializeFishes();

            timer1.Interval = 20;
            timer1.Enabled = true;
        }

        private void InitializeFishes()
        {
            int fishCount = random.Next(5, 11); // От 5 до 10 рыб

            for (int i = 0; i < fishCount; i++)
            {
                Color fishColor = fishColors[random.Next(fishColors.Length)];
                Fish fish = new Fish(fishImage, akvarium.Width, akvarium.Height, fishColor);
                fishes.Add(fish);
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            // Очищаем и перерисовываем фон
            g.DrawImage(akvarium, new Point(0, 0));

            // Обновляем и рисуем всех рыб
            foreach (Fish fish in fishes)
            {
                fish.Move(akvarium.Width, akvarium.Height);
                fish.Draw(g);
            }

            // Отображаем обновляемую область
            Rectangle updateRegion = GetUpdateRegion();

            if (!demo)
            {
                this.Invalidate(updateRegion);
            }
            else
            {
                g.DrawRectangle(Pens.Black, updateRegion.X, updateRegion.Y, updateRegion.Width - 1, updateRegion.Height - 1);
                this.Invalidate(updateRegion);
            }
        }

        private Rectangle GetUpdateRegion()
        {
            // Находим общую область, занимаемую всеми рыбами
            if (fishes.Count == 0)
                return new Rectangle(0, 0, akvarium.Width, akvarium.Height);

            int minX = fishes[0].X;
            int minY = fishes[0].Y;
            int maxX = fishes[0].X + fishes[0].Width;
            int maxY = fishes[0].Y + fishes[0].Height;

            foreach (Fish fish in fishes)
            {
                minX = Math.Min(minX, fish.X);
                minY = Math.Min(minY, fish.Y);
                maxX = Math.Max(maxX, fish.X + fish.Width);
                maxY = Math.Max(maxY, fish.Y + fish.Height);
            }

            // Добавляем отступы
            int margin = 10;
            minX = Math.Max(0, minX - margin);
            minY = Math.Max(0, minY - margin);
            maxX = Math.Min(akvarium.Width, maxX + margin);
            maxY = Math.Min(akvarium.Height, maxY + margin);

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
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
            // Можно добавить управление количеством рыб
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            // При клике добавляем новую рыбу или перемешиваем всех рыб
            if (random.Next(2) == 0 && fishes.Count < 20)
            {
                // Добавляем новую рыбу
                Color fishColor = fishColors[random.Next(fishColors.Length)];
                Fish newFish = new Fish(fishImage, akvarium.Width, akvarium.Height, fishColor);
                fishes.Add(newFish);
            }
            else
            {
                // Перемещаем всех рыб в новые позиции
                foreach (Fish fish in fishes)
                {
                    fish.Relocate(akvarium.Width, akvarium.Height);
                }
            }
        }

        // Метод для добавления определенного количества рыб
        public void AddFish(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (fishes.Count < 50) // Максимум 50 рыб
                {
                    Color fishColor = fishColors[random.Next(fishColors.Length)];
                    Fish newFish = new Fish(fishImage, akvarium.Width, akvarium.Height, fishColor);
                    fishes.Add(newFish);
                }
            }
        }

        // Метод для удаления рыб
        public void RemoveFish(int count)
        {
            for (int i = 0; i < count && fishes.Count > 0; i++)
            {
                fishes.RemoveAt(fishes.Count - 1);
            }
        }

        // Метод для получения количества рыб
        public int GetFishCount()
        {
            return fishes.Count;
        }
    }
}