using Raylib_cs;
using System.Numerics;


namespace BasicGUI
{
    class BaseGUI
    {
        // -------------------------
        // PUBLIC COLORS FOR UI
        // -------------------------
        public static Color Hover = Color.LightGray;
        public static Color Main = Color.White;
        public static Color Outline = Color.Black;
        public const float THICKNESS = 5;

        // -------------------------
        // LOGICAL FUNCTIONS FOR UI
        // -------------------------
        private static int FontSize, PosX, PosY;
        public static void DrawLabel(Rectangle Bounds, string Text, Color color)
        {
            if (Text.Length == 0) return;
            FontSize = (int)(Bounds.Width - 5) / Text.Length;
            PosX = (int)(Bounds.Center.X - (Raylib.MeasureText(Text, FontSize) / 2));
            PosY = (int)(Bounds.Center.Y - (FontSize / 2));
            Raylib.DrawText(Text, PosX, PosY, FontSize, color);
        }
        public static void DrawHeading(Rectangle Bounds, string Text, Color FontColor, int FontSize, float padding)
        {
            if (Text.Length == 0) return;
            PosX = (int)Bounds.Center.X - (Raylib.MeasureText(Text, FontSize) / 2);
            PosY = (int)((int)Bounds.Y + padding + (FontSize / 2));
            Raylib.DrawText(Text, PosX, PosY, FontSize, FontColor);
        }
        public static bool UIClicked(Rectangle Bounds)
        {
            // if the mouse is clicked and its hovered 
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && UIHover(Bounds))
            {
                return true; // its clicked   
            }
            return false;
        }
        public static bool UIHeld(Rectangle Bounds)
        {
            // mouse if down not just clicked
            if (Raylib.IsMouseButtonDown(MouseButton.Left) && UIHover(Bounds))
            {
                return true; // being held
            }

            return false; // not held
        }
        public static bool UIHover(Rectangle Bounds)
        {
            if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), Bounds))
            {
                return true;
            }
            return false;
        }

        // -------------------------
        // ACTUAL FUNCTIONS FOR UI
        // -------------------------
        private static Rectangle Slider = new();
        private static float ratio = 0;
        public static void UISlider(Rectangle Bounds, ref Vector2 SliderCenter, ref float Value, float Min, float Max, bool fit=false, string label="Slider")
        {
            // Logic ---------------------------

            // Defining the slider rectangle
            Slider.Position = SliderCenter;
            Slider.Height = (fit == false)? Bounds.Height + 7: Bounds.Height;
            Slider.Width = Slider.Height / 2;

            // Keeping the slider with the bounds
            SliderCenter.Y = Bounds.Center.Y - (Slider.Height / 2);

            // Checking if it collided with the slider
            if (UIHeld(Slider))
            {
                // follow the mouse
                SliderCenter.X = Raylib.GetMouseX();
                SliderCenter.X -= Slider.Width / 2;
                SliderCenter.X = Math.Clamp(SliderCenter.X, Bounds.X - (Slider.Width/2), Bounds.X + Bounds.Width - (Slider.Width/2));

                // Changing the value
                ratio = (Slider.X-Bounds.X) / Bounds.Width;
                Value = ratio * Max;
                Value = Math.Clamp(Value, Min, Max); // clamping    
            }
            // Drawing

            // Base rectangle
            Raylib.DrawRectangleRec(Bounds, Main);

            // Outline
            Raylib.DrawRectangleLinesEx(Bounds, THICKNESS, Outline);

            // Label
            DrawLabel(Bounds, label, Color.Black);

            // Slider rectangle
            Raylib.DrawRectangleRec(Slider, Main);

            // Hovering
            if (UIHover(Slider))
            {
                // Highlighting
                Raylib.DrawRectangleRec(Slider, Hover);
            }

            // outline
            Raylib.DrawRectangleLinesEx(Slider, THICKNESS, Outline);        
        }
        private static Color TempColor = Main;
        public static void UIToggle(Rectangle Bounds, ref bool Value, string Label = "Toggle")
        {
            // Setting Colors
            TempColor = Main;

            // Logic
            if (UIClicked(Bounds))
            {
                Value = !Value; // switch the value
            }
            else if (UIHover(Bounds)) // setting the hover color
            {
                TempColor = Hover;
            }
            // Drawing 
            Raylib.DrawRectangleRec(Bounds, TempColor);
            Raylib.DrawRectangleLinesEx(Bounds, THICKNESS, Outline);
            DrawLabel(Bounds, Label, Outline);
        }
        public static void UIButton(Rectangle Bounds, Action? action, string Label = "Button")
        {
            // Setting Colors
            TempColor = Main;

            // Logic
            if (UIClicked(Bounds))
            {
                action?.Invoke(); // run the action
            }
            else if (UIHover(Bounds)) // setting the hover color
            {
                TempColor = Hover;
            }
            // Drawing 
            Raylib.DrawRectangleRec(Bounds, TempColor);
            Raylib.DrawRectangleLinesEx(Bounds, THICKNESS, Outline);
            DrawLabel(Bounds, Label, Outline);
        }
        public static void UIPanel(Rectangle Bounds, float padding, int FontSize, Color FontColor, string Heading = "Panel")
        {
            // Drawing the bounds
            Raylib.DrawRectangleRec(Bounds, Main);
            Raylib.DrawRectangleLinesEx(Bounds, THICKNESS, Outline);

            // Drawing the label
            DrawHeading(Bounds, Heading, FontColor, FontSize, padding);
        }
        
    }
    class ColorPicker
    {
        // Defining the rectangles of the color picker
        public Rectangle ColorPickRec;
        public string Label;
        private Rectangle Red, Green, Blue, LabelRec;
        private Vector2 RedC, GreenC, BlueC;
        private float R,G,B;
        private Color SelfColor;

        // constructor for defining all of the variables
        public ColorPicker(Vector2 Center, Color color, string label)
        {
            // Label and color
            Label = label;
            SelfColor = color;

            // Defining the rectangles
            LabelRec = new(Center.X - 50, Center.Y - 60, 100, 30);
            Red = new(Center.X - 50, Center.Y - 30, 100, 30);
            Blue = new(Center.X - 50, Center.Y, 100, 30);
            Green = new(Center.X - 50, Center.Y + 30, 100, 30);
            ColorPickRec = new(LabelRec.Position, 100, 120);

            // Defining the centers
            RedC = Red.Center;
            BlueC = Blue.Center;
            GreenC = Green.Center;

            // Defining the colors
            R=0;G=0;B= 0;
        }

        public Color Update()
        {
            // Drawing the panel and label
            BaseGUI.UIPanel(ColorPickRec, 0, 0, Color.White, "");
            BaseGUI.DrawHeading(LabelRec, Label, Color.Black, 15, 5);

            // Sliders for changing color

            // RED
            BaseGUI.UISlider(Red, ref RedC, ref R, 0, 255, true, "");
            Raylib.DrawRectangleLinesEx(Red, BaseGUI.THICKNESS, Color.Red);

            // GREEN
            BaseGUI.UISlider(Green, ref GreenC, ref G, 0, 255, true, "");
            Raylib.DrawRectangleLinesEx(Green, BaseGUI.THICKNESS, Color.Green);

            // BLUE
            BaseGUI.UISlider(Blue, ref BlueC, ref B, 0, 255, true, "");
            Raylib.DrawRectangleLinesEx(Blue, BaseGUI.THICKNESS, Color.Blue);
            MapColorValueFloat(ref SelfColor, R, G, B);

            return SelfColor;
        }
        private static void MapColorValueFloat(ref Color c, float r, float g, float b)
        {
            c.R = (byte)r; // RED
            c.G = (byte)g; // GREEN
            c.B = (byte)b; // BLUE
        }
    }

    
}
