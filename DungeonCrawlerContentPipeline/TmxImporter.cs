//-----------------------------------------------------------------------------
//Based on Nathan Bean's file from Scrolling Shooter Game(Copyright (C) CIS 580 Fall 2012 Class).
// Author: Jiri Malina
//
// Kansas State Univerisity CIS 580 Fall 2012 Dungeon Crawler Game
// Copyright (C) CIS 580 Fall 2012 Class. All rights reserved.
// Released under the Microsoft Permissive Licence 
//-----------------------------------------------------------------------------
using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using DungeonCrawlerWindowsLibrary;


namespace DungeonCrawlerContentPipeline
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to import a TMX file from disk
    /// </summary>
    [ContentImporter(".tmx", DisplayName = "TMX Importer", DefaultProcessor = "TilemapProcessor")]
    public class TmxImporter : ContentImporter<TilemapContent>
    {
        // A global reference to the tmx file's directory
        // (Needed to grab the tilesheets)
        string directory;

        /// <summary>
        /// Global lists to store image and tile data
        /// </summary>
        List<string> ImagePaths = new List<string>();
        List<Tile> Tiles = new List<Tile>();

        /// <summary>
        /// Import a tmx file and load its meaningful data into a
        /// TilemapContent instance
        /// </summary>
        /// <param name="filename">The file to load</param>
        /// <param name="context">The import context</param>
        /// <returns></returns>
        public override TilemapContent Import(string filename, ContentImporterContext context)
        {
            directory = Path.GetDirectoryName(filename);

            TilemapContent output = new TilemapContent();
            output.Name = Path.GetFileNameWithoutExtension(filename);

            // Create an XML reader for reading the tmx file
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(filename, settings);

            // Lists to temporarily store ...
            //List<Tile> tiles = new List<Tile>();
            List<TilemapLayerContent> layers = new List<TilemapLayerContent>();
            List<GameObjectGroupContent> objectGroups = new List<GameObjectGroupContent>();

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.DocumentType:
                        if(reader.Name != "map")
                            throw new FormatException("Invalid Map Format");
                        break;

                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "map":
                                output.Width = int.Parse(reader.GetAttribute("width"));
                                output.Height = int.Parse(reader.GetAttribute("height"));
                                output.TileWidth = int.Parse(reader.GetAttribute("tilewidth"));
                                output.TileHeight = int.Parse(reader.GetAttribute("tileheight"));
                                break;

                            case "properties":
                                using (var st = reader.ReadSubtree())
                                {
                                    st.Read();
                                    output.Properties = LoadProperties(st);
                                }
                                break;

                            case "tileset":
                                //System.Diagnostics.Debugger.Launch();
                                using (var st = reader.ReadSubtree())
                                {
                                    st.Read();
                                    LoadTileset(st, output.TileWidth, output.TileHeight);
                                }
                                break;

                            case "layer":
                                using (var st = reader.ReadSubtree())
                                {
                                    st.Read();
                                    layers.Add(LoadLayer(st));
                                }
                                break;

                            case "objectgroup":
                                using (var st = reader.ReadSubtree())
                                {
                                    st.Read();
                                    objectGroups.Add(LoadObjectGroup(st, output.TileWidth, output.TileHeight));
                                }
                                break;
                        }
                        break;

                    case XmlNodeType.EndElement:
                        break;

                    case XmlNodeType.Whitespace:
                        break;
                }
            }

            // Transfer our lists into the TilemapContent for later processing
            output.ImagePaths = ImagePaths.ToArray();
            output.TileCount = Tiles.Count;
            output.Tiles = Tiles.ToArray();
            output.LayerCount = layers.Count;
            output.Layers = layers.ToArray();
            output.GameObjectGroupCount = objectGroups.Count;
            output.GameObjectGroups = objectGroups.ToArray();
            // Initializes the Wall Width to 0 just in case it is not specified in the tilemap
            output.WallWidth = 0;
            
            return output;
        }


        /// <summary>
        /// A helper method for loading properties from the tmx file.  It operates on 
        /// a single subtree of the main XML document, which contains "property"
        /// elements.  This same structure is found on Tiles, Tilemaps, Tilesets,
        /// and Layers - so we use this helper for all four.
        /// </summary>
        /// <param name="reader">The XML reader representing the properties subtree
        /// </param>
        /// <returns>A dictonary with string keys and values containing the property table
        /// </returns>
        protected Dictionary<string, string> LoadProperties(XmlReader reader)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && reader.Name == "property")
                {
                    string key = reader.GetAttribute("name");
                    string value = reader.GetAttribute("value");
                    output[key] = value;
                }
            }

            return output;
        }


        /// <summary>
        /// A helper method for loading a tileset from an XML subtree.  Unlike the
        /// other helper methods, it does not return a value; rather it populates
        /// two global lists - ImagePath and Tiles.  
        /// </summary>
        /// <param name="reader">The XML reader for the tileset subtree</param>
        void LoadTileset(XmlReader reader, int tileWidth, int tileHeight)
        {
            // Read the subtree
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {   
                    switch (reader.Name)
                    {
                        case "image":
                            // Store the absolute file path of the tileset's image 
                            // in the global list of image paths for later processing
                            string imagePath = Path.Combine(directory, reader.GetAttribute("source"));
                            ImagePaths.Add(imagePath);

                            // Load the image width and height attributes; these,
                            // combined with our tilewidth and height, let us
                            // know how many tiles fit on our image
                            int width = int.Parse(reader.GetAttribute("width"));
                            int height = int.Parse(reader.GetAttribute("height"));

                            for (int y = 0; y < height / tileHeight; y++)
                            {
                                for (int x = 0; x < width / tileWidth; x++)
                                {
                                    Tile tile = new Tile();

                                    // Since this textuer was the last added,
                                    // the texture's index in the ImagePath list
                                    // is one less than the count
                                    tile.TextureID = ImagePaths.Count - 1;

                                    // Calculate the tile's source bounds
                                    tile.Source.X = x * tileWidth;
                                    tile.Source.Y = y * tileHeight;
                                    tile.Source.Width = tileWidth;
                                    tile.Source.Height = tileHeight;

                                    // Add the tile to our global list
                                    Tiles.Add(tile);
                                }
                            }
                            break;
                    }
                }
            }
        }


        /// <summary>
        /// A helper method for loading TileData from a layer in a tmx file.
        /// Currently uncompressed Base64 and CSV encoding are supported.
        /// </summary>
        /// <param name="reader">The XML reader</param>
        /// <param name="width">The width of the layer</param>
        /// <param name="height">The height of the layer</param>
        /// <returns>The TileData as an array</returns>
        TileData[] LoadTileData(XmlReader reader, int width, int height)
        {
            // Flags indicating flipped status of a tile instance
            const uint FLIPPED_HORIZONTALLY_FLAG = 0x80000000;
            const uint FLIPPED_VERTICALLY_FLAG = 0x40000000;
            const uint FLIPPED_DIAGONALLY_FLAG = 0x20000000;

            // The tile instances contained within this level
            List<TileData> TileData = new List<TileData>(width * height);

            // Handle the different compression techniques
            if (reader.GetAttribute("compression") != null)
            {
                throw new NotImplementedException("Processing of compressed layer data is not currently implemented");
            }

            // Handle the different encoding styles
            switch (reader.GetAttribute("encoding"))
            {
                case "base64":

                    // Read in the Base64 data 
                    byte[] data = new byte[width * height * 4];
                    reader.ReadElementContentAsBase64(data, 0, width * height * 4);

                    // Convert the encoded data into TileData
                    int tileIndex = 0;
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            // Create a TileData to represent our tile
                            TileData Tile = new TileData();

                            // Extract the four bytes of our base64 data and convert
                            // it back into an unsigned integer
                            uint globalTileID = (uint)(data[tileIndex] |
                                                        data[tileIndex + 1] << 8 |
                                                        data[tileIndex + 2] << 16 |
                                                        data[tileIndex + 3] << 24);
                            tileIndex += 4;

                            // Read out our flags
                            Tile.SpriteEffects = SpriteEffects.None;
                            if ((globalTileID & FLIPPED_HORIZONTALLY_FLAG) > 0)
                                Tile.SpriteEffects |= SpriteEffects.FlipHorizontally;
                            if ((globalTileID & FLIPPED_VERTICALLY_FLAG) > 0)
                                Tile.SpriteEffects |= SpriteEffects.FlipVertically;
                            if ((globalTileID & FLIPPED_DIAGONALLY_FLAG) > 0)
                                Tile.SpriteEffects |= SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;

                            // Clear the flags
                            globalTileID &= ~(FLIPPED_VERTICALLY_FLAG |
                                                FLIPPED_HORIZONTALLY_FLAG |
                                                FLIPPED_DIAGONALLY_FLAG);

                            // Set the tile id
                            Tile.TileID = globalTileID;

                            // Add tile to our TileData
                            TileData.Add(Tile);
                        }
                    }
                    break;

                case "csv":

                    // Read in the csv data 
                    string csvData = reader.ReadElementContentAsString();
                    string[] splitData = csvData.Split(',');

                    // Convert the encoded data into TileData
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            // Create a TileData to represent our tile
                            TileData Tile = new TileData();

                            // Extract the four bytes of our base64 data and convert
                            // it back into an unsigned integer
                            uint globalTileID = UInt32.Parse(splitData[x * width + y]);

                            // Read out our flags
                            Tile.SpriteEffects = SpriteEffects.None;
                            if ((globalTileID & FLIPPED_HORIZONTALLY_FLAG) > 0)
                                Tile.SpriteEffects |= SpriteEffects.FlipHorizontally;
                            if ((globalTileID & FLIPPED_VERTICALLY_FLAG) > 0)
                                Tile.SpriteEffects |= SpriteEffects.FlipVertically;
                            if ((globalTileID & FLIPPED_DIAGONALLY_FLAG) > 0)
                                Tile.SpriteEffects |= SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;

                            // Clear the flags
                            globalTileID &= ~(FLIPPED_VERTICALLY_FLAG |
                                                FLIPPED_HORIZONTALLY_FLAG |
                                                FLIPPED_DIAGONALLY_FLAG);

                            // Set the tile id
                            Tile.TileID = globalTileID;

                            // Add tile to our TileData
                            TileData.Add(Tile);
                        }
                    }break;

                default:
                    throw new NotImplementedException("Unknown encoding in layer data");
            }

            return TileData.ToArray();
        }


        /// <summary>
        /// A helper method for loading a layer from an XML subtree
        /// </summary>
        /// <param name="reader">The XML reader for the layer subtree</param>
        /// <returns>A TilemapLayerContent object</returns>
        TilemapLayerContent LoadLayer(XmlReader reader)
        {
            TilemapLayerContent output = new TilemapLayerContent();
            output.Properties = new Dictionary<string, string>();

            string name = reader.GetAttribute("name");
            int width = int.Parse(reader.GetAttribute("width"));
            int height = int.Parse(reader.GetAttribute("height"));

            // Read the subtree
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "data":
                            using (XmlReader st = reader.ReadSubtree())
                            {
                                st.Read();
                                output.TileData = LoadTileData(st, width, height);
                            }
                            break;

                        case "properties":
                            using (XmlReader st = reader.ReadSubtree())
                            {
                                st.Read();
                                output.Properties = LoadProperties(st);
                            }
                            break;
                    }
                }
            }

            return output;
        }


        /// <summary>
        /// A helper method for loading an object layer from an XML subtree
        /// </summary>
        /// <param name="reader">The XML reader for the layer subtree</param>
        /// <returns>A GroupObjectContent object</returns>
        GameObjectGroupContent LoadObjectGroup(XmlReader reader, int tilewidth, int tileheight)
        {
            GameObjectGroupContent output = new GameObjectGroupContent();
            output.Properties = new Dictionary<string, string>();
            
            List<GameObjectData> gameObjects = new List<GameObjectData>(); 

            // Read the subtree
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "object":
                            GameObjectData go = new GameObjectData();
                            
                            go.Category = reader.GetAttribute("name");
                            go.Type = reader.GetAttribute("type");
                            go.Position.X = int.Parse(reader.GetAttribute("x"));
                            go.Position.Y = int.Parse(reader.GetAttribute("y"));
                            go.properties = LoadProperties(reader);
                            // By default, Tiled sets objects to tile width and height - if
                            // there is no change to these, then it does not add a width
                            // and height attribute to the object XML - so we'll check
                            // for those elements and use them or the default, as needed
                            if (reader.GetAttribute("width") != null)
                            {
                                go.Position.Width = int.Parse(reader.GetAttribute("width"));
                            }
                            else
                            {
                                go.Position.Width = tilewidth;
                            }
                            if (reader.GetAttribute("height") != null)
                            {
                                go.Position.Height = int.Parse(reader.GetAttribute("height"));
                            }
                            else
                            {
                                go.Position.Width = tileheight;
                            }
                            gameObjects.Add(go);
                            break;

                        //case "properties":
                        //    using (XmlReader st = reader.ReadSubtree())
                        //    {
                        //        st.Read();
                        //        output.Properties = LoadProperties(st);
                        //        if (output.Properties.ContainsKey("ScrollingSpeed"))
                        //            output.ScrollingSpeed = float.Parse(output.Properties["ScrollingSpeed"]);
                        //        if (output.Properties.ContainsKey("ScrollOffset"))
                        //            output.ScrollOffset = float.Parse(output.Properties["ScrollOffset"]);
                        //    }
                        //    break;
                    }
                }
            }

            output.GameObjectData = gameObjects.ToArray();

            return output;
        }
    }
}
