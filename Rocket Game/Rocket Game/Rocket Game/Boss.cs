using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Rocket_Game
{
    class Boss
    {

        public List<BossBullets> bossBullets = new List<BossBullets>();

        protected Texture2D bossTexture;
        protected Texture2D bulletTexture;
        protected Vector2 bossPosition = new Vector2(10000, 0);
        protected Vector2 bossVelocity = new Vector2(0, 5);
        public int health = 100;
        public Rectangle bossBounds;
        public bool isAlive = true;

        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;

        public void LoadContent(ContentManager Content)
        {
            bossTexture = Content.Load<Texture2D>(@"Sprites/squirrel");
            bulletTexture = Content.Load<Texture2D>(@"Sprites/explosion");
            audioEngine = new AudioEngine(@"Content\Audio\GameAudio.xgs");
            waveBank = new WaveBank(audioEngine, @"Content\Audio\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Audio\Sound Bank.xsb");
        }

        public void Update(GameTime gameTime, Player player1, Player player2, HealthBar health1, HealthBar health2, Vector2 explosionPos, int score)
        {
            bossBounds = new Rectangle((int)bossPosition.X, (int)bossPosition.Y, bossTexture.Width, bossTexture.Height);

            bossPosition += bossVelocity;

            if (bossPosition.Y <= 0 || bossPosition.Y + bossTexture.Height >= 480)
                bossVelocity *= -1;

            for (int i = 0; i < player1.bullets.Count; i++)
                if (player1.bullets[i].bounds.Intersects(bossBounds) && isAlive)
                {
                    health -= 1;
                    player1.bullets.RemoveAt(i);
                    i--;
                }
            for (int i = 0; i < player2.bullets.Count; i++)
                if (player2.bullets[i].bounds.Intersects(bossBounds) && isAlive)
                {
                    health -= 1;
                    player2.bullets.RemoveAt(i);
                    i--;
                }

            if (isAlive && player1.position.X > 10000)
                player1.position.X = 10000;
            if (isAlive && player2.position.X > 10000)
                player2.position.X = 10000;

            if (gameTime.TotalGameTime.Milliseconds % 500 == 0 && player1.position.X >= 9000 && isAlive)
                bossBullets.Add(new BossBullets(bossPosition + new Vector2(20, 60), bulletTexture));
            //else if (gameTime.TotalGameTime.Milliseconds % 300 == 0 && player1.position.X >= 9000 && isAlive && health <= 25)
            //    bossBullets.Add(new BossBullets(bossPosition + new Vector2(20, 60), bulletTexture));

            if (health == 0)
            {
                soundBank.PlayCue("boss kill");
                health -= 1;
            }

            if (health <= 0)
            {
                score += 20;
                explosionPos = bossPosition + new Vector2(50, 100);
                isAlive = false;
            }
            else
                isAlive = true;

            for (int i = 0; i < bossBullets.Count; i++)
                if (bossBullets[i].bulletBounds.Intersects(player1.bounds))
                {
                    soundBank.PlayCue("get hit");
                    health1.health -= 8;
                    explosionPos = bossBullets[i].bulletPos;
                    bossBullets.RemoveAt(i);
                    i--;
                }
            for (int i = 0; i < bossBullets.Count; i++)
                if (bossBullets[i].bulletBounds.Intersects(player2.bounds))
                {
                    soundBank.PlayCue("get hit");
                    health2.health -= 8;
                    explosionPos = bossBullets[i].bulletPos;
                    bossBullets.RemoveAt(i);
                    i--;
                }

            if (player1.bounds.Intersects(bossBounds) && isAlive)
                health1.health -= 20;
            if (player2.bounds.Intersects(bossBounds) && isAlive)
                health2.health -= 20;

            for (int i = 0; i < bossBullets.Count; i++)
            {
                if (bossBullets[i].bulletPos.X < 8500)
                {
                    bossBullets.RemoveAt(i);
                    i--;
                }

            }

            for (int i = 0; i < player1.bullets.Count; i++)
                for (int j = 0; j < bossBullets.Count; j++)
                    if (player1.bullets[i].bounds.Intersects(bossBullets[j].bulletBounds))
                    {
                        //player1.bullets.RemoveAt(i);
                        bossBullets.RemoveAt(j);
                        j--;
                        //if (i > 0)
                        //    i--;
                    }

            for (int i = 0; i < player2.bullets.Count; i++)
                for (int j = 0; j < bossBullets.Count; j++)
                    if (player2.bullets[i].bounds.Intersects(bossBullets[j].bulletBounds))
                    {
                        //player2.bullets.RemoveAt(i);
                        bossBullets.RemoveAt(j);
                        //if (i > 0)
                        //    i--;
                        j--;
                    }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isAlive)
                spriteBatch.Draw(bossTexture, bossPosition, Color.White);
        }

    }
}
