
using System;
using OpenTK.Graphics.OpenGL4;

namespace Sharpen.RenderEngine
{
    
    internal class BasicRenderer: IDisposable
    {
        private const float CLEAR_RED   = 50.0f/255.0f;
        private const float CLEAR_GREEN = 60.0f/255.0f;
        private const float CLEAR_BLUE  = 110.0f/255.0f;
        private const float CLEAR_ALPHA = 1.0f;

        private ShaderProgram _shader;
        private int vertexCoordinatesLocation;
        private int textureCoordinatesLocation;

        public BasicRenderer()
        {
            _shader = new ShaderProgram("Basic");
            vertexCoordinatesLocation = _shader.GetAttributeLocation("vertexCoordinates");
            textureCoordinatesLocation = _shader.GetAttributeLocation("textureCoordinates");
        }
        public void PrepareFrame()
        {
            GL.ClearColor(CLEAR_RED, CLEAR_GREEN, CLEAR_BLUE, CLEAR_ALPHA);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void RenderFrame(Entity entity)
        {
            StartShader();
            entity.bindToRender(vertexCoordinatesLocation, textureCoordinatesLocation);
            _shader.SetMatrix4("transformation", entity.transformEntity());
            GL.DrawElements(BeginMode.Triangles, entity.model.VertexCount, DrawElementsType.UnsignedInt, 0);
            entity.releaseFromRender(vertexCoordinatesLocation, textureCoordinatesLocation);
            StopShader();
        }

        public void StartShader()
        {
            _shader.Run();
        }

        public void StopShader()
        {
            _shader.Stop();
        }

        public void Dispose()
        {
            _shader.Dispose();
        }

    }
}