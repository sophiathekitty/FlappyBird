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
    partial class Program : MyGridProgram
    {
        //=======================================================================
        FlappyGame game;
        Sign sign;
        public Program()
        {
            Echo("Flappy Bird Booting.....");
            GridInfo.Init("FlappyBird", this);
            GridBlocks.InitBlocks(GridTerminalSystem);
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            game = new FlappyGame(GridBlocks.GetTextSurface("Main Display"));
            sign = new Sign(GridBlocks.GetTextSurface("Sign"));
            Echo("FlappyGame Initialized");
        }

        public void Save()
        {
            GridInfo.Save();
        }

        public void Main(string argument, UpdateType updateSource)
        {
            game.Main(argument);
            sign.Draw();
        }
        //=======================================================================
    }
}
