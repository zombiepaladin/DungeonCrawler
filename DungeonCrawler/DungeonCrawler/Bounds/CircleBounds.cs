#region File Description
//-----------------------------------------------------------------------------
// CircleBounds.cs 
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
            return Vector2.DistanceSquared(this.Center, obj.Center) < Math.Pow(this.Radius + obj.Radius, 2);
        }

        private bool handleRectangleIntersect(RectangleBounds obj)
        {
            Vector2 closestPoint = obj.GetClosestPoint(this.Center);

            return Vector2.DistanceSquared(this.Center, closestPoint) < Math.Pow(this.Radius, 2);
        }

        public void UpdatePosition(Vector2 position)
        {
            this.Center = position;
        }
    }
}
