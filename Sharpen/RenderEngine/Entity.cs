using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Sharpen.RenderEngine
{
    public class Entity
    {
        public readonly Mesh model;
        public readonly Texture texture;
        public Vector3 Position; // TODO: Decide how this attribute should be accessed: specific methods?
        public Vector3 Orientation; // TODO: Decide how this attribute should be accessed: specific methods?
        public float Scale {set; get; }

        public Entity (Mesh mesh, Texture skin)
        {
            model = mesh;
            texture = skin;
            Position = new Vector3(0.0f, 0.0f, 0.0f);
            Orientation = new Vector3(0.0f, 0.0f, 0.0f);
            Scale = 1.0f;
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

        public Matrix4 transformEntity()
        {
            Matrix4 transformation = Matrix4.Identity;
            transformation *= Matrix4.CreateScale(Scale);
            transformation *= Matrix4.CreateRotationX(Orientation.X);
            transformation *= Matrix4.CreateRotationY(Orientation.Y);
            transformation *= Matrix4.CreateRotationZ(Orientation.Z);
            transformation *= Matrix4.CreateTranslation(Position);
            return transformation;
        }
    }
}