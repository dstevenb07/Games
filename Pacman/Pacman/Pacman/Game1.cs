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

namespace Pacman
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics; //800*480
        SpriteBatch spriteBatch;
        Ghost redGhost;
        Ghost blueGhost;
        Player pacman;
        Level level;
        //Pellet pellet;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            pacman = new Player();
            redGhost = new Ghost(Color.Red, new Vector2(40, 40));
            blueGhost = new Ghost(Color.Blue, new Vector2(480, 400));
            level = new Level();
            //pellet = new Pellet();
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
            pacman.LoadContent(Content);
            redGhost.LoadContent(Content);
            blueGhost.LoadContent(Content);
            level.LoadLevel(LevelData.Data, Content);
            pacman.bounds = pacman.BoundInitiate();
            //pellet.LoadContent(Content);
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

            pacman.Update(gameTime);
            redGhost.Update(gameTime);
            blueGhost.Update(gameTime);
            //pellet.Update(gameTime);

            if (pacman.Bounds.Intersects(blueGhost.Bounds))
                this.Exit();

            for (int i = 0; i < pacman.bounds.Count; i++){
                if (pacman.NextBounds.Intersects(pacman.bounds[i]))
                    this.Exit();
        }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend);
            
            pacman.Draw(spriteBatch);
            redGhost.Draw(spriteBatch);
            blueGhost.Draw(spriteBatch);
            //pellet.Draw(spriteBatch);
            level.Draw(spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}