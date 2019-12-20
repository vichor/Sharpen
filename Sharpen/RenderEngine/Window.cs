﻿using System;
using Serilog;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Sharpen.Interface;
using l = Serilog.Log;

namespace Sharpen.RenderEngine
{
    public class Window : GameWindow 
    {
        public enum _WindowState { Starting, Running, Exiting };
        public _WindowState State { get; private set; }

        private IApplication _application;
        private Loader _loader;
        private BasicRenderer _renderer;



        public Window(int width, int height, IApplication app) : 
            base(width, height, GraphicsMode.Default, app.Title) 
        {
            State = _WindowState.Starting;
            _application = app;
            _loader = Engine.Loader();
            _renderer = new BasicRenderer();

            var __log = new Serilog.LoggerConfiguration()
                .WriteTo.File("sharpen.log")
                .CreateLogger();
            l.Logger = __log;
            //l.Information("Created window.");
        }

        protected override void OnLoad(System.EventArgs e)
        {
            State = _WindowState.Running;
            _application.Engage();
            base.OnLoad(e);
        }

        // Update the application state
        protected override void OnUpdateFrame(FrameEventArgs e) 
        {
            _application.Step();
            InternalLogic();
            base.OnUpdateFrame(e);
        }

        // Render the world defined by the application
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"(Vsync: {VSync}) FPS: {1f / e.Time:0}";
            RenderLoop(e.Time);
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        private void RenderLoop(double time)
        {
            _renderer.PrepareFrame();
            _renderer.RenderFrame(Engine.GetMeshes()[0]);
        }

        // Window related application level logic
        private void InternalLogic()
        {
            if (_application.IsFinished())
            {
                CleanUp();
                Exit();
            }
        }

        public void CleanUp()
        {
            _loader.CleanUp();
            l.CloseAndFlush();
        }
        
    }
}
