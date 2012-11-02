#region File Description
//-----------------------------------------------------------------------------
// Bounds.cs 
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
    public interface Bounds
    {
        bool Intersect(Bounds obj);

        void UpdatePosition(Vector2 location);
    }
}
