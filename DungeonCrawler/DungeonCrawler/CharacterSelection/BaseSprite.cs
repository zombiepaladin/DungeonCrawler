using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawler
{
    public abstract class BaseSprite
    {

        #region Fields

        protected Vector2 position = Vector2.Zero;
        protected Color textColor = Color.White;
        protected float rotation = 0.0f;
        protected float scale = 1.0f;
        protected Vector2 origin = Vector2.Zero;
        protected SpriteEffects spriteEffect = SpriteEffects.None;
        protected int depth = 1;
        protected bool visible = true;

        #endregion

        #region Properties

        public int X
        {
            get { return (int)position.X; }
            set { position.X = (float)value; }
        }

        public int Y
        {
            get { return (int)position.Y; }
            set { position.Y = (float)value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Color Color
        {
            get { return textColor; }
            set { textColor = value; }
        }

        public float Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Vector2 Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public SpriteEffects SpriteEffects
        {
            get { return spriteEffect; }
            set { spriteEffect = value; }
        }

        public int Depth
        {
            get { return depth; }
            set { depth = value; }
        }

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        #endregion
    }
}
