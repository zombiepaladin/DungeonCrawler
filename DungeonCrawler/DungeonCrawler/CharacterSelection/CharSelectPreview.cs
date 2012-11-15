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

        public int fileNumber;

        public int aggregate;

        public bool newGame;

        public CharSelectPreview(Texture2D image, Vector2 pos, SpriteFont font, String charType, String level, Color color, String fileName)
        {
            this.fileName = fileName;
            this.image = new ImageSprite(image, (int)(pos.X - image.Width / 2), (int)pos.Y, color);
            this.charType = new TextSprite(font, charType, new Vector2(pos.X + 35, pos.Y - 8), color);
            this.level = new TextSprite(font, level, new Vector2(pos.X + 35, pos.Y + 12), color);
            position = pos;
            Color = color;
            this.charType = new TextSprite(font, charType, new Vector2(pos.X + 25, pos.Y - 8), color);
            this.level = new TextSprite(font, level, new Vector2(pos.X + 25, pos.Y + 12), color);
            position = pos;
            Color = color;
            newGame = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image.Image, image.Position, new Rectangle?(), Color, 0, image.Origin, 1, SpriteEffects.None, 1);
            spriteBatch.DrawString(charType.Font, charType.Text, charType.Position, Color, 0, charType.Origin, 1, SpriteEffects.None, 1);
            spriteBatch.DrawString(level.Font, level.Text, level.Position, Color, 0, level.Origin, 1, SpriteEffects.None, 1);

            spriteBatch.DrawString(charType.Font, charType.Text, charType.Position, Color, 0, new Vector2(0, charType.Origin.Y), 1, SpriteEffects.None, 1);
            spriteBatch.DrawString(level.Font, level.Text, level.Position, Color, 0, new Vector2(0, level.Origin.Y), 1, SpriteEffects.None, 1);
        }

        public void MoveByOffset(Vector2 offset)
        {
            image.Position += offset;
            charType.Position += offset;
            level.Position += offset;
        }

        public void SetPosition(Vector2 position)
        {
            image.X = (int)(position.X - image.Image.Width / 2);
            image.Y = (int)position.Y;
            charType.Position = new Vector2(position.X + 25, position.Y - 8);
            level.Position = new Vector2(position.X + 25, position.Y + 12);
        }

    }
}
