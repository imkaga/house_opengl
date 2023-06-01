using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using Project;
using System.Drawing;
using GlmSharp;
using Models;
using Project.Helpers;
using Program;

namespace House
{
    public class Window : GameWindow
    {
        private Shader shader;
        private Vector2 lastPos;

        private Matrix4 view;
        private Matrix4 projection;
        private Matrix4 rotation;

        private float rotationRads = 0;

        static float speed_y; //Prędkość obrotu wokół osi Y [rad/s]
        static float speed_x; //Prędkość obrotu wokół osi X [rad/s]

        private Cube[] cubes = new Cube[] {
            // (center_x, center_y, center_z, width(x), height(y), depth(z), texture_path, texture_unit)
            new Cube( 0, -1.2f, 0, 12, 0.05f, 12,   "../../..//Textures/grass.png"),
            new Cube( 0, 0, 0, 5, 2.4f, 5,          "../../../Textures/wall1.jpg"),

            // Drzwi
            new Cube( 0,    -0.6f,  2.52f,  0.6f,  1.2f,  0.02f,        "../../../Textures/door.png"),
            

            // Okna front
            // Frontowe lewe okno                                    
            new Cube(-1.5f,  -0.3f,    2.52f,  0.6f,  0.6f,  0.02f,     "../../../Textures/okno.png"),
            // Frontowe prawe okno                                       
            new Cube( 1.5f,  -0.3f,    2.52f,  0.6f,  0.6f,  0.02f,     "../../../Textures/okno.png"),


            // Okna z tyłu
            // Tylne lewe okno
            new Cube(-1.5f,  -0.3f,    -2.52f,  0.6f,  0.6f,  0.02f, "../../../Textures/okno.png"),
            // Tylne prawe okno                                       
            new Cube( 1.5f,  -0.3f,    -2.52f,  0.6f,  0.6f,  0.02f, "../../../Textures/okno.png"),

            //Okna z prawej
            new Cube(2.5f, -0.3f, -1.5f, 0.02f, 0.6f, 0.6f, "../../..//Textures/okno.png"),
            new Cube(2.5f, -0.3f, 1.5f, 0.02f, 0.6f, 0.6f,  "../../../Textures/okno.png"),

            //Okna z lewej
            new Cube(-2.5f, -0.3f, -1.5f, 0.02f, 0.6f, 0.6f, "../../../Textures/okno.png"),
            new Cube(-2.5f, -0.3f, 1.5f, 0.02f, 0.6f, 0.6f,  "../../../Textures/okno.png"),

            // Dekoracje
            new Cube( 2,    -0.7f,  8.4f,  2.4f,  1.2f,  0.05f, "../../../Textures/tabliczka.png"),
            new Cube(-1.5f, -0.9f,  2.52f,  0.6f,  0.6f,  0.6f, "../../../Textures/krzak.jpg"),
            new Cube(-2.82f, -0.9f,  -2.2f,  0.6f,  0.6f,  0.6f, "../../../Textures/krzak.jpg"),
            new Cube(2.82f, -0.9f,  -1.4f,  0.6f,  0.6f,  0.6f, "../../../Textures/krzak.jpg"),

            // Cubes
            new Cube(2.9f, -0.9f, -4.2f, 0.6f, 0.6f, 0.6f, "../../../Textures/krzak.jpg"),
            new Cube(2.9f + 0.7f, -0.9f, -4.2f, 0.6f, 0.6f, 0.6f, "../../../Textures/krzak.jpg"),
            new Cube(2.9f + 0.7f * 2, -0.9f, -4.2f, 0.6f, 0.6f, 0.6f, "../../../Textures/krzak.jpg"),

            // Drzewo
            new Cube(-4, -0.9f,  0.4f,  0.6f,  0.6f,  0.6f, "../../../Textures/wood.jpg"),
            new Cube(-4, -0.3f,  0.4f,  0.6f,  0.6f,  0.6f, "../../../Textures/wood.jpg"),
            new Cube(-4,  0.3f,  0.4f,  0.6f,  0.6f,  0.6f, "../../../Textures/wood.jpg"),
            new Cube(-4,  0.9f,  0.4f,  0.6f,  0.6f,  0.6f, "../../../Textures/wood.jpg"),
            new Cube(-4.6f,  0.9f,  0.4f,  0.6f,  0.6f,  0.6f, "../../../Textures/krzak.jpg"),
            new Cube(-4f,  0.9f,  1f,  0.6f,  0.6f,  0.6f, "../../../Textures/krzak.jpg"),
            new Cube(-4f,  1.5f,  0.4f,  0.6f,  0.6f,  0.6f, "../../../Textures/krzak.jpg"),
            new Cube(-4f,  0.9f,  -0.2f,  0.6f,  0.6f,  0.6f, "../../../Textures/krzak.jpg"),
            new Cube(-3.4f,  0.9f,  0.4f,  0.6f,  0.6f,  0.6f, "../../../Textures/krzak.jpg"),

        };

