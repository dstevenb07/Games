using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rocket_Game
{
    class Camera
    {
        public Matrix transform;
        Viewport viewport;
        public Vector2 centre;

        public Camera(Viewport viewport)
        {
            this.viewport = viewport;
        }

        public void Update(GameTime gameTime, Player player) 
        {
            centre = new Vector2(player.position.X + (player.bounds.Width /2) - 200, 0);
            transform = Matrix.CreateScale(new Vector3(1, 1, 0)) * Matrix.CreateTranslation(new Vector3(-centre.X, -centre.Y, 0));
        }

    }
}
