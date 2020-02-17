using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using l = Serilog.Log;

namespace Sharpen.RenderEngine
{
    /// <summary>Represents a texture object inside the graphical pipeline.</summary>
    internal class Texture
    {
        /// <value>OpenGL name for this <c>Texture</c>.</value>
        public int Id { get; private set; }

        /// <summary>Creates a <c>Texture</c> object.</summary>
        /// <remarks>
        ///     This constructor loads the image from the supplied path and 
        ///     creates the needed OpenGL objects to represent it
        ///     <para> 
        ///     Supported file types are the ones supported by the System.Drawing.Bitmap API 
        ///     </para>
        /// </remarks>
        /// <param name="path">Path to the graphics file.</param>
        public Texture(string path)
        {
            l.Information($"Loading texture {path}");
            Id = GL.GenTexture();
            Use();

            // We use .NET's System.Drawing library to load textures.
            using (var image = new Bitmap(path)) 
            {
                l.Information("Texture file loaded. Building OpenGL objects.");
                // Get pixels from the bitmap 
                var data = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                // Generate a texture
                GL.TexImage2D(
                    TextureTarget.Texture2D,    // type of texture. Standard 2D texture
                    0,                          // Level of detail (mipmap level 0)
                    PixelInternalFormat.Rgba,   // Pixel internal format of openGL internally stored image. 
                    image.Width,                // Size X
                    image.Height,               // Size Y
                    0,                          // Deprecated, use 0
                    PixelFormat.Bgra,           // TODO: Check why it does not match internal format
                    PixelType.UnsignedByte,     // Data type of pixels
                    data.Scan0);                // Pointer to data
            }
            
            // Settings affecting how the image appears on rendering.

            // Set the minifying function, used when the texture is scaled down. Use mipmaps and use linear among two of them 
            // (chosen by GL); then, the obtained values are again linearized.
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);

            // Set the magnifying function, used to scale up the texture. Linear aproximation (no mipmap available when going up)
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Set the wrapping mode of the texture. (S,T) are the coordinates names in texture domain.
            // Set this to Repeat so that textures will repeat when wrapped.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        }

        /// <summary>Binds this texture on the graphics pipeline for its use in the render loop.</summary>
        /// <param name="unit">Texture unit identifier (default is 0).
        ///     <remarks>
        ///     Multiple textures can be bound, if your shader needs more than just one.
        ///     If you want to do that, use unit = TextureUnit.TextureX, where is is the texture ID.
        ///     The OpenGL standard requires that there be at least 16, but there can be more depending on the graphics card.
        ///     <para>
        ///     Current implementation in Sharpen is to use just one texture for <see><c>Entity</c></see> so
        ///     the available pipeline and <see><c>ShaderProgram</c></see> supports just one and this parameter is not needed.
        ///     </para>
        ///     </remarks>
        /// </param>
        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Id);
        }

        /// <summary>Unbinds the texture from the graphics pipeline.</summary>
        public void Release()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

    }
}