        private Pyramid roof = new Pyramid(new Punkt { x = 0f, y = 1.25f, z = 0f }, 5f, 2.5f, 5f, "../../../Textures/oak.png");

        // nie mogę coś wyświetlić tej piramidy
        // Pyramid pyramid = new Pyramid(-10f, -0.3f, 2.52f, 5f, 5f, 5f, "C:/Users/kacpe/source/repos/house_opengl/house_opengl/Textures/grass.png");

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.5f, 0.5f, 0.9f, 1.0f);
     
            GL.Enable(EnableCap.DepthTest);

            foreach (Cube cube in cubes)
            {
                cube.vbo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, cube.vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, cube.vertices.Length * sizeof(float), cube.vertices, BufferUsageHint.StaticDraw);

                cube.vao = GL.GenVertexArray();
                GL.BindVertexArray(cube.vao);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);
                
                shader = new Shader(@"C:\Users\kacpe\source\repos\house_opengl\house_opengl\Helpers\Shaders\shader.vert", @"C:\Users\kacpe\source\repos\house_opengl\house_opengl\Helpers\Shaders\shader.frag");
                shader.Use();


                var positionLocation = shader.GetAttribLocation("aPosition");
                GL.EnableVertexAttribArray(positionLocation);
                GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

                var texCoordLocation = shader.GetAttribLocation("aTexCoord");
                GL.EnableVertexAttribArray(texCoordLocation);
                GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

                cube.LoadTexture();
                cube.texture.Use(TextureUnit.Texture0);

                shader.SetInt("texture0", 0);
            }
            
            foreach (Triangle side in roof.triangles)
            {

                side.vbo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ArrayBuffer, side.vbo);
                GL.BufferData(BufferTarget.ArrayBuffer, side.vertices.Length * sizeof(float), side.vertices, BufferUsageHint.StaticDraw);

                side.vao = GL.GenVertexArray();
                GL.BindVertexArray(side.vao);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

                GL.EnableVertexAttribArray(0);

                shader = new Shader("../../../Helpers/Shaders/shader.vert", "../../../Helpers/Shaders/shader.frag");
                shader.Use();

                var vertexLocation = shader.GetAttribLocation("aPosition");
                GL.EnableVertexAttribArray(vertexLocation);
                GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

                var texCoordLocation = shader.GetAttribLocation("aTexCoord");
                GL.EnableVertexAttribArray(texCoordLocation);
                GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

                side.LoadTexture();
                side.texture.Use(TextureUnit.Texture0);

                shader.SetInt("texture0", 0);
            }

            view = Matrix4.CreateTranslation(0.0f, -2f, -20.0f);
            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f, 100.0f);

        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            var model = Matrix4.Identity * rotation;

            foreach (Cube cube in cubes)
            {
                GL.BindVertexArray(cube.vao);

                cube.texture.Use(TextureUnit.Texture0);

                shader.Use();

                shader.SetMatrix4("model", model);
                shader.SetMatrix4("view", view);
                shader.SetMatrix4("projection", projection);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
            }


            foreach (Triangle side in roof.triangles)
            {
                GL.BindVertexArray(side.vao);
                side.texture.Use(TextureUnit.Texture0);

                shader.Use();
                shader.SetMatrix4("model", model);
                shader.SetMatrix4("view", view);
                shader.SetMatrix4("projection", projection);

                GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            }

            SwapBuffers();
        }


        protected override void OnUpdateFrame(FrameEventArgs e)
        {

            base.OnUpdateFrame(e);

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            rotationRads += speed_y;
            Matrix4.CreateRotationY(rotationRads, out rotation);

            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            if (input.IsKeyDown(Keys.Left))
            {
                speed_y = 0.001f;
            }

            if (input.IsKeyDown(Keys.Right))
            {
                speed_y = -0.001f;
            }

            if (input.IsKeyReleased(Keys.Left))
            {
                speed_y = 0;
            }

            if (input.IsKeyReleased(Keys.Right))
            {
                speed_y = 0;
            }
        }
    }
}
