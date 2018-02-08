using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Breakout
{
    public class Block : Sprite
    {
        private Color _color;
        public Color Color { get { return _color; } }

        private UpdateScoreArgs _scoreArgs;

        public Block(int xPos, int yPos, Texture2D texture, Color color) : base("Block", xPos, yPos, texture)
        {
            _color = color;
            _scoreArgs = new UpdateScoreArgs();
            _scoreArgs.Points = 100;
        }

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void OnCollision(string group, Rectangle other)
        {
            if (group == "Ball")
            {
                Events.FireUpdateScore(this, _scoreArgs);
                DestroySelf();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, _color);
        }
    }
}
