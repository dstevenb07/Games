using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rocket_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Background background;
        Player player1;
        Player player2;
        HealthBar health1;
        HealthBar health2;
        Enemy dragon;
        Camera camera;

        enum GameState 
        { 
            Menu,
            OnePlayer,
            TwoPlayer,
            Win,
            Lose
        }
        
        GameState currentState = GameState.Menu;
        bool isPaused = false;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            background = new Background();
            player1 = new Player(Content.Load<Texture2D>(@"Sprites/rocketFlames half size"), new Vector2(0, 150), Color.White);
            player2 = new Player(Content.Load<Texture2D>(@"Sprites/rocketFlames half size"), new Vector2(0, 350), Color.MediumPurple);
            health1 = new HealthBar(30, Color.Red);
            health2 = new HealthBar(570, Color.Purple);
            dragon = new Enemy();
            camera = new Camera(GraphicsDevice.Viewport);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            IsMouseVisible = true;

            background.LoadContent(Content, GraphicsDevice.Viewport);
            health1.LoadContent(Content, GraphicsDevice.Viewport);
            health2.LoadContent(Content, GraphicsDevice.Viewport);
            dragon.LoadContent(Content, GraphicsDevice.Viewport);
            player1.LoadContent(GraphicsDevice.Viewport);

            player2.LoadContent(GraphicsDevice.Viewport);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            switch (currentState) 
            { 
                case GameState.Menu:
                    if (Keyboard.GetState().IsKeyDown(Keys.D1))
                        currentState = GameState.OnePlayer;
                    if (Keyboard.GetState().IsKeyDown(Keys.D2))
                        currentState = GameState.TwoPlayer;
                    break;
                case GameState.OnePlayer:
                    if (isPaused == false)
                    {
                        background.Update(gameTime, player1);
                        player1.Update(gameTime, Keys.W, Keys.A, Keys.D, player1);
                        dragon.Update(gameTime, player1);
                        camera.Update(gameTime, player1);
                        health1.Update(gameTime, player1);
                    }
                    break;
                case GameState.TwoPlayer:
                    if (isPaused == false)
                    {
                        background.Update(gameTime, player1);
                        player1.Update(gameTime, Keys.W, Keys.A, Keys.D, player1);
                        dragon.Update(gameTime, player1);
                        camera.Update(gameTime, player1);
                        health1.Update(gameTime, player1);
                        health2.Update(gameTime, player1);
                        player2.Update(gameTime, Keys.Up, Keys.Left, Keys.Right, player1);
                    }
                    else 
                    {
                        //Pause.Update(gameTime);
                    }
                    break;
                case GameState.Lose:

                    break;
                case GameState.Win:

                    break;
            }
            
            
            //background.Update(gameTime, player1);
            //player1.Update(gameTime, Keys.W, Keys.A, Keys.D, player1);
            //dragon.Update(gameTime, player1);
            //camera.Update(gameTime, player1);
            //health1.Update(gameTime, player1);
            //health2.Update(gameTime, player1);

            //player2.Update(gameTime, Keys.Up, Keys.Left, Keys.Right, player1);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Deferred, 
                                BlendState.AlphaBlend,
                                null, null, null, null,
                                camera.transform);

            switch (currentState)
            {
                case GameState.Menu:

                    break;

                case GameState.OnePlayer:
                    background.Draw(spriteBatch);
                    dragon.Draw(spriteBatch);
                    player1.Draw(spriteBatch);
                    health1.Draw(spriteBatch);

                    if (isPaused == true)
                    {
                        // Draw pause screen over everything with 50% opacity
                    }

                    break;

                case GameState.TwoPlayer:
                    background.Draw(spriteBatch);
                    dragon.Draw(spriteBatch);
                    player2.Draw(spriteBatch);
                    player1.Draw(spriteBatch);
                    health1.Draw(spriteBatch);
                    health2.Draw(spriteBatch);

                    if (isPaused == true)
                    {
                        // Draw pause screen over everything with 50% opacity
                    }

                    break;

                case GameState.Lose:

                    break;

                case GameState.Win:

                    break;
            }
            
            
            //background.Draw(spriteBatch);
            //dragon.Draw(spriteBatch);

            //player2.Draw(spriteBatch);

            //player1.Draw(spriteBatch);
            //health1.Draw(spriteBatch);

            //health2.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
