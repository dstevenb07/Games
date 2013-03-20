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

namespace Pacman
{
    public class Level
    {
        public int columnCount;
        public List<Tile> tiles = new List<Tile>();
        int tileSize = 40;

        public void LoadLevel(string data, ContentManager Content) 
        {
            var tileRows = data.Split('\n');
            columnCount = tileRows[0].ToCharArray().Length;

            foreach (var tileRow in tileRows) 
            {
                foreach (var tileType in tileRow) 
                {
                    switch (tileType) 
                    {
                        case '0' :
                            tiles.Add(new Tile(Content.Load<Texture2D>(@"Tiles/Empty"), TileType.Passable));
                            break;
                        case '.' :
                            tiles.Add(new Tile(Content.Load<Texture2D>(@"Tiles/Wall"), TileType.Impassable));
                            //for (int i = 0; i < tiles.Count; i++)
                            //{
                            //    var xPos = i % columnCount;
                            //    var yPos = i / columnCount;
                            //    bounds.Add(new Rectangle(xPos * 40, yPos * 40, 40, 40));
                            //}
                            break;
                    }
                }
            }

            //for (int i = 0; i < tiles.Count; i++)
            //{
            //    var xPos = i % columnCount;
            //    var yPos = i / columnCount;

            //    if (tiles[i].type == TileType.Impassable)
            //        bounds.Add(new Rectangle(xPos * 40, yPos * 40, 40, 40));
            //    else
            //        i++;
            //}
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                var xPos = i % columnCount;
                var yPos = i / columnCount;

                spriteBatch.Draw(tiles[i].texture, new Vector2(xPos, yPos) * tileSize, Color.White);

                //if (tiles[i].type == TileType.Impassable)
                //    bounds.Add(new Rectangle (xPos, yPos, 40, 40));
                
                //if (tiles[i].type == TileType.Passable)
                //    spriteBatch.Draw(pellet,
                //        new Vector2(xPos * 40 + ((tiles[i].texture.Width / 2) - (pellet.Width / 2)),
                //            yPos * 40 + ((tiles[i].texture.Height / 2) - (pellet.Height / 2))), Color.White);
            }
        }
    }
}
