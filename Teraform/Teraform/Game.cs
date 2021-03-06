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

using Teraform.Camera;
using Teraform;
using TeraformData;

namespace Teraform
{
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        static Camera2D _gameCamera;
        Player theDude;
        //GameObject theBabe;
        SpriteFont font;
        static CollisionGrid _theGrid;

        //Vector2 spriteAcceleration = new Vector2(100.0f, 0.0f);
        //const int JUMP_VELOCITY = -250;
        //const int RUN_VELOCITY = 500;

        static GraphicsDeviceManager _graphics;
        public static ItemCatalog itemCatalog;
        SpriteBatch spriteBatch;
        
        public Game()
        {
            
            _graphics = new GraphicsDeviceManager(this);

            
            Content.RootDirectory = "Content";

            //Texture2D.FromStream(graphics, new System.IO.StreamReader(Environment.SpecialFolder.Resources + "Custom\\" + );
            
        }

        public static Camera2D CameraInstance
        {
            get { return _gameCamera; }
        }
        public static CollisionGrid GridInstane
        {
            get { return _theGrid; }
        }
        public static GraphicsDeviceManager GraphicsInstance
        {
            get { return _graphics; }
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
            if (_gameCamera == null)
            {
                _gameCamera = new Camera2D(_graphics.GraphicsDevice.Viewport);
            }
            Texture2D theDudeTexture;
            theDudeTexture = Content.Load<Texture2D>("dude");

            font = Content.Load<SpriteFont>("SpriteFont1");


            CustomWorldData worldData = new CustomWorldData("Examplia");
            worldData.LoadContent(Content);

            itemCatalog = new ItemCatalog(worldData._itemData);

            if (System.IO.File.Exists("C:\\Users\\Public\\MyLevel.lvl") == true)
            {
                
                    try
                    {
                        _theGrid = new CollisionGrid("C:\\Users\\Public\\MyLevel.lvl", Content.Load<Texture2D>("Active"), Content.Load<Texture2D>("Inactive"), Content.Load<Texture2D>("Platform"));
                    }
                    catch //err as F
                    {
                        _theGrid = new CollisionGrid(100, 100);
                    }

            }
            else
            {
                _theGrid = new CollisionGrid(100, 100);
            }



            theDude = new Player(theDudeTexture, Vector2.Zero);
            theDude.TempAddItem(itemCatalog.GetItem(2));
            theDude.TempAddItem(itemCatalog.GetItem(1));
            
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
            if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) ||
                (Keyboard.GetState().IsKeyDown(Keys.Escape) == true))
            {
                _theGrid.Save();
                this.Exit();
            }
            if (_gameCamera == null)
            {
                
            }
            //Detecting button presses this frame
            //Buttons lastFrame;
            //Buttons currentFrame;
            //Buttons pressed = ~lastFrame & currentFrame;

            
            theDude.Update(gameTime.ElapsedGameTime.TotalSeconds, _theGrid);
            
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
                _gameCamera = new Camera2D(_graphics.GraphicsDevice.Viewport);
            }
            GraphicsDevice.Clear(Color.LightSkyBlue);

            Plane y_plane = new Plane(-1 * Vector3.UnitY, _graphics.GraphicsDevice.Viewport.Height / 2);
            //Plane y_plane = new Plane(Vector3.UnitY, (float)-gameTime.TotalGameTime.TotalSeconds);
            _gameCamera.Center = theDude.BoundingBox.Center;

            //spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, Matrix.Identity);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, _gameCamera.ViewMatrix);
            _theGrid.Draw(spriteBatch);
            theDude.Draw(spriteBatch);
            spriteBatch.DrawString(font,"Test", Vector2.Zero, Color.Maroon);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
