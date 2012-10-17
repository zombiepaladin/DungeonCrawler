using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DungeonCrawler.Components
{
    public struct WeaponSprite
    {
        public uint EntityID;
        public Texture2D SpriteSheet;
        public Rectangle Spritebounds;
        public Facing Facing;
    }

    public class WeaponSpriteComponent : GameComponent<WeaponSprite>
    {
    }

    public struct BulletSprite
    {
        public uint EntityID;
        public Texture2D SpriteSheet;
        public Rectangle Spritebounds;
    }

    public class BulletSpriteComponent : GameComponent<BulletSprite>
    {
    }
}
