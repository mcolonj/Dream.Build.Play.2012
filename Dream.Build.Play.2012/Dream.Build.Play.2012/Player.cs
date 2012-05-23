/** 
* HighBeta Dream.Build.Play 2012 http://github.com/mcolonj/Dream.Build.Play.2012
*
* Copyright (c) Aaron Schultheis
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
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dream.Build.Play._2012.Plists;


namespace Dream.Build.Play._2012
{
    class Player
    {
        const int MAXHEALTH = 100;
        const int MAXENERGY = 100;

        private float speed;
        private Vector2 position;

        // Keep animations in dictionary
        Dictionary<string, Animation> playerAnimation = new Dictionary<string, Animation>();
        Animation currentAnimation;
        //Animation playerAnimation;
        Texture2D displayFrame { set; get; }

        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Player()
        {
            speed = 2.0f;
        }

        public void Initialize(Animation animation, Vector2 location)
        {
            currentAnimation = animation;
            position = location;
        }
        public void Initialize(Vector2 location)
        {
            position = location;
        }

        public void Initialize(Vector2 location, SpriteSheet sheet)
        {
            LoadAnimations(sheet);
            currentAnimation = playerAnimation["down"];
            position = location;
        }

        public void Update(GameTime gameTime, Joystick input)
        {
            position.X += input.X * speed;
            position.Y -= input.Y * speed;

            if (input.Left && input.Up)
            {
                currentAnimation = (Animation)playerAnimation["leftup"];
                position.X -= speed;
                position.Y -= speed;
            }
            else if (input.Left && input.Down)
            {
                currentAnimation = (Animation)playerAnimation["leftdown"];
                position.X -= speed;
                position.Y += speed;
            }
            else if (input.Right && input.Down)
            {
                currentAnimation = (Animation)playerAnimation["rightdown"];
                position.X += speed;
                position.Y += speed;
            }
            else if (input.Right && input.Up)
            {
                currentAnimation = (Animation)playerAnimation["rightup"];
                position.X += speed;
                position.Y -= speed;
            }
            else if (input.Left)
            {
                currentAnimation = (Animation)playerAnimation["left"];
                position.X -= speed;
            }
            else if (input.Right)
            {
                currentAnimation = (Animation)playerAnimation["right"];
                position.X += speed;
            }
            else if (input.Up)
            {
                currentAnimation = (Animation)playerAnimation["up"];
                position.Y -= speed;
            }
            else if (input.Down)
            {
                currentAnimation = (Animation)playerAnimation["down"];
                position.Y += speed;
            }
            else
            {
               
            }
           
            currentAnimation.Position = position;
            currentAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentAnimation.Draw(spriteBatch);
        }


        public void AddAnimation(String key, Animation a)
        {
            playerAnimation.Add(key, a);
        }

        public void LoadAnimations(SpriteSheet sheet)
        {
            int animationTime = 100;

            AddAnimation("down", sheet.AnimationForFrameNames(
                new string[] { "walkdown0.png", "walkdown1.png", "walkdown2.png", "walkdown3.png", "walkdown4.png", "walkdown5.png", "walkdown6.png", "walkdown7.png" },
                animationTime, Color.White, true));

            AddAnimation("up", sheet.AnimationForFrameNames(
                new string[] { "walkup0.png", "walkup1.png", "walkup2.png", "walkup3.png", "walkup4.png", "walkup5.png", "walkup6.png", "walkup7.png", },
                animationTime, Color.White, true));

            AddAnimation("left", sheet.AnimationForFrameNames(
                new string[] { "walkleft0.png", "walkleft1.png", "walkleft2.png", "walkleft3.png", "walkleft4.png", "walkleft5.png", "walkleft6.png", "walkleft7.png", },
                animationTime, Color.White, true));

            AddAnimation("right", sheet.AnimationForFrameNames(
                new string[] { "walkright0.png", "walkright1.png", "walkright2.png", "walkright3.png", "walkright4.png", "walkright5.png", "walkright6.png", "walkright7.png", },
                animationTime, Color.White, true));

            AddAnimation("rightup", sheet.AnimationForFrameNames(
                new string[] { "walkrightup0.png", "walkrightup1.png", "walkrightup2.png", "walkrightup3.png", "walkrightup4.png", "walkrightup5.png", "walkrightup6.png", "walkrightup7.png", },
                animationTime, Color.White, true));

            AddAnimation("rightdown", sheet.AnimationForFrameNames(
                new string[] { "walkrightdown0.png", "walkrightdown1.png", "walkrightdown2.png", "walkrightdown3.png", "walkrightdown4.png", "walkrightdown5.png", "walkrightdown6.png", "walkrightdown7.png", },
                animationTime, Color.White, true));

            AddAnimation("leftdown", sheet.AnimationForFrameNames(
                new string[] { "walkleftdown0.png", "walkleftdown1.png", "walkleftdown2.png", "walkleftdown3.png", "walkleftdown4.png", "walkleftdown5.png", "walkleftdown6.png", "walkleftdown7.png", },
                animationTime, Color.White, true));

            AddAnimation("leftup", sheet.AnimationForFrameNames(
                new string[] { "walkleftup0.png", "walkleftup1.png", "walkleftup2.png", "walkleftup3.png", "walkleftup4.png", "walkleftup5.png", "walkleftup6.png", "walkleftup7.png", },
                animationTime, Color.White, true));

        }

    }
}
