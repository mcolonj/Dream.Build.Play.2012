/*
 * HighBeta Dream.Build.Play 2012 : http://github.com/mcolonj/Dream.Build.Play.2012
 *
 * Copyright (c) 2012 Ralph Sharp.
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
 *
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Dream.Build.Play._2012
{
    struct Frame
    {
        public int x, y, offsetX, offsetY, width, height, originalWidth, originalHeight;
    }
    
    class Animation
    {
        Texture2D spriteStrip;

        float scaleOfStrip;

        int timeSinceUpdate;

        int frameTime;

        int numOfFrames;

        int frameCount;

        Color frameColor;

        Rectangle sourceRectangle;

        Rectangle destinationRectangle;

        public int FrameWidth;

        public int FrameHeight;

        public bool IsActive;

        public bool IsLooping;

        public Vector2 Position;

        private Frame[] frames;


        public void Initialize(Texture2D texture, Vector2 position,
                                    int frameWidth, int frameHeight, int numOfFrames,
                                    int frameTime, Color frameColor, float scale, 
                                    bool isLooping)
        {
            this.frameColor = frameColor;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.numOfFrames = numOfFrames;
            this.frameTime = frameTime;
            this.scaleOfStrip = scale;

            IsLooping = isLooping;
            Position = position;
            spriteStrip = texture;

            timeSinceUpdate = 0;
            frameCount = 0;

            frames = new Frame[numOfFrames];
            for (int i = 0; i < numOfFrames; i++)
            {
                frames[i].x = (FrameWidth/numOfFrames) * i;
                frames[i].y = 0;
                frames[i].width = FrameWidth/numOfFrames;
                frames[i].height = frameHeight;
            }

            IsActive = true;
        }


        public void Initialize(Texture2D texture, Vector2 position, int numOfFrames,
                                    int frameTime, Color frameColor, float scale,
                                    bool isLooping)
        {
            this.frameColor = frameColor;
            this.FrameWidth = texture.Width;
            this.FrameHeight = texture.Height;
            this.numOfFrames = numOfFrames;
            this.frameTime = frameTime;
            this.scaleOfStrip = scale;

            IsLooping = isLooping;
            Position = position;
            spriteStrip = texture;

            timeSinceUpdate = 0;
            frameCount = 0;

            frames = new Frame[numOfFrames];

            for (int i = 0; i < numOfFrames; i++)
            {
                frames[i].x = (FrameWidth / numOfFrames) * i;
                frames[i].y = 0;
                frames[i].width = FrameWidth / numOfFrames;
                frames[i].height = FrameHeight;
            }

            IsActive = true;
        }

        public void Initialize(Texture2D texture, Frame[] animFrames,
                                    int frameTime, Color frameColor,
                                    bool isLooping)
        {
            this.frameColor = frameColor;
            this.frameTime = frameTime;
            //this.scaleOfStrip = scale;

            IsLooping = isLooping;
            spriteStrip = texture;

            timeSinceUpdate = 0;
            frameCount = 0;
        
            frames = animFrames;
            numOfFrames = frames.Length;
            IsActive = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsActive)
            {
                return;
            }

            timeSinceUpdate += (int)gameTime.ElapsedGameTime.TotalMilliseconds;

            if (timeSinceUpdate > frameTime)
            {

                frameCount++;

                if (frameCount == numOfFrames)
                {
                    frameCount = 0; // Resets to zero to set up for loop or finish

                    if (!IsLooping)
                    {
                        IsActive = false;
                    }
                }

                timeSinceUpdate = 0;  //Marks as updated

            }
            // Only works with one row of sprites this line is key.
            sourceRectangle = new Rectangle(frames[frameCount].x,
                                            frames[frameCount].y,
                                        frames[frameCount].width, frames[frameCount].height);

            int paddingWidth = ((frames[frameCount].originalWidth - frames[frameCount].width) / 2);
            int paddingHeight = ((frames[frameCount].originalHeight - frames[frameCount].height) / 2);

            destinationRectangle = new Rectangle((int)Position.X + paddingWidth + frames[frameCount].offsetX,
                                                    (int)Position.Y + paddingHeight + frames[frameCount].offsetY,
                                                    (int)(frames[frameCount].width),
                                                    (int)(frames[frameCount].height));


        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive)
            {
                spriteBatch.Draw(spriteStrip, destinationRectangle,
                                 sourceRectangle, frameColor);
            }
        }


        ///////////////////////////////////////////////

       
    }
}
