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


namespace Dream.Build.Play._2012
{
    class Player
    {
        const int MAXHEALTH = 100;
        const int MAXENERGY = 100;
        //const int PLAYERSPEED = 100;


        Animation playerAnimation;
        Texture2D displayFrame { set; get; }

        public Vector2 Position;
        public bool Active;

        public void Initialize(Animation animation, Vector2 location)
        {
            playerAnimation = animation;
            Position = location;
            Active = false;
        }
        public void Initialize(Vector2 location)
        {
            Position = location;
            Active = false;
        }

        public void Update(Animation animation, GameTime gameTime)
        {
            if (playerAnimation != animation)
            {
                playerAnimation = animation;
            
            }
            playerAnimation.Position = Position;
            playerAnimation.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            playerAnimation.Draw(spriteBatch);
        }



    }
}
