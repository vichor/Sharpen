using l = Serilog.Log;

namespace Sharpen.RenderEngine
{
    class Mesh
    {
        public int VaoId { get; private set; }
        public int VertexCount { get; private set; }
        public Mesh(int vaoId, int vertexCount)
        {
            VaoId = vaoId;
            VertexCount = vertexCount;
            l.Information($"Created Mesh: vao={vaoId}, vertexCount={vertexCount}");
        }
    }
    
}