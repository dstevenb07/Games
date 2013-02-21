using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace AnimatedSprites
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteManager spriteManager;
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue trackCue;
        public Random rnd { get; private set; }
        int currentScore = 0;
        SpriteFont scoreFont;
        Texture2D backgroundTexture;
        enum GameState { Start, InGame, GameOver };
        GameState currentGameState = GameState.Start;
        int numberLivesRemaining = 3;

        public int NumberLivesRemaining
        {
            get { return numberLivesRemaining; }
            set
            {
                numberLivesRemaining = value;
                if (numberLivesRemaining == 0)
                {
                    currentGameState = GameState.GameOver;
                    spriteManager.Enabled = false;
                    spriteManager.Visible = false;
                }
            }
        }
        
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            rnd = new Random();
            graphics.PreferredBackBufferHeight = 768;
            graphics.PreferredBackBufferWidth = 1024;
        }
        
        protected override void Initialize()
        {
            spriteManager = new SpriteManager(this);
            Components.Add(spriteManager);
            rnd = new Random();
            spriteManager.Enabled = false;
            spriteManager.Visible = false;

            base.Initialize();
        }

        //public int NumberLivesRemaining
        //{
        //    get { return numberLivesRemaining; }
        //    set
        //    {
        //        numberLivesRemaining = value;
        //        if (numberLivesRemaining == 0)
        //        {
        //            currentGameState = GameState.GameOver;
        //            spriteManager.Enabled = false;
        //            spriteManager.Visible = false;
        //        }
        //    }
        //}


        public void AddScore(int score)
        {
            currentScore += score;
        }

        public void PlayCue(string cueName)
        {
            soundBank.PlayCue(cueName);
        }
        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
            scoreFont = Content.Load<SpriteFont>(@"Fonts\scoreFont");
            backgroundTexture = Content.Load<Texture2D>(@"Images\background");

            // Start the soundtrack audio
            trackCue = soundBank.GetCue("track");
            trackCue.Play();
            // Play the start sound
            soundBank.PlayCue("start");
        }
        
        protected override void UnloadContent()
        {
        // TODO: Unload any non-ContentManager content here
        }
        
        protected override void Update(GameTime gameTime)
        {
            switch (currentGameState) 
            {
                case GameState.Start:
                    if (Keyboard.GetState().GetPressedKeys().Length > 0 || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
                    {
                        currentGameState = GameState.InGame;
                        spriteManager.Enabled = true;
                        spriteManager.Visible = true;
                    }
                    break;

                case GameState.InGame:
                    break;

                case GameState.GameOver:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        Exit();
                    break;
            }
            
            
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back ==
                ButtonState.Pressed)
            this.Exit();

            audioEngine.Update();

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            switch (currentGameState)
            {
                case GameState.Start:

                    GraphicsDevice.Clear(Color.AliceBlue);
                    // Draw text for intro splash screen
                    spriteBatch.Begin( );

                    string text = "Avoid the blades or die!";

                    spriteBatch.DrawString(scoreFont, text,
                    new Vector2((Window.ClientBounds.Width / 2)
                    - (scoreFont.MeasureString(text).X / 2),
                    (Window.ClientBounds.Height / 2)
                    - (scoreFont.MeasureString(text).Y / 2)),
                    Color.SaddleBrown);
                        text = "(Press any key to begin(or \"A\" on the gamepad))";

                    spriteBatch.DrawString(scoreFont, text,
                        new Vector2((Window.ClientBounds.Width / 2)
                        - (scoreFont.MeasureString(text).X / 2),
                        (Window.ClientBounds.Height / 2)
                        - (scoreFont.MeasureString(text).Y / 2) + 30),
                        Color.SaddleBrown);

                    spriteBatch.End( );
                    break;

                case GameState.InGame:
                    GraphicsDevice.Clear(Color.White);

                    spriteBatch.Begin();

                    spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0, Window.ClientBounds.Width, Window.ClientBounds.Height),
                        null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);

                    spriteBatch.DrawString(scoreFont, "Score: " + currentScore,
                                new Vector2(10, 10), Color.DarkBlue, 0, Vector2.Zero,
                                1, SpriteEffects.None, 1);

                    spriteBatch.End();
                    break;

                case GameState.GameOver:
                    GraphicsDevice.Clear(Color.AliceBlue);
                    
                    spriteBatch.Begin();

                    string gameOver = "Game Over! The blades win again!";
                    spriteBatch.DrawString(scoreFont, gameOver,
                        new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(gameOver).X /2), 
                            (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(gameOver).Y /2)),
                            Color.SaddleBrown);

                    gameOver = "Your Score: " + currentScore;
                    spriteBatch.DrawString(scoreFont, gameOver,
                        new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(gameOver).X / 2),
                            (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(gameOver).Y / 2) + 30),
                            Color.SaddleBrown);

                    gameOver = "Press ENTER to exit";
                    spriteBatch.DrawString(scoreFont, gameOver,
                        new Vector2((Window.ClientBounds.Width / 2) - (scoreFont.MeasureString(gameOver).X / 2),
                            (Window.ClientBounds.Height / 2) - (scoreFont.MeasureString(gameOver).Y / 2) + 60),
                            Color.SaddleBrown);
                    
                    spriteBatch.End();
                    break;
            }
          
            base.Draw(gameTime);
        }
    }
}