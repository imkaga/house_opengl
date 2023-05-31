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


        private Cube[] cubes = new Cube[] {
            // (center_x, center_y, center_z, width(x), height(y), depth(z), texture_path, texture_unit)
            new Cube( 0,     -1.2f,     0,      12,     0.05f,    12,   "../../..//Textures/grass.png"),
            new Cube( 0,     0,     0,      5,     2.4f,    5,          "../../../Textures/wall1.jpg"),

            // drzwi
            new Cube( 0,    -0.6f,  2.52f,  0.6f,  1.2f,  0.02f,        "../../../Textures/door.png"),
            

            // Okna front
            // frontowe lewe okno                                    
            new Cube(-1.5f,  -0.3f,    2.52f,  0.6f,  0.6f,  0.02f,     "../../../Textures/okno.png"),
            // frontowe prawe okno                                       
            new Cube( 1.5f,  -0.3f,    2.52f,  0.6f,  0.6f,  0.02f,     "../../../Textures/okno.png"),


            // Okna z tyłu
            // tylne lewe okno
            new Cube(-1.5f,  -0.3f,    -2.52f,  0.6f,  0.6f,  0.02f, "../../../Textures/okno.png"),
            // tylne prawe okno                                       
            new Cube( 1.5f,  -0.3f,    -2.52f,  0.6f,  0.6f,  0.02f, "../../../Textures/okno.png"),


            //Okna z prawej
            new Cube(2.5f, -0.3f, -1.5f, 0.02f, 0.6f, 0.6f, "../../..//Textures/okno.png"),
            new Cube(2.5f, -0.3f, 1.5f, 0.02f, 0.6f, 0.6f,  "../../../Textures/okno.png"),

            //Okna z lewej
            new Cube(-2.5f, -0.3f, -1.5f, 0.02f, 0.6f, 0.6f, "../../../Textures/okno.png"),
            new Cube(-2.5f, -0.3f, 1.5f, 0.02f, 0.6f, 0.6f,  "../../../Textures/okno.png"),

            // Dekoracje
            new Cube( 2,    -0.6f,  3.2f,  1.2f,  0.6f,  0.05f, "../../../Textures/tabliczka.png"),
            new Cube(-1.5f, -0.9f,  2.52f,  0.6f,  0.6f,  0.6f, "../../../Textures/krzak.jpg"),
            new Cube(-2.82f, -0.9f,  -2.2f,  0.6f,  0.6f,  0.6f, "../../../Textures/krzak.jpg"),
            new Cube(2.82f, -0.9f,  -1.4f,  0.6f,  0.6f,  0.6f, "../../../Textures/krzak.jpg"),

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

        private Pyramid roof = new Pyramid(new Punkt { x = 0f, y = 2.9f, z = 0f }, 5.5f, 2f, 4.5f, "../../../grass.jpg");

        // nie mogę coś wyświetlić tej piramidy
        // Pyramid pyramid = new Pyramid(-10f, -0.3f, 2.52f, 5f, 5f, 5f, "C:/Users/kacpe/source/repos/house_opengl/house_opengl/Textures/grass.png");



        private Camera camera;
        private bool firstMove = true;


        private List<string> ImageFaces
        {
            get;
            set;
        }

        public void GetImageFaces()
        {
            ImageFaces = new List<string>();
            string dir = "C:/Users/kacpe/source/repos/house_opengl/house_opengl/Textures/cubemap/";
            ImageFaces.Add(Path.Combine(dir, "front.png"));
            ImageFaces.Add(Path.Combine(dir, "back.png"));
            ImageFaces.Add(Path.Combine(dir, "top.png"));
            ImageFaces.Add(Path.Combine(dir, "bottom.png"));
            ImageFaces.Add(Path.Combine(dir, "right.png"));
            ImageFaces.Add(Path.Combine(dir, "left.png"));
        }


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



            camera = new Camera(Vector3.UnitZ * 3, Size.X / (float)Size.Y);

            CursorState = CursorState.Grabbed;

        }


        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            foreach (Cube cube in cubes)
            {
                GL.BindVertexArray(cube.vao);


                cube.texture.Use(TextureUnit.Texture0);

                shader.Use();


                shader.SetMatrix4("model", Matrix4.Identity);
                shader.SetMatrix4("view", camera.GetViewMatrix());
                shader.SetMatrix4("projection", camera.GetProjectionMatrix());

                GL.DrawArrays(PrimitiveType.Triangles, 0, 36);
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

            KeyboardState input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 3f;
            const float sensitivity = 0.5f;

            if (input.IsKeyDown(Keys.W))
            {
                camera.Position += camera.Front * cameraSpeed * (float)e.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                camera.Position -= camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                camera.Position -= camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                camera.Position += camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.Space))
            {
                camera.Position += camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                camera.Position -= camera.Up * cameraSpeed * (float)e.Time; // Down
            }
        }


        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            camera.AspectRatio = Size.X / (float)Size.Y;


        }

    }
}