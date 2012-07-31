﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Blocks;


namespace MyCollisionGrid
{
    
    public class CollisionGrid
    {
        public bool DEBUG_ENABLED = false;
        const int BLOCK_WIDTH = 16;
        const int BLOCK_HEIGHT = 16;
        private int _width;
        private int _height;
    
        BasicBlock[,] Blocks;

        //TODO fix texture loading so this isn't needed, it's really dumb
        public CollisionGrid(int width, int height, Texture2D active, Texture2D passive)
        {
            _width = width;
            _height = height;
            Blocks = new BasicBlock[width, height];
            for (int column = 0; column < width; column++)
            {
                for (int row = 0; row < height; row++)
                {
                    Blocks[column, row] = new BasicBlock(active, passive, column * BLOCK_WIDTH, row * BLOCK_HEIGHT, false);
                }
            }

        }
        public CollisionGrid(String filename, Texture2D active, Texture2D passive)
        {
            //TODO read from file
            using (System.IO.StreamReader file = new System.IO.StreamReader(filename))
            {
                int width = int.Parse(file.ReadLine());
                int height = int.Parse(file.ReadLine());

                _width = width;
                _height = height;
                Blocks = new BasicBlock[width, height];

                for (int column = 0; column < width; column++)
                {
                    for (int row = 0; row < height; row++)
                    {
                        Blocks[column, row] = new BasicBlock(active, passive, column * BLOCK_WIDTH, row * BLOCK_HEIGHT, bool.Parse(file.ReadLine()));
                    }
                }
                file.Close();
            }
        }

        
        public void Save()
        {
            //TODO write to file
            const String filePath = "C:\\Users\\Public\\MyLevel.lvl";
            const String backupPath = "C:\\Users\\Public\\MyLevel.bak";
            
            //if (System.IO.File.Exists(backupPath) == true)
            //    System.IO.File.Delete(backupPath);

            //if (System.IO.File.Exists(filePath) == true)
            //    System.IO.File.Move(filePath, backupPath);
           
            //System.IO.File.Create(filePath);
            
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
            {
                file.WriteLine("{0}", _width);
                file.WriteLine("{0}", _height);
                foreach (BasicBlock block in Blocks)
                {
                    file.WriteLine(block.IsActive);       
                }
                file.Write(file.NewLine);
                file.Close();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (BasicBlock block in Blocks)
            {
                block.Draw(spriteBatch);
            }
        }
        public Vector2 CheckCollision(Rectangle object_bounds, Vector2 object_velocity)
        {          
            if (object_velocity == Vector2.Zero)
                return object_velocity;
            
            //TODO handle rounding, fix sign errors since everything is backwards
            int current_left_block = object_bounds.X / BLOCK_WIDTH;
            int current_right_block = object_bounds.Right / BLOCK_WIDTH;
            int current_top_block = object_bounds.Top / BLOCK_HEIGHT;
            int current_bottom_block = object_bounds.Bottom / BLOCK_HEIGHT;

            //TODO handle rounding
            int blocks_to_travel_x = 0;
            if (object_velocity.X < 0)
            {
                blocks_to_travel_x = ((int)object_velocity.X - (BLOCK_WIDTH - (object_bounds.Left % BLOCK_WIDTH))) / BLOCK_WIDTH;
            }
            else if (object_velocity.X > 0)
            {
                blocks_to_travel_x = ((int)object_velocity.X + (object_bounds.Right % BLOCK_WIDTH)) / BLOCK_WIDTH;
            }

            //TODO handle rounding
            int blocks_to_travel_y = 0;
            if (object_velocity.Y < 0)
            {
                blocks_to_travel_y = ((int)object_velocity.Y - (BLOCK_HEIGHT - (object_bounds.Top % BLOCK_HEIGHT))) / BLOCK_HEIGHT;
            }
            else if (object_velocity.Y > 0)
            {
                blocks_to_travel_y = ((int)object_velocity.Y + (object_bounds.Bottom % BLOCK_HEIGHT)) / BLOCK_HEIGHT;
            }


            int blocks_to_travel_x_sign = Math.Sign(blocks_to_travel_x);
            int blocks_to_travel_y_sign = Math.Sign(blocks_to_travel_y);
            int blocks_traveled_x = 0;
            int blocks_traveled_y = 0;

            bool clip_x = false;
            bool clip_y = false;

            if ((DEBUG_ENABLED == true) && ((blocks_to_travel_x != 0) /*|| (blocks_to_travel_y !=0)*/))
            {
                Console.Out.WriteLine("dude block position: left={0}, top={1}, right={2}, bottom={3}", current_left_block, current_top_block, current_right_block, current_bottom_block);
                Console.Out.WriteLine("Blocks to travel: X={0} Y={1}", blocks_to_travel_x, blocks_to_travel_y);
                Console.Out.WriteLine("We're moving between blocks, checking the following:");
            }
            //TODO consider breaking collision detection into a seperate function.
            while ((blocks_to_travel_x != 0) || (blocks_to_travel_y !=0))
            {

                if (blocks_to_travel_x != 0)
                {
                    //Determine the location of the next block in the X direction to check for collisions
                    int next_block_x = current_left_block;
                    if (blocks_to_travel_x_sign == 1)
                    {
                        next_block_x = current_right_block + 1;
                    }
                    else if (blocks_to_travel_x_sign == -1)
                    {
                        next_block_x = current_left_block - 1;
                    }

                    //Check from top to bottom for an active block in the next X direction
                    for (int block = current_top_block; block <= current_bottom_block; block++)
                    {
                        if (DEBUG_ENABLED == true) Console.Out.Write("[{0}, {1}]", next_block_x, block);

                        //If an active block is found, travel no more blocks
                        //TODO check that all are within height and width
                        bool is_active = true;
                        try
                        {
                            is_active = Blocks[next_block_x,block].IsActive;
                        }
                        catch (IndexOutOfRangeException err)
                        {
                            is_active = true;
                        }

                        if (is_active == true)
                        {
                            if (DEBUG_ENABLED == true) Console.Out.Write("X HIT!{0}", Console.Out.NewLine);

                            blocks_to_travel_x = 0;
                            blocks_to_travel_x_sign = 0;    //Zero the sign so the below if conditions don't execute
                            clip_x = true;
                            break;
                        }
                    }
                    //Check an edge case where the tail end of the rectangle is not being checked after the move
                    if ((Math.Sign(blocks_traveled_y) != 0) && (object_bounds.Height > BLOCK_HEIGHT) && ((object_bounds.Height % BLOCK_HEIGHT) != 0))
                    {
                        bool is_active = false;
                        int trailing_y_block;
                        if (Math.Sign(blocks_traveled_y) == -1)
                        {
                            trailing_y_block = current_bottom_block + 1;
                        }
                        else
                        {
                            trailing_y_block = current_top_block - 1;
                        }

                        if (DEBUG_ENABLED == true) Console.Out.Write("[{0}, {1}]", next_block_x, trailing_y_block);

                        try
                        {
                            is_active = Blocks[next_block_x, trailing_y_block].IsActive;
                        }
                        catch (IndexOutOfRangeException err)  //if the trailing block is out of bounds, we don't care
                        {
                            is_active = false;
                        }
                     
                        if (is_active == true)
                        {
                            if (DEBUG_ENABLED == true) Console.Out.Write("X HIT!{0}", Console.Out.NewLine);

                            blocks_to_travel_x = 0;
                            blocks_to_travel_x_sign = 0;    //Zero the sign so the below if conditions don't execute
                            clip_x = true;
                            break;
                        }
                    }

                    //Based on the direction being moved, move iterators
                    if (blocks_to_travel_x_sign == 1)
                    {
                        blocks_to_travel_x--;
                        current_left_block++;
                        current_right_block++;
                        blocks_traveled_x++;
                    }
                    else if (blocks_to_travel_x_sign == -1)
                    {
                        blocks_to_travel_x++;
                        current_left_block--;
                        current_right_block--;
                        blocks_traveled_x--;
                    }
                }
                //if (DEBUG_ENABLED == true) Console.Out.Write(Console.Out.NewLine);
                if (blocks_to_travel_y != 0)
                {
                    //Determine the location of the next block in the X direction to check for collisions
                    int next_block_y = current_top_block;
                    if (blocks_to_travel_y_sign == 1)
                    {
                        next_block_y = current_bottom_block + 1;
                    }
                    else if (blocks_to_travel_y_sign == -1)
                    {
                        next_block_y = current_top_block - 1;
                    }

                    //Check from left to right for an active block in the next Y direction
                    for (int block = current_left_block; block <= current_right_block; block++)
                    {
                        //if (DEBUG_ENABLED == true) Console.Out.Write("[{0}, {1}]", block, next_block_y);
                        //If an active block is found, travel no more blocks
                        //TODO check that all are within height and width
                        bool is_active = true;
                        try 
                        {
                            is_active = Blocks[block, next_block_y].IsActive;
                        }
                        catch (IndexOutOfRangeException err)
                        {
                            is_active = true;
                        }

                        if (is_active == true)
                        {
                            //if (DEBUG_ENABLED == true) Console.Out.Write("Y HIT!{0}", Console.Out.NewLine);
                            blocks_to_travel_y = 0;
                            blocks_to_travel_y_sign = 0;    //Zero the sign so the below if conditions don't execute
                            clip_y = true;
                        }

                    }

                    //Check an edge case where the tail end of the rectangle is not being checked after the move
                    if ((Math.Sign(blocks_traveled_x) != 0) && (object_bounds.Width > BLOCK_WIDTH) && ((object_bounds.Width % BLOCK_WIDTH) != 0))
                    {
                        bool is_active = false;
                        int trailing_x_block;
                        if (Math.Sign(blocks_traveled_x) == -1)
                        {
                            trailing_x_block = current_right_block + 1;
                        }
                        else
                        {
                            trailing_x_block = current_left_block - 1;
                        }

                        if (DEBUG_ENABLED == true) Console.Out.Write("[{0}, {1}]", trailing_x_block, next_block_y);
                        try
                        {
                            is_active = Blocks[trailing_x_block, next_block_y].IsActive;
                        }
                        catch (IndexOutOfRangeException err)  //if the trailing block is out of bounds, we don't care
                        {
                            is_active = false;
                        }

                        if (is_active == true)
                        {
                            if (DEBUG_ENABLED == true) Console.Out.Write("Y HIT!{0}", Console.Out.NewLine);
                            blocks_to_travel_y = 0;
                            blocks_to_travel_y_sign = 0;    //Zero the sign so the below if conditions don't execute
                            clip_y = true;
                        }
                    }

                    //Based on the direction being moved, move iterators
                    if (blocks_to_travel_y_sign == 1)
                    {
                        blocks_to_travel_y--;
                        current_top_block++;
                        current_bottom_block++;
                        blocks_traveled_y++;
                    }
                    else if (blocks_to_travel_y_sign == -1)
                    {
                        blocks_to_travel_y++;
                        current_top_block--;
                        current_bottom_block--;
                        blocks_traveled_y--;
                    }

                    //Recalculate sign 
                    //blocks_to_travel_y_sign = Math.Sign(blocks_to_travel_y); //not needed, doesn't change until it's zero
                }


            }

            //If we clipped in the x direction
            if (clip_x == true)
            {
                //we can move the remainder of the occupied block, plus block width * blocks traveled
                if (Math.Sign(object_velocity.X) == 1)
                {
                    float clipped_velocity = (BLOCK_WIDTH - (object_bounds.Right % BLOCK_WIDTH)) + (BLOCK_WIDTH * blocks_traveled_x) - 1;
                    if (clipped_velocity < object_velocity.X)
                        object_velocity.X = clipped_velocity;
                }
                else if (Math.Sign(object_velocity.X) == -1)
                {
                    float clipped_velocity = (BLOCK_WIDTH * blocks_traveled_x) - (object_bounds.Left % BLOCK_WIDTH) + 1;
                    if (clipped_velocity > object_velocity.X)
                        object_velocity.X = clipped_velocity;
                }
            }

            //If we clipped in the y direction
            if (clip_y == true)
            {
                //we can move the remainder of the occupied block, plus block width * blocks traveled
                if (Math.Sign(object_velocity.Y) == 1)
                {
                    float clipped_velocity = (BLOCK_HEIGHT - (object_bounds.Bottom % BLOCK_HEIGHT)) + (BLOCK_HEIGHT * blocks_traveled_y) - 1;
                    if (clipped_velocity < object_velocity.Y)
                        object_velocity.Y = clipped_velocity;
                }
                else if (Math.Sign(object_velocity.Y) == -1)
                {
                    float clipped_velocity = (BLOCK_HEIGHT * blocks_traveled_y) - (object_bounds.Top % BLOCK_HEIGHT) + 1;
                    if (clipped_velocity > object_velocity.Y)
                        object_velocity.Y = clipped_velocity;
                }
            }

            //TODO check for non-resolved collisions, possibly allow unhindered movement in the x direction, or possible forcefully move back to last known good position? (possibly over time)?  Or just kill'em (MUHAHAH)
            return object_velocity;
        }

        public void SetBlockState(int x, int y, bool is_active)
        {
            int block_x = x / BLOCK_WIDTH;
            int block_y = y / BLOCK_HEIGHT;
            if ((block_x < _width) && (block_y < _height) && (block_x >= 0) && (block_y >= 0))
            {
                Blocks[block_x, block_y].IsActive = is_active;
            }
        }
        //TODO handle the mouse pointer clicks and other events.

        public void GetBlockCoordinates(int point_x, int point_y, out int block_x, out int block_y)
        {
            block_x = point_x / BLOCK_WIDTH;
            block_y = point_y / BLOCK_HEIGHT;
        }

        public void GetBlockCoordinates(Rectangle world_rect, out Rectangle block_rect)
        {
            block_rect.X = world_rect.Left / BLOCK_WIDTH;
            block_rect.Width = block_rect.X - (world_rect.Right / BLOCK_WIDTH);
            block_rect.Y = world_rect.Top / BLOCK_HEIGHT;
            block_rect.Height = block_rect.Y - (world_rect.Bottom / BLOCK_HEIGHT);
        }

        public void CheckColisions(Rectangle object_bounds)
        {

        }
    }
}