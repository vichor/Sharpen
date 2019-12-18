using System;
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

        private float[] _vertices =
        {
			-0.5f,  0.5f, 0f,       // a
			-0.5f, -0.5f, 0f,       // b
			 0.5f,  0.5f, 0f,       // c

       		 0.5f,  0.5f, 0f,       // c
			-0.5f, -0.5f, 0f,       // b
			 0.5f, -0.5f, 0f,       // d

       		 0.5f,  0.5f, 0f,       // c
			 0.5f, -0.5f, 0f,       // d
             0.75f, 0f, 0f,         // e

			-0.5f, 0.5f,  0f,       // a
			 0.5f, 0.5f,  0f,       // c
             0f,   0.75f, 0f,       // f

			-0.5f, -0.5f,  0f,      // b
             0f,   -0.75f, 0f,      // f
			 0.5f, -0.5f,  0f,      // d

			-0.5f,  0.5f, 0f,       // a
            -0.75f, 0f,   0f,       // g
			-0.5f, -0.5f, 0f,       // b

        };
        private Mesh _model;

        public Window(int width, int height, IApplication app) : 
            base(width, height, GraphicsMode.Default, app.Title) 
        {
            State = _WindowState.Starting;
            _application = app;
            _loader = new Loader();
            _renderer = new BasicRenderer();
            _model = null;

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
            _model = _loader.LoadMesh(_vertices);
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
            _renderer.RenderFrame(_model);
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
