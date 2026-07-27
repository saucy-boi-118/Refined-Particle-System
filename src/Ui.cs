using Raylib_cs;


namespace UI
{
    class UIFunctions
{
    public enum Style
    {
        BaseSimple,        
    }

    private static readonly Dictionary<Style, Color> MainColor = new()
    {
        {Style.BaseSimple, Color.White},
    };
    private static readonly Dictionary<Style, Color> HoverColor = new()
    {
        {Style.BaseSimple, Color.DarkGray},
    };
    private static readonly Dictionary<Style, Color> HighLightColor = new()
    {
        {Style.BaseSimple, Color.Black},
    };
    private static readonly Dictionary<Style, float> Thickness = new()
    {
        {Style.BaseSimple, 5},
    };

    // -------------------------
    // LOGICAL FUNCTIONS FOR UI
    // -------------------------
    private static int FontSize, PosX, PosY;
    public static void DrawLabel(Rectangle Bounds, string Text, Color color)
    {
        if (Text.Length == 0) return;
        FontSize = (int)(Bounds.Width-5) / Text.Length;
        PosX = (int)(Bounds.Center.X-(Raylib.MeasureText(Text,FontSize)/2));
        PosY = (int)(Bounds.Center.Y-(FontSize/2));
        Raylib.DrawText(Text, PosX, PosY, FontSize, color);
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
    private static Color Main;
    private static float ratio;
    public static void UISlider(ref float value, float min, float max, Rectangle Base, ref Rectangle slider, Style style)
    {
        // Setting color
        Main = MainColor[style];

        // Logic ---------------------------

        // Keeping the slider with the bounds
        slider.Y = Base.Center.Y - (slider.Height/2);

        // Checking if it collided with the slider
        if (UIHeld(slider))
        {
            // follow the mouse
            slider.X = Raylib.GetMouseX();
            slider.X -= slider.Width/2;
            slider.X = Math.Clamp(slider.X, Base.X-slider.Width, Base.X+Base.Width);

            // Changing the value
            ratio = (slider.X+slider.Width - Base.X)  / Base.Width;
            value = ratio * max;
            value = Math.Clamp(value, min, max); // clamping    
        }
        // Drawing

        // Base rectangle
        Raylib.DrawRectangleRec(Base, Main);
        Raylib.DrawRectangleLinesEx(Base, Thickness[Style.BaseSimple], HighLightColor[Style.BaseSimple]);

        // Slider rectangle
        Raylib.DrawRectangleRec(slider, Main);
        Raylib.DrawRectangleLinesEx(slider, Thickness[Style.BaseSimple], HighLightColor[Style.BaseSimple]);

        // Hovering
        if (UIHover(slider))
        {
            // Highlighting
            Raylib.DrawRectangleRec(slider, HoverColor[Style.BaseSimple]);
        }

    }
    public static void UIToggle(ref bool Value, Rectangle Bounds, Style style, string Label="Toggle") 
    {
        // Setting Colors
        Main = MainColor[style];
        // Logic
        if (UIClicked(Bounds))
        {
            Value = !Value; // switch the value
        }
        else if (UIHover(Bounds)) // setting the hover color
        {
            Main = HoverColor[style];
        }
        // Drawing 
        Raylib.DrawRectangleRec(Bounds, Main);
        Raylib.DrawRectangleLinesEx(Bounds, Thickness[style], HighLightColor[style]);
        DrawLabel(Bounds, Label, HighLightColor[style]);
    }
    public static void UIButton(Rectangle Bounds, Style style, Action? action, string Label="Button")
    {
        // Setting Colors
        Main = MainColor[style];
        // Logic
        if (UIClicked(Bounds))
        {
            action?.Invoke(); // run the action
        }
        else if (UIHover(Bounds)) // setting the hover color
        {
            Main = HoverColor[style];
        }
        // Drawing 
        Raylib.DrawRectangleRounded(Bounds, 0.5f, 5, Main);
        Raylib.DrawRectangleRoundedLinesEx(Bounds, 0.5f, 5, Thickness[style], HighLightColor[style]);
        DrawLabel(Bounds, Label, HighLightColor[style]);
    }
    public static void UIPanel(Rectangle Bounds, Style style, string Label="Panel") // just a rectangle
    {
        // Setting the main color
        Main = MainColor[style];

        // Drawing
        Raylib.DrawRectangleRounded(Bounds, 0.5f, 5, Main);
        Raylib.DrawRectangleRoundedLinesEx(Bounds, 0.5f, 5, Thickness[style], HighLightColor[style]);
        DrawLabel(Bounds, Label, HighLightColor[style]);
    }
}
    
}
