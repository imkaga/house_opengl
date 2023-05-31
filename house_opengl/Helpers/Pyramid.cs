using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Program
{
    class Pyramid
    {
        private float positionX;
        private float positionY;
        private float positionZ;
        private float width;
        private float height;
        private float depth;
        private string texturePath;

        public float PositionX { get => positionX; set => positionX = value; }
        public float PositionY { get => positionY; set => positionY = value; }
        public float PositionZ { get => positionZ; set => positionZ = value; }
        public float Width { get => width; set => width = value; }
        public float Height { get => height; set => height = value; }
        public float Depth { get => depth; set => depth = value; }
        public string TexturePath { get => texturePath; set => texturePath = value; }

        public Pyramid(float posX, float posY, float posZ, float w, float h, float d, string texPath)
        {
            positionX = posX;
            positionY = posY;
            positionZ = posZ;
            width = w;
            height = h;
            depth = d;
            texturePath = texPath;
        }

        public void Draw()
        {
            GL.PushMatrix();
            GL.Translate(PositionX, PositionY, PositionZ);

            GL.BindTexture(TextureTarget.Texture2D, LoadTexture(TexturePath));

            GL.Begin(PrimitiveType.Triangles);

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, Height / 2, 0.0f);
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(-Width / 2, -Height / 2, Depth / 2);
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(Width / 2, -Height / 2, Depth / 2);

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, Height / 2, 0.0f);
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(Width / 2, -Height / 2, Depth / 2);
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0.0f, -Height / 2, -Depth / 2);

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, Height / 2, 0.0f);
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(0.0f, -Height / 2, -Depth / 2);
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(-Width / 2, -Height / 2, Depth / 2);

            GL.Color3(1.0f, 0.0f, 0.0f);
            GL.Vertex3(0.0f, Height / 2, 0.0f);
            GL.Color3(0.0f, 1.0f, 0.0f);
            GL.Vertex3(-Width / 2, -Height / 2, Depth / 2);
            GL.Color3(0.0f, 0.0f, 1.0f);
            GL.Vertex3(0.0f, -Height / 2, -Depth / 2);

            GL.End();
            GL.PopMatrix();
        }


        private int LoadTexture(string path)
        {
            // Code to load texture from path and return texture ID
            return 0;
        }
    }
}
