using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;

namespace Teraform
{
    public class Player : GameCharacter
    {
        PlayerIndex _controllerIndex;
        bool _usingPcControls;

        public Player(PlayerIndex controllerIndex, Texture2D texture, Vector2 position, bool usesKeyboardMouse = false)
            : base(texture, position)
        {
            _usingPcControls = usesKeyboardMouse;
            _controllerIndex = controllerIndex;
        }

        public Player(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            _usingPcControls = true;
            _controllerIndex = PlayerIndex.One;
        }

        public override void Update(double total_seconds_elapsed, CollisionGrid grid)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.A) == true)
            {
                this.Run(-1.0f);
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D) == true)
            {
                this.Run(1.0f);
            }
            else
            {
                this.Run(GamePad.GetState(_controllerIndex).ThumbSticks.Left.X);
            }

            bool jump = (GamePad.GetState(_controllerIndex).Buttons.A == ButtonState.Pressed);
            if (_usingPcControls == true)
                jump |= Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.W);

            this.Jump(jump);
                
            bool fall_through = (GamePad.GetState(_controllerIndex).ThumbSticks.Left.Y < -0.25);
            if (_usingPcControls == true)
                fall_through |= Keyboard.GetState().IsKeyDown(Keys.S);

            if (fall_through == true)
                this.FallThrough = (1 << (int)Teraform.Item.BLOCK_SURFACE.BLOCK_TOP);
            else
                this.FallThrough = 0;

            if ((GamePad.GetState(_controllerIndex).Buttons.B == ButtonState.Pressed))
            {
                grid.DEBUG_ENABLED = true;
            }
            else
            {
                grid.DEBUG_ENABLED = false;
            }

            base.Update(total_seconds_elapsed, grid);
        }
    }
}
