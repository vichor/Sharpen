using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace Sharpen.RenderEngine
{
    /// <summary>The class <c>Entity</c> is a graphical entity renderable into the 
    /// world. It is created from a 3D model and a texture.</summary>
    public class Entity
    {
        /// <summary>Internal representation of the entity's 3D model. </summary>
        public readonly Mesh model;

        /// <summary>Entity's texture </summary>
        public readonly Texture texture;

        /// <summary>Entity position in world coordinates.
        /// <par>The three coordinates are named X, Y and Z and can be accessed directly as a public attribute.
        /// <example>myentity.Position.X += 2.3f;
        /// </example></par></summary>
        /// <remarks>TODO: Maybe create a custom class for this instead of publishing to the outside OpenTk's Vector3</remarks>
        public Vector3 Position; // TODO: Decide how this attribute should be accessed: specific methods?

        /// <summary>Entity orientation of each of the axis expressed in degrees.
        /// <par>The three axis are named X, Y and Z and can be accessed directly as a public attribute.
        /// <example>myentity.Orientation.Y = 45f;
        /// </example></par></summary>
        /// <remarks>TODO: Maybe create a custom class for this instead of publishing to the outside OpenTk's Vector3</remarks>
        public Vector3 Orientation; 

        /// <summary>Scale of the entity.
        /// <par>A scale of 1.0f means original object size. This is useful to adapt the size of the different 
        /// entities when placing them in the world, independently of the size during the 3D model development.
        /// </par></summary>
        public float Scale {set; get; }

        /// <summary>Constructor of an Entity from a given mesh and texture</summary>
        /// <param name="mesh">The mesh the entity refers to.</param>
        /// <param name="skin">The texture that the entity when use when rendering.</param>
        /// <returns>The new Entity object</returns>
        /// <remarks>It is recommended to use the <see><c>Loader</c></see> class to create instances of Entity. 
        /// It will take care to create the <c>Mesh</c> and load the graphics file for the <c>Texture</c>
        /// </remarks>
        /// <seealso>Loader</seealso>
        public Entity (Mesh mesh, Texture skin)
        {
            model = mesh;
            texture = skin;
            Position = new Vector3(0.0f, 0.0f, 0.0f);
            Orientation = new Vector3(0.0f, 0.0f, 0.0f);
            Scale = 1.0f;
        }

        /// <summary>Binds the entity to the render pipeline.</summary>
        /// <param name="vertexLocation">OpenGL VAO for the vertex coordinates to enable.</param>
        /// <param name="textureLocation">OpenGL VAO for the texture coordinates to enable.</param>
        public void bindToRender(int vertexLocation, int textureLocation)
        {
            GL.BindVertexArray(model.VaoId);
            GL.EnableVertexAttribArray(vertexLocation);
            GL.EnableVertexAttribArray(textureLocation);
            texture.Use();
        }


        /// <summary>Unbinds the entity from the render pipeline.</summary>
        /// <param name="vertexLocation">OpenGL VAO for the vertex coordinates to disable.</param>
        /// <param name="textureLocation">OpenGL VAO for the texture coordinates to disable.</param>
        public void releaseFromRender(int vertexLocation, int textureLocation)
        {
            texture.Release();
            GL.DisableVertexAttribArray(vertexLocation);
            GL.DisableVertexAttribArray(textureLocation);
            GL.BindVertexArray(0);
        }

        /// <summary>Creates the model/view matrix for the entity.</summary>
        /// <remarks>
        ///     It uses internal <see><c>Position</c></see> and 
        ///     <see><c>Orientation</c></see> to calculate the transformation
        ///     model/view matrix.
        ///     <para>
        ///     TODO: Consider using internal type if this has to be exposed to the applicaiton
        ///     </para>
        /// </remarks>
        /// <returns>The transformation matrix</returns>
        public Matrix4 transformEntity()
        {
            Matrix4 transformation = Matrix4.Identity;
            transformation *= Matrix4.CreateScale(Scale);
            transformation *= Matrix4.CreateRotationX(MathHelper.DegreesToRadians(Orientation.X));
            transformation *= Matrix4.CreateRotationY(MathHelper.DegreesToRadians(Orientation.Y));
            transformation *= Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(Orientation.Z));
            transformation *= Matrix4.CreateTranslation(Position);
            return transformation;
        }
    }
}