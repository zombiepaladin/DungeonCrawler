using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawler
{
    public class ImageSprite : BaseSprite
    {
        #region Fields

        Texture2D image;
        Rectangle? sourceBounds;

        #endregion

        #region Properties

        public Texture2D Image
        {
            get { return image; }
            set
            {
                image = value;
                origin = new Vector2(value.Width / 2.0f, value.Height / 2.0f);
            }
        }

        public Rectangle? SourceRectangle
        {
            get { return sourceBounds; }
            set { sourceBounds = value; }
        }

        #endregion

        public ImageSprite() { }

        public ImageSprite(Texture2D image, int x, int y, Color color)
        {

            if (image == null)
                throw new ArgumentNullException("image");

            origin = new Vector2(image.Width / 2.0f, image.Height / 2.0f);

            this.image = image;
            Position = new Vector2(x, y);
            Color = color;
        }
    }
}
