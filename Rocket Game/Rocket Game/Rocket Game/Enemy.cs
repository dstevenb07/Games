using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rocket_Game
{
    class Enemy
    {
        Texture2D texture;
        Vector2 position = new Vector2(1000, 175);
        Vector2 velocity = new Vector2(-3, 0);
        Rectangle bounds;
        Camera camera;
        bool toDraw = true;

        // Animation variables
        int prevFrame = 0;
        Vector2 currentFrame = Vector2.Zero;
        Vector2 frameSize = new Vector2(183, 142);
        Rectangle sheetPos;

        public void LoadContent(ContentManager Content, Viewport viewport)
        {
            texture = Content.Load<Texture2D>(@"Sprites/redDragonFlying");
            camera = new Camera(viewport);
        }

        public void Update(GameTime gameTime, Player player)
        {
            camera.Update(gameTime, player);
            if (position.X > camera.centre.X - 200 && toDraw == true)
            {
                position += velocity;
            }
            bounds = new Rectangle((int)position.X, (int)position.Y, (int)frameSize.X, (int)frameSize.Y);
            Animate(gameTime);
            if (position.X < camera.centre.X - 200)
                toDraw = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (position.X > camera.centre.X - 200 && toDraw == true)
                spriteBatch.Draw(texture, position, sheetPos, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
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
