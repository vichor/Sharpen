

namespace Sharpen.Interface
{
    public interface IApplication
    {
        string  Title { get; set; }

        // Startup the application once there is a valid graphics context
        void Engage(); 

        // Executes an application step. To be called from the main application
        // manager.
        void Step();

        // Tests for application finished
        bool IsFinished();
    }

}
