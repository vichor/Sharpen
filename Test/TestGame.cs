using OpenTK.Input;
using Sharpen;
using Sharpen.Interface;
using Sharpen.RenderEngine;

using l = Serilog.Log;    

namespace SharpenTest
{
    internal class TestGame : IApplication
    {
        public enum ApplicationState { Run, Pause, Exit }
        public ApplicationState State { get; private set; }

        public string Title { get; set; }

        private float[] _vertices =
        {
			-0.5f,  0.5f,  0f,      // 0
			-0.5f, -0.5f,  0f,      // 1
			 0.5f,  0.5f,  0f,      // 2
			 0.5f, -0.5f,  0f,      // 3
             0.75f, 0f,    0f,      // 4
             0f,    0.75f, 0f,      // 5
             0f,   -0.75f, 0f,      // 6
            -0.75f, 0f,    0f,      // 7
             0f,    0.75f, -0.25f,  // 8 (5b)
            -0.75f, 0f,    -0.25f,  // 9 (7b)
        };
        private int[] _indices =
        {
            0, 1, 2,  // triangle 1
            2, 1, 3,  // triangle 2
            2, 3, 4,  // triangle 3
            0, 2, 5,  // triangle 4
            1, 6, 3,  // triangle 5
            0, 7, 1,  // triangle 6
            8, 0, 5,  // triangle 7
            9, 7, 0,  // triangle 8
            8, 9, 0,  // triangle 9
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
            0.125f, 0.5f,       // texture coordinate for vertex 7
            0.0f,   0.0f,       // texture coordinate for vertex 8
            0.0f,   0.5f,       // texture coordinate for vertex 9
        };

        private Entity _entity;
        private bool _goingFar=true;
        private double _epoch = 0.0;

        public TestGame(string newTitle)
        {
            Title = newTitle;
            State = ApplicationState.Pause;
        }

        public void Engage()
        {
            _entity = Sharpen.Engine.Loader().LoadEntity(_vertices, _indices, _textureCoordinates, "example.png");
            _entity.Position.Z = -1.0f;
            _entity.Orientation.Y = 65f;
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
            if ( (Engine.RunningTime - _epoch) > 3.0)
            {
                if (State == ApplicationState.Run)
                {
                    l.Information($"changing direction at {Engine.RunningTime}");
                    _goingFar = !_goingFar;
                }
                _epoch = Engine.RunningTime;
            }

            if (State == ApplicationState.Run)
            {
                _entity.Orientation.Y += 2f;
                if (_goingFar)
                {
                    _entity.Position.Z -= 0.05f;
                }
                else
                {
                    _entity.Position.Z += 0.05f;
                }
                if (((int)_epoch / 2) % 2 == 0)
                {
                    _entity.Position.X -= 0.025f;
                }
                else
                {
                    _entity.Position.X += 0.025f;
                }
            }
        }

        private void Input() 
        {
            var input = Keyboard.GetState();
            if (input.IsKeyDown(Key.Escape)) 
            {
                l.Information($"exiting at {Engine.RunningTime}");
                State = ApplicationState.Exit;
            }
            else if (input.IsKeyDown(Key.Space))
            {
                if (State == ApplicationState.Run)
                {
                    l.Information($"pausing at {Engine.RunningTime}");
                    State = ApplicationState.Pause;
                }
                else 
                {
                    l.Information($"resuming at {Engine.RunningTime}");
                    State = ApplicationState.Run;
                }
            }
        }
    }

}