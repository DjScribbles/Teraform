using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Teraform
{
    public class Platform : Block
    {
        public Platform(Point position, Texture2D texture, ITEM_STATE itemState)
            : base(position, texture, itemState)
        {         
        }

        public override bool CheckGridCollision(BLOCK_SURFACE contactSurface, int tryFallThrough)
        {
            //You can only collide with the top surface, but you can fall through it if you want to
            if ((_currentState == ITEM_STATE.IN_GRID) && (contactSurface == BLOCK_SURFACE.BLOCK_TOP) && ((tryFallThrough & (1 << (int)BLOCK_SURFACE.BLOCK_TOP)) == 0))
                return true;

            return false;
        }
        
    }
}
