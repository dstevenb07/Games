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

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Vector2 ballSpeed = new Vector2(6, 5);
        Texture2D ballTexture;
        Texture2D player1Texture;
        Texture2D player2Texture;
        Vector2 ballPos = new Vector2(391, 241);
        Vector2 player1Pos = new Vector2(30, 170);
        Vector2 player2Pos = new Vector2(760, 170);
        Vector2 player1Speed = new Vector2(0, 3);
        Vector2 player2Speed = new Vector2(0, 3);
        enum GameState { Start, Single, TwoPlayer, GameOver }
        GameState currentGameState = GameState.Start;
        SpriteFont BasicFont;
        int player1Score = 0;
        int player2Score = 0;
        Vector2 originalBallPos = new Vector2(391, 241);
        Rectangle player1Bounds;
        Rectangle player2Bounds;
        Rectangle ballBounds;
        

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
            IsMouseVisible = true;
            

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

            ballTexture = Content.Load<Texture2D>(@"Images\GreenSquare");
            player1Texture = Content.Load<Texture2D>(@"Images\GreenPlayer");
            player2Texture = Content.Load<Texture2D>(@"Images\GreenEnemy");
            BasicFont = Content.Load<SpriteFont>(@"Fonts\BasicFont");

            player1Bounds = new Rectangle(((int)player1Pos.X), ((int)player1Pos.Y), player1Texture.Width, player1Texture.Height);
            player2Bounds = new Rectangle(((int)player2Pos.X), ((int)player2Pos.Y), player2Texture.Width, player2Texture.Height);
            ballBounds = new Rectangle(((int)ballPos.X), ((int)ballPos.Y), ballTexture.Width, ballTexture.Height);

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            switch (currentGameState)
            {
                case GameState.Start:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.D1))
                            currentGameState = GameState.Single;
                        if (Keyboard.GetState().IsKeyDown(Keys.D2))
                            currentGameState = GameState.TwoPlayer;
                        break;
                    }
                case GameState.Single:
                    {
                        //player2Speed = new Vector2(0, 2);
                        
                        if (Keyboard.GetState().IsKeyDown(Keys.W))
                            player1Pos -= player1Speed;
                        if (Keyboard.GetState().IsKeyDown(Keys.S))
                            player1Pos += player1Speed;
                        
                        if (player1Pos.Y < 0)
                            player1Pos.Y = 0;
                        if (player1Pos.Y > Window.ClientBounds.Height - player1Texture.Height)
                            player1Pos.Y = Window.ClientBounds.Height - player1Texture.Height;

                        if (player2Pos.Y < 0)
                            player2Pos.Y = 0;
                        if (player2Pos.Y > Window.ClientBounds.Height - player2Texture.Height)
                            player2Pos.Y = Window.ClientBounds.Height - player2Texture.Height;
                        
                        ballPos += ballSpeed;

                        if (ballPos.Y < 0 || ballPos.Y > (Window.ClientBounds.Height - ballTexture.Height))
                            ballSpeed.Y *= -1;

                        if ((player2Pos.Y + 65) > ballPos.Y)
                            player2Pos -= player2Speed;

                        if ((player2Pos.Y + 65) < ballPos.Y)
                            player2Pos += player2Speed;

                        if (ballPos.X > Window.ClientBounds.Width + 20)
                        {
                            player1Score += 1;
                            ballSpeed.X *= -1;
                            ballPos = originalBallPos;
                        }

                        if (ballPos.X < -20)
                        {
                            player2Score += 1;
                            ballSpeed.X *= -1;
                            ballPos = originalBallPos;
                        }

                        if (player1Score == 7 || player2Score == 7)
                            currentGameState = GameState.GameOver;

                        if (ballBounds.Intersects(player1Bounds))
                        {
                            ballPos.X += 10;
                            ballSpeed.X *= -1;
                        }

                        if (ballBounds.Intersects(player2Bounds))
                        {
                            ballPos.X -= 10;
                            ballSpeed.X *= -1;
                        }

                        ballBounds.X = (int)ballPos.X;
                        ballBounds.Y = (int)ballPos.Y;
                        player1Bounds.X = (int)player1Pos.X;
                        player1Bounds.Y = (int)player1Pos.Y;
                        player2Bounds.X = (int)player2Pos.X;
                        player2Bounds.Y = (int)player2Pos.Y;
                            
                        break;
                    }
                case GameState.TwoPlayer:
                    {
                        player2Speed = player1Speed;
                        if (Keyboard.GetState().IsKeyDown(Keys.W))
                            player1Pos -= player1Speed;
                        if (Keyboard.GetState().IsKeyDown(Keys.S))
                            player1Pos += player1Speed;

                        if (Keyboard.GetState().IsKeyDown(Keys.Up))
                            player2Pos -= player2Speed;
                        if (Keyboard.GetState().IsKeyDown(Keys.Down))
                            player2Pos += player2Speed;

                        if (player1Pos.Y < 0)
                            player1Pos.Y = 0;
                        if (player1Pos.Y > Window.ClientBounds.Height - player1Texture.Height)
                            player1Pos.Y = Window.ClientBounds.Height - player1Texture.Height;

                        if (player2Pos.Y < 0)
                            player2Pos.Y = 0;
                        if (player2Pos.Y > Window.ClientBounds.Height - player2Texture.Height)
                            player2Pos.Y = Window.ClientBounds.Height - player2Texture.Height;

                        if (ballPos.Y < 0 || ballPos.Y > (Window.ClientBounds.Height - ballTexture.Height))
                            ballSpeed.Y *= -1;

                        ballPos += ballSpeed;

                        if (ballPos.X > Window.ClientBounds.Width + 20)
                        {
                            player1Score += 1;
                            ballSpeed.X *= -1;
                            ballPos = originalBallPos;
                        }
                        
                        if (ballPos.X < -20)
                        {
                            player2Score += 1;
                            ballSpeed.X *= -1;
                            ballPos = originalBallPos;
                        }
                        if (player1Score == 7 || player2Score == 7)
                            currentGameState = GameState.GameOver;

                        if (ballBounds.Intersects(player1Bounds))
                        {
                            ballPos.X += 10;
                            ballSpeed.X *= -1;
                        }

                        if (ballBounds.Intersects(player2Bounds))
                        {
                            ballPos.X -= 10;
                            ballSpeed.X *= -1;
                        }

                        ballBounds.X = (int)ballPos.X;
                        ballBounds.Y = (int)ballPos.Y;
                        player1Bounds.X = (int)player1Pos.X;
                        player1Bounds.Y = (int)player1Pos.Y;
                        player2Bounds.X = (int)player2Pos.X;
                        player2Bounds.Y = (int)player2Pos.Y;
                        break;
                    }
                case GameState.GameOver:
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                            this.Exit();
                        break;
                    }
            }
            
            //if (Keyboard.GetState().IsKeyDown(Keys.W))
            //    player1Pos -= player1Speed;
            //if (Keyboard.GetState().IsKeyDown(Keys.S))
            //    player1Pos += player1Speed;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            switch (currentGameState) { 
            
                case GameState.Start:
                {
                    spriteBatch.Begin();

                    string basicFont = "Press 1 to play alone";
                    spriteBatch.DrawString(BasicFont, basicFont, new Vector2(
                        (Window.ClientBounds.Width / 2) - (BasicFont.MeasureString(basicFont).X / 2),
                        (Window.ClientBounds.Height / 2) - (BasicFont.MeasureString(basicFont).Y / 2) - 15), Color.White);
                    basicFont = "Or press 2 to play with a friend!";
                    spriteBatch.DrawString(BasicFont, basicFont, new Vector2(
                        (Window.ClientBounds.Width / 2) - (BasicFont.MeasureString(basicFont).X / 2),
                        (Window.ClientBounds.Height / 2) - (BasicFont.MeasureString(basicFont).Y / 2) + 15), Color.White);

                    spriteBatch.End();
                    break;
                }
                case GameState.Single:
                {
                    spriteBatch.Begin();

                    spriteBatch.Draw(ballTexture, ballPos, Color.White);

                    spriteBatch.Draw(player1Texture, player1Pos, Color.White);

                    spriteBatch.Draw(player2Texture, player2Pos, Color.White);

                    spriteBatch.DrawString(BasicFont, player1Score.ToString(), 
                        new Vector2((Window.ClientBounds.Width / 2) - 150, 20), Color.White);

                    spriteBatch.DrawString(BasicFont, player2Score.ToString(),
                        new Vector2((Window.ClientBounds.Width / 2) + 150, 20), Color.White);

                    spriteBatch.End();
                    break;
                }

                case GameState.TwoPlayer:
                {
                    spriteBatch.Begin();

                    spriteBatch.Draw(ballTexture, ballPos, Color.White);

                    spriteBatch.Draw(player1Texture, player1Pos, Color.White);

                    spriteBatch.Draw(player2Texture, player2Pos, Color.White);

                    spriteBatch.DrawString(BasicFont, player1Score.ToString(),
                        new Vector2((Window.ClientBounds.Width / 2) - 150, 20), Color.White);

                    spriteBatch.DrawString(BasicFont, player2Score.ToString(),
                        new Vector2((Window.ClientBounds.Width / 2) + 150, 20), Color.White);

                    spriteBatch.End();
                    break;
                }

                case GameState.GameOver:
                {
                    spriteBatch.Begin();
                    if (player1Score > player2Score)
                    {
                        string basicFont = "Player 1 wins!!";
                        spriteBatch.DrawString(BasicFont, basicFont, 
                            new Vector2((Window.ClientBounds.Width / 2) - (BasicFont.MeasureString(basicFont).X / 2),
                                (Window.ClientBounds.Height / 2) - (BasicFont.MeasureString(basicFont).Y / 2)), Color.White);
                    }
                    if (player2Score > player1Score)
                    {
                        string basicFont = "Player 2 wins!!";
                        spriteBatch.DrawString(BasicFont, basicFont, 
                            new Vector2((Window.ClientBounds.Width / 2) - (BasicFont.MeasureString(basicFont).X / 2),
                                (Window.ClientBounds.Height / 2) - (BasicFont.MeasureString(basicFont).Y / 2)), Color.White);
                    }

                    string gameOverString = "(Press ENTER to Exit)";
                    spriteBatch.DrawString(BasicFont, gameOverString,
                        new Vector2(((Window.ClientBounds.Width / 2) - (BasicFont.MeasureString(gameOverString).X / 2)),
                                ((Window.ClientBounds.Height / 2) - (BasicFont.MeasureString(gameOverString).Y / 2)) + 30), Color.White);

                    spriteBatch.End();
                    break;
                }
            }

            base.Draw(gameTime);
        }
    }
}
