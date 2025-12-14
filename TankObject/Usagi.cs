using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankObject
{
    public class Usagi
    {
        private DateTime lastShootEffectTime = DateTime.MinValue;
        private const int SHOOT_EFFECT_DURATION_MS = 200;

        private bool isShootingEffectActive = false;
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public void StartShootEffect() {
            isShootingEffectActive = true; // 激活射击特效
            lastShootEffectTime = DateTime.Now; // 记录特效开始时间
         }
         public void Update()
        {
            // 检查特效是否已超过持续时间
            if (isShootingEffectActive && 
            (DateTime.Now - lastShootEffectTime).TotalMilliseconds > SHOOT_EFFECT_DURATION_MS)
            {
                isShootingEffectActive = false; // 关闭射击特效
            }
}
        public Image GetCurrentImage()
        {
             if (isShootingEffectActive)
    {
        return ResourceManager.Instance.GetImage("Usagi_Atk");
    }

            if (directionImages.ContainsKey(CurrentDirection))
            {
                return directionImages[CurrentDirection];
            }
            return null;
        }

        public Direction CurrentDirection { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        private Dictionary<Direction, Image> directionImages;

        public Usagi(int x, int y)
        {
            X = x;
            Y = y;
            Speed = 10;
            CurrentDirection = Direction.Down;
            directionImages = new Dictionary<Direction, Image>();
            LoadDirectionImages();
        }

        

        private void LoadDirectionImages()
        {
            directionImages[Direction.Up] = ResourceManager.Instance.GetImage("Usagi_behind");
            directionImages[Direction.Down] = ResourceManager.Instance.GetImage("Usagi_front");
            directionImages[Direction.Left] = ResourceManager.Instance.GetImage("Usagi_left");
            directionImages[Direction.Right] = ResourceManager.Instance.GetImage("Usagi_right");
        }

        

        public void Draw(Graphics g)
{
        Image currentImage = GetCurrentImage();
        if (currentImage != null)
            {
                g.DrawImage(currentImage, X, Y);
            }
}

        public void Move(Direction direction)
        {
            CurrentDirection = direction;
            int newX = X;
            int newY = Y;
            Image currentImage = GetCurrentImage();
            int width = currentImage?.Width ?? 0;
            int height = currentImage?.Height ?? 0;//获取image值,如果为null,则为0
            switch (direction)
            {
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
            if (!DrawMap.IsCollidingWithWall(newX, newY, width, height)) {
            X = newX;
            Y = newY;
            }
        }
    }
}
