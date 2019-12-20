using System.Collections.Generic;

namespace Sharpen
{
    static class Engine
    {
        static public RenderEngine.Window _window = null;
        static public RenderEngine.Loader _loader = null;
        static private List<RenderEngine.Mesh> _meshes = new List<RenderEngine.Mesh>();

        public static RenderEngine.Window CreateWindow(Interface.IApplication app)
        {
            if (_window == null)
            {
                _window = new RenderEngine.Window(800, 600, app);
            }
            return _window;
        }

        public static void Run(float renderTime, float appTime)
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

        public static void RegisterMesh(RenderEngine.Mesh mesh)
        {
            _meshes.Add(mesh);
        }

        public static List<RenderEngine.Mesh> GetMeshes()
        {
            return _meshes;
        }
    }
}