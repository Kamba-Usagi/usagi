using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankObject
{
    internal class Explosion
    {
        public int X {get;set; }
        public int Y{ get; set; }
        public int CurrentFrame { get; set; }//当前播放帧数
        public bool IsFinished { get; private set; }
        private int FrameDelay { get; set; }//单帧播放速度

        private const int All_Frame = 12;

        private const int Frame_time = 5;//爆炸动画播放速度

        public Explosion(int x,int y) {
            X=x;Y = y;
            CurrentFrame = 1;
            IsFinished = false;
            FrameDelay = 0;

        }

        public void Update() {
            if (IsFinished) return;
            FrameDelay += 16;
            while (FrameDelay >= Frame_time)
            {
                CurrentFrame++;
                
                FrameDelay -= Frame_time;               
                
                if (CurrentFrame > All_Frame)
                {
                    IsFinished = true;
                    break;
                }
            }
        }

        public void Draw(Graphics g) {
            if (IsFinished || CurrentFrame < 1 || CurrentFrame > All_Frame)
                return;
            string imageName = $"Bullet_Boom_{CurrentFrame:D2}";
            Image explosionImage = ResourceManager.Instance.GetImage(imageName);
            if (explosionImage != null)
            {
                int drawX = X - explosionImage.Width / 2;
                int drawY = Y - explosionImage.Height / 2;            
                g.DrawImage(explosionImage, drawX, drawY);
            }
         }
        



    }
}
