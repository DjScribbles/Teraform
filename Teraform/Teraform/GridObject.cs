using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Teraform
{
    public class GridObject
    {
        private Point _gridPosition;
        private Vector2 _worldPosition;
        private bool _isActive;
        private Texture2D _texture;

        public GridObject(Texture2D texture, Point gridPosition, bool isActive = true)
        {
            GridPosition = gridPosition;
            _isActive = isActive;
            _texture = texture;

        }

        public GridObject(GridObject gridObject, Point gridPosition)
        {
            GridPosition = gridPosition;
            IsActive = gridObject.IsActive;
            _texture = gridObject.Texture;
        }
        public GridObject(GridObject gridObject)
        {
            GridPosition = gridObject.GridPosition;
            IsActive = gridObject.IsActive;
            _texture = gridObject.Texture;
        }
        public GridObject(Texture2D texture = null, int gridX = 0, int gridY = 0, bool isActive = true)
        {
            GridPosition = new Point(gridX, gridY);
            _isActive = isActive;
            _texture = texture;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _worldPosition, Color.White);
        }

        public virtual Point GridPosition
        {
            get
            {
                return _gridPosition;
            }
            set
            {
                _gridPosition = value;
                _worldPosition.X = CollisionGrid.BLOCK_WIDTH * value.X;
                _worldPosition.Y = CollisionGrid.BLOCK_HEIGHT * value.Y;
            }
        }

        public virtual Vector2 WorldPosition
        {
            get
            {
                return _worldPosition;
            }
        }

        public Texture2D Texture
        {
            get { return _texture; }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; }
        }


        public virtual Rectangle BoundingBox
        {
            get
            {
                if (_isActive == true)
                    return new Rectangle((int)_worldPosition.X, (int)_worldPosition.Y, _texture.Height, _texture.Width);
                else
                    return new Rectangle(0, 0, 0, 0);
            }
            
        }

        public virtual bool CheckCollision(bool tryFallThrough = false)
        {
            return _isActive;
        }
    }
}
