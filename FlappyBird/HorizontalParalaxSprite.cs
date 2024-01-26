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
        // HorizontalParalaxSprite
        //----------------------------------------------------------------------
        public class HorizontalParalaxSprite : IScreenSpriteProvider
        {
            RasterSprite sprite;
            RasterSprite sprite2;
            Vector2 position;
            RectangleF viewPort;
            Vector2 size;
            public Vector2 Position
            {
                get
                {
                    return position;
                }
                set
                {
                    Delta = value - position;
                    position.X = value.X;
                }
            }
            public Vector2 Delta
            {
                set
                {
                    value.Y = 0;
                    sprite.Position += value;
                    sprite2.Position += value;
                    if(value.X > 0)
                    {
                        if(sprite.Position.X > viewPort.X + viewPort.Width)
                        {
                            sprite.Position = sprite2.Position - new Vector2(size.X, 0);
                        }
                        else if(sprite2.Position.X > viewPort.X + viewPort.Width)
                        {
                            sprite2.Position = sprite.Position - new Vector2(size.X, 0);
                        }
                    }
                    else
                    {
                        if (sprite.Position.X < sprite2.Position.X && sprite.Position.X < viewPort.X - size.X)
                        {
                            sprite.Position = sprite2.Position + new Vector2(size.X, 0);
                        }
                        else if (sprite2.Position.X < sprite.Position.X && sprite2.Position.X < viewPort.X - size.X)
                        {
                            sprite2.Position = sprite.Position + new Vector2(size.X, 0);
                        }
                    }
                }
            }
            void IScreenSpriteProvider.AddToScreen(Screen screen)
            {
                viewPort = screen.Viewport;
                screen.AddSprite(sprite);
                screen.AddSprite(sprite2);
            }

            void IScreenSpriteProvider.RemoveToScreen(Screen screen)
            {
                screen.RemoveSprite(sprite);
                screen.RemoveSprite(sprite2);
            }
            public HorizontalParalaxSprite(Vector2 position, string spriteData, float scale = 0.1f)
            {
                this.position = position;
                sprite = new RasterSprite(position,scale,Vector2.Zero,spriteData);
                sprite2 = new RasterSprite(position + new Vector2(sprite.Size.X, 0), scale, Vector2.Zero, spriteData);
                size = sprite.PixelToScreen(sprite.Size);
                sprite2.Position = sprite.Position + new Vector2(size.X, 0);
            }
        }
        //----------------------------------------------------------------------
    }
}
