using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Teraform
{
    //TODO figure out if this class should actually do anything, if not, nix it, item isn't an abstract class, this is just pointless right now
    class BasicBlock : Item
    {
        //Texture2D PassiveTexture; 

        //public BasicBlock(Texture2D active_texture/*, Texture2D passive_texture*/, int grid_x = 0, int grid_y = 0, bool is_active = false) : base(new Point(grid_x,grid_y),active_texture)
        //{
        //    //PassiveTexture = passive_texture;
        //}

        public BasicBlock(Point position, Texture2D active_texture/*, Texture2D passive_texture*/, ITEM_STATE itemState)
            : base(position, active_texture, itemState)
        {
            //PassiveTexture = passive_texture;
        }

        //public override void Draw(SpriteBatch spriteBatch)
        //{
        //    Texture2D texture = PassiveTexture;
        //    if (IsActive)
        //    {
        //        texture = Texture;
        //    }
        //    spriteBatch.Draw(texture, WorldPosition, Color.White);
        //}

        //public override Rectangle BoundingBox
        //{
        //    get
        //    {
        //        if (IsActive)
        //        {
        //            return new Rectangle(
        //                (int)WorldPosition.X,
        //                (int)WorldPosition.Y,
        //                Texture.Width,
        //                Texture.Height);
        //        }
        //        return new Rectangle(
        //            (int)WorldPosition.X,
        //            (int)WorldPosition.Y,
        //            PassiveTexture.Width,
        //            PassiveTexture.Height);
        //    }
        //}
    }
}
