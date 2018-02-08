using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Breakout
{
    public abstract class Sprite
    {
        protected Vector2 _position;
        protected Vector2 _initialPosition;
        public Vector2 Position { get { return _position; } }
        protected Vector2 _velocity;
        public Vector2 Velocity { get { return _velocity; } }
        public bool IsEnabled { get; set; }
        private bool _isAlive;
        public bool IsAlive { get { return _isAlive; } }
        protected Texture2D _texture;
        protected Rectangle _collider;
        public Rectangle Collider { get { return _collider; } }
        protected string _group;
        public string Group { get { return _group; } }

        public Sprite(string group, int xPos, int yPos, Texture2D texture)
        {
            _group = group;
            _position = new Vector2(xPos, yPos);
            _initialPosition = _position;
            _velocity = Vector2.Zero;
            _isAlive = true;
            IsEnabled = true;
            _texture = texture;
            _collider = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
        }

        /// <summary>
        /// Move the sprite by setting its velocity vector.
        /// </summary>
        /// <param name="newVelocity"></param>
        public void SetVelocity(Vector2 newVelocity)
        {
            _velocity = newVelocity;
        }

        /// <summary>
        /// Move the sprite by setting its individual x and y velocities.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetVelocity(float x, float y)
        {
            _velocity.X = x;
            _velocity.Y = y;
        }

        public void MoveRelative(int x, int y)
        {
            _position.X += x;
            _position.Y += y;
        }

        public void MoveTo(Vector2 newPosition)
        {
            _position = newPosition;
        }

        /// <summary>
        /// Reset sprite to their initial position.
        /// </summary>
        public void Reset()
        {
            _position = _initialPosition;
        }

        /// <summary>
        /// Flag the sprite so that it's resources may be cleaned up.
        /// </summary>
        public void DestroySelf()
        {
            _isAlive = false;
        }


        public void ToggleEnabled()
        {
            IsEnabled = !IsEnabled;
        }

        /// <summary>
        /// Check for a collision with another sprite's collider.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public void HandleCollision(Sprite other)
        {
            if (_collider.Intersects(other.Collider))
            {
                OnCollision(other.Group, other.Collider);
            }
        }

        /// <summary>
        /// Callback for when sprite collides with a certian group of sprites.
        /// </summary>
        /// <param name="group"></param>
        public abstract void OnCollision(string group, Rectangle other);

        /// <summary>
        /// Update method to be overriden by child classes.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move the collider
            _collider.X = (int)_position.X;
            _collider.Y = (int)_position.Y;
        }

        /// <summary>
        /// Draw the sprite at its current position with the current texture.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position);
        }
    }
}
