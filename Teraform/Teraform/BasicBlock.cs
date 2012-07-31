using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Blocks
{
    class BasicBlock
    {
        Texture2D ActiveTexture;
        Texture2D PassiveTexture; 
        public Vector2 Position;
        public bool IsActive;

        public BasicBlock(Texture2D active_texture, Texture2D passive_texture, int x = 0, int y = 0, bool is_active = false)
        {
            ActiveTexture = active_texture;
            PassiveTexture = passive_texture;
            Position.X = x;
            Position.Y = y;
            IsActive = is_active;
        }

        public BasicBlock(Texture2D active_texture, Texture2D passive_texture, Vector2 position, bool is_active = false)
        {
            ActiveTexture = active_texture;
            PassiveTexture = passive_texture;
            Position = position;
            IsActive = is_active;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Texture2D texture = PassiveTexture;
            if (IsActive)
            {
                texture = ActiveTexture;
            }
            spriteBatch.Draw(texture, Position, Color.White);
        }

        public Rectangle BoundingBox
        {
            get
            {
                if (IsActive)
                {
                    return new Rectangle(
                        (int)Position.X,
                        (int)Position.Y,
                        ActiveTexture.Width,
                        ActiveTexture.Height);
                }
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    PassiveTexture.Width,
                    PassiveTexture.Height);
            }
        }
    }
}
