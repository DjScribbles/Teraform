using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Teraform
{
    public class Item
    {
        public enum ITEM_STATE 
        {
            IN_WORLD,
            IN_GRID,
            IN_INVENTORY,
            EQUIPPED
        };

        public enum BLOCK_SURFACE
        {
            BLOCK_TOP,
            BLOCK_LEFT,
            BLOCK_BOTTOM,
            BLOCK_RIGHT
        };

        protected ITEM_STATE _currentState;
        public Vector2 _drawLocation;
        public decimal _drawRotation;

        private Texture2D _gridTexture;
        private Texture2D _worldTexture;
        private Texture2D _inventoryTexture;
        private Texture2D _equippedTexture;

        public Item(Point location, Texture2D allTextures, ITEM_STATE itemState)
        {
            _currentState = itemState;
            _drawLocation.X = location.X;
            _drawLocation.Y = location.Y;

            _drawRotation = 0;

            _gridTexture = allTextures;
            _worldTexture = allTextures;
            _inventoryTexture = allTextures;
            _equippedTexture = allTextures;
        }

        public Item(Point location, Texture2D worldTexture, Texture2D inventoryTexture, ITEM_STATE itemState, Texture2D gridTexture = null, Texture2D equippedTexture = null)
        {
            _currentState = itemState;
            _drawLocation.X = location.X;
            _drawLocation.Y = location.Y;
            _drawRotation = 0;

            _gridTexture = gridTexture;
            _worldTexture = worldTexture;
            _inventoryTexture = inventoryTexture;
            _equippedTexture = equippedTexture;
        }

        public bool Use(Point location, GameCharacter user) 
        {
            switch (_currentState)
            {
                case ITEM_STATE.IN_GRID:
                    break;

                case ITEM_STATE.IN_INVENTORY:
                    break;

            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_currentState == ITEM_STATE.IN_GRID)
            {
                _drawLocation.X = ((int)_drawLocation.X) & (~0xF);
                _drawLocation.Y = ((int)_drawLocation.Y) & (~0xF);
            }
            
            spriteBatch.Draw(CurrentTexture, _drawLocation, Color.White);
        }

        public ITEM_STATE ItemState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        protected virtual bool UseFromInventory(Point target, GameCharacter user)
        {
            return false;
        }

        protected virtual bool UseFromGrid(GameCharacter user)
        {
            return false;
        }

        public virtual Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)_drawLocation.X, (int)_drawLocation.Y, CurrentTexture.Height, CurrentTexture.Width);
            }

        }

        public virtual Texture2D CurrentTexture
        {
            get
            {
                switch (_currentState)
                {
                    default:
                    case ITEM_STATE.IN_GRID:
                        return _gridTexture;
                    case ITEM_STATE.IN_WORLD:
                        return _worldTexture;
                    case ITEM_STATE.IN_INVENTORY:
                        return _inventoryTexture;
                    case ITEM_STATE.EQUIPPED:
                        return _equippedTexture;

                }
            }
        }


        //Grid checks only occur when a block is occupied, so as long as the item is on the grid, default to true
        public virtual bool CheckGridCollision(BLOCK_SURFACE contactSurface, int tryFallThrough)
        {
            if (_currentState == ITEM_STATE.IN_GRID)
                return true;
            return false;
        }

        public virtual bool CheckCollision(Rectangle opposingRectangle)
        {
            return opposingRectangle.Intersects(BoundingBox);
        }

        public virtual void Update(double total_seconds_elapsed, CollisionGrid grid)
        {
            if (_currentState == ITEM_STATE.IN_WORLD)
            {

            }
            else if (_currentState == ITEM_STATE.EQUIPPED)
            {

            }
        }

    }
}
