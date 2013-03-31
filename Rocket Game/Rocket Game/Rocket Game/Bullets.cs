using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Rocket_Game
{
    public class Bullets
    {
        Texture2D texture;
        Vector2 velocity;
        float rotation;
        public Vector2 position;
        public Rectangle bounds;
        Color colour;

        public Bullets(Texture2D texture, Vector2 position, Vector2 velocity, float rotation, Color colour)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
            this.rotation = rotation;
            this.colour = colour;
        }

        public void Update(GameTime gameTime, Player player)
        {
            bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            
            position += (new Vector2((float)Math.Cos(rotation) * 15, (float)Math.Sin(rotation) * 15) + velocity);

            //for (int i = 0; i < player.bullets.Count; i++)
            //    if (position.X > player.position.X + 800 || position.X < player.position.X - 800 ||
            //        position.Y > player.position.Y + 500 || position.Y < player.position.Y - 500)
            //        player.bullets.RemoveAt(i);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, colour, rotation, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

    }
}
