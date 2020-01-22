using System.Collections.Generic;

namespace Sharpen
{
    public static class Engine
    {
        static private RenderEngine.Window _window = null;
        static private RenderEngine.Loader _loader = null;
        static private List<RenderEngine.Entity> _entities = new List<RenderEngine.Entity>();

        public static RenderEngine.Window Create(Interface.IApplication app)
        {
            if (_window == null)
            {
                _window = new RenderEngine.Window(800, 600, app);
            }
            return _window;
        }

        public static void Run(float appTime, float renderTime)
        {
            if (_window != null)
            {
                _window.Run(renderTime, appTime);
            }
        }
        
        public static RenderEngine.Loader Loader()
        {
            if (_loader == null)
            {
                _loader = new RenderEngine.Loader();
            }
            return _loader;
        }

        public static void RegisterEntity(RenderEngine.Entity entity)
        {
            _entities.Add(entity);
        }

        public static List<RenderEngine.Entity> GetEntities()
        {
            return _entities;
        }
    }
}