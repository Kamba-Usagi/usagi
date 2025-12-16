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

        public enum Mode
        {
            Normal,     // 常规模式
            Aggressive  // 遇到Usagi后的模式
        }

        public Direction CurrentDirection{ get; set; }
        public Mode CurrentMode { get; set; } = Mode.Normal;

        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        private Dictionary<Direction, Image> directionImages;
        private Random random = new Random();
        private DateTime lastShootTime = DateTime.MinValue;
        private const int SHOOT_COOLDOWN_MS = 1000; // 1秒射击冷却时间

        public bool CanShoot { get; private set; } = false;
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

        /// <summary>
        /// 射击方法，向当前方向发射子弹
        /// </summary>
        /// <returns>发射的子弹实例</returns>
        public Bullet Shoot()
        {
            // 计算子弹起始位置（角色中心）
            int bulletX = X + Width / 2 - Bullet.WIDTH / 2;
            int bulletY = Y + Height / 2 - Bullet.HEIGHT / 2;
            
            // 将Gebulin.Direction转换为Usagi.Direction
            Usagi.Direction bulletDirection = (Usagi.Direction)Enum.Parse(typeof(Usagi.Direction), CurrentDirection.ToString());
            
            // 创建并返回子弹实例
            return new Bullet(bulletX, bulletY, bulletDirection);
        }

        public void Update()
        {
            // 重置射击状态
            CanShoot = false;
            
            // 根据当前模式执行不同逻辑
            switch (CurrentMode)
            {
                case Mode.Normal:
                    // 常规模式：移动并自动射击
                    Move(CurrentDirection);
                    
                    // 检查是否可以射击（每隔1秒）
                    if ((DateTime.Now - lastShootTime).TotalMilliseconds > SHOOT_COOLDOWN_MS)
                    {
                        // 设置射击状态为true，让Form1处理子弹创建
                        CanShoot = true;
                        lastShootTime = DateTime.Now;
                    }
                    break;
                    
                case Mode.Aggressive:
                    // 遇到Usagi后的模式（暂不实现，留空）
                    break;
            }
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