using ParticleSystem;
using System.Numerics;
using BasicGUI;
using Raylib_cs;

class Program
{
    // WINDOW VARIABLES
    public const int WINW = 1024;
    public const int WINH = 512;
    public static void Main()
    {
        // -----------------------------------------------------------------------------
        // INIT
        // -----------------------------------------------------------------------------
        Raylib.InitWindow(WINW, WINH, "Basic Window");
        Raylib.SetTargetFPS(60);

        // -----------------------------------------------------------------------------
        // PARTICLE SYSTEM INTERACTIONS
        // -----------------------------------------------------------------------------
        PARTICLE2D particleSystem = new(180, PARTICLE2D.Shapes.Septagon);
        Vector2 origin;
        Vector2 mouse;

        // -----------------------------------------------------------------------------
        // BOUNDS AND PANELS
        // -----------------------------------------------------------------------------
        Rectangle psBounds = new(212,0,545,512);
        Rectangle LeftPanel = new(0, psBounds.Y, psBounds.X, psBounds.Height);
        Rectangle RightPanel = new(psBounds.Width+psBounds.X, psBounds.Y, WINW-psBounds.X-psBounds.Width, psBounds.Height);

        // -----------------------------------------------------------------------------
        // CHANGING THE START AND END COLORS
        // -----------------------------------------------------------------------------
        Color startColor = Color.Black;
        ColorPicker startpicker = new(new(RightPanel.Center.X-50, RightPanel.Y + 125), startColor, "START");
        Color endColor = Color.White;
        ColorPicker endpicker = new(new(RightPanel.Center.X+50, RightPanel.Y + 125), endColor, "END");

        // -----------------------------------------------------------------------------
        // CHANGING THE DIRECTION
        // -----------------------------------------------------------------------------
        Vector2 Direction = Vector2.Zero;
        Rectangle DirectionSlider = new(LeftPanel.X + 25, LeftPanel.Y + 40, 100, 30);
        Vector2 DirectionCenter = DirectionSlider.Center;
        float circleRadius = 20, angle = 0, angleMax = MathF.PI*2;
        Vector2 CircleCenter = new(DirectionCenter.X + DirectionSlider.Width, DirectionCenter.Y);

        // -----------------------------------------------------------------------------
        // DIRECTION STRENGTH
        // -----------------------------------------------------------------------------
        Rectangle StrengthSlider = new(LeftPanel.X + 25, LeftPanel.Y + 110, 100, 30);
        Vector2 StrengthCenter = StrengthSlider.Center;
        float Strength = 0;

        // -----------------------------------------------------------------------------
        // START SIZE
        // -----------------------------------------------------------------------------
        Rectangle SizeSlider = new(LeftPanel.X + 25, LeftPanel.Y + 180, 100, 30);
        Vector2 SizeCenter = SizeSlider.Center;
        float size = 5;

        // -----------------------------------------------------------------------------
        // LIFESPAN
        // -----------------------------------------------------------------------------
        Rectangle LifeSlider = new(LeftPanel.X + 25, LeftPanel.Y + 250, 100, 30);
        Vector2 LifeCenter = LifeSlider.Center;
        float Lifespan = 1;
        
        // -----------------------------------------------------------------------------
        // SIZE DIFFERENCE
        // -----------------------------------------------------------------------------
        Rectangle SizeDiffSlider = new(LeftPanel.X + 25, LeftPanel.Y + 320, 100, 30);
        Vector2 SizeDiffCenter = SizeDiffSlider.Center;
        float SizeDiff = 0.3f;
        
        // -----------------------------------------------------------------------------
        // SPREAD REGULATION
        // -----------------------------------------------------------------------------
        Rectangle SpreadSlider = new(LeftPanel.X + 25, LeftPanel.Y + 390, 100, 30);
        Vector2 SpreadCenter = SpreadSlider.Center;
        float Spread = 1;

        // -----------------------------------------------------------------------------
        // SHAPE
        // -----------------------------------------------------------------------------
        float minShape = 0, maxShape = Enum.GetNames<PARTICLE2D.Shapes>().Length, CurrentShape = 0;
        Rectangle ShapeSlider = new(RightPanel.Center.X-100, RightPanel.Y + 200, 200, 85);
        Vector2 ShapeCenter = ShapeSlider.Center;

        while (!Raylib.WindowShouldClose())
        {
            // -----------------------------------------------------------------------------
            // SPECIFICATIONS FOR THE PARTICLE SYSTEM
            // -----------------------------------------------------------------------------

            mouse = Raylib.GetMousePosition(); // mouse position

            // Specific area for particles to update
            origin = psBounds.Center;

            // If its in the bounds origin is the center of the rectangle
            if (Raylib.CheckCollisionPointRec(mouse, psBounds)) origin = mouse;

            // -----------------------------------------------------------------------------
            // DRAWING
            // -----------------------------------------------------------------------------
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            // -----------------------------------------------------------------------------
            // PANELS
            // -----------------------------------------------------------------------------
            BaseGUI.UIPanel(psBounds, 5, 25, Color.Black, "Live Screen");
            BaseGUI.UIPanel(LeftPanel, 5, 20, Color.Black, "Details");
            BaseGUI.UIPanel(RightPanel, 5, 20, Color.Black, "Visuals");
            
            // -----------------------------------------------------------------------------
            // UPDATING PARTICLES
            // -----------------------------------------------------------------------------
            BaseGUI.UISlider(ShapeSlider, ref ShapeCenter, ref CurrentShape, minShape, maxShape, false, "Shape");
            PARTICLE2D.ChangeShape((PARTICLE2D.Shapes)CurrentShape); // changing the shape
            particleSystem.UpdateParticles(origin, // Where the particles start from
                                           Direction, // Where the particles go to
                                           (int)Lifespan, // life span of the particles in seconds
                                           startColor, // start color
                                           endColor, // end color
                                           size, // start size
                                           Strength, // how strong the direction is
                                           SizeDiff, // size difference each frame
                                           4, // alpha difference each frame
                                           Spread // spread regulation
                                          );

            // -----------------------------------------------------------------------------
            // COLOR PICKING
            // -----------------------------------------------------------------------------
            startColor = startpicker.Update();
            endColor = endpicker.Update();

            // -----------------------------------------------------------------------------
            // DIRECTION VARIABLES
            // -----------------------------------------------------------------------------

            // slider for changing direction
            BaseGUI.UISlider(DirectionSlider, ref DirectionCenter, ref angle, 0, angleMax, false,"direction");

            // updating the direction
            Direction.X = MathF.Cos(angle);
            Direction.Y = MathF.Sin(angle);

            // visualizing the direction
            Raylib.DrawCircleLinesV(CircleCenter, circleRadius, Color.Black);
            Raylib.DrawLineEx(CircleCenter, CircleCenter + (Direction * circleRadius), 5, Color.Red);

            // strength of the direction
            BaseGUI.UISlider(StrengthSlider, ref StrengthCenter, ref Strength, 0, 1500, false, "strength");

            // -----------------------------------------------------------------------------
            // VISUAL ATTRIBUTES
            // -----------------------------------------------------------------------------
            BaseGUI.UISlider(SizeSlider, ref SizeCenter, ref size, 5, 30,false,"size"); // start size
            BaseGUI.UISlider(LifeSlider, ref LifeCenter, ref Lifespan, 1, 3,false,"lifespan"); // lifespan
            BaseGUI.UISlider(SizeDiffSlider, ref SizeDiffCenter, ref SizeDiff, 0.3f, 1.1f, false,"size diff"); // size difference
            BaseGUI.UISlider(SpreadSlider, ref SpreadCenter, ref Spread, 1, 13, false,"spread"); // spread regulation


            Raylib.EndDrawing();
        }

        // closing and unloading assets
        Raylib.CloseWindow();
    }

    
}