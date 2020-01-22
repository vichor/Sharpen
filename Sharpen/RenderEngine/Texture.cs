using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using l = Serilog.Log;

namespace Sharpen.RenderEngine
{
    public class Texture
    {
        public readonly int Id;

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

        // Activate texture
        // Multiple textures can be bound, if your shader needs more than just one.
        // If you want to do that, use GL.ActiveTexture to set which slot GL.BindTexture binds to.
        // The OpenGL standard requires that there be at least 16, but there can be more depending on your graphics card.
        public void Use(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Id);
        }

        public void Release()
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

    }
}

