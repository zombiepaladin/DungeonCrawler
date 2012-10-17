using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawler
{
    /// <summary>
    /// This class represents a sprite composed of text for rendering in
    /// and overlay
    /// </summary>
    public class TextSprite : BaseSprite
    {
        #region Fields

        protected SpriteFont spriteFont = null;
        protected string text = String.Empty;

        #endregion

        #region Properties

        public SpriteFont Font
        {
            get { return spriteFont; }
            set
            {
                spriteFont = value;
                origin = value.MeasureString(text) / new Vector2(2.0f, 2.0f);
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                origin = spriteFont.MeasureString(text) / new Vector2(2.0f, 2.0f);
            }
        }

        #endregion

        public TextSprite() { }

        public TextSprite(SpriteFont font, string text, Vector2 position, Color color)
        {
            if (font == null)
                throw new ArgumentNullException("font");

            if (String.IsNullOrEmpty(text))
                throw new ArgumentNullException("text");

            origin = font.MeasureString(text) / new Vector2(2.0f, 2.0f);

            spriteFont = font;
            this.Text = text;
            Position = position;
            Color = color;
        }

    }
}
