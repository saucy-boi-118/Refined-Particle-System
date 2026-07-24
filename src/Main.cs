using ParticleSystem;
using System.Numerics;
using Raylib_cs;

class Program
{
    // WINDOW VARIABLES
    public const int WINW = 1024;
    public const int WINH = 512;
    public static void Main()
    {
        // Initialization
        Raylib.InitWindow(WINW, WINH, "Basic Window");
        Raylib.SetTargetFPS(60);

        // Use of particle system
        PARTICLE2D particleSystem = new(180, PARTICLE2D.Shapes.Septagon);
        Rectangle particleSystemBounds = new(212,5,600,256);
        Vector2 origin;
        Vector2 mouse;

        while (!Raylib.WindowShouldClose())
        {
            mouse = Raylib.GetMousePosition(); // mouse position

            

            // Drawing
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            // Specific area for particles to update
            origin = particleSystemBounds.Center;

            // If its in the bounds origin is the center of the rectangle
            if (Raylib.CheckCollisionPointRec(mouse, particleSystemBounds)) origin = mouse;

            // Update Particles
            particleSystem.UpdateParticles(origin,Vector2.Zero,1,Color.Black,Color.Red,20,350,0.3f,4);

            // Drawing the rectangle bounds
            Raylib.DrawRectangleRoundedLinesEx(particleSystemBounds,0.5f,5,5,Color.Black);

            // UPDATING UI

            Raylib.EndDrawing();
        }

        // closing and unloading assets
        Raylib.CloseWindow();
    }
}