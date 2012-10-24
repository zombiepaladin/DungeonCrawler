using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;
using DungeonCrawlerWindowsLibrary;

namespace DungeonCrawlerContentPipeline
{
    /// <summary>
    /// This class is instanciated by the XNA Content Pipeline
    /// to perform processing on a TilemapContent instance to
    /// prepare it for use in a game.
    /// </summary>
    [ContentProcessor(DisplayName = "Tilemap Processor")]
    public class TilemapProcessor : ContentProcessor<TilemapContent, TilemapContent>
    {
        /// <summary>
        /// Process a TilemapContent instance to prepare it for in-game use
        /// </summary>
        /// <param name="input">The TilemapContent instance to process</param>
        /// <param name="context">The context to process it within</param>
        /// <returns>The processed TilemapContent instance</returns>
        public override TilemapContent Process(TilemapContent input, ContentProcessorContext context)
        {
            // Process the properties of this tilemap
            foreach (string property in input.Properties.Keys)
            {
                switch (property)
                {
                    case "Music":
                        input.MusicTitle = input.Properties[property];
                        break;
                    case "WallWidth":
                        input.WallWidth = Convert.ToInt32(input.Properties[property]);
                        break;
                    default:
                        ParamArrayAttribute[] attrs = new ParamArrayAttribute[0];
                        context.Logger.LogMessage("Unknown property " + property + " in tilemap", attrs);
                        break;
                }
            }

            // Restructure the image paths used in this tilemap
            for (int i = 0; i < input.ImagePaths.Length; i++)
            {
                string filename = Path.GetFileNameWithoutExtension(input.ImagePaths[i]);
                string[] directory = Path.GetDirectoryName(input.ImagePaths[i]).Split(Path.DirectorySeparatorChar);
                input.ImagePaths[i] = Path.Combine(directory[directory.Length-1], filename);
            }
            
            // Process the layers in this tilemap
            for (int i = 0; i < input.LayerCount; i++)
            {

                // Convert Properties to fields on the Layer
                foreach (string property in input.Layers[i].Properties.Keys)
                {
                    switch (property)
                    {
                        //case "ScrollingSpeed":
                        //    input.Layers[i].ScrollingSpeed = float.Parse(input.Layers[i].Properties["ScrollingSpeed"]);
                        //    break;

                        //case "ScrollOffset":
                        //    input.Layers[i].ScrollOffset = float.Parse(input.Layers[i].Properties["ScrollOffset"]);
                        //    break;

                        case "LayerDepth":
                            input.Layers[i].LayerDepth = float.Parse(input.Layers[i].Properties["LayerDepth"]);
                            break;

                        default:
                            ParamArrayAttribute[] attrs = new ParamArrayAttribute[0];
                            context.Logger.LogMessage("Unknown property " + property + " in tilemap", attrs);
                            break;
                    }       
                }
            }

            // Process the game object groups in this tilemap
            for (int i = 0; i < input.GameObjectGroupCount; i++)
            {
                // Convert Properties to fields on the GameObjectGroup
                foreach (string property in input.GameObjectGroups[i].Properties.Keys)
                {
                    switch (property)
                    {
                        //case "ScrollingSpeed":
                        //    input.GameObjectGroups[i].ScrollingSpeed = float.Parse(input.GameObjectGroups[i].Properties["ScrollingSpeed"]);
                        //    break;

                        //case "ScrollOffset":
                        //    input.GameObjectGroups[i].ScrollOffset = float.Parse(input.GameObjectGroups[i].Properties["ScrollOffset"]);
                        //    break;

                        case "LayerDepth":
                            input.GameObjectGroups[i].LayerDepth = float.Parse(input.GameObjectGroups[i].Properties["LayerDepth"]);
                            break;

                        default:
                            ParamArrayAttribute[] attrs = new ParamArrayAttribute[0];
                            context.Logger.LogMessage("Unknown property " + property + " in tilemap", attrs);
                            break;
                    }

                    // Find the player starting position, and store it in the tilemap
                    foreach (GameObjectData goData in input.GameObjectGroups[i].GameObjectData)
                    {
                        if (goData.Category == "PlayerStart")
                        {
                            input.PlayerStart = new Microsoft.Xna.Framework.Vector2(goData.Position.Center.X, goData.Position.Center.Y);
                            input.PlayerLayer = i;
                        }
                    }
                }
            }

            return input;
        }
    }
}
