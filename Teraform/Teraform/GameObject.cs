using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using MyCollisionGrid;

namespace BoundingBoxCollision
{
    public class GameObject
    {
        Texture2D _texture;
        private Vector2 _positionCarryOver;
        private Rectangle _boundingBox;
        private Vector2 _velocity;
        private Vector2 _accelleration;
        private Vector2 _maxVelocity = new Vector2(500,500);
        private float _gravitationalAccelleration = 300;

        public Rectangle BoundingBox
        {
            get
            {
                return _boundingBox;
            }
        }
        public Vector2 Accelleration
        {
            set
            {
                _accelleration.X = value.X;
                _accelleration.Y = value.Y;
            }
            get
            {
                return _accelleration;
            }
        }
        public Vector2 Velocity
        {
            set
            {
                _velocity = value;
            }
            get
            {
                return _velocity;
            }
        }
        public Vector2 MaxVelocity
        {
            set
            {
                _maxVelocity = value;
            }
            get
            {
                return _maxVelocity;
            }
        }

        public float RunSpeed
        {
            set
            {
                _maxVelocity.X = value;
            }
            get{ return _maxVelocity.X;}
        }

        private Vector2 Position
        {
            set
            {
                _boundingBox.X = (int)value.X;
                _boundingBox.Y = (int)value.Y;
            }
            get
            {
                return new Vector2(_boundingBox.X, _boundingBox.Y);
            }

        }

        public GameObject(Texture2D texture, Vector2 position)
        {
            _texture = texture;

            _boundingBox = new Rectangle(
                 (int)position.X,
                 (int)position.Y,
                 _texture.Width,
                 _texture.Height);

            _positionCarryOver = Vector2.Zero;
            _velocity = Vector2.Zero;
            _accelleration = Vector2.Zero;
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _boundingBox, Color.White);
        }

        public void Update(double total_seconds_elapsed, CollisionGrid grid)
        {
            if (total_seconds_elapsed == 0.0)
                return;
            Vector2 new_velocity = _velocity;
            new_velocity += (_accelleration * (float)total_seconds_elapsed);
            new_velocity.Y += _gravitationalAccelleration * (float)total_seconds_elapsed;
            new_velocity = Vector2.Clamp(new_velocity, Vector2.Negate(_maxVelocity), _maxVelocity);

            Vector2 distance_to_travel = (new_velocity * (float)total_seconds_elapsed) + _positionCarryOver; 
            Vector2 distance_traveled = grid.CheckCollision(BoundingBox, distance_to_travel);

            //Check to see if we moved as expected, if not we hit a block, so set the new velocity to 0.
            if (distance_traveled.X != distance_to_travel.X)
                new_velocity.X = 0;
            if (distance_traveled.Y != distance_to_travel.Y)
                new_velocity.Y = 0;

            Velocity = new_velocity;


            AddToPositionAndCarry(distance_traveled);
        }

        private void AddToPositionAndCarry(Vector2 distance_traveled)
        {
            _boundingBox.X += (int)Math.Truncate((double)distance_traveled.X);
            _positionCarryOver.X = distance_traveled.X - (float)Math.Truncate((double)distance_traveled.X);
            
            _boundingBox.Y += (int)Math.Truncate((double)distance_traveled.Y);
            _positionCarryOver.Y = distance_traveled.Y - (float)Math.Truncate((double)distance_traveled.Y);
        }
    }
}

