using OpenTK.Graphics.OpenGL4;

namespace Project.Helpers
{
    public class CubemapTexture
    {
        public int Handle { get; private set; }

        public CubemapTexture(string[] faces)
        {
            Handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, Handle);

            for (int i = 0; i < faces.Length; i++)
            {
                byte[] image = LoadImage(faces[i]);
                GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i, 0, PixelInternalFormat.Rgb, 512, 512, 0, PixelFormat.Rgb, PixelType.UnsignedByte, image);
            }

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
        }

        private byte[] LoadImage(string path)
        {
            // Implement image loading logic here and return the byte array
            // corresponding to the loaded image.
            // This depends on the image loading library or method you are using.
            // You can use libraries like FreeImage, stb_image, or the built-in .NET Image class.

            // Example using System.Drawing.Image:
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
            using (var bitmap = new System.Drawing.Bitmap(image))
            {
                var data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                byte[] pixels = new byte[data.Stride * data.Height];
                System.Runtime.InteropServices.Marshal.Copy(data.Scan0, pixels, 0, pixels.Length);

                bitmap.UnlockBits(data);

                return pixels;
            }
        }

        public void Use(TextureUnit textureUnit)
        {
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.TextureCubeMap, Handle);
        }
    }
}
