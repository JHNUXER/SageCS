﻿using OpenTK;
using OpenTK.Input;
using SageCS.Core.Graphics;
using System;
using System.IO;

using SageCS.Audio;
using SageCS.Core.Loaders;
using System.Diagnostics;

namespace SageCS.Core
{
    class Engine : GameWindow
    {
        ~Engine()
        {
            Renderer.DeleteBuffers();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.WindowBorder = WindowBorder.Hidden;
            base.OnLoad(e);

            Title = "SageCS - BFME II";

            Renderer.shaders.Add("textured", new Shader(Resource.GetShader("tex.vert"), Resource.GetShader("tex.frag")));
            Renderer.activeShader = "textured";
            
            try
            {
                Renderer.textures.Add("splash", new Texture(File.Open("GermanSplash.jpg", FileMode.Open)).ID());
            }
            catch
            {
                Renderer.textures.Add("splash", new Texture(File.Open("EnglishSplash.jpg", FileMode.Open)).ID());
            }
            
            Sprite background = new Sprite("splash");

            Renderer.initProgram(Width, Height);

            FileSystem.Init();
            AudioSystem.Init();
            var s = FileSystem.Open("language.ini");
            StreamReader sr = new StreamReader(s);
            string content = sr.ReadToEnd();

            Stopwatch stopwatch = Stopwatch.StartNew();

            W3DLoader.Load(FileSystem.Open("art\\w3d\\gu\\gumaarms_skn.w3d"));
            W3DLoader.Load(FileSystem.Open("art\\w3d\\gu\\gumaarms_runa.w3d"));
            W3DLoader.Load(FileSystem.Open("art\\w3d\\gu\\gumaarms_skl.w3d"));

            INIParser.Parse(FileSystem.Open("data\\ini\\gamedata.ini"));

            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            Renderer.render();
            base.SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (Keyboard[Key.Escape])
            {
                Exit();
            }
            Renderer.update();
        }
    }
}
