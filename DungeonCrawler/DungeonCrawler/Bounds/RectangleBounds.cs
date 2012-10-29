using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DungeonCrawler
{
    public class RectangleBounds : Bounds
    {
        public Rectangle Rectangle;

        public RectangleBounds(Rectangle rectangle)
        {
            Rectangle = rectangle;
        }

        public RectangleBounds(int x, int y, int width, int height)
        {
            Rectangle = new Rectangle(x, y, width, height);
        }

        public bool Intersect(Bounds obj)
        {
            if (obj is CircleBounds)
                return handleCircleIntersect(obj as CircleBounds);
            else
                return handleRectangleIntersect(obj as RectangleBounds);
        }

        private bool handleCircleIntersect(CircleBounds obj)
        {
            return false;
        }

        private bool handleRectangleIntersect(RectangleBounds obj)
        {
            return false;
        }
    }
}
