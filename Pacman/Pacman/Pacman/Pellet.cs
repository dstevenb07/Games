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
using System.Text;

namespace Pacman
{
    class Pellet
    {
        Texture2D texture;
        public List<Pellet> pellets = new List<Pellet>();
        Vector2 position;
        Level level;

        public Pellet(Vector2 position) 
        {
            this.position = position;
        }
        
        public void LoadContent(ContentManager Content) 
        {
            texture = Content.Load<Texture2D>(@"Coins/pellet");
            level = new Level();
        }

        public void Update(GameTime gameTime) 
        {   
            //hit detection here
        }

        public void Draw(SpriteBatch spritebatch) 
        {
            for (int i = 0; i < level.tiles.Count; i++)
            {
                var xPos = i % level.columnCount;
                var yPos = i / level.columnCount;
                
                if (level.tiles[i].type == TileType.Passable)
                    pellets.Add(new Pellet(new Vector2((xPos * 40) + 20 - texture.Height, (yPos * 40) + 20 - texture.Height)));

                
            }
            for (int i = 0; i < pellets.Count; i++ )
                spritebatch.Draw(texture,pellets[i].position, Color.White);
        }
    }
}
