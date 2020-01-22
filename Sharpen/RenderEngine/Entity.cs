using OpenTK.Graphics.OpenGL4;

namespace Sharpen.RenderEngine
{
    public class Entity
    {
        public readonly Mesh model;
        public readonly Texture texture;

        public Entity (Mesh mesh, Texture skin)
        {
            model = mesh;
            texture = skin;
        }

        public void bindToRender(int vertexLocation, int textureLocation)
        {
            GL.BindVertexArray(model.VaoId);
            GL.EnableVertexAttribArray(vertexLocation);
            GL.EnableVertexAttribArray(textureLocation);
            texture.Use();
        }

        public void releaseFromRender(int vertexLocation, int textureLocation)
        {
            texture.Release();
            GL.DisableVertexAttribArray(vertexLocation);
            GL.DisableVertexAttribArray(textureLocation);
            GL.BindVertexArray(0);
        }
    }
}