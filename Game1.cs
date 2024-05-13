using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;


namespace Mini_Golf
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D ballTexture;
        Vector2 ballPosition;
        Vector2 initialVelocity;
        Vector2 velocity;
        float angularVelocity;
        Boolean decreaseVelocity;

        Boolean ready;

        Texture2D course;

        Vector2 startPosition;
        Vector2 endPosition;
        Boolean pressed;
        Vector2 force;
        MouseState mouseState;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 800;
        }

        protected override void Initialize()
        {
            ballPosition = new Vector2(176f, 719f);
            initialVelocity.X = 1.5f;
            initialVelocity.Y = 1.5f;
            velocity = initialVelocity;

            angularVelocity = 0.5f;

            force = Vector2.Zero;
            pressed = false;
            decreaseVelocity = false;

            ready = false;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ballTexture = Content.Load<Texture2D>("ball");
            course = Content.Load<Texture2D>("course");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();    

            ballPosition.X += force.X * velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            ballPosition.Y += force.Y * velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            mouseState = Mouse.GetState();

            Boolean horizontal = mouseState.X >= ballPosition.X - ballTexture.Width && mouseState.X <= ballPosition.X + ballTexture.Width;
            Boolean vertical = mouseState.Y >= ballPosition.Y - ballTexture.Width && mouseState.Y <= ballPosition.Y + ballTexture.Width;

            if (mouseState.LeftButton == ButtonState.Pressed && (horizontal && vertical))
            {
                startPosition = ballPosition;
                pressed = true;

        
            }


            if (mouseState.LeftButton == ButtonState.Released && pressed)
            {
                endPosition = new Vector2(mouseState.X, mouseState.Y);
                
                force = new Vector2(startPosition.X - endPosition.X, startPosition.Y - endPosition.Y);
                pressed = false;
                decreaseVelocity = true;
            } 
           

            if (decreaseVelocity) 
            {
                velocity *= 0.996f;
            }


            if (ballPosition.X > _graphics.PreferredBackBufferWidth - ballTexture.Width / 2)
            {
                angularVelocity = -angularVelocity;
                velocity.X = -velocity.X;
            }
            else if (ballPosition.X < ballTexture.Width / 2)
            {
                angularVelocity = -angularVelocity;
                velocity.X = -velocity.X;
            }

            if (ballPosition.Y > _graphics.PreferredBackBufferHeight - ballTexture.Height / 2)
            {
                angularVelocity = -angularVelocity;
                velocity.Y = -velocity.Y;
            }
            else if (ballPosition.Y < ballTexture.Height / 2)
            {
                angularVelocity = -angularVelocity;
                velocity.Y = -velocity.Y;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(course, new Vector2(0, 0), Color.White);

            _spriteBatch.Draw(ballTexture,
                                ballPosition,
                                null,
                                Color.White,
                                angularVelocity,
                                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                                Vector2.One,
                                SpriteEffects.None,
                                0f);

            _spriteBatch.End();

            base.Draw(gameTime);

        }
    }
}
