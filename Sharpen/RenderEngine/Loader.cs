using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using l = Serilog.Log;

namespace Sharpen.RenderEngine 
{
    /// <summary>Class <c>Loader</c> is an object creation factory for Sharpen.</summary>
    public class Loader : IDisposable
    {
        private List<int> vaos = new List<int>();
        private List<int> vbos = new List<int>();
        private List<int> textureIds = new List<int>();

        /// <summary>Loads a <see><c>Mesh</c></see> from the given data.</summary>
        /// <remarks>
        ///     The data received to build the <see><c>Mesh</c></see> comes from
        ///     the 3D model data.
        /// </remarks>
        /// <param name="vertices">Array of vertex coordinates.</param>
        /// <param name="indices">Array detailing how the triangle primitives shall 
        /// be constructed from vertex data.</param>
        /// <param name="textureCoordinates">Array detailing how to map the texture to the Mesh.</param>
        /// <returns><see><c>Created Mesh</c></see></returns>
        public Mesh LoadMesh(float[] vertices, int[] indices, float[] textureCoordinates)
        {
            // Vertex Buffer Object (VBO) for vertex coordinates
            int vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length*sizeof(float), vertices, BufferUsageHint.StaticDraw);

            // Vertex Buffer Object (VBO) for texture coordinates
            int tbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, tbo);
            GL.BufferData(BufferTarget.ArrayBuffer, textureCoordinates.Length*sizeof(float), textureCoordinates, BufferUsageHint.StaticDraw);

            // Element Buffer Object (EBO)
            int ebo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length*sizeof(int), indices, BufferUsageHint.StaticDraw);

            // Vertex Attribute Object (VAO)
            int vao = GL.GenVertexArray();                                            // Generate a new VAO
            GL.BindVertexArray(vao);                                                  // Bind the VAO. Everything we do now will affect it
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);                             // Bind the vertex buffer object for vertices to the VAO, location 0
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 0, 0); // configure data for vertex positions on location 0 of the vao
            GL.BindBuffer(BufferTarget.ArrayBuffer, tbo);                             // Bind the vertex buffer object for texture coordinates to the VAO, location 1
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, 0, 0); // configure data for texture positions on location 1 of the vao
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);                      // Bind the element buffer object to the VAO, location 2
            GL.BindVertexArray(0);                                                    // Unbind the VAO

            // Store for later disposal
            vaos.Add(vao);
            vbos.Add(vbo);
            vbos.Add(tbo);
            vbos.Add(ebo);

            // Create Mesh object
            Mesh mesh = new Mesh(vao, indices.Length);
            return mesh;
        }

        /// <summary>Loads the given texture into the render pipeline.</summary>
        /// <param name="path">File name and path to the graphic texture file.</param>
        /// <returns><see><c>Texture</c></see> object.</returns>
        public Texture LoadTexture(string path)
        {
            var texture = new Texture(path);
            textureIds.Add(texture.Id);
            return texture;
        }

        /// <summary>Creates a new <see><c>Entity</c></see> using the received 3D model and texture data.</summary>
        /// <param name="vertices">3D coordinates for each of the vertices of the 3D model.</param>
        /// <param name="indices">order in which the vertices shall be applied to build the triangle primitives of the 3D model.</param>
        /// <param name="textureCoordinates">Indicates, for each vertex, the texture pixel to use.</param>
        /// <param name="texturePath">Path and file name of the graphical file to use for the texture</param>
        /// <seealso>Entity</seealso>
        public Entity LoadEntity (float[] vertices, int[] indices, float[] textureCoordinates, string texturePath)
        {
            var mesh = LoadMesh(vertices, indices, textureCoordinates);
            var texture = LoadTexture(texturePath);
            var entity = new Entity(mesh, texture);
            Engine.RegisterEntity(entity);
            return entity;
        }

        /// <summary>Clean-up method to be called when exiting.</summary>
        public void Dispose()
        {
            l.Information("Disposing loader");
            foreach (var vao in vaos) { GL.DeleteVertexArray(vao); }
            foreach (var vbo in vbos) { GL.DeleteBuffer(vbo); }
            foreach (var texture in textureIds) { GL.DeleteTexture(texture); }
        }

    }
}
