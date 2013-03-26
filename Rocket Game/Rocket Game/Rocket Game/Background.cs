using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rocket_Game
{
    class Background
    {
        Texture2D texture1, texture2;
        Vector2 pos1 = Vector2.Zero;
        Vector2 pos2 = new Vector2(800, 0);
        Camera camera;

        public void LoadContent(ContentManager Content, Viewport viewport)
        {
            texture1 = Content.Load<Texture2D>(@"Backgrounds/emptySpace");
            texture2 = Content.Load<Texture2D>(@"Backgrounds/emptySpace");
            camera = new Camera(viewport);
        }

        public void Update(GameTime gameTime, Player player)
        {
            camera.Update(gameTime, player);
            if (pos1.X + texture1.Width <= camera.centre.X)
                pos1 = new Vector2(pos2.X + texture1.Width, 0);
            if (pos1.X >= camera.centre.X + 800)
                pos1 = new Vector2(pos2.X - texture1.Width, 0);
            if (pos2.X >= camera.centre.X + 800)
                pos2 = new Vector2(pos1.X - texture1.Width, 0);
            if (pos2.X + texture1.Width <= camera.centre.X)
                pos2 = new Vector2(pos1.X + texture1.Width, 0);

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture1, pos1, Color.White);
            spriteBatch.Draw(texture2, pos2, Color.White);
        }
    }
}
