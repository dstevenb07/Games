using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Rocket_Game
{
    public class Player
    {
        Texture2D texture;
        public Vector2 velocity = Vector2.Zero;
        float friction = 0.05f;
        public float rotation;
        public Rectangle bounds;
        public Vector2 position;
        float originalVelocity = 5f;
        Vector2 origin;
        Color colour;
        Camera camera;

        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        //Cue trackCue;

        // Animation variables
        int totalFrames = 4;
        Vector2 currentFrameFlying = Vector2.Zero;
        Vector2 currentFrameStill = new Vector2(1, 0);
        Vector2 frameSize = new Vector2(78, 48);
        Rectangle sheetPos;

        // Bullet vars
        public List<Bullets> bullets = new List<Bullets>();

        public Player(Texture2D texture, Vector2 position, Color colour)
        {
            this.texture = texture;
            this.position = position;
            this.colour = colour;
        }

        public void LoadContent(ContentManager Content, Viewport viewport)
        {
            camera = new Camera(viewport);
            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
        }
        
        public void Update(GameTime gameTime, Keys up, Keys left, Keys right, Player player)
        {
            bounds = new Rectangle((int)position.X - ((int)frameSize.X / 2),
                (int)position.Y - ((int)frameSize.Y / 2), (int)frameSize.X, (int)frameSize.Y);
            origin = new Vector2((frameSize.X / 2), (frameSize.Y / 2));
            position += velocity;
            if (velocity == Vector2.Zero)
                AnimateStill(gameTime);

            camera.Update(gameTime, player);
            if (position.Y < 50)
                position.Y = 50;
            if (position.Y > 480 - 50)
                position.Y = 480 - 50;
            if (position.X < camera.centre.X + 50)
                position.X = camera.centre.X + 50;
            if (position.X > camera.centre.X + 750)
                position.X = camera.centre.X + 750;

            if (Keyboard.GetState().IsKeyDown(left))
                rotation -= 0.15f;
            if (Keyboard.GetState().IsKeyDown(right))
                rotation += 0.15f;

            if (Keyboard.GetState().IsKeyDown(up))
            {
                velocity.X = (float)Math.Cos(rotation) * originalVelocity;
                velocity.Y = (float)Math.Sin(rotation) * originalVelocity;
                AnimateFlying(gameTime);
            }
            else if (velocity != Vector2.Zero)
            {
                Vector2 i = velocity;
                velocity = i -= friction * i;
                AnimateStill(gameTime);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sheetPos, colour, rotation, origin, 1f, SpriteEffects.None, 0);
        }

        private void AnimateStill(GameTime gameTime)
        {
            sheetPos = new Rectangle(
                (int)currentFrameStill.X * (int)frameSize.X,
                (int)currentFrameStill.Y * (int)frameSize.Y,
                (int)frameSize.X, (int)frameSize.Y);
            
            if(gameTime.TotalGameTime.Milliseconds % 50 == 0)
            currentFrameStill.Y += 1;

            if (currentFrameStill.Y >= totalFrames)
                currentFrameStill.Y = 0;
            
        }
        private void AnimateFlying(GameTime gameTime)
        {
            sheetPos = new Rectangle(
                (int)currentFrameFlying.X * (int)frameSize.X,
                (int)currentFrameFlying.Y * (int)frameSize.Y,
                (int)frameSize.X, (int)frameSize.Y);

            if (gameTime.TotalGameTime.Milliseconds % 50 == 0)
                currentFrameFlying.Y += 1;

            if (currentFrameFlying.Y >= 4)
                currentFrameFlying.Y = 0;
        }

        public void shoot(Keys shoot, ContentManager Content, GameTime gameTime, Texture2D texture, Color colour)
        {
            if (Keyboard.GetState().IsKeyDown(shoot) && gameTime.TotalGameTime.Milliseconds % 150 == 0)
            {
                soundBank.PlayCue("Shooting noise");
                bullets.Add(new Bullets(texture, position, velocity, rotation, colour));
            }
        }
    }
}
