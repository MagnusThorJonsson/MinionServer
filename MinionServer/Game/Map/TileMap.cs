using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//using Minion2D;
//using AcousticKitty.Entities;

namespace MinionServer.Game.Map
{
    /// <summary>
    /// Renders the Tile Map
    /// </summary>
    public class TileMap
    {
        #region Variables
        private Point _size;
        private Tile[,] _tileMap;
        public Tile[,] Map { get { return this._tileMap; } }

        private Point _tileSize;
        public Point TileSize { get { return this._tileSize; } }
        private Texture2D _tileset;

        private Rectangle[] _frameSet;
        private Texture2D[] _frames;

        private bool _isServer;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the TileMap
        /// </summary>
        /// <param name="size">Size of the map</param>
        /// <param name="tileSize">Size of each tile</param>
        public TileMap(Point size, Point tileSize)
        {
            this._tileSize = tileSize;
            this._size = size;
            this._tileMap = new Tile[size.X, size.Y];

            this._isServer = true;
        }

        /// <summary>
        /// Constructor for the TileMap
        /// </summary>
        /// <param name="size">Size of the map</param>
        /// <param name="tileset">Image used for the tileset</param>
        /// <param name="tileSize">Size of each tile</param>
        public TileMap(Point size, Texture2D tileset, Point tileSize)
        {
            this._tileSize = tileSize;
            this._tileset = tileset;
            this._size = size;
            this._tileMap = new Tile[size.X, size.Y];

            this._isServer = false;

            // TODO: Maybe move this into an initialization function
            // Generate source rectangles
            int column = 0;
            int row = 0;
            this._frameSet = new Rectangle[((tileset.Width / tileSize.X) * (tileset.Height / tileSize.Y))];
            this._frames = new Texture2D[((tileset.Width / tileSize.X) * (tileset.Height / tileSize.Y))];
            for (int i = 0; i < this._frames.Length; i++)
            {
                // If the current tile frame is outside of the image bounds then we reset the column and move one row down
                if (column > tileset.Width)
                {
                    column = 0;
                    row += tileSize.Y;
                }
                this._frameSet[i] = new Rectangle(column, row, tileSize.X, tileSize.Y);
                this._frames[i] = TileMap.Crop(this._tileset, this._frameSet[i]);
                column += tileSize.X;
            }
        }
        #endregion

        /// <summary>
        /// Generates the map
        /// </summary>
        public void GenerateMap()
        {
            for (int x = 0; x < _size.X; x++)
            {
                for (int y = 0; y < _size.Y; y++)
                {
                    if (y < 50)
                    {
                        this._tileMap[x, y] = null;
                    }
                    else
                    {
                        // TODO: Don't know about this whole is server thing
                        // If we're generating this on the server
                        if (_isServer)
                        {
                            this._tileMap[x, y] = new Tile(
                                TileType.PLAIN_DARK,
                                _tileSize.X,
                                _tileSize.Y,
                                new Vector2((x * this._tileSize.X), (y * this._tileSize.Y)),
                                Color.Green
                            );
                        }
                        else
                        {
                            this._tileMap[x, y] = new Tile(
                                this._frames[TileType.PLAIN_DARK - 1],
                                TileType.PLAIN_DARK,
                                new Vector2((x * this._tileSize.X), (y * this._tileSize.Y)),
                                Color.Green
                            );
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the TileMap
        /// </summary>
        /// <param name="gameTime">Current gametime</param>
        public void Update(GameTime gameTime)
        {
        }

        /// <summary>
        /// Draws the TileMap
        /// </summary>
        /// <param name="spriteBatch">The Spritebatch used to render the tilemap</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Render Tiles (We use the tileset image rather than the cropped tile image to speed up rendering)
            for (int y = 0; y < _size.Y; ++y)
                for (int x = 0; x < _size.X; ++x)
                    if (this._tileMap[x, y] != null)
                    {
                        spriteBatch.Draw(
                            //this._tileMap[x, y].Image,
                            this._tileset,
                            this._tileMap[x, y].Position,
                            this._frameSet[this._tileMap[x, y].Type - 1],
                            this._tileMap[x, y].Color
                        );
                        this._tileMap[x, y].Color = Color.Green; // TODO: Remove Me
                    }
        }

        #region Helpers
        // TODO: Move to a helper class
        /// <summary>
        /// Crops a part of an image
        /// </summary>
        /// <param name="source">Source texture</param>
        /// <param name="area">Area to crop</param>
        /// <returns>Cropped part of the image</returns>
        private static Texture2D Crop(Texture2D source, Rectangle area)
        {
            if (source == null)
                return null;

            Texture2D cropped = new Texture2D(source.GraphicsDevice, area.Width, area.Height);
            Color[] data = new Color[source.Width * source.Height];
            Color[] cropData = new Color[cropped.Width * cropped.Height];

            source.GetData<Color>(data);

            int index = 0;
            for (int y = area.Y; y < area.Y + area.Height; y++)
            {
                for (int x = area.X; x < area.X + area.Width; x++)
                {
                    cropData[index] = data[x + (y * source.Width)];
                    index++;
                }
            }

            cropped.SetData<Color>(cropData);

            return cropped;
        }

        // TODO: Change int type
        /// <summary>
        /// Gets an image from the frames array
        /// </summary>
        /// <param name="type">Tile type</param>
        /// <returns>Texture used by the tile type</returns>
        public Texture2D GetImage(int type)
        {
            return this._frames[type - 1];
        }
        #endregion
    }
}
