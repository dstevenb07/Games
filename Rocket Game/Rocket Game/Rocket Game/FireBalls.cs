using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Rocket_Game
{
    public class FireBalls
    {
        Texture2D texture;
        Vector2 velocity;
        public Vector2 position;
        public Rectangle bounds;

        public FireBalls(Texture2D texture, Vector2 position, Vector2 velocity)
        {
            this.texture = texture;
            this.position = position;
            this.velocity = velocity;
        }

        public void Update(GameTime gameTime)
        {
            bounds = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);

            position +=  velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }

    }
}
