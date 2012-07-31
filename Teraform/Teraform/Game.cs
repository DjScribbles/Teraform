using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BoundingBoxCollision;
using Blocks;

using MyGame.Camera;
using MyCollisionGrid;

namespace MyGame
{
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        static Camera2D _gameCamera;
        GameObject theDude;
        GameObject theBabe;
        SpriteFont font;
        CollisionGrid theGrid;

        Vector2 spriteAcceleration = new Vector2(100.0f, 0.0f);
        const int JUMP_VELOCITY = -250;
        const int RUN_VELOCITY = 500;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
      
        public Game1()
        {
            
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
        }

        public static Camera2D CameraInstance
        {
            get { return _gameCamera; }
        }
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.IsMouseVisible = true;

            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Texture2D theDudeTexture;
            Texture2D theBabeTexture; 
            theDudeTexture = Content.Load<Texture2D>("dude");
            theBabeTexture = Content.Load<Texture2D>("babe");
            font = Content.Load<SpriteFont>("SpriteFont1");
            if (System.IO.File.Exists("C:\\Users\\Public\\MyLevel.lvl") == true)
            {
                
                    try
                    {
                        theGrid = new CollisionGrid("C:\\Users\\Public\\MyLevel.lvl", Content.Load<Texture2D>("Active"), Content.Load<Texture2D>("Inactive"));
                    }
                    catch //err as F
                    {
                        theGrid = new CollisionGrid(100, 100, Content.Load<Texture2D>("Active"), Content.Load<Texture2D>("Inactive"));
                    }

            }
            else
            {
                    theGrid = new CollisionGrid(100, 100, Content.Load<Texture2D>("Active"), Content.Load<Texture2D>("Inactive"));
            }

            theDude = new GameObject(theDudeTexture, Vector2.Zero);
            theBabe = new GameObject(theBabeTexture, new Vector2(150.0f, 150.0f));
            
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                theGrid.Save();
                this.Exit();
            }
            if (_gameCamera == null)
            {
                _gameCamera = new Camera2D(graphics.GraphicsDevice.Viewport);
            }
            //Detecting button presses this frame
            //Buttons lastFrame;
            //Buttons currentFrame;
            //Buttons pressed = ~lastFrame & currentFrame;

            Vector2 analogInput = Vector2.One;

            analogInput = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
            analogInput.Y *= -1;
            theDude.Accelleration = (spriteAcceleration * analogInput);
            theDude.RunSpeed = RUN_VELOCITY * Math.Abs(analogInput.X);

            Vector2 dude_velocity = theDude.Velocity;
            if ((dude_velocity.Y == 0) && (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed))
            {
                dude_velocity.Y = JUMP_VELOCITY;
                theDude.Velocity = dude_velocity;
            }
            else if ((dude_velocity.Y < (JUMP_VELOCITY / 4)) && (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Released))
            {
                dude_velocity.Y = (JUMP_VELOCITY / 4);
                theDude.Velocity = dude_velocity;
            }

            if ((GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed))
            {
                theGrid.DEBUG_ENABLED = true;
            }
            else
            {
                theGrid.DEBUG_ENABLED = false;
            }

            theDude.Update(gameTime.ElapsedGameTime.TotalSeconds, theGrid);
       

            //int MaxX = graphics.GraphicsDevice.Viewport.Width - theDude.BoundingBox.Width;
            //int MinX = 0;
            //int MaxY = graphics.GraphicsDevice.Viewport.Height - theDude.BoundingBox.Height;
            //int MinY = 0;

            //if (theDude._position.X > MaxX)
            //{
            //    theDude._position.X = MaxX;
            //}
            //else if (theDude._position.X < MinX)
            //{
            //    theDude._position.X = MinX;
            //}
            //else if (theDude._position.Y > MaxY)
            //{
            //    theDude._position.Y = MaxY;
            //}
            //else if (theDude._position.Y < MinY)
            //{
            //    theDude._position.Y = MinY;
            //}


            //analogInput = Vector2.One;

            //analogInput = GamePad.GetState(PlayerIndex.Two).ThumbSticks.Left;
            //analogInput.Y *= -1;
            //theBabe._position += (spriteSpeed * analogInput) * (float)gameTime.ElapsedGameTime.TotalSeconds;

            //MaxX = graphics.GraphicsDevice.Viewport.Width - theBabe.BoundingBox.Width;
            //MaxY = graphics.GraphicsDevice.Viewport.Height - theBabe.BoundingBox.Height;

            //if (theBabe._position.X > MaxX)
            //{
            //    theBabe._position.X = MaxX;
            //}
            //else if (theBabe._position.X < MinX)
            //{
            //    theBabe._position.X = MinX;
            //}
            //else if (theBabe._position.Y > MaxY)
            //{
            //    theBabe._position.Y = MaxY;
            //}
            //else if (theBabe._position.Y < MinY)
            //{
            //    theBabe._position.Y = MinY;
            //}
            //if (theDude.BoundingBox.Intersects(theBabe.BoundingBox))
            //{
            //    GamePad.SetVibration(PlayerIndex.One, 1.0f, 0.1f);
            //    GamePad.SetVibration(PlayerIndex.Two, 1.0f, 0.1f);
            //}
            //else
            //{
            //    GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
            //    GamePad.SetVibration(PlayerIndex.Two, 0.0f, 0.0f);
            //}

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                theGrid.SetBlockState(Mouse.GetState().X + _gameCamera.Left, Mouse.GetState().Y + _gameCamera.Top, true);
            }
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                theGrid.SetBlockState(Mouse.GetState().X + _gameCamera.Left, Mouse.GetState().Y + _gameCamera.Top, false);
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (_gameCamera == null)
            {
                _gameCamera = new Camera2D(graphics.GraphicsDevice.Viewport);
            }
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Plane y_plane = new Plane(-1 * Vector3.UnitY, graphics.GraphicsDevice.Viewport.Height / 2);
            //Plane y_plane = new Plane(Vector3.UnitY, (float)-gameTime.TotalGameTime.TotalSeconds);
            _gameCamera.Center = theDude.BoundingBox.Center;

            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Matrix.Identity);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, _gameCamera.ViewMatrix);
            theGrid.Draw(spriteBatch);
            theDude.Draw(spriteBatch);
            theBabe.Draw(spriteBatch);
            spriteBatch.DrawString(font,"Test", Vector2.Zero, Color.Maroon);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
