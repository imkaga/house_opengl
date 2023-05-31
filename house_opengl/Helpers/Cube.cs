using System;
using OpenTK.Graphics.OpenGL4;
using Project.Helpers;

namespace Project
{
    public class Cube
    {
        public int vbo { get; set; }
        public int vao { get; set; }
        private string texturePath;
        public Texture texture { get; set; }

        public Cube(float center_x, float center_y, float center_z, float width, float height, float depth, string texturePath)
        {
            this.texturePath = texturePath;
            float left_x = center_x - (width / 2);
            float right_x = center_x + (width / 2);
            float top_y = center_y + (height / 2);
            float bottom_y = center_y - (height / 2);
            float close_z = center_z + (depth / 2);
            float far_z = center_z - (depth / 2);

            vertices = new float[] {
                left_x, bottom_y,  far_z,  0.0f, 0.0f,
                right_x, bottom_y, far_z,  1.0f, 0.0f,
                right_x,  top_y,   far_z,  1.0f, 1.0f,
                right_x,  top_y,   far_z,  1.0f, 1.0f,
                left_x,  top_y,    far_z,  0.0f, 1.0f,
                left_x, bottom_y,  far_z,  0.0f, 0.0f,

                left_x, bottom_y,   close_z,  0.0f, 0.0f,
                right_x, bottom_y,  close_z,  1.0f, 0.0f,
                right_x, top_y,     close_z,  1.0f, 1.0f,
                right_x, top_y,     close_z,  1.0f, 1.0f,
                left_x,  top_y,     close_z,  0.0f, 1.0f,
                left_x, bottom_y,   close_z,  0.0f, 0.0f,

                left_x,  top_y,     close_z,  1.0f, 0.0f,
                left_x,  top_y,    far_z,  1.0f, 1.0f,
                left_x, bottom_y,  far_z,  0.0f, 1.0f,
                left_x, bottom_y,  far_z,  0.0f, 1.0f,
                left_x, bottom_y,   close_z,  0.0f, 0.0f,
                left_x,  top_y,     close_z,  1.0f, 0.0f,

                 right_x, top_y,     close_z,  1.0f, 0.0f,
                 right_x, top_y,     far_z,  1.0f, 1.0f,
                 right_x, bottom_y,  far_z,  0.0f, 1.0f,
                 right_x, bottom_y,  far_z,  0.0f, 1.0f,
                 right_x, bottom_y,  close_z,  0.0f, 0.0f,
                 right_x, top_y,     close_z,  1.0f, 0.0f,

                left_x,  bottom_y, far_z,  0.0f, 1.0f,
                right_x, bottom_y, far_z,  1.0f, 1.0f,
                right_x, bottom_y, close_z,  1.0f, 0.0f,
                right_x, bottom_y, close_z,  1.0f, 0.0f,
                left_x,  bottom_y, close_z,  0.0f, 0.0f,
                left_x,  bottom_y, far_z,  0.0f, 1.0f,

                left_x,  top_y, far_z,  0.0f, 1.0f,
                right_x, top_y, far_z,  1.0f, 1.0f,
                right_x, top_y, close_z,  1.0f, 0.0f,
                right_x, top_y, close_z,  1.0f, 0.0f,
                left_x,  top_y,close_z,  0.0f, 0.0f,
                left_x,  top_y, far_z,  0.0f, 1.0f
            };
        }

        public void LoadTexture()
        {
            texture = Texture.LoadFromFile(texturePath);
        }

        public float[] vertices { get; }


        public readonly uint[] indices =
        {
                //left
                0, 2, 1,
                0, 3, 2,
                //back
                1, 2, 6,
                6, 5, 1,
                //right
                4, 5, 6,
                6, 7, 4,
                //top
                2, 3, 6,
                6, 3, 7,
                //front
                0, 7, 3,
                0, 4, 7,
                //bottom
                0, 1, 5,
                0, 5, 4
        };


    }

}