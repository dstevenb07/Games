using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rocket_Game
{
    class HealthBar
    {
        Texture2D textureBar;
        Texture2D textureOutline;
        public int health = 100;
        Vector2 barPos;
        Vector2 outlinePos;
        Camera camera;
        Color colour;
        int Xpos;

        public HealthBar(int Xpos, Color colour)
        {
            this.Xpos = Xpos;
            this.colour = colour;
        }
        
        public void LoadContent(ContentManager Content, Viewport viewport)
        {
            textureBar = Content.Load<Texture2D>("Health/Health bar 1");
            textureOutline = Content.Load<Texture2D>("Health/Health bar 1 outline");
            //barPos = new Vector2(viewport.Bounds.Left + 30, viewport.Bounds.Top + 30);
            camera = new Camera(viewport);
            //outlinePos = new Vector2(camera.centre.X - 400 + 26, camera.centre.Y + 26);
        }

        public void Update(GameTime gameTime, Player player)
        {
            if (health < 0)
                health = 0;
            camera.Update(gameTime, player);
            barPos = new Vector2(camera.centre.X + Xpos, camera.centre.Y + 30);
            outlinePos = new Vector2(camera.centre.X + Xpos - 4, camera.centre.Y + 26);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureBar, barPos, new Rectangle(0, 0, health * 2, textureBar.Height), colour);
            spriteBatch.Draw(textureOutline, outlinePos, Color.Wheat);
        }
    }
}
