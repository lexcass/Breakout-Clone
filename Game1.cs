using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Breakout
{
    public struct Screen
    {
        public static int Width = 480;
        public static int Height = 480;
        public static Color Color = Color.DodgerBlue;
    }

    
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D paddleTex;
        Texture2D ballTex;
        Texture2D blockTex;
        SpriteFont font;

        Paddle paddle;
        Ball ball;
        List<Block> blocks;

        int score;
        Vector2 scorePosition;
        int lives;
        Vector2 livesPosition;
        bool pause;
        bool gameOver;
        Vector2 pauseTextPosition;
        Vector2 gameOverTextPosition;

        Rectangle screen;
        KeyboardState prevKeyState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Screen.Width;
            graphics.PreferredBackBufferHeight = Screen.Height;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }


        protected override void Initialize()
        {
            base.Initialize();

            // Subscribe to update score event
            Events.AttachToUpdateScore(OnUpdateScore);
            Events.AttachToLoseLife(OnLoseLife);

            pause = false;
            gameOver = false;
        }

        
        protected override void LoadContent()
        {
            screen = Window.ClientBounds;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            paddleTex = Content.Load<Texture2D>("images/paddle");
            ballTex = Content.Load<Texture2D>("images/ball");
            blockTex = Content.Load<Texture2D>("images/block");
            font = Content.Load<SpriteFont>("font");

            score = 0;
            lives = 3;
            scorePosition = new Vector2(16, Screen.Height - 16);
            livesPosition = new Vector2(Screen.Width - 96, Screen.Height - 16);
            pauseTextPosition = new Vector2(Screen.Width / 2 - 36, Screen.Height / 2);
            gameOverTextPosition = new Vector2(Screen.Width / 2 - 48, Screen.Height / 2);

            paddle = new Paddle(Screen.Width / 2 - paddleTex.Width / 2, Screen.Height - 48, paddleTex);
            ball = new Ball(Screen.Width / 2 - ballTex.Width / 2, Screen.Height - 60, ballTex);
            blocks = LevelLoader.Load("level1", blockTex);
        }

        
        protected override void UnloadContent()
        {
            paddleTex.Dispose();
            ballTex.Dispose();
            blockTex.Dispose();
        }

        
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A) && prevKeyState.IsKeyUp(Keys.A))
            {
                Console.WriteLine("A");
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // Pause
            if (Keyboard.GetState().IsKeyDown(Keys.P) && prevKeyState.IsKeyUp(Keys.P))
            {
                paddle.ToggleEnabled();
                ball.ToggleEnabled();
                pause = !pause;
            }

            // Game over
            if (gameOver)
            {
                paddle.IsEnabled = false;
                ball.IsEnabled = false;
            }

            if (paddle.IsAlive && paddle.IsEnabled) paddle.Update(gameTime);
            if (ball.IsAlive && ball.IsEnabled)
            {
                ball.Update(gameTime);
                if (ball.Collider.Intersects(paddle.Collider))
                {
                    ball.OnCollision("Paddle", paddle.Collider);
                }
                
                foreach (var block in blocks)
                {
                    if (ball.Collider.Intersects(block.Collider))
                    {
                        ball.OnCollision("Block", block.Collider);
                        block.OnCollision("Ball", ball.Collider);
                        break;
                    }
                }
            }

            // Remove destroyed blocks
            blocks.RemoveAll(block => !block.IsAlive);


            prevKeyState = Keyboard.GetState();
            base.Update(gameTime);
        }

        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Screen.Color);

            spriteBatch.Begin();
            if (paddle.IsAlive) paddle.Draw(spriteBatch);
            if (ball.IsAlive) ball.Draw(spriteBatch);

            // Draw Blocks
            foreach (var block in blocks)
            {
                if (block.IsAlive) block.Draw(spriteBatch);
            }

            // Draw the score
            spriteBatch.DrawString(font, "Score: " + score, scorePosition, Color.White);
            spriteBatch.DrawString(font, "Lives: " + lives, livesPosition, Color.White);

            // Pause text
            if (pause)
            {
                spriteBatch.DrawString(font, "PAUSED", pauseTextPosition, Color.White);
            }

            // Game Over text
            if (gameOver)
            {
                spriteBatch.DrawString(font, "Game Over", gameOverTextPosition, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }


        public void OnUpdateScore(object sender, UpdateScoreArgs e)
        {
            score += e.Points;
        }

        public void OnLoseLife(object sender, EventArgs e)
        {
            lives--;

            if (lives < 0)
            {
                gameOver = true;
            }

            paddle.Reset();
            ball.Reset();
        }
    }
}
