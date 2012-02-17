using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MinionServer.Game.Map
{
    /// <summary>
    /// Controls the collision detection and response behavior of a tile.
    /// </summary>
    public enum TileCollision
    {
        /// <summary>
        /// A passable tile is one which does not hinder player motion at all.
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// A platform tile is one which behaves like a passable tile except when the
        /// player is above it. A player can jump up through a platform as well as move
        /// past it to the left and right, but can not fall down through the top of it.
        /// </summary>
        Platform = 2,
    }


    public class Tile
    {
        #region Variables
        protected Texture2D image;
        public Texture2D Image { get { return this.image; } set { this.image = value; } }

        // TODO: Change this
        protected int type;
        public int Type { get { return this.type; } set { this.type = value; } }

        public TileCollision CollisionType;

        protected Vector2 position;
        public Vector2 Position { get { return this.position; } }

        protected Rectangle boundingBox;
        public Rectangle BoundingBox { get { return this.boundingBox; } }

        public Color Color { get; set; }
        #endregion

        #region Constructor
        public Tile()
        {
            this.image = null;
            this.type = TileType.NONE;
            this.position = Vector2.Zero;
            this.Color = Color.White;
            this.boundingBox = Rectangle.Empty;
            this.CollisionType = TileCollision.Passable;
        }

        public Tile(int type, int width, int height, Vector2 position, Color color, TileCollision collisionType = TileCollision.Impassable)
        {
            this.image = null;
            this.type = type;
            this.position = position;
            this.Color = color;
            this.CollisionType = collisionType;
            this.boundingBox = new Rectangle((int)position.X, (int)position.Y, width, height);
        }

        public Tile(Texture2D image, int type, Vector2 position, Color color, TileCollision collisionType = TileCollision.Impassable)
        {
            this.image = image;
            this.type = type;
            this.position = position;
            this.Color = color;
            this.CollisionType = collisionType;
            this.boundingBox = new Rectangle((int)position.X, (int)position.Y, image.Width, image.Height);
        }
        #endregion
    }
}
