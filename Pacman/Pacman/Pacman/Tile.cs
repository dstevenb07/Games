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
        public struct Tile
        {
            public Texture2D texture;
            public TileType type;

            public const int Width = 40;
            public const int Height = 40;

            public static readonly Vector2 Size = new Vector2(Width, Height);

            public Tile(Texture2D texture, TileType type)
            {
                this.texture = texture;
                this.type = type;
            }
        }  
    public enum TileType
    {
        Passable,
        Impassable
    }
}
