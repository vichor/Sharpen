    
using OpenTK.Input;
using Sharpen.Interface;

namespace SharpenTest
{
    class TestApplication : IApplication
    {
        public enum ApplicationState { Run, Exit }
        private ApplicationState _state;
        public ApplicationState State { 
            get => _state; 
            set => _state = value; 
        }

        private string _title;
        public string Title 
        {
            get => _title;                
            set => _title = value;
        }

        public TestApplication(string newTitle)
        {
            _title = newTitle;
            _state = ApplicationState.Run;
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