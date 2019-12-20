    
using OpenTK.Input;
using Sharpen.Interface;

namespace SharpenTest
{
    internal class TestGame : IApplication
    {
        public enum ApplicationState { Run, Exit }
        public ApplicationState State { get; private set; }

        public string Title { get; set; }

        private float[] _vertices =
        {
			-0.5f,  0.5f, 0f,       // 0
			-0.5f, -0.5f, 0f,       // 1
			 0.5f,  0.5f, 0f,       // 2
			 0.5f, -0.5f, 0f,       // 3
             0.75f, 0f, 0f,         // 4
             0f,   0.75f, 0f,       // 5
             0f,   -0.75f, 0f,      // 6
            -0.75f, 0f,   0f,       // 7
        };
        private int[] _indices =
        {
            0, 1, 2,
            2, 1, 3,
            2, 3, 4,
            0, 2, 5,
            1, 6, 3,
            0, 7, 1,
        };


        public TestGame(string newTitle)
        {
            Title = newTitle;
            State = ApplicationState.Run;
        }

        public void Engage()
        {
            Sharpen.Engine.Loader().LoadMesh(_vertices, _indices);
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