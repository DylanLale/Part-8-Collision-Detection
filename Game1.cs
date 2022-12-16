using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading;

namespace Part_8__Collision_Detection
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        KeyboardState keyboardState, keyboardPreviousState;
        MouseState mouseState, mousePreviousState;
        // Textures
        Texture2D pacLeftTexture;
        Texture2D pacRightTexture;
        Texture2D pacUpTexture;
        Texture2D pacDownTexture;
        Texture2D currentPacTexture; // Current Pacman texture to draw
        Rectangle pacRect; // This rectangle will track where Pacman is and his size
        Texture2D exitTexture;
        Rectangle exitRect;
        Texture2D barrierTexture;
        Texture2D coinTexture;
        List<Rectangle> coins;
        List<Rectangle> barriers;
        int pacSpeed;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            pacSpeed = 3;
            pacRect = new Rectangle(10, 10, 60, 60);

            barriers = new List<Rectangle>();
            barriers.Add(new Rectangle(0, 250, 350, 75));
            barriers.Add(new Rectangle(450, 250, 350, 75));

            coins = new List<Rectangle>();
            coins.Add(new Rectangle(400, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(475, 50, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(200, 350, coinTexture.Width, coinTexture.Height));
            coins.Add(new Rectangle(400, 350, coinTexture.Width, coinTexture.Height));

            exitRect = new Rectangle(700, 380, 100, 100);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            pacDownTexture = Content.Load<Texture2D>("pac_down");
            pacUpTexture = Content.Load<Texture2D>("pac_up");
            pacRightTexture = Content.Load<Texture2D>("pac_right");
            pacLeftTexture = Content.Load<Texture2D>("pac_left");
            currentPacTexture = pacRightTexture;
            barrierTexture = Content.Load<Texture2D>("rock_barrier");
            exitTexture = Content.Load<Texture2D>("hobbit_door");
            coinTexture = Content.Load<Texture2D>("coin");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            keyboardPreviousState = keyboardState;
            keyboardState = Keyboard.GetState();

            mousePreviousState = mouseState;
            mouseState = Mouse.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if (keyboardState.IsKeyDown(Keys.Left))
            {
                pacRect.X -= pacSpeed;
                currentPacTexture = pacLeftTexture;
                foreach (Rectangle barrier in barriers)
                    
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.X = barrier.Right;
                    }

            }
            if (keyboardState.IsKeyDown(Keys.Right))
            {
                pacRect.X += pacSpeed;
                currentPacTexture = pacRightTexture;
                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.X = barrier.Left - pacRect.Width;
                    }
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                pacRect.Y -= pacSpeed;
                currentPacTexture = pacUpTexture;
                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.Y = barrier.Bottom;
                    }
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                pacRect.Y += pacSpeed;
                currentPacTexture = pacDownTexture;
                foreach (Rectangle barrier in barriers)
                    if (pacRect.Intersects(barrier))
                    {
                        pacRect.Y = barrier.Top - pacRect.Height;
                    }
            }

            for (int i = 0; i < coins.Count; i++)
            {
                if (pacRect.Intersects(coins[i]))
                {
                    coins.RemoveAt(i);
                    i--;   
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton != mousePreviousState.LeftButton)
            {
                if (exitRect.Contains(mouseState.X, mouseState.Y))
                    Exit();
            }

            if (exitRect.Contains(pacRect))
                Exit();

            base.Update(gameTime);
        }
    }

        protected override void Draw(GameTime gameTime)
        {
        GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();

            _spriteBatch.Draw(exitTexture, exitRect, Color.White);
            _spriteBatch.Draw(currentPacTexture, pacRect, Color.White);
            foreach (Rectangle coin in coins)
                _spriteBatch.Draw(coinTexture, coin, Color.White);
            foreach (Rectangle barrier in barriers)
                _spriteBatch.Draw(barrierTexture, barrier, Color.White);

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}