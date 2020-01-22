    
using OpenTK.Input;
using Sharpen.Interface;
using Sharpen.RenderEngine;

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
            0, 1, 2,  // triangle 1
            2, 1, 3,  // triangle 2
            2, 3, 4,  // triangle 3
            0, 2, 5,  // triangle 4
            1, 6, 3,  // triangle 5
            0, 7, 1,  // triangle 6
        };

        private float[] _textureCoordinates =
        {
            // (vertex coordinate + 1) / 2
            0.25f,  0.25f,      // texture coordinate for vertex 0
            0.25f,  0.75f,      // texture coordinate for vertex 1
            0.75f,  0.25f,      // texture coordinate for vertex 2
            0.75f,  0.75f,      // texture coordinate for vertex 3
            0.875f, 0.5f,       // texture coordinate for vertex 4
            0.5f,   0.125f,     // texture coordinate for vertex 5
            0.5f,   0.875f,     // texture coordinate for vertex 6
            0.125f, 0.5f        // texture coordinate for vertex 7
        };

        private Entity entity;

        public TestGame(string newTitle)
        {
            Title = newTitle;
            State = ApplicationState.Run;
        }

        public void Engage()
        {
            entity = Sharpen.Engine.Loader().LoadEntity(_vertices, _indices, _textureCoordinates, "example.png");
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