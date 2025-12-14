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
        private Dictionary<Direction, Image> directionImages;

        public Gebulin(int x, int y) {
            X = x;
            Y = y;
            Speed = 5;
            CurrentDirection = Direction.Down;
            directionImages = new Dictionary<Direction, Image>();
            LoadDirectionImages();
        }

        private void LoadDirectionImages()
        {
            directionImages[Direction.Up] = ResourceManager.Instance.GetImage("Gebulin_back");
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
            Image currentImage = GetCurrentImage();
            int width = currentImage?.Width ?? 50;
            int height = currentImage?.Height ?? 50;
            if (!DrawMap.IsCollidingWithWall(newX, newY, width, height))
            {
                X = newX;
                Y = newY;
            }
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
