using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace Sharpen.RenderEngine
{
    class Loader
    {
        private List<int> vaos = new List<int>();
        private List<int> vbos = new List<int>();

        public Mesh LoadMesh(float[] vertices)
        {
            int vaoId = CreateVao();
            StoreDataInAttributeList(0, vertices);
            UnbindVao();
            return new Mesh(vaoId, vertices.Length/3);
        }

        private int CreateVao()
        {
            int vao = GL.GenVertexArray();
            vaos.Add(vao);
            GL.BindVertexArray(vao);
            return vao;
        }

        private void StoreDataInAttributeList(int attributeNumber, float[] data)
        {
            int vbo = GL.GenBuffer();
            vbos.Add(vbo);
            // Bind the buffer, load the data and describe the format of the data
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length*sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeNumber, 3, VertexAttribPointerType.Float, false, 0, 0);
        }

        private void UnbindVao()
        {
            GL.BindVertexArray(0);
        }

        public void CleanUp()
        {
            foreach (var vao in vaos) { GL.DeleteVertexArray(vao); }
            foreach (var vbo in vbos) { GL.DeleteBuffer(vbo); }
        }
    }
}