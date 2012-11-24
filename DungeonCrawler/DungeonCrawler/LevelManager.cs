//-----------------------------------------------------------------------------
//Based on Nathan Bean's file from Scrolling Shooter Game(Copyright (C) CIS 580 Fall 2012 Class).
// Author: Jiri Malina
//
// Modified By: Nicholas Strub - Added handling for PlayerSpawns objects (11/3/2012)
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using DungeonCrawlerWindowsLibrary;
using DungeonCrawler.Components;
using DungeonCrawler.Entities;


namespace DungeonCrawler
{
    /// <summary>
    /// A class for managing the loading, updating, and rendering of
    /// levels.
    /// </summary>
    public class LevelManager
    {
        Game game;
        SpriteBatch spriteBatch;
        BasicEffect basicEffect;

        public uint currentRoomID;

        public bool Loading = true;
        public bool Paused = false;

        public Tilemap CurrentMap;
        public Song CurrentSong;

        private Dictionary<string,Tilemap> LoadedTilemaps;


        /// <summary>
        /// Creates a new LevelManager
        /// </summary>
        /// <param name="game"></param>
        public LevelManager(Game game)
        {
            this.game = game;

            LoadedTilemaps = new Dictionary<string,Tilemap>();
        }


        /// <summary>
        /// Loads the LevelManager's content and initializes any
        /// functionality that needs a created graphics device
        /// </summary>
        public void LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            // Create a basic effect, used with the spritebatch 
            basicEffect = new BasicEffect(game.GraphicsDevice)
            {
                TextureEnabled = true,
                VertexColorEnabled = true,
            };
        }


