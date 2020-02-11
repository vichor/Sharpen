using System;
using Serilog;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Sharpen.Interface;
using l = Serilog.Log;

namespace Sharpen.RenderEngine
{
    /// <summary>Manages the game window and represents the main interface with Sharpen.</summary>
    /// <remarks>
    ///     This class is the one created by the <see><c>Engine</c></see> class when
    ///     requested to create the engine. The resulting will be a window having the
    ///     OpenGL context bind to and ready to accept renderings.
    /// </remarks>
    public class Window : GameWindow, IDisposable
    {
        /// <value>Lifecycle stages of the window</value>
        public enum WindowStage 
        { 
            /// <value>The windowing subsystem is starting and not yet ready to be used.</value>
            Starting, 
            /// <value>The window is up and running and can be used in full.</value>
            Running, 
            /// <value>The window subsystem is shutting down.Execution is already stopped.</value>
            Exiting 
        };

        /// <value>Stores the window stage as described in <see><c>WindowState</c></see>.</value>
        public WindowStage State { get; private set; }

        /// <value>Execution time since window construction, expressed in seconds </value>
        public double RunningTime { get; private set; }

        private IApplication _application;
        private Loader _loader;
        private BasicRenderer _renderer;

        /// <summary>Creates a <c>Window</c> object leaving it in <see><c>Starting</c></see> state.</summary>
        /// <remarks>
        ///     Internally, this class contains a <see><c>RenderEngine</c></see> and a <see><c>Loader</c></see>.
        ///     This constructor will create these objects and store them in attributes.
        /// </remarks>
        /// <param name="width">Desired window width.</param>
        /// <param name="height">Desired window height.</param>
        /// <param name="app">Application to be used.</param>
        public Window(int width, int height, IApplication app) : 
            base(width, height, GraphicsMode.Default, app.Title) 
        {
            State = WindowStage.Starting;
            _application = app;
            _loader = Engine.Loader();
            _renderer = new BasicRenderer();
            RunningTime = 0.0f;

            var __log = new Serilog.LoggerConfiguration()
                .WriteTo.File("sharpen.log")
                .CreateLogger();
            l.Logger = __log;
            l.Information("Created window.");
        }

        /// <summary>Actions done once the <see><c>Starting</c></see> stage has finished.</summary>
        /// <param name="e">Defined by parent method. Not used at this level.</param>
        protected override void OnLoad(System.EventArgs e)
        {
            State = WindowStage.Running;
            _application.Engage();
            base.OnLoad(e);
        }

        /// <summary>Actions done for each application step</summary>
        /// <remarks>This method is the responsible for calling the application <c>Step</c>.</remarks>
        /// <param name="e">Object containing the ellapsed time</param>
        protected override void OnUpdateFrame(FrameEventArgs e) 
        {
            RunningTime += e.Time;
            _application.Step();
            InternalLogic();
            base.OnUpdateFrame(e);
        }

        /// <summary>Actions done for each render step</summary>
        /// <remarks>
        ///     This method is the responsible for starting the rendering of a frame. It will use the 
        ///     <see><c>RenderEngine</c></see> and request the swap of the OpenGL buffers.
        /// </remarks>
        /// <param name="e">Object containing the ellapsed time</param>
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"(Vsync: {VSync}) FPS: {1f / e.Time:0}";
            RenderLoop();
            SwapBuffers();
            base.OnRenderFrame(e);
        }

        /// <summary>Actions done when the window has been resized by the user.</summary>
        /// <param name="e">Defined by parent method. Not used at this level.</param>
        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
        }

        private void RenderLoop()
        {
            _renderer.PrepareFrame();
            _renderer.RenderFrame(Engine.GetEntities()[0]);
        }

        // Window related application level logic
        private void InternalLogic()
        {
            if (_application.IsFinished())
            {
                State = WindowStage.Exiting;
                Dispose();
                Exit();
            }
        }

        /// <summary>Clean-up method.</summary>
        public override void Dispose()
        {
            _renderer.Dispose();
            _loader.Dispose();
            l.Information("Disposing window");
            l.CloseAndFlush();
            base.Dispose();
        }
        
    }
}
