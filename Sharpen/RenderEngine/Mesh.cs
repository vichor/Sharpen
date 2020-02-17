using l = Serilog.Log;

namespace Sharpen.RenderEngine
{
    /// <summary>Class <c>Mesh</c> defines the OpenGL representation of a 3D model.</summary>
    /// <remarks>
    ///     It is recommended to create Mesh objects by using the <see><c>Loader.LoadMesh</c></see>) method.
    ///     <para>
    ///     The <see><c>Loader.LoadMesh</c></see> method creates all the needed OpenGL bindings and provide the IDs
    ///     needed to create a <c>Mesh</c> object correctly.
    ///     </para>
    /// </remarks>
    internal class Mesh
    {
        /// <value>OpenGL Vertex array object identifier</value>
        public int VaoId { get; private set; }
        /// <value>Number of vertex to this <c>Mesh</c>.</value>
        public int VertexCount { get; private set; }
        /// <summary>Creates a <c>Mesh</c></summary>
        /// <param name="vaoId">OpenGL VAO name.</param>
        /// <param name="vertexCount">Number of vertexes of the represented mesh.</param>
        public Mesh(int vaoId, int vertexCount)
        {
            VaoId = vaoId;
            VertexCount = vertexCount;
            l.Information($"Created Mesh: vao={vaoId}, vertexCount={vertexCount}");
        }
    }
    
}