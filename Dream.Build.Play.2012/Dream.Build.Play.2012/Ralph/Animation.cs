using System;
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
            sourceRectangle = new Rectangle(frames[frameCount].x, frames[frameCount].y,
                                        frames[frameCount].width, frames[frameCount].height);

            destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y,
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
