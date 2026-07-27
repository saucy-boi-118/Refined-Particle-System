using ParticleSystem;
using System.Numerics;
using static UI.UIFunctions;
using Raylib_cs;

class Program
{
    // WINDOW VARIABLES
    public const int WINW = 1024;
    public const int WINH = 512, THICK = 10;
    public static readonly Vector2 SIZE = new(45,45);
    public static readonly Vector2 BASES = new(125,30);
    public static void Main()
    {
        // Initialization
        Raylib.InitWindow(WINW, WINH, "Basic Window");
        Raylib.SetTargetFPS(60);

        // Use of particle system
        PARTICLE2D particleSystem = new(180, PARTICLE2D.Shapes.Septagon);
        Rectangle psBounds = new(212,0,545,512);
        Vector2 origin;
        Vector2 mouse;

        // Panels for the other UI
        Rectangle LeftPanel = new(0, psBounds.Y, psBounds.X, psBounds.Height);
        Rectangle RightPanel = new(psBounds.Width+psBounds.X, psBounds.Y, WINW-psBounds.X-psBounds.Width, psBounds.Height);

        // Enum value for changing shape
        float minShape = 0, maxShape = Enum.GetNames<PARTICLE2D.Shapes>().Length-1, CurrentShape = 0;
        Rectangle shapeBase = new(20,40,125,30);
        Rectangle sliderShape = new(shapeBase.Center.X,shapeBase.Center.Y, SIZE);

        // Sliders for changing colors

        // Primary SLIDERS
        float colorSR = 0, colorSG = 0, colorSB = 0;
        Color START = Color.Black;

        // RED
        Rectangle StartR = new(RightPanel.X+35, RightPanel.Y+50, BASES);
        Rectangle SliderR = new(StartR.Center, SIZE);

        // GREEN
        Rectangle StartG = new(StartR.X, StartR.Y+60, BASES);
        Rectangle SliderG = new(StartG.Center, SIZE);

        // BLUE
        Rectangle StartB = new(StartR.X, StartG.Y+50, BASES);
        Rectangle SliderB = new(StartB.Center, SIZE);

        // End SLIDERS
        float colorER = 0, colorEG = 0, colorEB = 0;
        Color END = Color.Red;

        // RED
        Rectangle EndR = new(StartB.X, StartB.Y+100, BASES);
        Rectangle SliderEndR = new(EndR.Center, SIZE);

        // GREEN
        Rectangle EndG = new(EndR.X, EndR.Y+60, BASES);
        Rectangle SliderEndG = new(EndG.Center, SIZE);

        // BLUE
        Rectangle EndB = new(EndG.X, EndG.Y+50, BASES);
        Rectangle SliderEndB = new(EndB.Center, SIZE);
        
        // Labels
        string primary = "Primary Color";
        string end = "End Color";

        // clamping colors
        float minColor = 0;
        float maxColor = 255;

        // Direction changing and visualization
        Vector2 Direction = Vector2.Zero;
        float Angle = 0, AngleMin = 0, AngleMax = 2*MathF.PI, CircleRadius = 20;
        Vector2 CircleCenter = new(LeftPanel.Center.X + CircleRadius + 45, LeftPanel.Y+165+(BASES.Y/2));
        Rectangle BaseAngle = new(LeftPanel.X + 20, LeftPanel.Y+165, BASES);
        Rectangle AngleSlider = new(BaseAngle.Center, SIZE);
        string directionText = "DIRECTION";

        // Direction factor changing
        Rectangle dirFactorBase = new(LeftPanel.X + 20, LeftPanel.Y+230, BASES);
        Rectangle dirFactorSlider = new(dirFactorBase.Center, SIZE);
        float dirFactor = 0;

        // Changing the speed Divisor
        Rectangle DivisorBase = new(LeftPanel.X + 20, LeftPanel.Y + 270, BASES);
        Rectangle DivisorSlider = new(dirFactorBase.Center, SIZE);
        float divisor = 1;

        while (!Raylib.WindowShouldClose())
        {
            // SPECIFICATIONS FOR THE PARTICLE SYSTEM

            mouse = Raylib.GetMousePosition(); // mouse position

            // Specific area for particles to update
            origin = psBounds.Center;

            // If its in the bounds origin is the center of the rectangle
            if (Raylib.CheckCollisionPointRec(mouse, psBounds)) origin = mouse;

            // Drawing
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.White);

            // Particle system bounds(
            Raylib.DrawRectangleLinesEx(psBounds, THICK, Color.Black);

            // Update Particles
            PARTICLE2D.ChangeShape((PARTICLE2D.Shapes)CurrentShape);
            particleSystem.UpdateParticles(origin,Direction,1,START,END,20,dirFactor,0.3f,4, divisor);

            // Other panels
            Raylib.DrawRectangleRec(LeftPanel, Color.White);
            Raylib.DrawRectangleLinesEx(LeftPanel, THICK, Color.Black);
            Raylib.DrawRectangleRec(RightPanel, Color.White);
            Raylib.DrawRectangleLinesEx(RightPanel, THICK, Color.Black);

            // UPDATING UI
            UISlider(ref CurrentShape, minShape, maxShape, shapeBase, ref sliderShape, Style.BaseSimple);
            Raylib.DrawText($"{(PARTICLE2D.Shapes)CurrentShape}", (int)shapeBase.X, (int)(shapeBase.Y + 50), 25, Color.Black);

            // Direction Changing
            UISlider(ref Angle, AngleMin, AngleMax, BaseAngle, ref AngleSlider, Style.BaseSimple); // slider
            UISlider(ref dirFactor, 0, 1500, dirFactorBase, ref dirFactorSlider, Style.BaseSimple); // slider for dir factor
            UISlider(ref divisor, 1, 10, DivisorBase, ref DivisorSlider, Style.BaseSimple); // for constraint of direction factor

            // Text
            Raylib.DrawText(directionText, (int)BaseAngle.X, (int)BaseAngle.Y - 50, 25, Color.Black);

            // applying angle to direction
            Direction.X = MathF.Sin(Angle);
            Direction.Y = MathF.Cos(Angle);

            // drawing the circle for the direction
            Raylib.DrawCircleV(CircleCenter, CircleRadius*1.2f, Color.Black);
            Raylib.DrawCircleV(CircleCenter, CircleRadius, Color.White);

            // Drawing the line
            Raylib.DrawLineEx(CircleCenter, CircleCenter+(Direction*CircleRadius), 5, Color.Red);

            // UPDATING COLORS

            // Primary
            Raylib.DrawText(primary, (int)StartR.X, (int)(StartR.Y-25), 20, Color.Black); // text
            UISlider(ref colorSR, minColor, maxColor, StartR, ref SliderR, Style.BaseSimple); // RED
            DrawLabel(SliderR, "RED", Color.Red); // label
            UISlider(ref colorSG, minColor, maxColor, StartG, ref SliderG, Style.BaseSimple); // GREEN
            DrawLabel(SliderG, "GREEN", Color.Green); // label
            UISlider(ref colorSB, minColor, maxColor, StartB, ref SliderB, Style.BaseSimple); // BLUE
            DrawLabel(SliderB, "BLUE", Color.Blue); // label
            MapColorValueFloat(ref START, colorSR, colorSG, colorSB); // setting color
            Raylib.DrawText($"{START}", (int)StartB.X-25, (int)(StartB.Y + 35), 15, Color.Black); // draw color

            // End
            Raylib.DrawText(end, (int)EndR.X, (int)(EndR.Y-25), 20, Color.Black); // text
            UISlider(ref colorER, minColor, maxColor, EndR, ref SliderEndR, Style.BaseSimple); // RED
            DrawLabel(SliderEndR, "RED", Color.Red); // label
            UISlider(ref colorEG, minColor, maxColor, EndG, ref SliderEndG, Style.BaseSimple); // GREEN
            DrawLabel(SliderEndG, "GREEN", Color.Green); // label
            UISlider(ref colorEB, minColor, maxColor, EndB, ref SliderEndB, Style.BaseSimple); // BLUE
            DrawLabel(SliderEndB, "BLUE", Color.Blue); // label
            MapColorValueFloat(ref END, colorER, colorEG, colorEB); // setting color
            Raylib.DrawText($"{END}", (int)EndB.X-25, (int)(EndB.Y + 35), 15, Color.Black); // draw color


            Raylib.EndDrawing();
        }

        // closing and unloading assets
        Raylib.CloseWindow();
    }

    public static void MapColorValueFloat(ref Color c, float r, float g, float b)
    {
        c.R = (byte)(r); // RED
        c.G = (byte)(g); // GREEN
        c.B = (byte)(b); // BLUE
    }
}