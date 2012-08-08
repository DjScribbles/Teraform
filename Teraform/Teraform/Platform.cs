using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Teraform
{
    public class Platform : GridObject
    {
        public Platform(Texture2D texture, Point gridPosition, bool isActive = true)
            : base(texture, gridPosition, isActive)
        {         
        }

        public override bool CheckCollision(BLOCK_SURFACE contactSurface, int tryFallThrough)
        {
            //You can only collide with the top surface, but you can fall through it if you want to
            if ((IsActive == true) && (contactSurface == BLOCK_SURFACE.BLOCK_TOP) && ((tryFallThrough & (1 << (int)BLOCK_SURFACE.BLOCK_TOP)) == 0))
                return true;

            return false;
        }
        
    }
}
