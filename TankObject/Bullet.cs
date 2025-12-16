using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankObject
{
    internal class Bullet
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public Usagi.Direction CurrentDirection { get; set; }
        private Image BulletImage { get; set; }

        public bool RemoveBullet { get; set; }

        public const int WIDTH = 20;
        public const int HEIGHT = 20;

        public Bullet(int x, int y, Usagi.Direction direction)
        {
            X = x;
            Y = y;
            Speed = 20;
            CurrentDirection = direction;
            RemoveBullet = false;
            BulletImage = ResourceManager.Instance.GetImage("Bullet");
        }

        public void Update()
        {
            switch (CurrentDirection)
            {
                case Usagi.Direction.Up: Y -= Speed; break;
                case Usagi.Direction.Down: Y += Speed; break;
                case Usagi.Direction.Left: X -= Speed; break;
                case Usagi.Direction.Right: X += Speed; break;
            }
            // 检查与墙壁碰撞或超出游戏区域边界
            if (CheckWallCollision() || X < 0 || X + WIDTH > DrawMap.GameW || Y < 0 || Y + HEIGHT > DrawMap.GameH)
            {
                RemoveBullet = true;
            }
        }

        public bool CheckWallCollision() // 判断是否碰到墙
        {
            return DrawMap.HandleBulletCollision(X, Y, 20, 20);
        }

        public void Draw(Graphics g)
        {
            if (BulletImage != null)
            {
                g.DrawImage(BulletImage, X, Y);
            }
        }
    }
}