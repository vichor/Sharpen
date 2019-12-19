    
using OpenTK.Input;
using Sharpen.Interface;

namespace SharpenTest
{
    class TestGame : IApplication
    {
        public enum ApplicationState { Run, Exit }
        public ApplicationState State { get; private set; }

        public string Title { get; set; }

        public TestGame(string newTitle)
        {
            Title = newTitle;
            State = ApplicationState.Run;
        }

        public void Engage()
        {
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