using System.Collections.Generic;

namespace Sharpen
{
    /// <summary>Represents the whole Sharpen engine and defines the main interface to it.</summary>
    /// <remarks>
    ///     This class is static and presents methods to interact with
    ///     the engine at high level. 
    ///     <para>
    ///     To start working with the engine, first, create a the engine by 
    ///     using the method <see><c>Create</c></see> and then use the method 
    ///     <see><c>Run</c></see> to execute it.
    ///     </para>
    ///     <example>
    ///         MyGame app = new MyGame("Sharpen Test"); // MyGame is a class implementing <see>IApplication</see>
    ///         Sharpen.Engine.Create(app, 800, 600);
    ///         Sharpen.Engine.Run(60.0f, 60.0f); 
    ///     </example>
    /// </remarks>
    public static class Engine
    {
        /// <value>Window width</value>
        static public int Width { get; private set; }

        /// <value>Window height</value>
        static public int Height { get; private set; }
        static private RenderEngine.Window _window = null;
        static private RenderEngine.Loader _loader = null;
        static private List<RenderEngine.Entity> _entities = new List<RenderEngine.Entity>();
        /// <value>Engine running time, expressed in seconds, and updated on each application step.</value>
        static public double RunningTime {
            get { return _window.RunningTime; }
        }

        /// <summary>Creates the Sharpen system and leaves it ready to be <see><c>Run</c></see></summary>
        /// <remarks>After the call to this method, the window and the OpenGL context will be available.</remarks>
        /// <param name="app">Application interface to link with this engine.</param>
        /// <param name="width">Window desired width.</param>
        /// <param name="height">Window desired height.</param>
        /// <returns>Reference to the window object to interact with.</returns>
        public static RenderEngine.Window Create(Interface.IApplication app, int width, int height)
        {
            if (_window == null)
            {
                Width = width;
                Height = height;
                _window = new RenderEngine.Window(Width, Height, app);
            }
            return _window;
        }

        /// <summary>Executes the created engine.</summary>
        /// <remarks>
        ///     Calling this method will give control to the engine. From that point
        ///     forward, the engine will use the supplied application interface to
        ///     execute the steps and will drive internally the render pipeline.
        ///     <para>
        ///     TODO: Maybe it's a good idea to hardcode the parameters simplifying the interface
        ///     </para>
        /// </remarks>
        /// <param name="appTime"> Executions per second for the application step.</param>
        /// <param name="renderTime"> Executions per second for the render step.</param>
        public static void Run(float appTime, float renderTime)
        {
            if (_window != null)
            {
                _window.Run(renderTime, appTime);
            }
        }
        
        /// <summary>Gets a reference for the objects <see><c>Loader</c></see>.</summary>
        /// <returns>The reference to the <see><c>Loader</c></see>.</returns>
        public static RenderEngine.Loader Loader()
        {
            if (_loader == null)
            {
                _loader = new RenderEngine.Loader();
            }
            return _loader;
        }

        /// <summary>Registers the given <see><c>Entity</c></see> into the render pipeline.</summary>
        /// <param name="entity"><see><c>Entity</c></see> to register.</param>
        public static void RegisterEntity(RenderEngine.Entity entity)
        {
            _entities.Add(entity);
        }

        /// <summary>Gets a list of all the registered entities.</summary>
        /// <returns>List of <see><c>Entity</c></see>.</returns>
        public static List<RenderEngine.Entity> GetEntities()
        {
            return _entities;
        }
    }
}