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
    public class LevelData
    {
        public static string Data
        {
            get
            {
                return
                    "....................\n" +
                    ".000000000000000000.\n" +
                    ".0.0...0.....0.0..0.\n" +
                    ".0.0...0....00.0..0.\n" +
                    ".0.0..0000000..0..0.\n" +
                    "0000000.....0..0..0.\n" +
                    ".0....00000000000000\n" +
                    ".0000.0..0..0..0..0.\n" +
                    ".0..0.0..0..0..0..0.\n" +
                    ".0..0.0..0..0..0..0.\n" +
                    ".000000000000000000.\n" +
                    "....................";
            }
        }
    }
}
