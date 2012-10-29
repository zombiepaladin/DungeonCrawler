#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawler.Components;
#endregion

namespace DungeonCrawler.Entities
{
    class InvAggregateFactory
    {
        DungeonCrawlerGame game;
        public InvAggregateFactory(DungeonCrawlerGame game)
        {
            this.game = game;
        }

        public void CreateInv(Player player)
        {
            uint entityID;
            Texture2D spritesheet;
            Position position;
            InventorySprite Background,
                            Selector;

            Inventory inventory;
            entityID = Entity.NextEntity();
            spritesheet = game.Content.Load<Texture2D>("Spritesheets/inventoryBackground");
            position = new Position()
            {
                EntityID = entityID,
                Center = new Vector2(0,0),
                Radius = 0f,
                Collideable = false,
            };
            game.PositionComponent[entityID] = position;
            Background = new InventorySprite()
            {
                EntityID = entityID,
                isSeen = false,
                SpriteSheet = spritesheet,
                SpriteBounds = new Rectangle(0, 0, 1280, 720),
            };
            game.InventorySpriteComponent[entityID] = Background;

            entityID = Entity.NextEntity();
            spritesheet = game.Content.Load<Texture2D>("Spritesheets/selectInv");
            position = new Position()
            {
                EntityID = entityID,
                Center = new Vector2(100, 100),
                Radius = 0f,
                Collideable = false,
            };
            game.PositionComponent[entityID] = position;
            Selector = new InventorySprite()
            {
                EntityID = entityID,
                isSeen = false,
                SpriteSheet = spritesheet,
                SpriteBounds = new Rectangle(0, 0, 100, 80),
            };
            game.InventorySpriteComponent[entityID] = Selector;

            inventory = new Inventory()
            {
                EntityID = player.EntityID,
                BackgroundSpriteID = Background.EntityID,
                SelectorSpriteID = Selector.EntityID,
            };
            game.InventoryComponent[player.EntityID] = inventory;
        }
    }
}
