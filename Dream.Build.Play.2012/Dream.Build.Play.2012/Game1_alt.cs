/** 
* HighBeta Dream.Build.Play 2012 http://github.com/mcolonj/Dream.Build.Play.2012
*
* Copyright (c) Aaron Schultheis, Michael Colon
* Copyright (c) 2012 HighBeta, LLC.
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
**/

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
using Dream.Build.Play._2012.Plists;


namespace Dream.Build.Play._2012
{

    /// <summary>
    /// Joystick mapping struct(s)
    /// </summary>
    struct JoystickButtons
    {
        public bool A, B, X, Y, LB, RB, RT, LT, Start, Back;
    }

    struct Joystick
    {
        public bool Left, Right, Up, Down;
        public float X, Y;
        public JoystickButtons Buttons; 
       
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1_alt : Microsoft.Xna.Framework.Game
    {
        GamePadState currentGamePadState;
        GamePadState previousGamePadState;

        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // sprite sheet reader/helper/config class
        SpriteSheet sheet;

        //player class
        Player tuk;

        public Game1_alt()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            tuk = new Player();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // create new sprite sheet with texture
            Texture2D spriteSheet = Content.Load<Texture2D>("NakedSpriteMan");
            sheet = new SpriteSheet("Content/NakedSpriteMan.plist", spriteSheet);
           
            // Initialize Player Tuk
            Vector2 playerPosition = new Vector2(
                    GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2,                                 
                    GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
           
            tuk.Initialize(playerPosition, sheet);

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
            // Allows the game to exit, cause it sucks.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add Unnecessary Logic Here
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            // map input & update palyer
            Joystick input = MapInput(currentKeyboardState, currentGamePadState);
            tuk.Update(gameTime, input);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin(); // Begin Batch
            tuk.Draw(spriteBatch);
            spriteBatch.End();  // End Batch
            base.Draw(gameTime);
        }

        //
        // Joystick mapping method
        //
        Joystick MapInput(KeyboardState kb, GamePadState gp)
        {
            Joystick joystick = new Joystick();

            joystick.X = gp.ThumbSticks.Left.X;
            joystick.Y = gp.ThumbSticks.Left.Y;

            joystick.Left = (kb.IsKeyDown(Keys.Left) || gp.DPad.Left == ButtonState.Pressed);
            joystick.Right = (kb.IsKeyDown(Keys.Right) || gp.DPad.Right == ButtonState.Pressed);
            joystick.Up = (kb.IsKeyDown(Keys.Up) || gp.DPad.Up == ButtonState.Pressed);
            joystick.Down = (kb.IsKeyDown(Keys.Down) || gp.DPad.Down == ButtonState.Pressed);
               

            return joystick;
        }

    }
}
