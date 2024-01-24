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
            string[] sprites = new string[3];
            int frame = 0;
            int frameDelay = 0;
            int frameDelayMax = 5;
            GameInput input;
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
            public override MySprite ToMySprite(RectangleF _viewport)
            {
                if (input.PlayerPresent && input.SpacePressed)
                {
                    frameDelay = 0;
                    frame = 2;
                    Data = sprites[frame];
                }
                else if (frame > 0 && frameDelay++ >= frameDelayMax)
                {
                    frameDelay = 0;
                    frame--;
                    Data = sprites[frame];
                }
                return base.ToMySprite(_viewport);
            }
        }
        //----------------------------------------------------------------------
    }
}
