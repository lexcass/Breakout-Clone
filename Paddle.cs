using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Breakout
{
    class Paddle : Sprite
    {
        const int SPEED = 300;
        private PaddleMovedArgs moveArgs;

        public Paddle(int xPos, int yPos, Texture2D texture) : base("Paddle", xPos, yPos, texture)
        {
            moveArgs = new PaddleMovedArgs();
        }

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                SetVelocity(-SPEED, 0);
                moveArgs.X = -SPEED;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                SetVelocity(SPEED, 0);
                moveArgs.X = SPEED;
            }
            else
            {
                SetVelocity(0, 0);
                moveArgs.X = 0;
            }

            Events.FirePaddleMoved(this, moveArgs);

            // Launch the ball when space is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Events.FireBallLaunched(this, EventArgs.Empty);
            }

            // Keep paddle in the play area
            if (_position.X < 0) _position.X = 0;
            if (_position.X > Screen.Width - _texture.Width) _position.X = Screen.Width - _texture.Width;
        }

        public override void OnCollision(string group, Rectangle other)
        {
            throw new NotImplementedException();
        }
    }
}
