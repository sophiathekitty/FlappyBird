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
using VRage.GameServices;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        //----------------------------------------------------------------------
        // PipeGate
        //----------------------------------------------------------------------
        public class PipeGate : IScreenSpriteProvider
        {
            RasterSprite top;
            RasterSprite bottom;
            float gap;
            Vector2 position;
            public bool Scored { get; set; }
            public Vector2 Position
            {
                get
                {
                    return position;
                }
                set
                {
                    position = value;
                    Vector2 screenSize = top.PixelToScreen(top.Size);
                    top.Position = new Vector2(position.X, position.Y - (gap / 2) - screenSize.Y);
                    bottom.Position = new Vector2(position.X, position.Y + (gap / 2));
                }
            }
            public float X 
            { 
                get { return position.X; } 
                set 
                { 
                    position.X = value; 
                    top.Position = new Vector2(value,top.Position.Y); 
                    bottom.Position = new Vector2(value,bottom.Position.Y);
                }
            }
            public float Gap
            {
                get { return gap; }
                set
                {
                    gap = value;
                    Position = position;
                }
            }
            public PipeGate(Vector2 position, float gap)
            {
                this.gap = gap;
                this.position = position;
                top = new RasterSprite(position,RasterSprite.DEFAULT_PIXEL_SCALE,Vector2.Zero, SpriteLoader.Pipe);
                bottom = new RasterSprite(position, RasterSprite.DEFAULT_PIXEL_SCALE, top.Size, top.Data);
                top.FlipVertical();
                Vector2 screenSize = top.PixelToScreen(top.Size);
                top.Position = new Vector2(position.X, position.Y - (gap/2) - screenSize.Y);
                bottom.Position = new Vector2(position.X, position.Y + (gap / 2));
            }
            void IScreenSpriteProvider.AddToScreen(Screen screen)
            {
                screen.AddSprite(top);
                screen.AddSprite(bottom);
            }

            void IScreenSpriteProvider.RemoveToScreen(Screen screen)
            {
                screen.RemoveSprite(top);
                screen.RemoveSprite(bottom);
            }
            public bool Intersect(RasterSprite bird)
            {
                return top.Intersect(bird,true) || bottom.Intersect(bird,true);
            }
        }
        //----------------------------------------------------------------------
    }
}
