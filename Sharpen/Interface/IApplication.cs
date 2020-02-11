

namespace Sharpen.Interface
{
    /// <summary>Interface <c>IApplication</c> to interact the user application with Sharpen.</summary>
    /// <remarks>
    ///     When developing a game using Sharpen, a custom class shall be created which implements
    ///     this interface. The main target is to supply the initialization, execution and finish
    ///     condition to the engine. This will be handled by the methods required by the interface.
    ///     <para>
    ///     It is recommended to create a higher level engine which can be used later on to create,
    ///     based on configuration, similar games. the interaction between this high level engine
    ///     and Sharpen, the low level, will be through this interface.
    ///     Following this approach, the game logic will be found in configuration of the higher
    ///     engine, instead of being distributed among different methods launched asynchronously
    ///     by the low level engine.
    ///     </para>
    /// </remarks>
    public interface IApplication
    {
        /// <value>Game title</value>
        string  Title { get; set; }

        /// <summary>Startup the application once there is a valid graphics context</summary>
        void Engage(); 

        /// <summary>Executes an application step. To be called from the main application manager.</summary>
        void Step();

        /// <summary>Tests for application finish condition.</summary>
        /// <returns>Boolean indicating if the application wants to finish</returns>
        bool IsFinished();
    }

}
