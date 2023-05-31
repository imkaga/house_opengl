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
            // podłoga
            new Cube( 0,     -1.2f,     0,      10,     0.01f,    10,   "C:/Users/kacpe/source/repos/house_opengl/house_opengl/Textures/grass.png"),
            // sides (walls)
            new Cube( 0,     0,     0,      5,     2.4f,    5,          "C:/Users/kacpe/source/repos/house_opengl/house_opengl/Textures/wall1.jpg"),
            // drzwi
            // 

            // Okna front
            new Cube( 0,    -0.6f,  2.52f,  0.6f,  1.2f,  0.02f,        "C:/Users/kacpe/source/repos/house_opengl/house_opengl/Textures/door.png"),
            // frontowe lewe okno                                    
            new Cube(-1.5f,  -0.3f,    2.52f,  0.6f,  0.6f,  0.02f,     "C:/Users/kacpe/source/repos/house_opengl/house_opengl/Textures/okno.png"),
            // frontowe prawe okno                                    
            new Cube( 1.5f,  -0.3f,    2.52f,  0.6f,  0.6f,  0.02f,     "C:/Users/kacpe/source/repos/house_opengl/house_opengl/Textures/okno.png"),


            // Okna z tyłu
            // tylne lewe okno
            new Cube(-1.5f,  -0.3f,    -2.52f,  0.6f,  0.6f,  0.02f, "C:/Users/kacpe/source/repos/house_opengl/house_opengl/Textures/okno.png"),
            // tylne prawe okno
            new Cube( 1.5f,  -0.3f,    -2.52f,  0.6f,  0.6f,  0.02f, "C:/Users/kacpe/source/repos/house_opengl/house_opengl/Textures/okno.png"),


            //Okna z prawej
            new Cube(2.5f, -0.3f, -1.5f, 0.02f, 0.6f, 0.6f, "C:/Users/kacpe/source/repos/house_opengl/house_opengl/Textures/okno.png"),
            new Cube(2.5f, -0.3f, 1.5f, 0.02f, 0.6f, 0.6f, "C:/Users/kacpe/source/repos/house_opengl/house_opengl/Textures/okno.png"),

            //Okna z lewej
            new Cube(-2.5f, -0.3f, -1.5f, 0.02f, 0.6f, 0.6f, "C:/Users/kacpe/source/repos/house_opengl/house_opengl/Textures/okno.png"),
            new Cube(-2.5f, -0.3f, 1.5f, 0.02f, 0.6f, 0.6f, "C:/Users/kacpe/source/repos/house_opengl/house_opengl/Textures/okno.png"),

        };

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
            string dir = "/Users/olakrason/projects/HouseProject/Project/CubemapPhotos/";
            ImageFaces.Add(Path.Combine(dir, "posz.jpg"));
            ImageFaces.Add(Path.Combine(dir, "negz.jpg"));
            ImageFaces.Add(Path.Combine(dir, "posy.jpg"));
            ImageFaces.Add(Path.Combine(dir, "negy.jpg"));
            ImageFaces.Add(Path.Combine(dir, "posx.jpg"));
            ImageFaces.Add(Path.Combine(dir, "negx.jpg"));
        }

        ////

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

            // Get the mouse state
            var mouse = MouseState;

            if (firstMove) // This bool variable is initially set to true.
            {
                lastPos = new Vector2(mouse.X, mouse.Y);
                firstMove = false;
            }
            else
            {
                // Calculate the offset of the mouse position
                var deltaX = mouse.X - lastPos.X;
                var deltaY = mouse.Y - lastPos.Y;
                lastPos = new Vector2(mouse.X, mouse.Y);

                // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                camera.Yaw += deltaX * sensitivity;
                camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
            }

        }

        // In the mouse wheel function, we manage all the zooming of the camera.
        // This is simply done by changing the FOV of the camera.
        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            camera.Fov -= e.OffsetY;
        }


        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            camera.AspectRatio = Size.X / (float)Size.Y;


        }

    }
}