#region File Description
//-----------------------------------------------------------------------------
// RectangleBounds.cs 
//
// Author: Devin Kelly-Collins & Matthew McHaney
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence
//-----------------------------------------------------------------------------
#endregion

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
            Vector2 closestPoint = GetClosestPoint(obj.Center);

            return Vector2.DistanceSquared(obj.Center, closestPoint) < Math.Pow(obj.Radius, 2);
        }

        private bool handleRectangleIntersect(RectangleBounds obj)
        {
            return this.Rectangle.Intersects(obj.Rectangle);
        }

        public void UpdatePosition(Vector2 position)
        {
            //To do: use the center of the rectangle or upper left corner?
            this.Rectangle.Offset(
                (int)position.X - this.Rectangle.Center.X, 
                (int)position.Y - this.Rectangle.Center.Y);
        }

        //Returns the location on the rectangle closest to the input position
        public Vector2 GetClosestPoint(Vector2 obj)
        {
            Vector2 output = new Vector2();
            output.X = MathHelper.Clamp(obj.X, this.Rectangle.Left, this.Rectangle.Right);
            output.Y = MathHelper.Clamp(obj.Y, this.Rectangle.Top, this.Rectangle.Bottom);

            return output;
        }
    }
}
