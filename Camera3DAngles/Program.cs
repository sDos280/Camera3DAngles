/*******************************************************************************************
*
*   raylib [core] example - 3d camera first person
*
*   This example has been created using raylib 1.3 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2015 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using System;
using System.Numerics;
using Raylib_cs;

namespace Examples
{
    public class core_3d_camera_first_person
    {
        public struct float2
        {
            public float x;
            public float y;

            public float2(float _x, float _y)
            {
                this.x = _x;
                this.y = _y;
            }


            public override string ToString()
            {
                return $"{x}, {y}";
            }

            public static float2 operator +(float2 a, float2 b) => new float2(a.x + b.x, a.y + b.y);
            public static float2 operator -(float2 a, float2 b) => new float2(a.x - b.x, a.y - b.y);
            public static float2 operator *(float2 a, float b) => new float2(a.x * b, a.y * b);

            public static float2 operator %(float2 a, float b) => new float2(a.x % b, a.y % b);

        }

        public static int Main()
        {
            const int screenWidth = 800;
            const int screenHeight = 450;

            Raylib.InitWindow(screenWidth, screenHeight, "raylib [core] example - 3d camera first person");

            // Define the camera to look into our 3d world (position, target, up vector)
            Camera3D camera = new Camera3D();
            camera.position = new Vector3(-4.0f, 2.0f, -4.0f);
            camera.target = new Vector3(0.0f, 1.8f, 0.0f);
            camera.up = new Vector3(0.0f, 1.0f, 0.0f);
            camera.fovy = 60.0f;
            camera.projection = Raylib_cs.CameraProjection.CAMERA_PERSPECTIVE;

            Raylib.SetCameraMode(camera, Raylib_cs.CameraMode.CAMERA_CUSTOM);

            float2 angle = new float2(0f, 0f);

            Raylib.SetTargetFPS(60);
            Raylib.DisableCursor();
            while (!Raylib.WindowShouldClose())
            {
                Vector2 mousePos = Raylib.GetMousePosition();
                angle = GetAngleOfCameraUsingMousePos(mousePos, new float2(screenWidth, screenHeight), camera.fovy);
                camera.target = GetPointOnASphere(angle, 1) + camera.position;

                Raylib.BeginDrawing();
                Raylib.ClearBackground(Raylib_cs.Color.RAYWHITE);

                Raylib.BeginMode3D(camera);

                Raylib.DrawSphere(Vector3.Zero, .1f, Color.BLACK);

                Raylib.DrawSphere(new Vector3(1, 0, 0), .1f, Color.GREEN); // x
                Raylib.DrawSphere(new Vector3(0, 1, 0), .1f, Color.BLUE); // y
                Raylib.DrawSphere(new Vector3(0, 0, 1), .1f, Color.RED); // z

                if (Raylib.IsKeyDown(Raylib_cs.KeyboardKey.KEY_W))
                {
                    camera.position += GetPointOnASphere(angle, 1) * Raylib.GetFrameTime();
                }

                Raylib.DrawPlane(Vector3.Zero, Vector2.One * 50, Color.GRAY);

                Raylib.DrawSphere(GetPointOnASphere(angle, 1)+camera.position, .1f, Color.YELLOW);

                Raylib.EndMode3D();

                Raylib.EndDrawing();
            }

            Raylib.CloseWindow();

            return 0;
        }

        public static float2 GetAngleOfCameraUsingMousePos(Vector2 _mousePos, float2 screenSize, float cameraFOV)
        {
            return ((new float2(-(screenSize.x * 0.5f - _mousePos.X) / screenSize.x * cameraFOV , (screenSize.y * 0.5f - _mousePos.Y) / screenSize.y * cameraFOV))%360) * Raylib.DEG2RAD;
        }

        public static Vector3 GetPointOnASphere(float2 _angle, float radius)
        {
            return new Vector3(radius * MathF.Cos(_angle.x) * MathF.Cos(_angle.y), MathF.Sin(_angle.y) * radius, radius * MathF.Sin(_angle.x) * MathF.Cos(_angle.y));
            //return new Vector3(radius * MathF.Sin(_angle.x) * MathF.Cos(_angle.y), radius * MathF.Cos(_angle.x), radius * MathF.Sin(_angle.x) * MathF.Cos(_angle.y));
        }
    }
}