
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

        public BasicRenderer()
        {
            _shader = new ShaderProgram("Basic");
        }
        public void PrepareFrame()
        {
            GL.ClearColor(CLEAR_RED, CLEAR_GREEN, CLEAR_BLUE, CLEAR_ALPHA);
            GL.Clear(ClearBufferMask.ColorBufferBit);
        }

        public void RenderFrame(Mesh model)
        {
            GL.BindVertexArray(model.VaoId);
            // Enable automatic filling of the attribute (VAO row)
            GL.EnableVertexAttribArray(0);
            GL.DrawElements(BeginMode.Triangles, model.VertexCount, DrawElementsType.UnsignedInt, 0);
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
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