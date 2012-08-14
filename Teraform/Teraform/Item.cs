using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Teraform
{
    class Item
    {
        public enum ITEM_STATE 
        {
            IN_WORLD,
            IN_GRID,
            IN_INVENTORY,
            EQUIPPED
        };
        
        private ITEM_STATE _currentState;
        public Point _drawLocation;
        public decimal _drawRotation;


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
            switch (_currentState)
            {
                case ITEM_STATE.IN_GRID:

                    break;

                case ITEM_STATE.IN_INVENTORY:
                    break;

                case ITEM_STATE.EQUIPPED:
                case ITEM_STATE.IN_WORLD:
                    break;
            }
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
    }
}
