using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DungeonCrawler
{
    public class CharSelectPreview
    {
        public ImageSprite image;

        public TextSprite charType;

        public TextSprite level;

        public Vector2 position;

        public Color Color;

        public String fileName;

        public CharSelectPreview(Texture2D image, Vector2 pos, SpriteFont font, String charType, String level, Color color, String fileName)
        {
            this.fileName = fileName;
            this.image = new ImageSprite(image, (int)(pos.X - image.Width / 2), (int)pos.Y, color);
            this.charType = new TextSprite(font, charType, new Vector2(pos.X + 35, pos.Y - 8), color);
            this.level = new TextSprite(font, level, new Vector2(pos.X + 35, pos.Y + 12), color);
            position = pos;
            Color = color;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image.Image, image.Position, new Rectangle?(), Color, 0, image.Origin, 1, SpriteEffects.None, 1);
            spriteBatch.DrawString(charType.Font, charType.Text, charType.Position, Color, 0, charType.Origin, 1, SpriteEffects.None, 1);
            spriteBatch.DrawString(level.Font, level.Text, level.Position, Color, 0, level.Origin, 1, SpriteEffects.None, 1);
        }

        public void MoveByOffset(Vector2 offset)
        {
            image.Position += offset;
            charType.Position += offset;
            level.Position += offset;
        }
    }
}
