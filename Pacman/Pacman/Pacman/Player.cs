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
    public class Player
    {
        public enum Direction
        {
            Right,
            Down,
            Left,
            Up
        };
        //Add next position/direction and impassable code
        Texture2D texture0;
        Texture2D texture1;
        Texture2D texture;
        public Direction direction;
        public Direction nextDirection;
        //bool alive = true;
        float rotation;
        int speed = 2;
        Vector2 position = new Vector2(40, 40);
        Vector2 drawingPosition;
        Level level;
        Direction prevDirection;
        public List<Rectangle> bounds = new List<Rectangle>();
        Rectangle nextDirectBounds
        {
            get { return new Rectangle((int)position.X + (int)MovementVector(direction).X + 10,
                (int)position.Y + (int)MovementVector(direction).Y + 10,
                texture.Width - 10, texture.Height - 10); }
        } 



        public void LoadContent(ContentManager Content)
        {
            texture0 = Content.Load<Texture2D>(@"Sprites/pacman0");
            texture1 = Content.Load<Texture2D>(@"Sprites/pacman1");
            startSprite();
            level = new Level();
            direction = Direction.Right;

            //for (int i = 0; i < level.tiles.Count; i++)
            //{
            //    var xPos = i % level.columnCount;
            //    var yPos = i / level.columnCount;

            //    if (level.tiles[i].type == TileType.Impassable)
            //        bounds.Add(new Rectangle(xPos * 40, yPos * 40, 40, 40));
            //    else
            //        i++;
            //}
        }

        public void Update(GameTime gameTime)
        {
            PacmanAnimate(gameTime);
            position += MovementVector(direction);

            //direction = GetNextDirection();

            nextDirection = GetNextDirection();
            if (nextDirection != prevDirection)
            {
                prevDirection = nextDirection;
            }

            for (int i = 0; i < bounds.Count; i++)
            {
                if (!NextBounds.Intersects(bounds[i]))
                {
                    direction = nextDirection;
                    speed = 2;
                }
                else if (nextDirectBounds.Intersects(bounds[i]))
                    speed = 0;
            }

            rotation = GetRotation(direction);
            if (rotation == MathHelper.TwoPi)
                rotation = 0;


            if (position.X < -20)
            {
                position.X = 820;
                position.Y += 40;
            }
            if (position.X > 820)
            {
                position.X = -20;
                position.Y -= 40;
            }

            //for (int i = 0; i < level.bounds.Count; i++)
            //    if (Bounds.Intersects(level.bounds[i]))
            //        speed = 10;

            //if (Bounds.Intersects(level.bounds[1]))
            //    speed = 10;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            drawingPosition = position + new Vector2(texture.Width / 2, texture.Height / 2);
            spriteBatch.Draw(texture, drawingPosition, null, Color.White,
                rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0);

        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)position.X + 10, (int)position.Y + 10, texture.Width - 10, texture.Height - 10); }
        }

        public Rectangle NextBounds
        {
            get
            {
                return new Rectangle((int)position.X + (int)MovementVector(nextDirection).X + 10,
                    (int)position.Y + (int)MovementVector(nextDirection).Y + 10, texture.Width - 10, texture.Height - 10);
            }
        }

        private void PacmanAnimate(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.Milliseconds % 250 == 0)
                if (texture == texture1)
                    texture = texture0;
                else texture = texture1;
        }

        public void startSprite()
        {
            texture = texture0;
            direction = Direction.Right;
        }

        float GetRotation(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up: return 3 * MathHelper.PiOver2;
                case Direction.Down: return MathHelper.PiOver2;
                case Direction.Left: return 2 * MathHelper.PiOver2;
                case Direction.Right: return 0;
                default: return 0;
            }
        }
        private Vector2 MovementVector(Direction direction)
        {
            Vector2 movement = new Vector2(0, 0);

            switch (direction)
            {
                case Direction.Right:
                    movement = new Vector2(speed, 0);
                    break;
                case Direction.Left:
                    movement = new Vector2(-speed, 0);
                    break;
                case Direction.Down:
                    movement = new Vector2(0, speed);
                    break;
                case Direction.Up:
                    movement = new Vector2(0, -speed);
                    break;
            }

            return movement;
        }

        Direction GetNextDirection()
        {
            KeyboardState kState = Keyboard.GetState();

            if (kState.IsKeyDown(Keys.Right))
                nextDirection = Direction.Right;
            else if (kState.IsKeyDown(Keys.Up))
                nextDirection = Direction.Up;
            else if (kState.IsKeyDown(Keys.Down))
                nextDirection = Direction.Down;
            else if (kState.IsKeyDown(Keys.Left))
                nextDirection = Direction.Left;
            else nextDirection = prevDirection;

            return nextDirection;
        }

        public List<Rectangle> BoundInitiate()
        {
            for (int i = 0; i < level.tiles.Count; i++)
            {
                var xPos = i % level.columnCount;
                var yPos = i / level.columnCount;

                if (level.tiles[i].type == TileType.Impassable)
                    bounds.Add(new Rectangle(xPos * texture.Width, yPos * texture.Height, texture.Width, texture.Height));
            }
            return bounds;
        }
    }
}
