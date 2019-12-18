using OpenTK;
using OpenTK.Graphics;
using Sharpen.Interface;

namespace Sharpen.RenderEngine
{
    public class Window : GameWindow 
    {
        public enum _WindowState { Starting, Running, Exiting };
        private _WindowState _state = _WindowState.Starting;
        public _WindowState State 
        { 
            get => _state; 
            set => _state = value; 
        }

        private IApplication _application;

        public Window(int width, int height, IApplication app) : 
            base(width, height, GraphicsMode.Default, app.Title) 
        {
           State = _WindowState.Starting;
           _application = app;
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
            // The Renderer shall be a specific object created when the Window
            // is opened (OnLoad).
            RenderLoop();
            base.OnRenderFrame(e);
        }

        private void RenderLoop()
        {

        }

        // Window related application level logic
        private void InternalLogic()
        {
            if (_application.IsFinished())
            {
                Exit();
            }
        }
    }
}
