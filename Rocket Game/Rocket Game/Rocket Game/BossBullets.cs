using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Rocket_Game
{
    class BossBullets : Boss
    {
        Texture2D bulletTextureInherited;
        public Vector2 bulletPos;
        Vector2 bulletVelo = new Vector2(-8, 0);
        public Rectangle bulletBounds;
        Random random = new Random();
        int randInt;
        bool playerChoice;

        public BossBullets(Vector2 position, Texture2D bulletTexture)
        {
            bulletTextureInherited = bulletTexture;
            bulletPos = position;
            
            randInt = random.Next(0, 2);
                    switch (randInt)
                    {
                        case 0:
                            playerChoice = false;
                            break;
                        case 1:
                            playerChoice = true;
                            break;
                    }
        }
        
        //public void LoadContentBullets(ContentManager Content)
        //{
        //    bulletPos = new Vector2(bossPosition.X + 20, bossPosition.Y + 56);
        //    bulletBounds = new Rectangle((int)bulletPos.X, (int)bulletPos.Y, bulletTexture.Width, bulletTexture.Height);

            
        //}

        public void UpdateBullets(GameTime gameTime, Player player1, Player player2)
        {
            bulletBounds = new Rectangle((int)bulletPos.X, (int)bulletPos.Y, bulletTextureInherited.Width, bulletTextureInherited.Height);
            
            if (player2 == null)
                playerChoice = true;

            bulletPos += bulletVelo;

            if (playerChoice)
            {
                if (bulletPos.Y < player1.position.Y)
                    bulletVelo.Y = 2;
                if (bulletPos.Y > player1.position.Y)
                    bulletVelo.Y = -2;
            }
            else if (!playerChoice)
            {
                if (bulletPos.Y < player2.position.Y)
                    bulletVelo.Y = 2;
                if (bulletPos.Y > player2.position.Y)
                    bulletVelo.Y = -2;
            }
        }

        public void DrawBullets(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(bulletTextureInherited, bulletPos, null, Color.DeepPink, 0, Vector2.Zero, 1, SpriteEffects.FlipHorizontally, 0);
        }

    }
}
