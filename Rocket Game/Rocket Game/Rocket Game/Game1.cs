using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Audio;

namespace Rocket_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        Cue trackCue;

        List<Enemy> enemies = new List<Enemy>();
        List<FireBalls> fireBalls = new List<FireBalls>();

        Texture2D explosion;
        Texture2D loseScreen;
        Texture2D pauseScreen;
        Texture2D winScreen;
        SpriteFont scoreFont;
        Texture2D againButton;
        Texture2D menuButton;
        Texture2D underline;

        Vector2 singleUnderline = new Vector2(260, 188);
        Vector2 coopUnderline = new Vector2(260, 280);
        Vector2 creditUnderline = new Vector2(260, 275);
        Vector2 exitUnderline = new Vector2(260, 459);
        Vector2 underlinePos = Vector2.Zero;

        Texture2D menuScreen;
        Rectangle singleBounds = new Rectangle(260, 125, 243, 59);
        Rectangle coopBounds = new Rectangle(290, 210, 199, 76);
        Rectangle creditBounds = new Rectangle(274, 297, 230, 83);
        Rectangle exitBounds = new Rectangle(252, 400, 276, 61);

        Color menuBColour = new Color(255, 255, 255, 255);
        Color againBColour = new Color(255, 255, 255, 255);

        Rectangle menuBounds;
        Rectangle againBounds;
        Rectangle mouseRect;

        Background background;
        Player player1;
        Player player2;
        HealthBar health1;
        HealthBar health2;
        Camera camera;
        Boss boss;

        Random random = new Random();

        float spawnTimer = 0;
        float shootTimer = 0;

        int score = 0;

        enum GameState
        {
            Menu,
            OnePlayer,
            TwoPlayer,
            Win,
            Lose
        }
        GameState currentState = GameState.Menu;
        GameState prevState;
        bool isPaused = false;

        // Animation vars
        int totalFrames = 8;
        Vector2 currentFrame = Vector2.Zero;
        Vector2 frameSize = new Vector2(65, 64);
        Rectangle sheetPos;
        Vector2 explosionPos;

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
            player1 = new Player(Content.Load<Texture2D>(@"Sprites/rocketFlames half size"), new Vector2(8000, 150), Color.White);
            player2 = new Player(Content.Load<Texture2D>(@"Sprites/rocketFlames half size"), new Vector2(0, 350), Color.MediumPurple);
            health1 = new HealthBar(30, Color.Red);
            health2 = new HealthBar(570, Color.Purple);
            camera = new Camera(GraphicsDevice.Viewport);
            boss = new Boss();

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
            player1.LoadContent(Content, GraphicsDevice.Viewport);
            player2.LoadContent(Content, GraphicsDevice.Viewport);
            boss.LoadContent(Content);

            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");

            explosion = Content.Load<Texture2D>(@"Sprites/fireburst");
            loseScreen = Content.Load<Texture2D>(@"Menu/meanGameOver");
            pauseScreen = Content.Load<Texture2D>(@"Menu/pauseScreenCompleted");
            scoreFont = Content.Load<SpriteFont>(@"ScoreFont");
            winScreen = Content.Load<Texture2D>(@"Menu/a winner is you");
            againButton = Content.Load<Texture2D>(@"Menu/playAgain");
            menuButton = Content.Load<Texture2D>(@"Menu/Menu button");
            menuScreen = Content.Load<Texture2D>(@"Menu/MenuSpaceFinished");
            underline = Content.Load<Texture2D>(@"Menu/TrueLies underlines");

            trackCue = soundBank.GetCue("somecrack song");
            trackCue.Play();
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
            spawnTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            shootTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            MouseState mouse = Mouse.GetState();
            mouseRect = new Rectangle(mouse.X, mouse.Y, 1, 1);

            switch (currentState)
            {
                case GameState.Menu:
                    //currentState = GameState.Menu;
                    camera.Update(gameTime, player1);

                    singleUnderline = new Vector2(camera.centre.X + 260, 188);
                    coopUnderline = new Vector2(camera.centre.X + 260, 280);
                    creditUnderline = new Vector2(camera.centre.X + 260, 375);
                    exitUnderline = new Vector2(camera.centre.X + 260, 459);

                    //health1.health = 100;
                    //health2.health = 100;
                    //for (int i = 0; i < enemies.Count; i++)
                    //    enemies.RemoveAt(i);
                    //for (int i = 0; i < fireBalls.Count; i++)
                    //    fireBalls.RemoveAt(i);
                    //explosionPos = Vector2.Zero;
                    //score = 0;
                    //player1.position = new Vector2(0, 150);
                    //player2.position = new Vector2(0, 350);
                    //player1.rotation = 0;
                    //player2.rotation = 0;
                    //player1.velocity = new Vector2(0, 0);
                    //player2.velocity = new Vector2(0, 0);
                    //boss.health = 100;

                    if (mouseRect.Intersects(singleBounds) && mouse.LeftButton == ButtonState.Pressed)
                        currentState = GameState.OnePlayer;
                    else if (mouseRect.Intersects(singleBounds))
                        underlinePos = singleUnderline;
                    //else
                    //    underlinePos = Vector2.Zero;

                    else if (mouseRect.Intersects(coopBounds) && mouse.LeftButton == ButtonState.Pressed)
                        currentState = GameState.TwoPlayer;
                    else if (mouseRect.Intersects(coopBounds))
                        underlinePos = coopUnderline;
                    //else
                    //    underlinePos = Vector2.Zero;

                    else if (mouseRect.Intersects(creditBounds) && mouse.LeftButton == ButtonState.Pressed)
                    {
                        //currentState = GameState.Credits;
                    }
                    else if (mouseRect.Intersects(creditBounds))
                        underlinePos = creditUnderline;

                    else if (mouseRect.Intersects(exitBounds) && mouse.LeftButton == ButtonState.Pressed)
                        this.Exit();
                    else if (mouseRect.Intersects(exitBounds))
                        underlinePos = exitUnderline;
                    else
                        underlinePos = Vector2.Zero;

                    break;
                case GameState.OnePlayer:
                    if (isPaused == false)
                    {

                        background.Update(gameTime, player1);
                        player1.Update(gameTime, Keys.W, Keys.A, Keys.D, player1);
                        camera.Update(gameTime, player1);
                        health1.Update(gameTime, player1);
                        for (int i = 0; i < enemies.Count; i++)
                            enemies[i].Update(gameTime, player1);
                        if (player1.position.X <= 9000)
                            EnemySpawn(gameTime);

                        player1.shoot(Keys.Space, Content, gameTime, Content.Load<Texture2D>(@"Sprites/explosion"), Color.Red);
                        foreach (Bullets bullet in player1.bullets)
                            bullet.Update(gameTime, player1);

                        for (int i = 0; i < fireBalls.Count; i++)
                            fireBalls[i].Update(gameTime);

                        boss.Update(gameTime, player1, player2, health1, health2, explosionPos, score);

                        for (int i = 0; i < boss.bossBullets.Count; i++)
                            boss.bossBullets[i].UpdateBullets(gameTime, player1, null);

                        CollisionChecks(gameTime, player1, player2, enemies, fireBalls, health1, health2);
                        if (health1.health <= 0)
                        {
                            soundBank.PlayCue("when you die");
                            prevState = GameState.OnePlayer;
                            currentState = GameState.Lose;
                        }

                        FireballVisibleCheck(fireBalls, player1);
                        BulletVisibleCheck(player1);



                        if (player1.position.X >= 10500)
                        {
                            soundBank.PlayCue("when you win");
                            prevState = GameState.OnePlayer;
                            currentState = GameState.Win;
                        }


                        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        {
                            isPaused = true;
                        }
                    }

                    else
                    {
                        //camera.Update(gameTime, player1);
                        //player1.Update(gameTime, Keys.W, Keys.A, Keys.D, player1);
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            isPaused = false;
                        }
                        //menuBounds = new Rectangle((int)player1.position.X + 140, 300, menuButton.Width, menuButton.Height);
                        //if (mouseRect.Intersects(menuBounds) && mouse.LeftButton == ButtonState.Pressed)
                        //{
                        //    isPaused = false;
                        //    currentState = GameState.Menu;
                        //}
                    }
                    break;
                case GameState.TwoPlayer:
                    if (isPaused == false)
                    {
                        background.Update(gameTime, player1);
                        player1.Update(gameTime, Keys.W, Keys.A, Keys.D, player1);
                        camera.Update(gameTime, player1);
                        health1.Update(gameTime, player1);
                        health2.Update(gameTime, player1);
                        player2.Update(gameTime, Keys.Up, Keys.Left, Keys.Right, player1);
                        for (int i = 0; i < enemies.Count; i++)
                            enemies[i].Update(gameTime, player1);
                        if (player1.position.X <= 9000)
                            EnemySpawn(gameTime);

                        foreach (FireBalls fireBall in fireBalls)
                            fireBall.Update(gameTime);

                        player1.shoot(Keys.Space, Content, gameTime, Content.Load<Texture2D>(@"Sprites/explosion"), Color.Red);
                        for (int i = 0; i < player1.bullets.Count; i++)
                        {
                            player1.bullets[i].Update(gameTime, player1);
                        }

                        player2.shoot(Keys.Enter, Content, gameTime, Content.Load<Texture2D>(@"Sprites/explosion"), Color.White);
                        foreach (Bullets bullet in player2.bullets)
                            bullet.Update(gameTime, player2);

                        boss.Update(gameTime, player1, player2, health1, health2, explosionPos, score);
                        for (int i = 0; i < boss.bossBullets.Count; i++)
                            boss.bossBullets[i].UpdateBullets(gameTime, player1, player2);

                        CollisionChecks(gameTime, player1, player2, enemies, fireBalls, health1, health2);

                        if (health1.health <= 0 || health2.health <= 0)
                        {
                            soundBank.PlayCue("when you die");
                            prevState = GameState.TwoPlayer;
                            currentState = GameState.Lose;
                        }

                        FireballVisibleCheck(fireBalls, player1);
                        BulletVisibleCheck(player1);
                        BulletVisibleCheck(player2);


                        if (player1.position.X >= 10500 || player2.position.X >= 10500)
                        {
                            soundBank.PlayCue("when you win");
                            prevState = GameState.TwoPlayer;
                            currentState = GameState.Win;
                        }


                        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        {
                            isPaused = true;
                        }

                    }
                    else
                    {
                        //camera.Update(gameTime, player1);
                        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        {
                            isPaused = false;
                        }
                        //menuBounds = new Rectangle((int)camera.centre.X + 340, 300, menuButton.Width, menuButton.Height);
                        //if (mouseRect.Intersects(menuBounds) && mouse.LeftButton == ButtonState.Pressed)
                        //{
                        //    isPaused = false;
                        //    currentState = GameState.Menu;
                        //}
                    }
                    break;
                case GameState.Lose:
                    health1.health = 100;
                    health2.health = 100;
                    for (int i = 0; i < enemies.Count; i++)
                        enemies.RemoveAt(i);
                    for (int i = 0; i < fireBalls.Count; i++)
                        fireBalls.RemoveAt(i);
                    explosionPos = Vector2.Zero;
                    score = 0;
                    player1.position = new Vector2(0, 150);
                    player2.position = new Vector2(0, 350);
                    player1.rotation = 0;
                    player2.rotation = 0;
                    player1.velocity = new Vector2(0, 0);
                    player2.velocity = new Vector2(0, 0);
                    boss.health = 100;

                    againBounds = new Rectangle(50, 350, againButton.Width, againButton.Height);
                    menuBounds = new Rectangle(620, 350, menuButton.Width, menuButton.Height);
                    //mouseRect = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);

                    bool goingDown = false;

                    if (mouseRect.Intersects(againBounds))
                    {
                        if (againBColour.A == 255)
                            goingDown = false;
                        if (againBColour.A <= 50)
                            goingDown = true;
                        if (goingDown == true)
                            againBColour.A += 5;
                        else
                            againBColour.A -= 5;

                    }
                    else if (againBColour.A < 255)
                    {
                        againBColour.A += 5;
                    }


                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                        currentState = prevState;

                    if (mouseRect.Intersects(againBounds) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                        currentState = prevState;

                    if (mouseRect.Intersects(menuBounds) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                        currentState = GameState.Menu;

                    break;
                case GameState.Win:
                    health1.health = 100;
                    health2.health = 100;
                    for (int i = 0; i < enemies.Count; i++)
                        enemies.RemoveAt(i);
                    for (int i = 0; i < fireBalls.Count; i++)
                        fireBalls.RemoveAt(i);
                    explosionPos = Vector2.Zero;
                    player1.position = new Vector2(0, 150);
                    player2.position = new Vector2(0, 350);
                    player1.rotation = 0;
                    player2.rotation = 0;
                    player1.velocity = new Vector2(0, 0);
                    player2.velocity = new Vector2(0, 0);
                    boss.health = 100;

                    againBounds = new Rectangle(50, 50, againButton.Width, againButton.Height);
                    menuBounds = new Rectangle(600, 50, menuButton.Width, menuButton.Height);
                    mouseRect = new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, 1, 1);

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        score = 0;
                        currentState = prevState;
                    }

                    if (mouseRect.Intersects(againBounds) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        score = 0;
                        currentState = prevState;
                    }

                    if (mouseRect.Intersects(menuBounds) && Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        score = 0;
                        currentState = GameState.Menu;
                    }

                    break;
            }

            base.Update(gameTime);
        }

        protected void EnemySpawn(GameTime gameTime)
        {
            int randY = random.Next(0, 350);

            if (spawnTimer >= 0.4f)
            {
                if (enemies.Count < 6)
                    enemies.Add(new Enemy(Content.Load<Texture2D>(@"Sprites/redDragonFlying"), new Vector2(camera.centre.X + 800, randY)));
                spawnTimer = 0;
            }

            if (shootTimer >= 1000)
            {
                for (int i = 0; i < enemies.Count; i++)
                {
                    shootFire(gameTime, Content.Load<Texture2D>(@"Sprites/fire"), new Vector2(enemies[i].position.X, enemies[i].position.Y + 50));
                }
                shootTimer = 0;
            }

            for (int i = 0; i < enemies.Count; i++)
                if (enemies[i].position.X < player1.position.X - 600)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
        }

        public void shootFire(GameTime gameTime, Texture2D texture, Vector2 position)
        {
            //for (int i = 0; i < enemies.Count; i++)
            //{
            //if (gameTime.TotalGameTime.Milliseconds % 1000 == 0)
            fireBalls.Add(new FireBalls(texture, position, new Vector2(-10, 0)));
            //}
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
                    spriteBatch.Draw(menuScreen, new Vector2(camera.centre.X, 0), Color.White);
                    if (underlinePos != Vector2.Zero)
                        spriteBatch.Draw(underline, underlinePos, Color.White);
                    break;

                case GameState.OnePlayer:
                    background.Draw(spriteBatch);

                    foreach (Enemy enemy in enemies)
                        enemy.Draw(spriteBatch);

                    player1.Draw(spriteBatch);

                    boss.Draw(spriteBatch);

                    foreach (FireBalls fireBall in fireBalls)
                        fireBall.Draw(spriteBatch);

                    foreach (Bullets bullet in player1.bullets)
                        bullet.Draw(spriteBatch);

                    for (int i = 0; i < boss.bossBullets.Count; i++)
                        boss.bossBullets[i].DrawBullets(spriteBatch);

                    if (explosionPos != new Vector2(0, 0))
                        for (int j = 0; j <= totalFrames; j++)
                            for (int i = j; i == j; i++)
                            {
                                sheetPos = new Rectangle(i * (int)frameSize.X, 0, (int)frameSize.X, (int)frameSize.Y);
                                spriteBatch.Draw(explosion, explosionPos, sheetPos, Color.White);
                            }
                    if (gameTime.TotalGameTime.Milliseconds % 500 == 0)
                        explosionPos = Vector2.Zero;

                    health1.Draw(spriteBatch);

                    string scoreText = "" + score;
                    spriteBatch.DrawString(scoreFont, scoreText,
                        new Vector2((camera.centre.X + 400) - (scoreFont.MeasureString(scoreText).X / 2), 0), Color.White);

                    if (isPaused == true)
                    {
                        spriteBatch.Draw(pauseScreen, new Vector2(camera.centre.X, 0), Color.White);
                        //spriteBatch.Draw(menuButton, new Vector2(camera.centre.X + menuBounds.X, menuBounds.Y), Color.White);
                    }

                    break;

                case GameState.TwoPlayer:
                    background.Draw(spriteBatch);
                    foreach (Enemy enemy in enemies)
                        enemy.Draw(spriteBatch);
                    player2.Draw(spriteBatch);
                    player1.Draw(spriteBatch);

                    foreach (FireBalls fireBall in fireBalls)
                        fireBall.Draw(spriteBatch);

                    foreach (Bullets bullet in player1.bullets)
                        bullet.Draw(spriteBatch);

                    foreach (Bullets bullet in player2.bullets)
                        bullet.Draw(spriteBatch);
                    boss.Draw(spriteBatch);
                    for (int i = 0; i < boss.bossBullets.Count; i++)
                        boss.bossBullets[i].DrawBullets(spriteBatch);

                    if (explosionPos != new Vector2(0, 0))
                        for (int j = 0; j <= totalFrames; j++)
                            for (int i = j; i == j; i++)
                            {
                                sheetPos = new Rectangle(i * (int)frameSize.X, 0, (int)frameSize.X, (int)frameSize.Y);
                                spriteBatch.Draw(explosion, explosionPos, sheetPos, Color.White);
                            }
                    if (gameTime.TotalGameTime.Milliseconds % 500 == 0)
                        explosionPos = Vector2.Zero;

                    health1.Draw(spriteBatch);
                    health2.Draw(spriteBatch);

                    scoreText = "" + score;
                    spriteBatch.DrawString(scoreFont, scoreText,
                        new Vector2((camera.centre.X + 400) - (scoreFont.MeasureString(scoreText).X / 2), 0), Color.White);

                    if (isPaused == true)
                    {
                        spriteBatch.Draw(pauseScreen, new Vector2(camera.centre.X, 0), Color.White);
                        //spriteBatch.Draw(menuButton, new Vector2(camera.centre.X + menuBounds.X, menuBounds.Y), Color.White);
                    }

                    break;

                case GameState.Lose:
                    spriteBatch.Draw(loseScreen, new Vector2(camera.centre.X, 0), Color.White);
                    spriteBatch.Draw(againButton, new Vector2(camera.centre.X + againBounds.X, camera.centre.Y + againBounds.Y), againBColour);
                    spriteBatch.Draw(menuButton, new Vector2(camera.centre.X + menuBounds.X, camera.centre.Y + menuBounds.Y), menuBColour);
                    break;

                case GameState.Win:
                    spriteBatch.Draw(winScreen, new Vector2(camera.centre.X, 0), Color.White);
                    spriteBatch.Draw(againButton, new Vector2(camera.centre.X + againBounds.X, camera.centre.Y + againBounds.Y), againBColour);
                    spriteBatch.Draw(menuButton, new Vector2(camera.centre.X + menuBounds.X, camera.centre.Y + menuBounds.Y), menuBColour);
                    //score = (int)((float)gameTime.TotalGameTime.TotalSeconds / (float)score);
                    scoreText = "You scored " + score;
                    spriteBatch.DrawString(scoreFont, scoreText,
                        new Vector2(camera.centre.X + 120, camera.centre.Y + 150), Color.White);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void AnimateExplosion(GameTime gameTime)
        {


            //for (int i = 0; i <= totalFrames; i++ )
            if (gameTime.TotalGameTime.Milliseconds % 50 == 0)
            {
                //sheetPos = new Rectangle((int)currentFrame.X * (int)frameSize.X,
                //    (int)currentFrame.Y * (int)frameSize.Y,
                //    (int)frameSize.X, (int)frameSize.Y);
                currentFrame = new Vector2(0, 0);
                currentFrame.X++;
            }
        }

        private void CollisionChecks(GameTime gameTime, Player player1, Player player2, List<Enemy> enemies, List<FireBalls> fireBalls, HealthBar health1, HealthBar health2)
        {
            for (int i = 0; i < enemies.Count; i++)
                if (player1.bounds.Intersects(enemies[i].bounds))
                {
                    soundBank.PlayCue("get hit");
                    health1.health -= 20;
                    explosionPos = enemies[i].position + new Vector2(50, 50);
                    enemies.RemoveAt(i);
                    i--;
                }

            for (int i = 0; i < fireBalls.Count; i++)
                if (player1.bounds.Intersects(fireBalls[i].bounds))
                {
                    soundBank.PlayCue("get hit");
                    health1.health -= 10;
                    explosionPos = fireBalls[i].position;
                    fireBalls.RemoveAt(i);
                    i--;
                }

            for (int i = 0; i < player1.bullets.Count; i++)
                for (int j = 0; j < enemies.Count; j++)
                    if (player1.bullets[i].bounds.Intersects(enemies[j].bounds))
                    {
                        soundBank.PlayCue("dragon kill");
                        explosionPos = enemies[j].position;
                        enemies.RemoveAt(j);
                        score++;
                        j--;
                    }

            for (int i = 0; i < player1.bullets.Count; i++)
                for (int j = 0; j < fireBalls.Count; j++)
                    if (player1.bullets[i].bounds.Intersects(fireBalls[j].bounds))
                    {
                        explosionPos = fireBalls[j].position;
                        fireBalls.RemoveAt(j);
                        j--;
                    }

            // Player 2 stuff
            for (int i = 0; i < enemies.Count; i++)
                if (player2.bounds.Intersects(enemies[i].bounds))
                {
                    soundBank.PlayCue("get hit");
                    health2.health -= 20;
                    explosionPos = enemies[i].position + new Vector2(50, 50);
                    enemies.RemoveAt(i);
                    i--;
                }

            for (int i = 0; i < fireBalls.Count; i++)
                if (player2.bounds.Intersects(fireBalls[i].bounds))
                {
                    soundBank.PlayCue("get hit");
                    health2.health -= 10;
                    explosionPos = fireBalls[i].position;
                    fireBalls.RemoveAt(i);
                    i--;
                }

            for (int i = 0; i < player2.bullets.Count; i++)
                for (int j = 0; j < enemies.Count; j++)
                    if (player2.bullets[i].bounds.Intersects(enemies[j].bounds))
                    {
                        soundBank.PlayCue("dragon kill");
                        explosionPos = enemies[j].position;
                        enemies.RemoveAt(j);
                        score++;
                        j--;
                    }

            for (int i = 0; i < player2.bullets.Count; i++)
                for (int j = 0; j < fireBalls.Count; j++)
                    if (player2.bullets[i].bounds.Intersects(fireBalls[j].bounds))
                    {
                        explosionPos = fireBalls[j].position;
                        fireBalls.RemoveAt(j);
                        j--;
                    }
        }

        private void FireballVisibleCheck(List<FireBalls> fireBalls, Player player)
        {
            for (int i = 0; i < fireBalls.Count; i++)
                if (fireBalls[i].position.X < player.position.X - 600 ||
                    fireBalls[i].position.X > player.position.X + 600 ||
                    fireBalls[i].position.Y < player.position.Y - 400 ||
                    fireBalls[i].position.Y > player.position.Y + 400)
                {
                    fireBalls.RemoveAt(i);
                    i--;
                }
        }

        private void BulletVisibleCheck(Player player)
        {
            for (int i = 0; i < player.bullets.Count; i++)
                if (player.bullets[i].position.X < player.position.X - 800 ||
                    player.bullets[i].position.X > player.position.X + 800 ||
                    player.bullets[i].position.Y < player.position.Y - 500 ||
                    player.bullets[i].position.Y > player.position.Y + 500)
                {
                    player.bullets.RemoveAt(i);
                    i--;
                }
        }
    }
}
