using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;

using Teraform.Camera;

namespace Teraform
{
    public class Player : GameCharacter
    {
        PlayerIndex _controllerIndex;
        bool _usingPcControls;
        Belt _belt;
        int _mouseScrollValue;
        bool _inventoryOpen = false;

        public Player(PlayerIndex controllerIndex, Texture2D texture, Vector2 position, bool usesKeyboardMouse = false)
            : base(texture, position)
        {
            _usingPcControls = usesKeyboardMouse;
            _controllerIndex = controllerIndex;
            _belt = new Belt();
            _mouseScrollValue = Mouse.GetState().ScrollWheelValue;
        }

        public Player(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
            _usingPcControls = true;
            _controllerIndex = PlayerIndex.One;
            _mouseScrollValue = Mouse.GetState().ScrollWheelValue;
            _belt = new Belt();
        }

        public override void Update(double total_seconds_elapsed, CollisionGrid grid)
        {
            //TODO check for window is active before doing stuff based on keys and buttons, it's getting pretty annoying

            Camera2D camera = Game.CameraInstance;
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

            if ((fall_through == true) && (IsAirborn == false))
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


            if (GamePad.GetState(_controllerIndex).Buttons.LeftShoulder == ButtonState.Pressed)
                _belt.SelectPreviousItem();
            if (GamePad.GetState(_controllerIndex).Buttons.RightShoulder == ButtonState.Pressed)
                _belt.SelectNextItem();

            //Handle mouse wheel
            if (_usingPcControls == true)
            {
                int newScrollValue = Mouse.GetState().ScrollWheelValue;
                int relativeScrollValue = newScrollValue - _mouseScrollValue;
                _mouseScrollValue = newScrollValue;
                
                if (_inventoryOpen == false)
                {
                    if (relativeScrollValue < 0)
                    {
                        _belt.SelectPreviousItem();
                    }
                    else if (relativeScrollValue > 0)
                    {
                        _belt.SelectNextItem();
                    }

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        Item selectedItem = _belt.GetCurrentBeltItem();
                        if (selectedItem != null)
                        {
                            selectedItem.Use(new Point(Mouse.GetState().X + camera.Left, Mouse.GetState().Y + camera.Top), this);
                        }
                    }
                    if (Mouse.GetState().RightButton == ButtonState.Pressed)
                    {
                        grid.RemoveObject(Mouse.GetState().X + camera.Left, Mouse.GetState().Y + camera.Top);
                    }
                }
            }

            base.Update(total_seconds_elapsed, grid);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        
        public void TempAddItem(Item item)
        {
            _belt.AddBeltItem(item);
        }

    }
}
