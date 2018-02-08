using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Breakout
{
    class Ball : Sprite
    {
        const int MAX_SPEED = 250;
        private bool _wasLaunched;

        public Ball(int xPos, int yPos, Texture2D texture) : base("Ball", xPos, yPos, texture)
        {
            _wasLaunched = false;
            Events.AttachToBallLaunch(OnLaunch);
            Events.AttachToPaddleMoved(OnPaddleMove);
        }


        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Keep ball in the play area
            int displaceX = 0;
            int displaceY = 0;
            int displaceAmount = 2;

            if (_position.X < 0)
            {
                displaceX = displaceAmount;
                Bounce(Side.Left);
            }
            if (_position.X > Screen.Width - _texture.Width)
            {
                displaceX = -displaceAmount;
                Bounce(Side.Right);
            }
            if (_position.Y < 0)
            {
                displaceY = displaceAmount;
                Bounce(Side.Top);
            }
            if (_position.Y > Screen.Height - _texture.Height)
            {
                Events.FireLoseLife(this, EventArgs.Empty);
            }
            MoveRelative(displaceX, displaceY);
        }

        public override void OnCollision(string group, Rectangle other)
        {
            Rectangle overlap;
            Rectangle.Intersect(ref _collider, ref other, out overlap);
            Vector2 unitVec = _velocity / _velocity.Length();

            if (group == "Paddle")
            {
                int paddleCenter = other.X + other.Width / 2;
                int distFromCenter = System.Math.Abs(paddleCenter - overlap.X);
                MoveRelative(0, -overlap.Width);

                if (_velocity.X < 0) SetVelocity(-distFromCenter * 4, _velocity.Y);
                else SetVelocity(distFromCenter * 4, _velocity.Y);
                Bounce(Side.Bottom);
            }

            if (group == "Block")
            {
                if (overlap.Width < overlap.Height)
                {
                    MoveRelative(overlap.Width * (int)unitVec.X, 0);
                    Bounce(Side.Right);
                }
                else
                {
                    MoveRelative(0, overlap.Height * (int)unitVec.Y);
                    Bounce(Side.Bottom);
                }
            }
        }

        public void Reset()
        {
            base.Reset();
            _wasLaunched = false;
            Events.AttachToBallLaunch(OnLaunch);
            Events.AttachToPaddleMoved(OnPaddleMove);

        }

        /// <summary>
        /// Launch the ball straight forward to start the game.
        /// </summary>
        public void OnLaunch(object sender, EventArgs e)
        {
            _wasLaunched = true;
            SetVelocity(new Vector2(0, -MAX_SPEED));
            Events.DetachFromBallLaunched(OnLaunch);
        }

        /// <summary>
        /// Callback for when paddle is moved before ball is launched.
        /// Ball moves with the paddle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnPaddleMove(object sender, PaddleMovedArgs e)
        {
            if (_wasLaunched)
            {
                Events.DetachFromPaddleMoved(OnPaddleMove);
            }
            else
            {
                SetVelocity(e.X, 0);
            }
        }

        /// <summary>
        /// Bounces the ball off of a surface.
        /// Basically reflects its vector over the x or y axis (like an absolute value function).
        /// </summary>
        public void Bounce(Side side)
        {
            switch(side)
            {
                case Side.Left:
                case Side.Right:
                    SetVelocity((int)-Velocity.X, (int)Velocity.Y);
                    break;
                case Side.Top:
                case Side.Bottom:
                    SetVelocity((int)Velocity.X, (int)-Velocity.Y);
                    break;
            }
        }
    }
}
