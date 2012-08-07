using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Teraform
{
    class BasicBlock : GridObject
    {
        Texture2D PassiveTexture; 

        public BasicBlock(Texture2D active_texture, Texture2D passive_texture, int grid_x = 0, int grid_y = 0, bool is_active = false) : base(active_texture,grid_x,grid_y,is_active)
        {
            PassiveTexture = passive_texture;
        }

        public BasicBlock(Texture2D active_texture, Texture2D passive_texture, Point position, bool is_active = false) : base(active_texture,position,is_active)
        {
            PassiveTexture = passive_texture;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = PassiveTexture;
            if (IsActive)
            {
                texture = Texture;
            }
            spriteBatch.Draw(texture, WorldPosition, Color.White);
        }

        public Rectangle BoundingBox
        {
            get
            {
                if (IsActive)
                {
                    return new Rectangle(
                        (int)WorldPosition.X,
                        (int)WorldPosition.Y,
                        Texture.Width,
                        Texture.Height);
                }
                return new Rectangle(
                    (int)WorldPosition.X,
                    (int)WorldPosition.Y,
                    PassiveTexture.Width,
                    PassiveTexture.Height);
            }
        }
    }
}
