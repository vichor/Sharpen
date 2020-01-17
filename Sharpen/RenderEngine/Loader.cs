using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using l = Serilog.Log;

namespace Sharpen.RenderEngine 
{
    public class Loader : IDisposable
    {
        private List<int> vaos = new List<int>();
        private List<int> vbos = new List<int>();

        public Mesh LoadMesh(float[] vertices, int[] indices)
        {
            int vaoId = CreateVao();
            BindIndicesBuffer(indices);
            StoreDataInAttributeList(0, vertices);
            UnbindVao();
            Mesh mesh = new Mesh(vaoId, indices.Length);
            Engine.RegisterMesh(mesh);
            return mesh;
        }

        private int CreateVao()
        {
            int vao = GL.GenVertexArray();
            vaos.Add(vao);
            GL.BindVertexArray(vao);
            return vao;
        }

        private void BindIndicesBuffer(int[] indices)
        {
            int vboId = GL.GenBuffer();
            vbos.Add(vboId);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vboId);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length*sizeof(int), indices, BufferUsageHint.StaticDraw);
        }

        private void StoreDataInAttributeList(int attributeNumber, float[] data)
        {
            int vbo = GL.GenBuffer();
            vbos.Add(vbo);
            // Bind the buffer, load the data and describe the format of the data
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length*sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(attributeNumber, 3, VertexAttribPointerType.Float, false, 0, 0);
            // Unbind the buffer
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        private void UnbindVao()
        {
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void Dispose()
        {
            l.Information("Disposing loader");
            foreach (var vao in vaos) { GL.DeleteVertexArray(vao); }
            foreach (var vbo in vbos) { GL.DeleteBuffer(vbo); }
        }

    }
}
