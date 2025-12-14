using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace TankObject
{
    
    public partial class Form1 : Form
    {
        private Usagi usagi;

        private List<Explosion> explosions = new List<Explosion>();

        private List<Bullet> bullets = new List<Bullet>();
        private List<Gebulin> gebulins=new List<Gebulin>();
        private DateTime lastShootTime = DateTime.MinValue;
    
        private const int SHOOT_COOLDOWN_MS = 500; // 射击冷却时间（毫秒）
        private Timer gameTimer;//主循环定时
        
        // 按键状态跟踪
        private bool isUpPressed = false;
        private bool isDownPressed = false;
        private bool isLeftPressed = false;
        private bool isRightPressed = false;

        public Form1()
        {
            InitializeComponent();
            
            this.ClientSize = new Size(1000+300, 1000);
            this.StartPosition=System.Windows.Forms.FormStartPosition.CenterScreen;
            System.Drawing.Rectangle screenArea = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;   
            this.MaximizeBox = false;    
            this.DoubleBuffered = true;

            gameTimer = new Timer();
            gameTimer.Interval = 16; // 约60fps
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
                                 
            
        }

        private void GameTimer_Tick(object sender, EventArgs e) {
            if (usagi != null)
                {
                    // 根据按键状态移动玩家
                    if (isUpPressed) {
                        usagi.Move(Usagi.Direction.Up);
                    }
                    if (isDownPressed) {
                        usagi.Move(Usagi.Direction.Down);
                    }
                    if (isLeftPressed) {
                        usagi.Move(Usagi.Direction.Left);
                    }
                    if (isRightPressed) {
                        usagi.Move(Usagi.Direction.Right);
                    }
                    usagi.Update();
                }
            UpdateExplosions();
            List<Bullet> bulletsToRemove = new List<Bullet>();
            foreach (var bullet in bullets) {
                bullet.Update();
                if (bullet.RemoveBullet) {
                    CreateExplosion(bullet.X,bullet.Y);
                    bulletsToRemove.Add(bullet);

                }
            }
            foreach (var bullet in bulletsToRemove) {
                bullets.Remove(bullet);
            }
            foreach (var gebulin in gebulins)
            {
                gebulin.Update();
            }
            this.Invalidate();
         }

        private void CreateExplosion(int x, int y) {
            Explosion explosion = new Explosion(x, y);
            explosions.Add(explosion);
        }

        protected override void OnKeyDown(KeyEventArgs e) // Usagi Move
        {
            base.OnKeyDown(e);
            if (usagi != null)
            {
                switch (e.KeyCode) {
                    case Keys.Up:
                        isUpPressed = true;
                        break;
                    case Keys.Down:
                        isDownPressed = true;
                        break;
                    case Keys.Left:
                        isLeftPressed = true;
                        break;
                    case Keys.Right:
                        isRightPressed = true;
                        break;
                    case Keys.Space:
                        FireBullet();
                        break;                    
                }
                // 通知系统重绘界面
                this.Invalidate();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            
            if (usagi != null)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        isUpPressed = false;
                        break;
                    case Keys.Down:
                        isDownPressed = false;
                        break;
                    case Keys.Left:
                        isLeftPressed = false;
                        break;
                    case Keys.Right:
                        isRightPressed = false;
                        break;
                }
            }
        }
        private void UpdateExplosions() {
            List<Explosion> finishedExplosions = new List<Explosion>();
            foreach (var explosion in explosions) {
                explosion.Update();
                if (explosion.IsFinished) {
                    finishedExplosions.Add(explosion);
                }
            }
            foreach (var explosion in finishedExplosions) {
                explosions.Remove(explosion);
            }

        }

        private void FireBullet() {
            if ((DateTime.Now - lastShootTime).TotalMilliseconds < SHOOT_COOLDOWN_MS)
            return;
            lastShootTime = DateTime.Now;
            usagi.StartShootEffect(); 
            Image usagiImage = usagi.GetCurrentImage();
            Image bulletImage = ResourceManager.Instance.GetImage("Bullet");
            int usagiWidth = usagiImage?.Width ?? 0;
            int usagiHeight = usagiImage?.Height ?? 0;
            int bulletWidth = bulletImage?.Width ?? 0;
            int bulletHeight= bulletImage?.Height ?? 0;
            int UsagiMidPointX = usagi.X + usagiWidth / 2;
            int UsagiMidPointY = usagi.Y + usagiHeight / 2;
            int BulletMidpointX, BulletMidpointY, BulletX, BulletY;           

            switch (usagi.CurrentDirection) {
                case Usagi.Direction.Up:
                    BulletMidpointX = UsagiMidPointX;
                    BulletMidpointY = UsagiMidPointY - usagiHeight / 2 - bulletHeight / 2;
                    break;
                case Usagi.Direction.Down:
                    BulletMidpointX = UsagiMidPointX;
                    BulletMidpointY = UsagiMidPointY + usagiHeight / 2 + bulletHeight / 2;
                    break;
                case Usagi.Direction.Left:
                    BulletMidpointX = UsagiMidPointX - usagiWidth / 2 - bulletWidth / 2;
                    BulletMidpointY = UsagiMidPointY;
                    break;
                case Usagi.Direction.Right:
                    BulletMidpointX = UsagiMidPointX + usagiWidth / 2 + bulletWidth / 2;
                    BulletMidpointY = UsagiMidPointY;
                    break;
                default:        
                    BulletMidpointX = UsagiMidPointX;
                    BulletMidpointY = UsagiMidPointY;
                    break;
            }
            BulletX = BulletMidpointX - bulletWidth / 2;
            BulletY = BulletMidpointY - bulletHeight / 2;
            bullets.Add(new Bullet(BulletX,BulletY,usagi.CurrentDirection));
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            DrawMap.DrawGame(e.Graphics,usagi);

            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(e.Graphics);
            }
            foreach (var explosion in explosions) {
                explosion.Draw(e.Graphics);
            }
            foreach (var gebulin in gebulins)
            {
                gebulin.Draw(e.Graphics);
            }

        }

        private void Form1_Load(object sender,EventArgs e) {
            ResourceManager.Instance.LoadImage("Background", Properties.Resources._1800背景);

            ResourceManager.Instance.LoadImage("Usagi_front", Properties.Resources._100乌萨);
            ResourceManager.Instance.LoadImage("Usagi_behind", Properties.Resources._100乌萨后);
            ResourceManager.Instance.LoadImage("Usagi_left", Properties.Resources._100乌萨左);
            ResourceManager.Instance.LoadImage("Usagi_right", Properties.Resources._100乌萨右);
            
            ResourceManager.Instance.LoadImage("Gebulin_front", Properties.Resources._100哥布林正);
            ResourceManager.Instance.LoadImage("Gebulin_behind", Properties.Resources._100哥布林后);
            ResourceManager.Instance.LoadImage("Gebulin_left", Properties.Resources._100哥布林左);
            ResourceManager.Instance.LoadImage("Gebulin_right", Properties.Resources._100哥布林右);

            ResourceManager.Instance.LoadImage("Wall", Properties.Resources._50砖块);
            ResourceManager.Instance.LoadImage("SteelWall", Properties.Resources._50钢砖);
            ResourceManager.Instance.LoadImage("Info", Properties.Resources._300得分板);
            ResourceManager.Instance.LoadImage("Bullet", Properties.Resources._20子弹);
            ResourceManager.Instance.LoadImage("Usagi_Atk",Properties.Resources._100乌萨atk);
            ResourceManager.Instance.LoadImage("Usagi_hurt",Properties.Resources._100乌萨受击);

            ResourceManager.Instance.LoadImage("Bullet_Boom_01",Properties.Resources._50爆炸01);
            ResourceManager.Instance.LoadImage("Bullet_Boom_02",Properties.Resources._50爆炸02);
            ResourceManager.Instance.LoadImage("Bullet_Boom_03",Properties.Resources._50爆炸03);
            ResourceManager.Instance.LoadImage("Bullet_Boom_04",Properties.Resources._50爆炸04);
            ResourceManager.Instance.LoadImage("Bullet_Boom_05",Properties.Resources._50爆炸05);
            ResourceManager.Instance.LoadImage("Bullet_Boom_06",Properties.Resources._50爆炸06);
            ResourceManager.Instance.LoadImage("Bullet_Boom_07",Properties.Resources._50爆炸07);
            ResourceManager.Instance.LoadImage("Bullet_Boom_08",Properties.Resources._50爆炸08);
            ResourceManager.Instance.LoadImage("Bullet_Boom_09",Properties.Resources._50爆炸09);
            ResourceManager.Instance.LoadImage("Bullet_Boom_10",Properties.Resources._50爆炸10);
            ResourceManager.Instance.LoadImage("Bullet_Boom_11",Properties.Resources._50爆炸11);
            ResourceManager.Instance.LoadImage("Bullet_Boom_12",Properties.Resources._50爆炸12);
            gebulins.Add(new Gebulin(200, 200));
         




            int startX = (DrawMap.GameW - Properties.Resources._100乌萨.Width) / 2;
            int startY = DrawMap.GameH - Properties.Resources._100乌萨.Height;
            usagi = new Usagi(startX, startY);
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResourceManager.Instance.ReleaseAll();
        }

    }
}
