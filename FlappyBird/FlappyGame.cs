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
            public static Action GameOver;
            public static int Score = 0;
            public int HighScore
            {
                get
                {
                    return GridInfo.GetVarAs("High Score", 0);
                }
                set
                {
                    GridInfo.SetVar("High Score", value.ToString());
                    highScoreDisplay.Text = value.ToString();
                }
            }
            BitmapFontSprite scoreDisplay;
            HorizontalParalaxSprite background;
            HorizontalParalaxSprite ground;
            Bird bird;
            GameInput input;
            List<PipeGate> pipes = new List<PipeGate>();
            ScreenSprite title;
            ScreenSprite gameOverSprite;
            ScreenSprite highScoreLabel;
            BitmapFontSprite highScoreDisplay;
            ScreenSprite PressSpace;
            bool gameOver = true;
            Random random = new Random();
            public FlappyGame(IMyTextSurface drawingSurface) : base(drawingSurface)
            {
                RasterSprite.DEFAULT_PIXEL_SCALE = 0.05f;
                RasterSprite.PIXEL_TO_SCREEN_RATIO = 28.7f;
                background = new HorizontalParalaxSprite(Vector2.Zero,SpriteLoader.Background);
                AddSprite(background);
                ground = new HorizontalParalaxSprite(new Vector2(0, Size.Y - 50), SpriteLoader.Ground,RasterSprite.DEFAULT_PIXEL_SCALE);
                AddSprite(ground);
                input = new GameInput(GridBlocks.GetPlayer());
                bird = new Bird(Size/2, SpriteLoader.BirdSprites, input);
                bird.Position = (Size/3) - (bird.PixelToScreen(bird.Size)/2);
                AddSprite(bird);
                GameOver += isGameOver;
                scoreDisplay = new BitmapFontSprite(new Vector2(Size.X/2, Size.Y*0.01f), SpriteLoader.Font,"01234\n56789","0",TextAlignment.CENTER);
                AddSprite(scoreDisplay);
                highScoreLabel = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopLeft, new Vector2(Size.X/2, Size.Y * 0.45f),1f, Vector2.Zero, Color.White,"Monospace", "Best", TextAlignment.CENTER,SpriteType.TEXT);
                AddSprite(highScoreLabel);
                highScoreLabel.Visible = false;
                highScoreDisplay = new BitmapFontSprite(new Vector2(Size.X / 2, Size.Y * 0.55f), SpriteLoader.Font, "01234\n56789", HighScore.ToString(), TextAlignment.CENTER);
                AddSprite(highScoreDisplay);
                highScoreDisplay.Visible = false;
                title = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopLeft, new Vector2(Size.X / 2, Size.Y * 0.25f), RasterSprite.DEFAULT_PIXEL_SCALE, Vector2.Zero, Color.White, "Monospace", SpriteLoader.Title, TextAlignment.CENTER, SpriteType.TEXT);
                AddSprite(title);
                gameOverSprite = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopLeft, new Vector2(Size.X / 2, Size.Y * 0.25f), RasterSprite.DEFAULT_PIXEL_SCALE, Vector2.Zero, Color.White, "Monospace", SpriteLoader.GameOver, TextAlignment.CENTER, SpriteType.TEXT);
                AddSprite(gameOverSprite);
                gameOverSprite.Visible = false;
                PressSpace = new ScreenSprite(ScreenSprite.ScreenSpriteAnchor.TopLeft, new Vector2(Size.X / 2, Size.Y /2 ), 1f, Vector2.Zero, Color.White, "Monospace", "Press Space", TextAlignment.CENTER, SpriteType.TEXT);
                AddSprite(PressSpace);
                PressSpace.Visible = false;
                Reset();
                gameOver = true;
                if(!input.PlayerPresent) {
                    title.Visible = true;
                    scoreDisplay.Visible = false;
                    PressSpace.Visible = true;
                }
            }
            // game over event
            void isGameOver()
            {
                gameOver = true;
                gameOverSprite.Visible = true;
                highScoreDisplay.Visible = true;
                highScoreLabel.Visible = true;
                if (Score > HighScore) HighScore = Score;
                RemoveSprite(highScoreDisplay);
                AddSprite(highScoreDisplay);
                RemoveSprite(gameOverSprite);
                AddSprite(gameOverSprite);
                RemoveSprite(highScoreLabel);
                AddSprite(highScoreLabel);
            }
            void Reset()
            {
                bird.Position = (Size / 3) - (bird.PixelToScreen(bird.Size) / 2);
                bird.Velocity = Vector2.Zero;
                foreach(PipeGate p in pipes) RemoveSprite(p);
                pipes.Clear();
                Vector2 pipePos = new Vector2(Size.X * 0.75f, Size.Y / 2f);
                for(int i = 0; i < 4; i++)
                {
                    PipeGate pipe = new PipeGate(new Vector2(pipePos.X, (float)random.NextDouble() * (Size.Y / 2) + (Size.Y / 4)), random.Next(150,250));
                    pipes.Add(pipe);
                    AddSprite(pipe);
                    pipePos.X += random.Next(200, 300);
                }
                gameOver = false;
                gameOverSprite.Visible = false;
                title.Visible = false;
                highScoreLabel.Visible = false;
                highScoreDisplay.Visible = false;
                PressSpace.Visible = false;
                Score = 0;
                scoreDisplay.Text = "0";
                RemoveSprite(scoreDisplay);
                AddSprite(scoreDisplay);
            }
            bool playerPresent = false;
            public override void Update()
            {
                if(playerPresent != input.PlayerPresent)
                {
                    playerPresent = input.PlayerPresent;
                    if (playerPresent)
                    {
                        title.Visible = false;
                        scoreDisplay.Visible = true;
                        PressSpace.Visible = true;
                    }
                    else
                    {
                        title.Visible = true;
                        RemoveSprite(title);
                        AddSprite(title);
                        gameOver = true;
                        gameOverSprite.Visible = false;
                        highScoreLabel.Visible = false;
                        highScoreDisplay.Visible = false;
                        scoreDisplay.Visible = false;
                    }
                }
                if (gameOver || !input.PlayerPresent)
                {
                    if(input.PlayerPresent && input.SpacePressed) Reset();
                    return;
                }
                // game logic go here
                bird.Update(Size);
                background.Delta = new Vector2(-0.75f, 0);
                ground.Delta = new Vector2(-1f, 0);
                for(int i = 0; i < pipes.Count; i++)
                {
                    PipeGate pipe = pipes[i];
                    pipe.X -= 1f;
                    if (pipe.X < -100)
                    {
                        RemoveSprite(pipe);
                        pipes.Remove(pipe);
                        PipeGate newPipe = new PipeGate(new Vector2(pipes[pipes.Count-1].Position.X + random.Next(200, 300), (float)random.NextDouble() * (Size.Y / 2) + (Size.Y / 4)), random.Next(100, 250));
                        pipes.Add(newPipe);
                        AddSprite(newPipe);
                    }
                    if (pipe.Intersect(bird))
                    {
                        GameOver();
                        break;
                    }
                    if (pipe.X < bird.Position.X && !pipe.Scored)
                    {
                        pipe.Scored = true;
                        Score++;
                        scoreDisplay.Text = Score.ToString();
                    }
                }
            }
        }
        //----------------------------------------------------------------------
    }
}
