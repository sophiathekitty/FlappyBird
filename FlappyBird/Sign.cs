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
        // Sign - arcade cabinate sign
        //----------------------------------------------------------------------
        public class Sign : Screen
        {
            RasterSprite title;
            ScreenSprite highScoreLabel;
            public Sign(IMyTextSurface drawingSurface) : base(drawingSurface)
            {
                BackgroundColor = Color.Black;
                title = new RasterSprite(new Vector2(50,150),0.08f,Vector2.Zero,SpriteLoader.Title);
                highScoreLabel = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopCenter,new Vector2(0,title.Position.Y + title.PixelToScreen(title.Size).Y + 10),2.4f,Vector2.Zero,Color.White,"Monospace","Best",TextAlignment.CENTER,SpriteType.TEXT);
                AddSprite(title);
                AddSprite(highScoreLabel);
                GridInfo.AddChangeListener("High Score", UpdateVar);
                highScoreLabel.Data = "Best\n" + GridInfo.GetVarAs("High Score","0");

            }
            void UpdateVar(string key, string value)
            {
                if(key == "High Score")
                {
                    highScoreLabel.Data = "Best\n" + value;
                }
            }
        }
        //----------------------------------------------------------------------
    }
}
