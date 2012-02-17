using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace MinionServer.Game.Entities
{
    class Player
    {
        public byte NetworkId;
        public int PosX;
        public int PosY;

        public float VelocityX;
        public float VelocityY;

        public float BaseSpeed;
        public float MaxVerticalVelocity;

        private float _jumpTime;

        public int OriginX;
        public int OriginY;

        private Rectangle _boundingBox;
        public Rectangle BoundingBox
        {
            get
            {
                // TODO: Gah, don't know about this casting position floats to int
                _boundingBox.X = (int)Math.Round((float)PosX - OriginX);
                _boundingBox.Y = (int)Math.Round((float)PosY - OriginY);
                return _boundingBox;
            }
        }
        private Rectangle _groundBox;
        public Rectangle GroundBox
        {
            get
            {
                _groundBox.X = (int)Math.Round((float)PosX - OriginX);
                _groundBox.Y = (int)Math.Round((float)PosX - OriginX + 1);
                return _groundBox;
            }
        }
        public bool IsOnGround;
        public bool IsJumping;

        public Player(byte id, int width, int height)
        {
            this.NetworkId = id;
            this.PosX = 0;
            this.PosY = 0;

            this.VelocityX = 0f;
            this.VelocityY = 0f;

            this.BaseSpeed = 6f;
            this.MaxVerticalVelocity = 12f;
            this._jumpTime = 0f;

            this.IsOnGround = false;
            this.IsJumping = false;

            this.OriginX = width / 2;
            this.OriginY = height / 2;

            // Movement 
            this._boundingBox = new Rectangle(0, 0, width, height);
            this._groundBox = new Rectangle(0, 0, width, 1);
        }

        public Player(byte id, int width, int height, int posX, int posY)
            : this(id, width, height)
        {
            this.PosX = posX;
            this.PosY = posY;
        }
    }
}
