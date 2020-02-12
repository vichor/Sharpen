
using System;
using OpenTK;
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
        private int _vertexCoordinatesLocation;
        private int _textureCoordinatesLocation;
        private Matrix4 _projection;

        public BasicRenderer()
        {
            _shader = new ShaderProgram("Basic");
            _vertexCoordinatesLocation = _shader.GetAttributeLocation("vertexCoordinates");
            _textureCoordinatesLocation = _shader.GetAttributeLocation("textureCoordinates");
            _projection = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(60f),
                Engine.Width / (float)Engine.Height,
                0.01f,
                100.0f);
        }
        public void PrepareFrame()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(CLEAR_RED, CLEAR_GREEN, CLEAR_BLUE, CLEAR_ALPHA);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        /// <summary>Executes the render pipeline.</summary>
        /// <remarks><para>TODO: This will need to be improved to render a list of entities</para></remarks>
        /// <param name="entity"><see><c>Entity</c></see> to be rendered.</param>
        public void RenderFrame(Entity entity)
        {
            StartShader();
            entity.bindToRender(_vertexCoordinatesLocation, _textureCoordinatesLocation);
            _shader.SetMatrix4("modelview", entity.transformEntity());
            _shader.SetMatrix4("projection", _projection);
            GL.DrawElements(BeginMode.Triangles, entity.model.VertexCount, DrawElementsType.UnsignedInt, 0);
            entity.releaseFromRender(_vertexCoordinatesLocation, _textureCoordinatesLocation);
            StopShader();
        }

        /// <summary>Starts the execution of the <see><c>Shader</c></see> of this renderer.</summary>
        public void StartShader()
        {
            _shader.Run();
        }

        /// <summary>Stops the execution of the <see><c>Shader</c></see> of this renderer.</summary>
        public void StopShader()
        {
            _shader.Stop();
        }

        /// <summary>Clean-up method.</summary>
        public void Dispose()
        {
            _shader.Dispose();
        }

    }
}