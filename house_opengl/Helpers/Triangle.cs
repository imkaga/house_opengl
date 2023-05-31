using System;
using OpenTK.Graphics.GL;

namespace Program
{
    public struct Punkt
    {
        public float x;
        public float y;
        public float z;
    }

    public class Triangle
    {
        public int vbo { get; set; }
        public int vao { get; set; }
        private string texturePath;

        public Texture texture { get; set; }

        public Triangle(Punkt vertex_1, Punkt vertex_2, Punkt vertex_3, string texturePath)
        {
            this.texturePath = texturePath;

            vertices = new float[]
            {                    // texcoords
             vertex_1.x, vertex_1.y, vertex_1.z, 0.0f, 0.0f, // Bottom-left vertex
             vertex_2.x, vertex_2.y, vertex_2.z, 1.0f, 0.0f,// Bottom-right vertex
             vertex_3.x, vertex_3.y, vertex_3.z, 0.5f, 1.0f,  // Top vertex
            };
        }

        public void LoadTexture()
        {
            texture = Texture.LoadFromFile(texturePath);
        }

        public float[] vertices { get; }

    }
}

