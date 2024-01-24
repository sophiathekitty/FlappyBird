﻿using Sandbox.Game.EntityComponents;
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
        // Screen - encapsulates a drawing surface
        //----------------------------------------------------------------------
        public class Screen
        {
            private IMyTextSurface _drawingSurface;
            private RectangleF _viewport;
            private List<ScreenSprite> _sprites = new List<ScreenSprite>();
            public float bottomPadding = 0f;
            public float topPadding = 0f;
            public float leftPadding = 0f;
            public float rightPadding = 0f;
            public bool hasColorfulIcons
            {
                get
                {
                    List<string> sprites = new List<string>();
                    _drawingSurface.GetSprites(sprites);
                    return sprites.Contains("ColorfulIcons_Ore/Iron");
                }
            }
            public Color BackgroundColor
            {
                get { return _drawingSurface.ScriptBackgroundColor; }
                set { _drawingSurface.ScriptBackgroundColor = value; }
            }
            public Color ForegroundColor
            {
                get { return _drawingSurface.ScriptForegroundColor; }
                set { _drawingSurface.ScriptForegroundColor = value; }
            }
            public Vector2 Size
            {
                get { return _drawingSurface.SurfaceSize; }
            }
            //
            // constructor
            //
            public Screen(IMyTextSurface drawingSurface)
            {
                _drawingSurface = drawingSurface;
                _drawingSurface.ContentType = ContentType.SCRIPT;
                _drawingSurface.Script = "";
                // calculate the viewport offset by centering the surface size onto the texture size
                _viewport = new RectangleF(
                                       (_drawingSurface.TextureSize - _drawingSurface.SurfaceSize) / 2f,
                                                          _drawingSurface.SurfaceSize
                                                                         );
            }
            public virtual void Update()
            {
                //GridInfo.Echo("Screen: Update");
            }
            public virtual void Main(string argument)
            {
                Update();
                Draw();
            }
            //
            // DrawSprites - draw sprites to the screen
            //
            public virtual void Draw()
            {
                //GridInfo.Echo("Screen: Draw");
                var frame = _drawingSurface.DrawFrame();
                DrawSprites(ref frame);
                frame.Dispose();
            }
            //
            // DrawSprites - draw sprites to the screen
            //
            private void DrawSprites(ref MySpriteDrawFrame frame)
            {
                //GridInfo.Echo("Screen: DrawSprites: "+_sprites.Count.ToString());
                // draw all the sprites
                foreach (ScreenSprite sprite in _sprites)
                {
                    //GridInfo.Echo("Screen: DrawSprites: sprite: viewport: "+_viewport.ToString());
                    //if(sprite != null) GridInfo.Echo("Screen: DrawSprites: sprite: position: "+sprite.GetPosition(_viewport).ToString());
                    //else GridInfo.Echo("Screen: DrawSprites: sprite: null");
                    if (sprite != null && sprite.Visible) frame.Add(sprite.ToMySprite(_viewport));
                }
            }
            // add a text sprite
            public ScreenSprite AddTextSprite(ScreenSprite.ScreenSpriteAnchor anchor, Vector2 position, float rotationOrScale, Color color, string fontId, string data, TextAlignment alignment)
            {
                ScreenSprite sprite = new ScreenSprite(anchor, position, rotationOrScale, new Vector2(0, 0), color, fontId, data, alignment, SpriteType.TEXT);
                _sprites.Add(sprite);
                return sprite;
            }
            // add a texture sprite
            public ScreenSprite AddTextureSprite(ScreenSprite.ScreenSpriteAnchor anchor, Vector2 position, float rotationOrScale, Vector2 size, Color color, string data)
            {
                ScreenSprite sprite = new ScreenSprite(anchor, position, rotationOrScale, size, color, "", data, TextAlignment.CENTER, SpriteType.TEXTURE);
                _sprites.Add(sprite);
                return sprite;
            }
            public void AddSprite(ScreenSprite sprite)
            {
                _sprites.Add(sprite);
            }
            public void AddSprite(IScreenSpriteProvider spriteProvider)
            {
                spriteProvider.AddToScreen(this);
            }
            public void RemoveSprite(ScreenSprite sprite)
            {
                if (_sprites.Contains(sprite)) _sprites.Remove(sprite);
            }
            public void RemoveSprite(IScreenSpriteProvider spriteProvider)
            {
                spriteProvider.RemoveToScreen(this);
            }
        }
        //----------------------------------------------------------------------
        // screen sprite - encapsulates a sprite
        //----------------------------------------------------------------------
        public class ScreenSprite
        {
            public static float DEFAULT_FONT_SIZE = 1.5f;
            public static float MONOSPACE_FONT_SIZE = 0.2f;
            public enum ScreenSpriteAnchor
            {
                TopLeft,
                TopCenter,
                TopRight,
                CenterLeft,
                Center,
                CenterRight,
                BottomLeft,
                BottomCenter,
                BottomRight
            }
            public ScreenSpriteAnchor Anchor { get; set; }
            public Vector2 Position { get; set; }
            public float RotationOrScale { get; set; }
            public Vector2 Size { get; set; }
            public Color Color { get; set; }
            public string FontId { get; set; }
            public string Data { get; set; }
            public TextAlignment Alignment { get; set; }
            public SpriteType Type { get; set; }
            public bool Visible { get; set; } = true;
            // constructor
            public ScreenSprite()
            {
                Anchor = ScreenSpriteAnchor.Center;
                Position = Vector2.Zero;
                RotationOrScale = 1f;
                Size = Vector2.Zero;
                Color = Color.White;
                FontId = "White";
                Data = "";
                Alignment = TextAlignment.CENTER;
                Type = SpriteType.TEXT;
            }
            // constructor
            public ScreenSprite(ScreenSpriteAnchor anchor, Vector2 position, float rotationOrScale, Vector2 size, Color color, string fontId, string data, TextAlignment alignment, SpriteType type)
            {
                Anchor = anchor;
                Position = position;
                RotationOrScale = rotationOrScale;
                Size = size;
                Color = color;
                FontId = fontId;
                Data = data;
                Alignment = alignment;
                Type = type;
            }
            // convert the sprite to a MySprite
            public virtual MySprite ToMySprite(RectangleF _viewport)
            {
                if (Type == SpriteType.TEXT)
                {
                    return new MySprite()
                    {
                        Type = Type,
                        Data = Data,
                        Position = GetPosition(_viewport),
                        RotationOrScale = RotationOrScale,
                        Color = Color,
                        Alignment = Alignment,
                        FontId = FontId
                    };
                }
                return new MySprite()
                {
                    Type = Type,
                    Data = Data,
                    Position = GetPosition(_viewport),
                    RotationOrScale = RotationOrScale,
                    Color = Color,
                    Alignment = Alignment,
                    Size = Size,
                    FontId = FontId
                };
            }
            public Vector2 GetPosition(RectangleF _viewport)
            {
                Vector2 _position = Position + _viewport.Position;
                switch (Anchor)
                {
                    case ScreenSpriteAnchor.TopCenter:
                        _position = Position + new Vector2(_viewport.Center.X, _viewport.Y);
                        break;
                    case ScreenSpriteAnchor.TopRight:
                        _position = Position + new Vector2(_viewport.Right, _viewport.Y);
                        break;
                    case ScreenSpriteAnchor.CenterLeft:
                        _position = Position + new Vector2(_viewport.X, _viewport.Center.Y);
                        break;
                    case ScreenSpriteAnchor.Center:
                        _position = Position + _viewport.Center;
                        break;
                    case ScreenSpriteAnchor.CenterRight:
                        _position = Position + new Vector2(_viewport.Right, _viewport.Center.Y);
                        break;
                    case ScreenSpriteAnchor.BottomLeft:
                        _position = Position + new Vector2(_viewport.X, _viewport.Bottom);
                        break;
                    case ScreenSpriteAnchor.BottomCenter:
                        _position = Position + new Vector2(_viewport.Center.X, _viewport.Bottom);
                        break;
                    case ScreenSpriteAnchor.BottomRight:
                        _position = Position + new Vector2(_viewport.Right, _viewport.Bottom);
                        break;
                }
                return _position;
            }

        }
        //----------------------------------------------------------------------
        // IScreenSpriteProvider - for classes that include sprites
        //----------------------------------------------------------------------
        public interface IScreenSpriteProvider
        {
            void AddToScreen(Screen screen);
            void RemoveToScreen(Screen screen);
        }
        public class RasterSprite : ScreenSprite
        {
            public static float PIXEL_TO_SCREEN_RATIO = 30f; // line height of monospace font at 1f scale
            public static float DEFAULT_PIXEL_SCALE = 0.1f; // the default scale for a monospace image
            public static char INVISIBLE = ''; //(char)0xE100;// maybe? 

            public RasterSprite(ScreenSpriteAnchor anchor, Vector2 position, float scale, Vector2 size, string data, TextAlignment alignment = TextAlignment.LEFT) : base(anchor, position, scale, size, Color.White, "Monospace", data, TextAlignment.LEFT, SpriteType.TEXT)
            {
                if(size == Vector2.Zero)
                {
                    string[] lines = data.Split('\n');
                    Size = new Vector2(lines[0].Length, lines.Length);
                }
            }
            public Vector2 PixelToScreen(Vector2 pixel)
            {
                return pixel * PIXEL_TO_SCREEN_RATIO * RotationOrScale;
            }
            public Vector2 PixelPosToScreenPos(Vector2 pixel)
            {
                return pixel * PIXEL_TO_SCREEN_RATIO * RotationOrScale + Position;
            }
            public Vector2 ScreenToPixel(Vector2 screen)
            {
                Vector2 res = screen / (PIXEL_TO_SCREEN_RATIO * RotationOrScale);
                return new Vector2((int)res.X, (int)res.Y);
            }
            public Vector2 ScreenPosToPixelPos(Vector2 screen)
            {
                Vector2 res = (screen - Position) / (PIXEL_TO_SCREEN_RATIO * RotationOrScale);
                return new Vector2((int)res.X, (int)res.Y);
            }
            // bytes between 0 - 7
            // usage: PixelIcon.rgb(0, 0, 7);
            public static char rgb(byte r, byte g, byte b)
            {
                return (char)(0xE100 + (r << 6) + (g << 3) + b);
            }
            // remap an int from 0-255 to a byte from 0-7
            public static byte remap(int value)
            {
                if (value < 0) return 0;
                if (value > 255) return 7;
                return (byte)(value / 32);
            }
            //
            // draw functions
            //
            // add a pixel to the icon
            // ints between 0 - 255
            Vector2 _addPosition = Vector2.Zero;
            public void addPixelRGB(int r, int g, int b)
            {
                if (_addPosition.Y >= Size.Y) return;
                Data += rgb(remap(r), remap(g), remap(b));
                _addPosition.X += 1f;
                if (_addPosition.X >= Size.X)
                {
                    _addPosition.X = 0f;
                    _addPosition.Y += 1f;
                    Data += "\n";
                }
            }
            // fill the icon with a color
            // ints between 0 - 255
            public void fillRGB(int r, int g, int b)
            {
                Data = "";
                for (int y = 0; y < Size.Y; y++)
                {
                    for (int x = 0; x < Size.X; x++)
                    {
                        addPixelRGB(r, g, b);
                    }
                }
            }
            public void fillRGB(Color color)
            {
                fillRGB(color.R, color.G, color.B);
            }
            // set a pixel to a color at a position
            // ints between 0 - 255
            public void setPixelRGB(int x, int y, int r, int g, int b)
            {
                if (x < 0 || x >= Size.X || y < 0 || y >= Size.Y) return;
                Data = Data.Remove((int)(y * (Size.X + 1) + x), 1);
                Data = Data.Insert((int)(y * (Size.X + 1) + x), rgb(remap(r), remap(g), remap(b)).ToString());
            }
            // get a box of pixels
            public string getPixels(int x, int y, int width, int height)
            {
                // make sure the box is within the Size range
                if (x < 0 || x >= Size.X || y < 0 || y >= Size.Y) return "error";
                string pixels = "";
                for (int y1 = y; y1 < y + height; y1++)
                {
                    pixels += Data.Substring((int)(y1 * (Size.X + 1) + x), width);
                    /*
                    for (int x1 = x; x1 < x + width; x1++)
                    {
                        pixels += Data[(int)(y1 * (Size.X + 1) + x1)];
                    }
                    */
                    pixels += "\n";
                }
                return pixels;
            }
            // draw a box of pixels over the image
            public void drawPixels(int x, int y, string pixels)
            {
                string[] lines = pixels.Split('\n');
                int width = lines[0].Length;
                int height = lines.Length;
                // make sure the box is within the Size range
                if (x < 0 || x+width >= Size.X || y < 0 || y+height >= Size.Y) return;
                int i = 0;
                for (int y1 = y; y1 < y + height; y1++)
                {
                    Data = Data.Remove((int)(y1 * (Size.X + 1) + x), width);
                    Data = Data.Insert((int)(y1 * (Size.X + 1) + x), lines[i]);
                    i++;
                    /*
                    for (int x1 = x; x1 < x + width; x1++)
                    {
                        // if the pixel is not invisible, draw it
                        if (pixels[i] != INVISIBLE)
                        {
                            Data = Data.Remove((int)(y1 * (Size.X + 1) + x1), 1);
                            Data = Data.Insert((int)(y1 * (Size.X + 1) + x1), pixels[i].ToString());
                        }
                        i++;
                    }
                    i++;
                    */
                }
            }
            // draw a line from x1,y1 to x2,y2
            // ints between 0 - 255
            public void drawLineRGB(int x1, int y1, int x2, int y2, int r, int g, int b)
            {
                int dx = Math.Abs(x2 - x1);
                int dy = Math.Abs(y2 - y1);
                int sx = (x1 < x2) ? 1 : -1;
                int sy = (y1 < y2) ? 1 : -1;
                int err = dx - dy;
                while (true)
                {
                    setPixelRGB(x1, y1, r, g, b);
                    if ((x1 == x2) && (y1 == y2)) break;
                    int e2 = 2 * err;
                    if (e2 > -dy)
                    {
                        err -= dy;
                        x1 += sx;
                    }
                    if (e2 < dx)
                    {
                        err += dx;
                        y1 += sy;
                    }
                }
            }
            public void drawLineRGB(Vector2 start, Vector2 end, Color color)
            {
                drawLineRGB((int)start.X, (int)start.Y, (int)end.X, (int)end.Y, color.R, color.G, color.B);
            }
            // draw a rectangle from x1,y1 to x2,y2
            // ints between 0 - 255
            public void drawRectRGB(int x1, int y1, int x2, int y2, int r, int g, int b)
            {
                drawLineRGB(x1, y1, x2, y1, r, g, b);
                drawLineRGB(x2, y1, x2, y2, r, g, b);
                drawLineRGB(x2, y2, x1, y2, r, g, b);
                drawLineRGB(x1, y2, x1, y1, r, g, b);
            }
            public void drawRectRGB(Vector2 start, Vector2 end, Color color)
            {
                drawRectRGB((int)start.X, (int)start.Y, (int)end.X, (int)end.Y, color.R, color.G, color.B);
            }
            // fill a rectangle from x1,y1 to x2,y2
            // ints between 0 - 255
            public void fillRectRGB(int x1, int y1, int x2, int y2, int r, int g, int b)
            {
                for (int y = y1; y <= y2; y++)
                {
                    drawLineRGB(x1, y, x2, y, r, g, b);
                }
            }
            public void fillRectRGB(Vector2 start, Vector2 end, Color color)
            {
                fillRectRGB((int)start.X, (int)start.Y, (int)end.X, (int)end.Y, color.R, color.G, color.B);
            }
            // draw a circle at x,y with radius r
            // ints between 0 - 255
            public void drawCircleRGB(int x, int y, int r, int red, int green, int blue)
            {
                int f = 1 - r;
                int ddF_x = 1;
                int ddF_y = -2 * r;
                int x1 = 0;
                int y1 = r;
                setPixelRGB(x, y + r, red, green, blue);
                setPixelRGB(x, y - r, red, green, blue);
                setPixelRGB(x + r, y, red, green, blue);
                setPixelRGB(x - r, y, red, green, blue);
                while (x1 < y1)
                {
                    if (f >= 0)
                    {
                        y1--;
                        ddF_y += 2;
                        f += ddF_y;
                    }
                    x1++;
                    ddF_x += 2;
                    f += ddF_x;
                    setPixelRGB(x + x1, y + y1, red, green, blue);
                    setPixelRGB(x - x1, y + y1, red, green, blue);
                    setPixelRGB(x + x1, y - y1, red, green, blue);
                    setPixelRGB(x - x1, y - y1, red, green, blue);
                    setPixelRGB(x + y1, y + x1, red, green, blue);
                    setPixelRGB(x - y1, y + x1, red, green, blue);
                    setPixelRGB(x + y1, y - x1, red, green, blue);
                    setPixelRGB(x - y1, y - x1, red, green, blue);
                }
            }
            // fill a circle at x,y with radius r
            // ints between 0 - 255
            public void fillCircleRGB(int x, int y, int r, int red, int green, int blue)
            {
                for (int y1 = -r; y1 <= r; y1++)
                    for (int x1 = -r; x1 <= r; x1++)
                        if (x1 * x1 + y1 * y1 <= r * r)
                            setPixelRGB(x + x1, y + y1, red, green, blue);
            }
        }
        //----------------------------------------------------------------------
        // end screen classes
        //----------------------------------------------------------------------
    }
}