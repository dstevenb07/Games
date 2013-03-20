using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pacman
{
    class Ghost
    {
        Texture2D texture;
        Vector2 position;
        Color colour;
        private Texture2D texture0;
        private Texture2D texture1;

        public Ghost(Color colour, Vector2 position) 
        {
            this.colour = colour;
            this.position = position;
        }

        public void LoadContent(ContentManager Content)
        {
            texture0 = Content.Load<Texture2D>(@"Sprites/ghost0");
            texture1 = Content.Load<Texture2D>(@"Sprites/ghost1");
            texture = texture0;
        }

        public void Update(GameTime gameTime)
        {
            GhostAnimate(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(texture, position, colour);
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X + 10, (int)position.Y + 10, texture.Width - 10, texture.Height - 10); }
        }

        private void GhostAnimate(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.Milliseconds % 250 == 0)
                if (texture == texture0)
                    texture = texture1;
                else texture = texture0;
        }

    }
}
