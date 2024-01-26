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
        // Flappy Sprite Loader
        //----------------------------------------------------------------------
        public class SpriteLoader
        {
            public static string BirdSprites
            {
                get
                {
                    IMyTextPanel panel = GridBlocks.GetTextPanel("Main Display");
                    if (panel != null)
                    {
                        return panel.CustomData;
                    }
                    return "";
                }
            }
            public static string Pipe
            {
                get
                {
                    IMyLightingBlock block = GridBlocks.GetLight("Pipe");
                    if (block != null)
                    {
                        return block.CustomData;
                    }
                    return "";
                }
            }
            public static string Ground
            {
                get
                {
                    IMyLightingBlock block = GridBlocks.GetLight("Base");
                    if (block != null)
                    {
                        return block.CustomData;
                    }
                    return "";
                }
            }
            public static string Background
            {
                get
                {
                    IMyLightingBlock block = GridBlocks.GetLight("Background");
                    if (block != null)
                    {
                        return block.CustomData;
                    }
                    return "";
                }
            }
            public static string Font
            {
                get
                {
                    IMyShipController controller = GridBlocks.GetPlayer();
                    if (controller != null)
                    {
                        return controller.CustomData;
                    }
                    return "";
                }
            }
            public static string Title
            {
                get
                {
                    IMyTextPanel panel = GridBlocks.GetTextPanel("Sign");
                    if (panel != null)
                    {
                        return panel.GetText();
                    }
                    return "";
                }
            }
            public static string GameOver
            {
                get
                {
                    IMyTextPanel panel = GridBlocks.GetTextPanel("Sign");
                    if (panel != null)
                    {
                        return panel.CustomData;
                    }
                    return "";
                }
            }
        }
    }
}
