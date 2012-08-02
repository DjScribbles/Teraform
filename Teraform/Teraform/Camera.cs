using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Teraform.Camera
{
    public class Camera2D
    {
        private Vector2 _position;
        private float _rotationRadians;
        private Viewport _viewport;
        private Matrix _viewMatrix;

        public Camera2D(Viewport viewport)
        {
            _viewport = viewport;
            _viewMatrix = Matrix.Identity;
            _rotationRadians = 0.0f;
            _position = Vector2.Zero;
        }

        public Vector2 Position
        {
            set { _position = value; UpdateCamera(); }
            get { return _position; }
        }

        public Point Center
        {
            set
            {
                //_position.X = value.X - (_viewport.Width / 2);
                //_position.Y = value.Y - (_viewport.Height / 2);'
                _position.X = value.X;
                _position.Y = value.Y;
                UpdateCamera();
            }
            get
            {
                return new Point((int)_position.X + (_viewport.Width / 2), (int)_position.Y + (_viewport.Height / 2));
            }
        }
        public int Top
        {
            get { return (int)_position.Y - (_viewport.Height / 2); }
        }
        public int Bottom
        {
            get { return (int)_position.Y + (_viewport.Height / 2); }
        }
        public int Left
        {
            get { return (int)_position.X - (_viewport.Width / 2); }
        }
        public int Right
        {
            get { return (int)_position.X + (_viewport.Width / 2); }
        }

        public float Rotate
        {
            set { _rotationRadians = value; UpdateCamera(); }
            get { return _rotationRadians; }
        }

        public Matrix ViewMatrix
        {
            get { return _viewMatrix; }
        }

        private void UpdateCamera()
        {

            _viewMatrix = Matrix.CreateRotationZ(_rotationRadians);
            _viewMatrix *= Matrix.CreateTranslation(-1 * _position.X,-1 * _position.Y, 0);

            Matrix projection = Matrix.CreateTranslation(new Vector3(_viewport.Width * 0.5f, _viewport.Height * 0.5f, 0)) *
                             Matrix.CreateScale(new Vector3(1f, 1f, 1f));
            _viewMatrix = projection * _viewMatrix;
        }
    }
}
