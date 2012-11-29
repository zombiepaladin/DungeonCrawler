//-----------------------------------------------------------------------------
// SpriteAnimationSystem.cs 
//
// Author: Samuel Fike, Jiri Malina
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DungeonCrawler.Components;

namespace DungeonCrawler.Systems
{
    public class SpriteAnimationSystem
    {
        private DungeonCrawlerGame game;

        public SpriteAnimationSystem(DungeonCrawlerGame game)
        {
            this.game = game;
        }

        public void Update(float elapsedTime)
        {
            List<uint> keyList = game.SpriteAnimationComponent.Keys.ToList<uint>();
            
            foreach (uint key in keyList)
            {
                SpriteAnimation spriteAnimation = game.SpriteAnimationComponent[key];

                //Skip if no sprite
                if (!game.SpriteComponent.Contains(spriteAnimation.EntityID) || !spriteAnimation.IsPlaying)
                    continue;

                Sprite sprite = game.SpriteComponent[spriteAnimation.EntityID];

                //if animation row has changed
                if (sprite.SpriteBounds.Y != spriteAnimation.CurrentAnimationRow * sprite.SpriteBounds.Height)
                {
                    sprite.SpriteBounds.X = 0;
                    sprite.SpriteBounds.Y = spriteAnimation.CurrentAnimationRow * sprite.SpriteBounds.Height;
                }

                spriteAnimation.TimePassed += elapsedTime;

                //time for next frame
                if(spriteAnimation.TimePassed > 1.0 / spriteAnimation.FramesPerSecond)
                {
                    spriteAnimation.TimePassed = 0;

                    //if on last frame
                    if (sprite.SpriteBounds.X + sprite.SpriteBounds.Width >= sprite.SpriteSheet.Width)
                    {
                        if (spriteAnimation.IsLooping)
                            sprite.SpriteBounds.X = 0;
                        else
                            spriteAnimation.IsPlaying = false;
                    }
                    else //next frame
                        sprite.SpriteBounds.X += sprite.SpriteBounds.Width;
                }

                game.SpriteComponent[sprite.EntityID] = sprite;
                game.SpriteAnimationComponent[spriteAnimation.EntityID] = spriteAnimation;
            }
        }
    }
}
