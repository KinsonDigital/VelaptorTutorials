using Velaptor;
using Velaptor.Content.Fonts;
using Velaptor.Factories;
using Velaptor.Graphics.Renderers;
using System.Drawing;
using Velaptor.UI;

namespace HelloVelaptor;

public class Game : Window
{
    private readonly Random random = new (); // Creates random numbers
    private IFont font; // The type of font
    private IFontRenderer fontRenderer; // Renders text
    private Color textColor = Color.White; // The color of the text
    private double elapsedMS; // Total amount of time that has passed in milliseconds

    public Game()
    {
        Title = "Hello Velaptor";
        var rendererFactory = new RendererFactory();
        this.fontRenderer = rendererFactory.CreateFontRenderer();
    }

    /// <summary>
    /// Loads game content.
    /// </summary>
    protected override void OnLoad()
    {
        this.font = ContentLoader.LoadFont("TimesNewRoman-Regular.ttf", 24);

        base.OnLoad();
    }

    /// <summary>
    /// Updates the application. Executes one time for every iteration of the game loop
    /// and always BEFORE the <see cref="OnDraw"/> method.
    /// </summary>
    /// <param name="frameTime">The amount of time that has passed for the current frame.</param>
    protected override void OnUpdate(FrameTime frameTime)
    {
        // Keep track of how many milli-seconds for the current frame
        this.elapsedMS += frameTime.ElapsedTime.TotalMilliseconds;

        // If 1,000 milli-seconds(1 second) has passed
        if (this.elapsedMS >= 1000)
        {
            this.elapsedMS = 0; // Reset back to zero to restart the timer

            var red = this.random.Next(0, 255); // Create a random red value
            var green = this.random.Next(0, 255); // Create a random green value
            var blue = this.random.Next(0, 255); // Create a random blue value

            // Set the text color
            this.textColor = Color.FromArgb(255, red, green, blue);
        }

        base.OnUpdate(frameTime);
    }

    /// <summary>
    /// Draws to the screen. Executes one time for every iteration of the game loop
    /// and always AFTER the <see cref="OnUpdate"/> method has finished.
    /// </summary>
    /// <param name="frameTime">The amount of time that has passed for the current frame.</param>
    protected override void OnDraw(FrameTime frameTime)
    {
        IRenderer.Begin();

        var x = (int)(Width / 2); // Center of the window horizontally
        var y = (int)(Height / 2); // Center of the window vertically

        // Render the string to the screen with the randomized color
        this.fontRenderer.Render(this.font, "Hello Velaptor!!", x, y, Color.Orange);// this.textColor);

        IRenderer.End();

        base.OnDraw(frameTime);
    }
}
