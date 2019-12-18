using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;

namespace Sharpen {

    public class Window : GameWindow {

        enum    _WindowState { Idle, Running, Exiting};
        private _WindowState _state = _WindowState.Idle;

        public Window(int width, int height, string title) : 
            base(width, height, GraphicsMode.Default, title) 
        {
           _state = _WindowState.Running;
        }

        // Update the application state
        protected override void OnUpdateFrame(FrameEventArgs e) 
        {
            // The Application should be a different object implementing the
            // needed application to run. It will need to implement a
            // Sharpen.Application interface, pending to be defined.
            ApplicationLoop();
            base.OnUpdateFrame(e);

        }

        // Render the world defined by the application
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            // The Renderer shall be a specific object created when the Window
            // is opened.
            RenderLoop();
            base.OnRenderFrame(e);
        }

        private void ApplicationLoop()
        {
            // This may just be moved to Sharpen.Application or call to it
            Input();
            Logic();
        }

        private void RenderLoop()
        {

        }

        // Specific application method : to be moved to separate class. 
        // Considering moving along other items: window state, input loop, etc
        private void Logic()
        {
            if (_state == _WindowState.Exiting)
            {
                Exit();
            }
        }

        // Input loop method
        private void Input() 
        {
            var input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape)) 
            {
                _state = _WindowState.Exiting;
            }
        }
    }
}
