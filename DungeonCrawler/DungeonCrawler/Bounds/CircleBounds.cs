using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DungeonCrawler
{
    public class CircleBounds : Bounds
    {
        public Vector2 Center;
        public float Radius;

        public CircleBounds(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
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
