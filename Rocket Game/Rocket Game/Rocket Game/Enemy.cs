using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Rocket_Game
{
    class Enemy
    {
        Texture2D texture;
        public Vector2 position;
        Vector2 velocity;
        public Rectangle bounds;
        public bool isVisible = true;

        //List<Bullets> fireBalls = new List<Bullets>();
        
        Random random = new Random();
        int randXVelo;

        // Animation variables
        int prevFrame = 0;
        Vector2 currentFrame = Vector2.Zero;
        Vector2 frameSize = new Vector2(183, 142);
        Rectangle sheetPos;

        public Enemy(Texture2D texture, Vector2 position) 
        {
            this.texture = texture;
            this.position = position;
            
            randXVelo = random.Next(-9, -6);
            velocity = new Vector2(randXVelo, 0);
        }
        
        
        public void LoadContent(Viewport viewport, Player player)
        {
        }

        public void Update(GameTime gameTime, Player player)
        {
            Animate(gameTime);
            position += velocity;

            //player.Update(gameTime, Keys.W, Keys.A, Keys.D, player);

            //if (position.X < player.position.X - 600)
            //{
            //    isVisible = false;
            //}
            
            bounds = new Rectangle((int)position.X + 50, (int)position.Y + 30, (int)frameSize.X -50, (int)frameSize.Y -30);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
                spriteBatch.Draw(texture, position, sheetPos, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.FlipHorizontally, 0);
        }

        private void Animate(GameTime gameTime) 
        {
            sheetPos = new Rectangle((int)currentFrame.X * (int)frameSize.X,
                                       (int)currentFrame.Y * (int)frameSize.Y,
                                        (int)frameSize.X, (int)frameSize.Y);
            if (gameTime.TotalGameTime.Milliseconds % 100 == 0 && currentFrame.X == 0)
            {
                currentFrame.X += 1;
                prevFrame = 0;
            }
            else if (gameTime.TotalGameTime.Milliseconds % 100 == 0 && currentFrame.X == 1 && prevFrame == 0)
                currentFrame.X += 1;
            else if (gameTime.TotalGameTime.Milliseconds % 100 == 0 && currentFrame.X == 1 && prevFrame == 2)
                currentFrame.X -= 1;
            else if (gameTime.TotalGameTime.Milliseconds % 100 == 0 && currentFrame.X == 2)
            {
                currentFrame.X -= 1;
                prevFrame = 2;
            }
        }

    }
}
