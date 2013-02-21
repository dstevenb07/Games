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


namespace AnimatedSprites
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        UserControlledSprite player;
        List<Sprite> spriteList = new List<Sprite>();
        //private SpriteFont textfont;
        //private SpriteFont AgainFont;
        //private Vector2 _position;
        int enemySpawnMinMilliseconds = 1000;
        int enemySpawnMaxMilliseconds = 2000;
        int enemyMinSpeed = 2;
        int enemyMaxSpeed = 6;
        int nextSpawnTime = 0;
        int likelihoodAutomated = 60;
        int likelihoodChasing = 35;
        //int likelihoodEvading = 5;
        int automatedSpritePointValue = 10;
        int chasingSpritePointValue = 20;
        int evadingSpritePointValue = 0;
        List<AutomatedSprite> livesList = new List<AutomatedSprite>();
        int nextSpawnTimeChange = 5000;
        int timeSinceLastSpawnTimeChange = 0;
        int powerUpExpiration = 0;
        int skullPowerUpExpiration = 0;
        int plusPowerUpExpiration = 0;
        int boltPowerUpExpiration = 0;



        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        protected override void LoadContent( )
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            player = new UserControlledSprite(
            Game.Content.Load<Texture2D>(@"Images/threerings"),
            new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2),
            new Point(75, 75), 10, new Point(0, 0),
            new Point(6, 8), new Vector2(6, 6));
            for (int i = 0; i < ((Game1)Game).NumberLivesRemaining; ++i)
            {
                int offset = 10 + i * 40;
                livesList.Add(new AutomatedSprite(
                Game.Content.Load<Texture2D>(@"images\threerings"),
                new Vector2(offset, 35), new Point(75, 75), 10,
                new Point(0, 0), new Point(6, 8), Vector2.Zero,
                null, 0, .5f));
            }

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            ResetSpawnTime();

            base.Initialize();
        }

        protected void AdjustSpawnTimes(GameTime gameTime)
        {
            // If the spawn max time is > 500 milliseconds,
            // decrease the spawn time if it is time to do
            // so based on the spawn-timer variables
            if (enemySpawnMaxMilliseconds > 500)
            {
                timeSinceLastSpawnTimeChange += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastSpawnTimeChange > nextSpawnTimeChange)
                {
                    timeSinceLastSpawnTimeChange -= nextSpawnTimeChange;
                    if (enemySpawnMaxMilliseconds > 1000)
                    {
                        enemySpawnMaxMilliseconds -= 100;
                        enemySpawnMinMilliseconds -= 100;
                    }
                    else
                    {
                        enemySpawnMaxMilliseconds -= 10;
                        enemySpawnMinMilliseconds -= 10;
                    }
                }
            }
        }

        private void ResetSpawnTime()
        {
            nextSpawnTime = ((Game1)Game).rnd.Next(
            enemySpawnMinMilliseconds,
            enemySpawnMaxMilliseconds);
        }

        private void SpawnEnemy( )
        {
            Vector2 speed = Vector2.Zero;
            Vector2 position = Vector2.Zero;
            // Default frame size
            Point frameSize = new Point(75, 75);
                        // Randomly choose which side of the screen to place enemy,
            // then randomly create a position along that side of the screen
            // and randomly choose a speed for the enemy
            switch (((Game1)Game).rnd.Next(4))
            {
                case 0: // LEFT to RIGHT
                    position = new Vector2(
                    -frameSize.X, ((Game1)Game).rnd.Next(0,
                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                    - frameSize.Y));
                    speed = new Vector2(((Game1)Game).rnd.Next(
                    enemyMinSpeed,
                    enemyMaxSpeed), 0);
                    break;
                case 1: // RIGHT to LEFT
                    position = new
                    Vector2(
                    Game.GraphicsDevice.PresentationParameters.BackBufferWidth,
                    ((Game1)Game).rnd.Next(0,
                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight
                    - frameSize.Y));
                    speed = new Vector2(-((Game1)Game).rnd.Next(
                    enemyMinSpeed, enemyMaxSpeed), 0);
                    break;
                case 2: // BOTTOM to TOP
                    position = new Vector2(((Game1)Game).rnd.Next(0,
                    Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                    - frameSize.X),
                    Game.GraphicsDevice.PresentationParameters.BackBufferHeight);
                    speed = new Vector2(0,
                    -((Game1)Game).rnd.Next(enemyMinSpeed,
                    enemyMaxSpeed));
                    break;
                case 3: // TOP to BOTTOM
                    position = new Vector2(((Game1)Game).rnd.Next(0,
                    Game.GraphicsDevice.PresentationParameters.BackBufferWidth
                    - frameSize.X), -frameSize.Y);
                    speed = new Vector2(0,
                    ((Game1)Game).rnd.Next(enemyMinSpeed,
                    enemyMaxSpeed));
                    break;
            }

            int random = ((Game1)Game).rnd.Next(100);
            if (random < likelihoodAutomated)
            {
                if (((Game1)Game).rnd.Next(2) == 0)
                {

                    spriteList.Add(
                    new AutomatedSprite(Game.Content.Load<Texture2D>(@"images\fourblades"),
                    position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8),
                    speed, "fourbladescollision", automatedSpritePointValue));
                }
                else 
                {
                    spriteList.Add(
                    new AutomatedSprite(Game.Content.Load<Texture2D>(@"images\threeblades"),
                    position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8),
                    speed, "threebladescollision", automatedSpritePointValue));
                }
            }
            else if (random < likelihoodAutomated + likelihoodChasing)
            {
                if (((Game1)Game).rnd.Next(2) == 0)
                {
                    spriteList.Add(
                    new ChasingSprite(Game.Content.Load<Texture2D>(@"images\skullball"),
                    position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8),
                    speed, "skullcollision", this, chasingSpritePointValue));
                }
                else
                {
                    spriteList.Add(
                    new ChasingSprite(Game.Content.Load<Texture2D>(@"images\plus"),
                    position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 4),
                    speed, "pluscollision", this, chasingSpritePointValue));
                }
            }
            else 
            {
                spriteList.Add(
                    new EvadingSprite(Game.Content.Load<Texture2D>(@"images\bolt"),
                    position, new Point(75, 75), 10, new Point(0, 0), new Point(6, 8),
                    speed, "boltcollision", this, .75f, 150, evadingSpritePointValue));
            }



        }

        protected void CheckPowerUpExpiration(GameTime gameTime)
        {
        // Is a power-up active?
            if (powerUpExpiration > 0 || skullPowerUpExpiration > 0 || plusPowerUpExpiration > 0 || boltPowerUpExpiration > 0)
            {
                if (powerUpExpiration > 0)
                {
                    powerUpExpiration -=
                    gameTime.ElapsedGameTime.Milliseconds;
                    if (powerUpExpiration <= 0)
                    {
                        powerUpExpiration = 0;
                        player.ResetScale();
                        player.ResetSpeed();
                    }
                }
                else if (skullPowerUpExpiration > 0) 
                {
                    skullPowerUpExpiration -=
                    gameTime.ElapsedGameTime.Milliseconds;
                    if (skullPowerUpExpiration <= 0)
                    {
                        skullPowerUpExpiration = 0;
                        player.ResetScale();
                        player.ResetSpeed();
                    }
                }
                else if (plusPowerUpExpiration > 0) 
                {
                    plusPowerUpExpiration -=
                    gameTime.ElapsedGameTime.Milliseconds;
                    if (plusPowerUpExpiration <= 0)
                    {
                        plusPowerUpExpiration = 0;
                        player.ResetScale();
                        player.ResetSpeed();
                    }
                }

                else if (boltPowerUpExpiration > 0) 
                {
                    boltPowerUpExpiration -=
                    gameTime.ElapsedGameTime.Milliseconds;
                    if (boltPowerUpExpiration <= 0)
                    {
                        boltPowerUpExpiration = 0;
                        player.ResetScale();
                        player.ResetSpeed();
                    }
                }
                
            }
        }



            protected void UpdateSprites(GameTime gameTime)
        {

            // Update player
            player.Update(gameTime, Game.Window.ClientBounds);
            // Update all nonplayer sprites
            for (int i = 0; i < spriteList.Count; ++i)
            {
                Sprite s = spriteList[i];
                s.Update(gameTime, Game.Window.ClientBounds);
                // Check for collisions
                if (s.collisionRect.Intersects(player.collisionRect))
                {
                    // Play collision sound
                    if (s.collisionCueName != null)
                    ((Game1)Game).PlayCue(s.collisionCueName);

                    if (s is AutomatedSprite) 
                    {
                        if (livesList.Count > 0)
                        {
                            livesList.RemoveAt(livesList.Count - 1);
                            --((Game1)Game).NumberLivesRemaining;
                        }
                    }

                    else if (s.collisionCueName == "pluscollision")
                    {
                        // Collided with plus - start plus power-up
                        plusPowerUpExpiration = 4000;
                        player.ModifyScale(2);
                    }
                    else if (s.collisionCueName == "skullcollision")
                    {
                        // Collided with skull - start skull power-up
                        skullPowerUpExpiration = 2000;
                        player.ModifySpeed(0);
                    }
                    else if (s.collisionCueName == "boltcollision")
                    {
                        // Collided with bolt - start bolt power-up
                        boltPowerUpExpiration = 8000;
                        player.ModifySpeed(2);
                    }



                    // Remove collided sprite from the game
                    spriteList.RemoveAt(i);
                    --i;
                }
                // Remove object if it is out of bounds
                if (s.IsOutOfBounds(Game.Window.ClientBounds))
                {
                    ((Game1)Game).AddScore(spriteList[i].scoreValue);
                    spriteList.RemoveAt(i);
                    --i;
                }
            }

            foreach (Sprite sprite in livesList)
                sprite.Update(gameTime, Game.Window.ClientBounds);
        }

        public Vector2 GetPlayerPosition()
        {
            return player.GetPosition;
        }



        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        public override void Update(GameTime gameTime)
        {
            // Time to spawn enemy?
            nextSpawnTime -= gameTime.ElapsedGameTime.Milliseconds;
            if (nextSpawnTime < 0)
            {
                SpawnEnemy( );
                // Reset spawn timer
                ResetSpawnTime( );
            }
            UpdateSprites(gameTime);

            AdjustSpawnTimes(gameTime);

            CheckPowerUpExpiration(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);
            //spriteBatch.DrawString(textfont, " ", _position, Color.White);
                // Draw the player
            player.Draw(gameTime, spriteBatch);
                // Draw all sprites
            foreach (Sprite s in spriteList)
                s.Draw(gameTime, spriteBatch);
            foreach (Sprite sprite in livesList)
                sprite.Draw(gameTime, spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
        
    }
}
