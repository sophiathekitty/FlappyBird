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
        // FlappyGame
        //----------------------------------------------------------------------
        public class FlappyGame : Screen
        {
            Bird bird;
            GameInput input;
            public FlappyGame(IMyTextSurface drawingSurface) : base(drawingSurface)
            {
                input = new GameInput(GridBlocks.GetPlayer());
                bird = new Bird((Size/2) - (bird.PixelToScreen(bird.Size)/2), SpriteLoader.BirdSprites, input);
                AddSprite(bird);
            }
            public override void Update()
            {
                // game logic go here
            }
        }
        //----------------------------------------------------------------------
    }
}