        /// <summary>
        /// Loads a new level asynchronously
        /// </summary>
        /// <param name="level">The name of the level to load</param>
        public void LoadLevel(string level)
        {
            Loading = true;
            DungeonCrawlerGame game = (DungeonCrawlerGame)this.game;

            ThreadStart threadStarter = delegate
            {
                if (!LoadedTilemaps.ContainsKey(level))
                {
                    CurrentMap = game.Content.Load<Tilemap>("Tilemaps/" + level);
                    CurrentMap.LoadContent(game.Content);

                    // Load the background music
                    //if (CurrentMap.MusicTitle != null && CurrentMap.MusicTitle != "")
                    //{
                    //    CurrentSong = game.Content.Load<Song>("Music/" + CurrentMap.MusicTitle);
                    //}
                    //else
                    //{
                    //    CurrentSong = null;
                    //}
                    currentRoomID = game.RoomFactory.CreateRoom(level, CurrentMap.Width, CurrentMap.Height, CurrentMap.TileWidth, CurrentMap.TileHeight, CurrentMap.WallWidth, level);
                    Room room = game.RoomComponent[currentRoomID];



                    for (int i = 0; i < CurrentMap.GameObjectGroupCount; i++)
                    {
                        for (int j = 0; j < CurrentMap.GameObjectGroups[i].GameObjectData.Count(); j++)
                        {
                            GameObjectData goData = CurrentMap.GameObjectGroups[i].GameObjectData[j];
                            Vector2 position = new Vector2(goData.Position.Center.X, goData.Position.Center.Y);

                            uint entityID = uint.MaxValue;


                        switch (goData.Category)
                        {
                            case "PlayerSpawn":
                                room.playerSpawns.Add(goData.properties["SpawnName"], new Vector2(goData.Position.X, goData.Position.Y));
                                break;
                            case "Enemy":
                                switch (goData.Type)
                                {
                                    case "MovingTarget":
                                        entityID = game.EnemyFactory.CreateEnemy(EnemyType.MovingTarget, new Position { Center = new Vector2(goData.Position.X, goData.Position.Y), RoomID = currentRoomID, Radius = 32 });
                                        break;
                                    case "StationaryTarget":
                                        entityID = game.EnemyFactory.CreateEnemy(EnemyType.MovingTarget, new Position { Center = new Vector2(goData.Position.X, goData.Position.Y), RoomID = currentRoomID, Radius = 32 });
                                        break;
                                    case "Alien":
                                        entityID = game.EnemyFactory.CreateEnemy(EnemyType.Alien, new Position { Center = new Vector2(goData.Position.X, goData.Position.Y), RoomID = currentRoomID, Radius = 16 });
                                        break;
                                    default:
                                        break;
                                }
                                break;
                            case "NPC":
                                entityID = game.NPCFactory.CreateNPC((NPCName) Enum.Parse(typeof(NPCName), goData.Type), new Position 
                                            { Center = new Vector2(goData.Position.X, goData.Position.Y), RoomID = currentRoomID, Radius = 32});
                                break;
                            case "Trigger":
                                switch (goData.Type)
                                {
                                    case "Door":
                                        entityID = game.DoorFactory.CreateDoor(currentRoomID, goData.properties["DestinationRoom"], goData.properties["DestinationSpawnName"], goData.Position);
                                        break;
                                    case "Wall":
                                        entityID = game.WallFactory.CreateWall(currentRoomID, goData.Position);
                                        break;
                                    default:
                                        break;
                                }
                                break;
                        }
                            if (goData.properties.Keys.Contains("id"))
                            {
                                room.idMap.Add(goData.properties["id"], entityID);
                                room.targetTypeMap.Add(goData.properties["id"], goData.Type);
                            }

                        }
                    }

                    game.RoomComponent[currentRoomID] = room;

                    //// Load the game objects
                    //for (int i = 0; i < CurrentMap.GameObjectGroupCount; i++)
                    //{
                    //    for (int j = 0; j < CurrentMap.GameObjectGroups[i].GameObjectData.Count(); j++)
                    //    {
                    //        GameObjectData goData = CurrentMap.GameObjectGroups[i].GameObjectData[j];
                    //        Vector2 position = new Vector2(goData.Position.Center.X, goData.Position.Center.Y);
                    //        GameObject go;

                    //        switch (goData.Category)
                    //        {
                    //            case "PlayerStart":
                    //                ScrollingShooterGame.Game.Player.Position = position;
                    //                ScrollingShooterGame.Game.Player.LayerDepth = CurrentMap.GameObjectGroups[i].LayerDepth;
                    //                scrollDistance = -2 * position.Y + 300;
                    //                break;

                    //            case "LevelEnd":
                    //                break;

                    //            case "Powerup":
                    //                go = ScrollingShooterGame.GameObjectManager.CreatePowerup((PowerupType)Enum.Parse(typeof(PowerupType), goData.Type), position);
                    //                CurrentMap.GameObjectGroups[i].GameObjectData[j].ID = go.ID;
                    //                go.LayerDepth = CurrentMap.GameObjectGroups[i].LayerDepth;
                    //                go.ScrollingSpeed = CurrentMap.GameObjectGroups[i].ScrollingSpeed;
                    //                break;

                    //            case "Enemy":
                    //                go = ScrollingShooterGame.GameObjectManager.CreateEnemy((EnemyType)Enum.Parse(typeof(EnemyType), goData.Type), position);
                    //                CurrentMap.GameObjectGroups[i].GameObjectData[j].ID = go.ID;
                    //                go.LayerDepth = CurrentMap.GameObjectGroups[i].LayerDepth;
                    //                go.ScrollingSpeed = CurrentMap.GameObjectGroups[i].ScrollingSpeed;
                    //                break;
                    //            case "Boss":
                    //                go = ScrollingShooterGame.GameObjectManager.CreateBoss((BossType)Enum.Parse(typeof(BossType), goData.Type), position);
                    //                CurrentMap.GameObjectGroups[i].GameObjectData[j].ID = go.ID;
                    //                go.LayerDepth = CurrentMap.GameObjectGroups[i].LayerDepth;
                    //                go.ScrollingSpeed = CurrentMap.GameObjectGroups[i].ScrollingSpeed;
                    //                break;
                    //        }
                    //    }
                    //}

                    LoadedTilemaps.Add(level, CurrentMap);
                }
                else
                {
                    Room newRoom = game.RoomComponent.FindRoom(level);
                    if(newRoom.Tilemap == level)
                    {
                        CurrentMap = LoadedTilemaps[level];
                        currentRoomID = newRoom.EntityID;
                    }
                }


                // Mark level as loaded
                Loading = false;
            };

            Thread loadingThread = new Thread(threadStarter);
            loadingThread.Start();
        }

