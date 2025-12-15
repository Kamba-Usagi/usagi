using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankObject
{
    internal class Gebulin
    {
        public enum Direction
        {
            Up,    
            Down,  
            Left,  
            Right  
        }

        public Direction CurrentDirection{ get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        private Dictionary<Direction, Image> directionImages;
        private Random random = new Random();
        public Gebulin(int x, int y) {
            X = x;
            Y = y;
            Speed = 5;
            CurrentDirection = Direction.Down;
            directionImages = new Dictionary<Direction, Image>();
            LoadDirectionImages();
            Image initialImage = GetCurrentImage();
            Width = initialImage?.Width ?? 100;
            Height = initialImage?.Height ?? 100;
        }

        private void LoadDirectionImages()
        {
            directionImages[Direction.Up] = ResourceManager.Instance.GetImage("Gebulin_behind");
            directionImages[Direction.Down] = ResourceManager.Instance.GetImage("Gebulin_front");
            directionImages[Direction.Left] = ResourceManager.Instance.GetImage("Gebulin_left");
            directionImages[Direction.Right] = ResourceManager.Instance.GetImage("Gebulin_right");
        }

        public Image GetCurrentImage() {
            if (directionImages.ContainsKey(CurrentDirection))
            {
                return directionImages[CurrentDirection];
            }
            return null;
         }

        public void Move(Direction direction) {
            CurrentDirection = direction;
            int newX=X;
            int newY=Y;
            switch (direction) {
                case Direction.Up:
                    newY -= Speed;
                    break;
                case Direction.Down:
                    newY += Speed;
                    break;
                case Direction.Left:
                    newX -= Speed;
                    break;
                case Direction.Right:
                    newX += Speed;
                    break;
            }
            int width = Width;
            int height = Height;
            if (!DrawMap.IsCollidingWithWall(newX, newY, width, height))
            {
                X = newX;
                Y = newY;
            }else
                {
                    // 碰撞墙壁时随机改变方向
                    ChangeRandomDirection();
                }
        }

        private void ChangeRandomDirection()
{
    // 获取所有可能的方向
    Direction[] directions = (Direction[])Enum.GetValues(typeof(Direction));
    
    // 随机选择一个方向
    CurrentDirection = directions[random.Next(directions.Length)];
}

        public void Update(){
            Move(CurrentDirection);
        }

        public void Draw(Graphics g)
        {
            Image currentImage = GetCurrentImage();
            if (currentImage != null)
            {
                g.DrawImage(currentImage, X, Y);
            }
        }



    }
}
