using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        //----------------------------------------------------------------------
        // Bird
        //----------------------------------------------------------------------
        public class Bird : RasterSprite
        {
            public static readonly float GRAVITY = 0.1f;
            public static readonly float JUMP = -4f;
            public static readonly float TERMINAL_VELOCITY = 8f;
            string[] sprites = new string[3];
            int frame = 0;
            int frameDelay = 0;
            int frameDelayMax = 5;
            GameInput input;
            Vector2 velocity = Vector2.Zero;
            public Vector2 Velocity { get { return velocity; } set { velocity = value; } }
            public Bird(Vector2 position, string sprites,GameInput input) : base(position, DEFAULT_PIXEL_SCALE, Vector2.Zero, sprites)
            {
                this.sprites[0] = getPixels(0, 0, (int)(Size.X/3), (int)Size.Y);
                this.sprites[1] = getPixels((int)(Size.X / 3), 0, (int)(Size.X / 3), (int)Size.Y);
                this.sprites[2] = getPixels((int)(Size.X / 3)*2, 0, (int)(Size.X / 3), (int)Size.Y);
                // recalculate size
                string[] lines = this.sprites[0].Split('\n');
                Size = new Vector2(lines[0].Length, lines.Length);
                // replace pink pixels with transparent pixels
                this.sprites[0] = this.sprites[0].Replace(IGNORE.ToString(),INVISIBLE);
                this.sprites[1] = this.sprites[1].Replace(IGNORE.ToString(), INVISIBLE);
                this.sprites[2] = this.sprites[2].Replace(IGNORE.ToString(), INVISIBLE);
                // set default sprite
                Data = this.sprites[0];
                this.input = input;
            }
            public void Update(Vector2 _viewport)
            {
                if (input.PlayerPresent)
                {
                    if (input.SpacePressed)
                    {
                        frameDelay = 0;
                        frame = 2;
                        Data = sprites[frame];
                        velocity = new Vector2(0, JUMP);
                    }
                    else if (frame > 0 && frameDelay++ >= frameDelayMax)
                    {
                        frameDelay = 0;
                        frame--;
                        Data = sprites[frame];
                    }

                    velocity += new Vector2(0, GRAVITY);
                    if(velocity.Y > TERMINAL_VELOCITY)
                    {
                        velocity.Y = TERMINAL_VELOCITY;
                    }   
                    Position += velocity;
                    if(Position.Y > _viewport.Y - 50 - PixelToScreen(Size).Y)
                    {
                        Position = new Vector2(Position.X, _viewport.Y - 50 - PixelToScreen(Size).Y);
                        velocity = Vector2.Zero;
                        FlappyGame.GameOver();
                    }
                }
            }
        }
        //----------------------------------------------------------------------
    }
}
