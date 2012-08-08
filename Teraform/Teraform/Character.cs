using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using BoundingBoxCollision;


namespace Teraform
{
    public class GameCharacter : GameObject
    {
        private float _desiredVelocity = 0;
        protected int _healthCurrent;
        protected int _healthMax;

        protected float _runVelocity = 350.0f;
        protected float _runAccelleration = 250.0f;
        protected float _runDeccelleration = -400.0f;
        protected float _jumpStrength = -410.0f;

        public GameCharacter(Texture2D texture, Vector2 position) : base(texture,position)
        {
        }

        public void Jump(bool keepJumping)
        {
            if (IsAirborn == false  && keepJumping == true)
            {
                _velocity.Y = _jumpStrength;
            }
            else if ((keepJumping == false) && (_velocity.Y < (_jumpStrength / 4)))
            {
                _velocity.Y = _jumpStrength / 4;
            }
        }

        public int FallThrough
        {
            get { return _fallThrough; }
            set { _fallThrough = value; }
        }

        /**
         * Sets the speed to go as a percentage of run speed (-1.0 to 1.0)
         */
        public void Run(float speed)
        {
            speed = MathHelper.Clamp(speed, -1.0f, 1.0f);

            _desiredVelocity = speed * _runVelocity;
        }

        public override void Update(double total_seconds_elapsed, CollisionGrid grid)
        {
            //do stuff before the update
            _maxVelocity.X = Math.Abs(_desiredVelocity);
            float current_velocity = Velocity.X;
            if (Math.Abs(current_velocity) < Math.Abs(_desiredVelocity))
            {
                _accelleration.X = _runAccelleration * Math.Sign(_desiredVelocity);
            }
            else if (Math.Abs(current_velocity) > Math.Abs(_desiredVelocity))
            {
                _accelleration.X = _runDeccelleration * Math.Sign(_desiredVelocity);
            }

            base.Update(total_seconds_elapsed, grid);
        }

    }
}