        /// <summary>
        /// Updates the Level
        /// </summary>
        /// <param name="elapsedTime">the time elapsed between this and the previous frame</param>
        public void Update(float elapsedTime)
        {
            if (Paused)
            {
                // Unpase on space press
                if (Keyboard.GetState().IsKeyDown(Keys.Space)) Paused = false;
            }
            //else
            //{
            // Update the scrolling distance - the distance
            // the screen has scrolled past the Player
            //if (Scrolling)
            //{

            //    float scrollDelta = elapsedTime * (CurrentMap.Layers[CurrentMap.PlayerLayer].ScrollingSpeed);
            //    scrollDistance += scrollDelta;
            //    ScrollingShooterGame.Game.Player.Position -= new Vector2(0, scrollDelta / 2);

            //    // Scroll all the tile layers
            //    for (int i = 0; i < CurrentMap.LayerCount; i++)
            //    {
            //        CurrentMap.Layers[i].ScrollOffset += elapsedTime * CurrentMap.Layers[i].ScrollingSpeed;
            //    }
            //    // Scrolls objects with the map
            //    foreach (uint goID in ScrollingShooterGame.GameObjectManager.scrollingObjects)
            //    {
            //        GameObject go = ScrollingShooterGame.GameObjectManager.GetObject(goID);
            //        go.ScrollWithMap(elapsedTime);
            //        ScrollingShooterGame.GameObjectManager.UpdateGameObject(goID);
            //    }

            //}
            // Update only the game objects that appear near our scrolling region
            //Rectangle bounds = new Rectangle(0,
            //    (int)(-scrollDistance / 2) - 100,
            //    CurrentMap.Width * CurrentMap.TileWidth,
            //    16 * CurrentMap.TileHeight + 100);
            //foreach (uint goID in ScrollingShooterGame.GameObjectManager.QueryRegion(bounds))
            //{
            //    GameObject go = ScrollingShooterGame.GameObjectManager.GetObject(goID);
            //    go.Update(elapsedTime);
            //    ScrollingShooterGame.GameObjectManager.UpdateGameObject(goID);
            //}
            //// Remove objects that we have passed
            //Rectangle deleteBounds = new Rectangle(0,
            //    (int)(-scrollDistance / 2) + (16 * CurrentMap.TileHeight + 50),
            //    CurrentMap.Width * CurrentMap.TileWidth,
            //    4480 - ((int)(-scrollDistance / 2) + (16 * CurrentMap.TileHeight + 50)));
            //foreach (uint goID in ScrollingShooterGame.GameObjectManager.QueryRegion(deleteBounds))
            //{
            //    ScrollingShooterGame.GameObjectManager.DestroyObject(goID);
            //}
            //}
        }


        /// <summary>
        /// Draw the level
        /// </summary>
        /// <param name="elapsedTime">The time between this and the last frame</param>
        public void Draw(float elapsedTime)
        {
            if (Paused)
            {
                // TODO: Draw "Paused" Overlay
            }

            // Draw level
            Viewport viewport = game.GraphicsDevice.Viewport;
            //basicEffect.World = Matrix.CreateScale(1, 1, 1);
            //basicEffect.View = Matrix.CreateTranslation(new Vector3(10, 0, 10));
            basicEffect.Projection = Matrix.CreateOrthographicOffCenter(-15, viewport.Width, viewport.Height, -15, 0, -1);

            spriteBatch.Begin(0, null, SamplerState.LinearClamp, null, null, basicEffect);

            if (CurrentMap != null && Loading == false)
            {
                for (int i = 0; i < CurrentMap.LayerCount; i++)
                {
                    // To minimize drawn tiles, we limit ourselves to those onscreen
                    int miny = 0;
                    //(int)((-scrollDistance - 2 * CurrentMap.Layers[i].ScrollOffset) / (CurrentMap.TileHeight * 2));
                    int maxy = CurrentMap.Height;

                    // And those that exist
                    //if (miny < 0) miny = 0;
                    //  if (maxy > CurrentMap.Height) maxy = CurrentMap.Height;

                    for (int y = miny; y < maxy; y++)
                    {
                        // Since our maps are only as wide as our rendering area, 
                        // no need for optimizaiton here
                        for (int x = 0; x < CurrentMap.Width; x++)
                        {
                            int index = x + y * CurrentMap.Width;
                            TileData tileData = CurrentMap.Layers[i].TileData[index];
                            if (tileData.TileID != 0)
                            {
                                Tile tile = CurrentMap.Tiles[tileData.TileID - 1];
                                Rectangle onScreen = new Rectangle(
                                    x * CurrentMap.TileWidth,
                                    (int)(y * CurrentMap.TileHeight),
                                    CurrentMap.TileWidth,
                                    CurrentMap.TileHeight);
                                spriteBatch.Draw(CurrentMap.Textures[tile.TextureID], onScreen, tile.Source, Color.White, 0f, new Vector2(CurrentMap.TileWidth / 2, CurrentMap.TileHeight / 2), tileData.SpriteEffects, CurrentMap.Layers[i].LayerDepth);
                            }
                        }
                    }
                }
            }
            //TODO draw all objects

            // Draw only the game objects that appear within our scrolling region
            //Rectangle bounds = new Rectangle(0,
            //    (int)(-scrollDistance / 2 - 100),
            //    CurrentMap.Width * CurrentMap.TileWidth,
            //    16 * CurrentMap.TileHeight + 100);

            //foreach (uint goID in ScrollingShooterGame.GameObjectManager.QueryRegion(bounds))
            //{
            //    GameObject go = ScrollingShooterGame.GameObjectManager.GetObject(goID);
            //    go.Draw(elapsedTime, spriteBatch);
            //}

            spriteBatch.End();

        }

        public Room getCurrentRoom()
        {
            return ((DungeonCrawlerGame)game).RoomComponent[currentRoomID];
        }
    }
}