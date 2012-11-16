#region File Description
//-----------------------------------------------------------------------------
// SoundComponenet.cs 
//
// Author: Devin Kelly-Collins
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
using Microsoft.Xna.Framework.Audio;

namespace DungeonCrawler.Components
{
    public struct Sound
    {
        public uint EntityID;
        public SoundEffect SoundEffect;
    }

    public class SoundComponent : GameComponent<Sound>
    {
    }
}
