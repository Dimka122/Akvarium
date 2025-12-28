using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Akvarium
{
    public class Fish
    {
        public Bitmap FishImage { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int DX { get; set; }
        public int DY { get; set; }
        public Color Color { get; set; }

        private static readonly Random random = new Random();

        public Fish(Bitmap fishImage, int aquariumWidth, int aquariumHeight, Color fishColor)
        {
            FishImage = fishImage;
            Color = fishColor;
            Width = fishImage.Width;
            Height = fishImage.Height;
            InitializePosition(aquariumWidth, aquariumHeight);
            InitializeSpeed();
        }

        public Rectangle Position
        {
            get { return new Rectangle(X, Y, Width, Height); }
        }

        private void InitializePosition(int aquariumWidth, int aquariumHeight)
        {
            X = random.Next(0, aquariumWidth - Width);
            Y = random.Next(0, aquariumHeight - Height);
        }

        private void InitializeSpeed()
        {
            DX = random.Next(2, 6) * (random.Next(2) == 0 ? -1 : 1);
            DY = random.Next(1, 4) * (random.Next(2) == 0 ? -1 : 1);
        }

        public void Move(int aquariumWidth, int aquariumHeight)
        {
            X += DX;
            Y += DY;

            CheckBoundaries(aquariumWidth, aquariumHeight);

            if (random.Next(100) < 2)
            {
                ChangeDirectionRandomly();
            }
        }

        private void CheckBoundaries(int aquariumWidth, int aquariumHeight)
        {
            if (X <= 0)
            {
                X = 0;
                DX = Math.Abs(DX);
            }

            if (X + Width >= aquariumWidth)
            {
                X = aquariumWidth - Width;
                DX = -Math.Abs(DX);
            }

            if (Y <= 0)
            {
                Y = 0;
                DY = Math.Abs(DY);
            }

            if (Y + Height >= aquariumHeight)
            {
                Y = aquariumHeight - Height;
                DY = -Math.Abs(DY);
            }
        }

        private void ChangeDirectionRandomly()
        {
            DX = random.Next(2, 6) * (DX > 0 ? 1 : -1);
            DY = random.Next(1, 4) * (random.Next(2) == 0 ? -1 : 1);
        }

        public void Draw(Graphics g)
        {
            // Используем цветовую матрицу для изменения цвета рыбы
            if (Color != Color.White) // Если это не исходный цвет
            {
                ColorMatrix colorMatrix = new ColorMatrix(new float[][]
                {
            new float[] {Color.R / 255f, 0, 0, 0, 0},
            new float[] {0, Color.G / 255f, 0, 0, 0},
            new float[] {0, 0, Color.B / 255f, 0, 0},
            new float[] {0, 0, 0, Color.A / 255f, 0},
            new float[] {0, 0, 0, 0, 1}
                });

                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.SetColorMatrix(colorMatrix);

                g.DrawImage(
                    FishImage,
                    new Rectangle(X, Y, Width, Height),
                    0, 0, FishImage.Width, FishImage.Height,
                    GraphicsUnit.Pixel,
                    imageAttributes);
            }
            else
            {
                g.DrawImage(FishImage, X, Y);
            }
        }

        public void Relocate(int aquariumWidth, int aquariumHeight)
        {
            InitializePosition(aquariumWidth, aquariumHeight);
            ChangeDirectionRandomly();
        }
    }
}