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
        Bitmap background; // Фон аквариума
        Bitmap fishImage;  // Изображение рыбы

        // Двойная буферизация для избежания мерцания
        private Bitmap bufferBitmap;
        private Graphics bufferGraphics;

        Graphics g;
        Random random = new Random();
        List<Fish> fishes = new List<Fish>();
        Boolean demo = true;

        // Цвета для разных рыб
        Color[] fishColors =
        {
            Color.FromArgb(255, 255, 100, 100),
            Color.FromArgb(255, 100, 255, 100),
            Color.FromArgb(255, 100, 100, 255),
            Color.FromArgb(255, 255, 255, 100),
            Color.FromArgb(255, 255, 100, 255),
            Color.FromArgb(255, 100, 255, 255),
            Color.FromArgb(255, 255, 150, 50),
        };

        public Form1()
        {
            InitializeComponent();

            try
            {
                // Загружаем изображения
                background = new Bitmap(@"C:\Users\user\source\repos\Akvariums\Akvarium\Akvarium\Resources\akvarium.jpg");
                fishImage = new Bitmap(@"C:\Users\user\source\repos\Akvariums\Akvarium\Akvarium\Resources\img33.png");

                fishImage.MakeTransparent();

                // Устанавливаем размер формы по размеру фона
                this.ClientSize = background.Size;

                // Создаем буфер для рисования
                bufferBitmap = new Bitmap(background.Width, background.Height);
                bufferGraphics = Graphics.FromImage(bufferBitmap);
                g = bufferGraphics;

                // Копируем фон в буфер
                g.DrawImage(background, 0, 0);

                // Создаем несколько рыб
                InitializeFishes();

                timer1.Interval = 50; // Увеличил интервал для плавности
                timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображений: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeFishes()
        {
            int fishCount = random.Next(5, 11);

            for (int i = 0; i < fishCount; i++)
            {
                Color fishColor = fishColors[random.Next(fishColors.Length)];
                Fish fish = new Fish(fishImage, background.Width, background.Height, fishColor);
                fishes.Add(fish);
            }
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            // Очищаем буфер, рисуя фон
            bufferGraphics.DrawImage(background, 0, 0);

            // Обновляем и рисуем всех рыб
            foreach (Fish fish in fishes)
            {
                fish.Move(background.Width, background.Height);
                fish.Draw(bufferGraphics);
            }

            // Принудительно обновляем всю форму
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Рисуем буфер на форме
            if (bufferBitmap != null)
            {
                e.Graphics.DrawImage(bufferBitmap, 0, 0);
            }

            if (demo && fishes.Count > 0)
            {
                Rectangle updateRegion = GetUpdateRegion();
                e.Graphics.DrawRectangle(Pens.Black,
                    updateRegion.X, updateRegion.Y,
                    updateRegion.Width - 1, updateRegion.Height - 1);
            }
        }

        private Rectangle GetUpdateRegion()
        {
            if (fishes.Count == 0)
                return new Rectangle(0, 0, background.Width, background.Height);

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

            int margin = 10;
            minX = Math.Max(0, minX - margin);
            minY = Math.Max(0, minY - margin);
            maxX = Math.Min(background.Width, maxX + margin);
            maxY = Math.Min(background.Height, maxY + margin);

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            // Включаем двойную буферизацию для формы
            this.DoubleBuffered = true;
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            if (random.Next(2) == 0 && fishes.Count < 20)
            {
                // Добавляем новую рыбу
                Color fishColor = fishColors[random.Next(fishColors.Length)];
                Fish newFish = new Fish(fishImage, background.Width, background.Height, fishColor);
                fishes.Add(newFish);
            }
            else
            {
                // Перемещаем всех рыб в новые позиции
                foreach (Fish fish in fishes)
                {
                    fish.Relocate(background.Width, background.Height);
                }
            }

            // Обновляем отображение
            this.Invalidate();
        }

        // Остальные методы остаются без изменений
        public void AddFish(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (fishes.Count < 50)
                {
                    Color fishColor = fishColors[random.Next(fishColors.Length)];
                    Fish newFish = new Fish(fishImage, background.Width, background.Height, fishColor);
                    fishes.Add(newFish);
                }
            }
            this.Invalidate();
        }

        public void RemoveFish(int count)
        {
            for (int i = 0; i < count && fishes.Count > 0; i++)
            {
                fishes.RemoveAt(fishes.Count - 1);
            }
            this.Invalidate();
        }

        public int GetFishCount()
        {
            return fishes.Count;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Освобождаем ресурсы
            if (bufferGraphics != null) bufferGraphics.Dispose();
            if (bufferBitmap != null) bufferBitmap.Dispose();
            if (g != null) g.Dispose();
        }
    }
}