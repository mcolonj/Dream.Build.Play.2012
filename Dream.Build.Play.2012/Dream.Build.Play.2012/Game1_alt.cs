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

        float playerMoveSpeed;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Keep animations in dictionary
        Dictionary<string, Animation> playerAnimation = new Dictionary<string, Animation>();
        Animation currentAnimation;

        // sprite sheet reader/helper/config class
        SpriteSheet sheet;

        //player class
        Player player1;

        public Game1_alt()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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
            playerMoveSpeed = 1.75f;
            player1 = new Player();

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

            //sprite texture
            Texture2D spriteSheet = Content.Load<Texture2D>("NakedSpriteMan");
            //sprite sheet coordinates
            sheet = new SpriteSheet("Content/NakedSpriteMan.plist", spriteSheet);
            //load bee animation from sheet coordinates
            LoadAnimations();
            currentAnimation = playerAnimation["down"];
            //player position
            Vector2 playerPosition = new Vector2(
                    GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2,                                 
                    GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            // initialize player with bee animation
            player1.Initialize(currentAnimation, playerPosition);

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
                this.Exit();

            // TODO: Add your update logic here
            previousGamePadState = currentGamePadState;
            previousKeyboardState = currentKeyboardState;

            currentKeyboardState = Keyboard.GetState();
            currentGamePadState = GamePad.GetState(PlayerIndex.One);

            UpdatePlayer(gameTime);

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
            spriteBatch.Begin();
            player1.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void UpdatePlayer(GameTime gameTime)
        {
            Joystick input = MapInput(currentKeyboardState, currentGamePadState);
            

            player1.Position.X += input.X * playerMoveSpeed;
            player1.Position.Y -= input.Y * playerMoveSpeed;

            if (input.Left && input.Up)
            {
                currentAnimation = (Animation)playerAnimation["leftup"];
                player1.Position.X -= playerMoveSpeed;
                player1.Position.Y -= playerMoveSpeed;
            }
            else if (input.Left && input.Down)
            {
                currentAnimation = (Animation)playerAnimation["leftdown"];
                player1.Position.X -= playerMoveSpeed;
                player1.Position.Y += playerMoveSpeed;
            }
            else if (input.Right && input.Down)
            {
                currentAnimation = (Animation)playerAnimation["rightdown"];
                player1.Position.X += playerMoveSpeed;
                player1.Position.Y += playerMoveSpeed;
            }
            else if (input.Right && input.Up)
            {
                currentAnimation = (Animation)playerAnimation["rightup"];
                player1.Position.X += playerMoveSpeed;
                player1.Position.Y -= playerMoveSpeed;
            }
            else if (input.Left)
            {
                currentAnimation = (Animation)playerAnimation["left"];
                player1.Position.X -= playerMoveSpeed;
            }
            else if (input.Right)
            {
                currentAnimation = (Animation)playerAnimation["right"];
                player1.Position.X += playerMoveSpeed;
            }
            else if (input.Up)
            {
                currentAnimation = (Animation)playerAnimation["up"];
                player1.Position.Y -= playerMoveSpeed;
            }
            else if (input.Down)
            {
                currentAnimation = (Animation)playerAnimation["down"];
                player1.Position.Y += playerMoveSpeed;
            }
            else
            {

            }

            ClearAnim();
            player1.Update(currentAnimation, gameTime);
        }

        private void ClearAnim()
        {
          
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

        //
        // Load Player Animations
        //
        void LoadAnimations()
        {
            int animationTime = 100;

            playerAnimation.Add("down", sheet.AnimationForFrameNames(
                new string[] { "walkdown0.png", "walkdown1.png", "walkdown2.png", "walkdown3.png", "walkdown4.png", "walkdown5.png", "walkdown6.png", "walkdown7.png" },
                animationTime, Color.White, true));

            playerAnimation.Add("up", sheet.AnimationForFrameNames(
                new string[] { "walkup0.png", "walkup1.png", "walkup2.png", "walkup3.png", "walkup4.png", "walkup5.png", "walkup6.png", "walkup7.png", },
                animationTime, Color.White, true));

            playerAnimation.Add("left", sheet.AnimationForFrameNames(
                new string[] { "walkleft0.png", "walkleft1.png", "walkleft2.png", "walkleft3.png", "walkleft4.png", "walkleft5.png", "walkleft6.png", "walkleft7.png", },
                animationTime, Color.White, true));

            playerAnimation.Add("right", sheet.AnimationForFrameNames(
                new string[] { "walkright0.png", "walkright1.png", "walkright2.png", "walkright3.png", "walkright4.png", "walkright5.png", "walkright6.png", "walkright7.png", },
                animationTime, Color.White, true));

            playerAnimation.Add("rightup", sheet.AnimationForFrameNames(
                new string[] { "walkrightup0.png", "walkrightup1.png", "walkrightup2.png", "walkrightup3.png", "walkrightup4.png", "walkrightup5.png", "walkrightup6.png", "walkrightup7.png", },
                animationTime, Color.White, true));

            playerAnimation.Add("rightdown", sheet.AnimationForFrameNames(
                new string[] { "walkrightdown0.png", "walkrightdown1.png", "walkrightdown2.png", "walkrightdown3.png", "walkrightdown4.png", "walkrightdown5.png", "walkrightdown6.png", "walkrightdown7.png", },
                animationTime, Color.White, true));

            playerAnimation.Add("leftdown", sheet.AnimationForFrameNames(
                new string[] { "walkleftdown0.png", "walkleftdown1.png", "walkleftdown2.png", "walkleftdown3.png", "walkleftdown4.png", "walkleftdown5.png", "walkleftdown6.png", "walkleftdown7.png", },
                animationTime, Color.White, true));

            playerAnimation.Add("leftup", sheet.AnimationForFrameNames(
                new string[] { "walkleftup0.png", "walkleftup1.png", "walkleftup2.png", "walkleftup3.png", "walkleftup4.png", "walkleftup5.png", "walkleftup6.png", "walkleftup7.png", },
                animationTime, Color.White, true));

        }
    }
}
