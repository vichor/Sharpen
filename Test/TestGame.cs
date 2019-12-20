    
using OpenTK.Input;
using Sharpen.Interface;

namespace SharpenTest
{
    class TestGame : IApplication
    {
        public enum ApplicationState { Run, Exit }
        public ApplicationState State { get; private set; }

        public string Title { get; set; }

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


        public TestGame(string newTitle)
        {
            Title = newTitle;
            State = ApplicationState.Run;
        }

        public void Engage()
        {
            Sharpen.Engine.Loader().LoadMesh(_vertices);
        }

        public void Step()
        {
            Input();
            Logic();
        }

        public bool IsFinished()
        {
            return State == ApplicationState.Exit;
        }

        private void Logic()
        {
        }

        private void Input() 
        {
            var input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape)) 
            {
                State = ApplicationState.Exit;
            }
        }
    }

}