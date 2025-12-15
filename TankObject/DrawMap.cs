using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.Win32;


namespace TankObject
{
    internal static class DrawMap
    {
        public const int GameW = 1000;
        public const int GameH = 1000;

        public const int InfoW = 300;

        private const int Grid = 50;

        private const int WidthGrid = 20;

        private const int HeightGrid = 20;

        #region 地图式样
        private static readonly int[,] MapGrid = new int[HeightGrid, WidthGrid]{
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},//1
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},//2
                {0,0,1,1,0,0,1,1,0,0,0,0,1,1,0,0,1,1,0,0},//3
                {0,0,1,1,0,0,1,1,0,0,0,0,1,1,0,0,1,1,0,0},//4
                {0,0,1,1,0,0,1,1,0,0,0,0,1,1,0,0,1,1,0,0},//5
                {0,0,1,1,0,0,1,1,0,0,0,0,1,1,0,0,1,1,0,0},//6
                {0,0,1,1,0,0,1,1,1,1,1,1,1,1,0,0,1,1,0,0},//7
                {0,0,1,1,0,0,1,1,1,2,2,1,1,1,0,0,1,1,0,0},//8
                {2,2,1,1,2,2,1,1,1,2,2,1,1,1,2,2,1,1,2,2},//9
                {0,0,1,1,0,0,1,1,1,1,1,1,1,1,0,0,1,1,0,0},//10
                {0,0,1,1,0,0,1,1,0,0,0,0,1,1,0,0,1,1,0,0},//11
                {0,0,1,1,0,0,1,1,0,0,0,0,1,1,0,0,1,1,0,0},//12
                {0,0,1,1,0,0,1,1,0,1,1,0,1,1,0,0,1,1,0,0},//13
                {0,0,1,1,0,0,1,1,1,1,1,1,1,1,0,0,1,1,0,0},//14
                {0,0,1,1,0,0,1,1,0,0,0,0,1,1,0,0,1,1,0,0},//15
                {0,0,1,1,0,0,1,1,0,0,0,0,1,1,0,0,1,1,0,0},//16
                {0,0,1,1,0,0,1,1,0,0,0,0,1,1,0,0,1,1,0,0},//17
                {0,0,1,1,0,0,1,1,0,0,0,0,1,1,0,0,1,1,0,0},//18
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},//19
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},//20
         };

        #endregion

        public static void DrawGame(Graphics g, Usagi usagi)
        {
            g.DrawImage(ResourceManager.Instance.GetImage("Background"), 0, 0, GameW, GameH);
            if (usagi != null)
            {
                usagi.Draw(g);
            }
            DrawWall(g);
            DrawInfo(g);
        }

        public static void DrawWall(Graphics g)
        {
            for (int y = 0; y < HeightGrid; y++)
            {
                for (int x = 0; x < WidthGrid; x++)
                {
                    int px = x * Grid;
                    int py = y * Grid;
                    switch (MapGrid[y, x])
                    {
                        case 1:
                            g.DrawImage(ResourceManager.Instance.GetImage("Wall"), px, py, Grid, Grid);
                            break;
                        case 2:
                            g.DrawImage(ResourceManager.Instance.GetImage("SteelWall"), px, py, Grid, Grid);
                            break;
                    }
                }
            }

        }

        public static void DrawInfo(Graphics g)
        {
            g.DrawImage(ResourceManager.Instance.GetImage("Info"), 1000, 0);
        }

        public static bool IsCollidingWithWall(int x, int y, int width, int height)
        {//遍历角色占据的区域,判断是否有墙,如无返回false
            int leftGrid = x / Grid;
            int topGrid = y / Grid;
            int rightGrid = (x + width - 1) / Grid;
            int bottomGrid = (y + height - 1) / Grid;

            int centerX = x + width / 2;
            int centerY = y + height / 2;
            int safeMargin = width / 2 - 5;
            if (centerX - safeMargin < 0 || centerX + safeMargin > GameW ||
            centerY - safeMargin < 0 || centerY + safeMargin > GameH)
                return true;
            for (int gridY = topGrid; gridY <= bottomGrid; gridY++)
            {
                for (int gridX = leftGrid; gridX <= rightGrid; gridX++)
                {

                    if (gridY >= 0 && gridY < HeightGrid && gridX >= 0 && gridX < WidthGrid)
                    {

                        if (MapGrid[gridY, gridX] == 1 || MapGrid[gridY, gridX] == 2)
                        {
                            return true;
                        }
                    }
                }
            
        }
    return false;

}
        public static bool HandleBulletCollision(int x, int y, int width, int height)
        {
            int leftGrid = x / Grid;
            int topGrid = y / Grid;
            int rightGrid = (x + width - 1) / Grid;
            int bottomGrid = (y + height - 1) / Grid;

            bool isCollided = false;


            for (int gridY = topGrid; gridY <= bottomGrid; gridY++)
            {
                for (int gridX = leftGrid; gridX <= rightGrid; gridX++)
                {
                    if (gridY >= 0 && gridY < HeightGrid && gridX >= 0 && gridX < WidthGrid)
                    {
                        if (MapGrid[gridY, gridX] == 1) // 普通砖块
                        {
                            MapGrid[gridY, gridX] = 0; // 普通砖块被击中后消失
                            isCollided = true;
                        }
                        else if (MapGrid[gridY, gridX] == 2) // 钢铁砖块
                        {
                            isCollided = true; // 钢铁砖块被击中但不消失
                        }
                    }
                }
            }

            return isCollided;
        }


    }
}
