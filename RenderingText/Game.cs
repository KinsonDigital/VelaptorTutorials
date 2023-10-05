// <copyright file="Game.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace RenderingText;

using Velaptor;
using Velaptor.Content.Fonts;
using Velaptor.Factories;
using Velaptor.Graphics.Renderers;
using System.Drawing;
using Velaptor.Batching;
using Velaptor.UI;

/// <summary>
/// The main game class.
/// </summary>
public class Game : Window
{
    private readonly Random random = new (); // Creates random numbers
    private readonly IFontRenderer fontRenderer; // Renders text
    private readonly IBatcher batcher;
    private IFont? font; // The type of font
    private Color textColor = Color.White; // The color of the text
    private double elapsedMs; // Total amount of time that has passed in milliseconds

    /// <summary>
    /// Initializes a new instance of the <see cref="Game"/> class.
    /// </summary>
    public Game()
    {
        Title = "Hello Velaptor";
        var rendererFactory = new RendererFactory();
        this.fontRenderer = rendererFactory.CreateFontRenderer();

        this.batcher = rendererFactory.CreateBatcher();
    }

    /// <summary>
    /// Loads game content.
    /// </summary>
    protected override void OnLoad()
    {
        this.font = ContentLoader.LoadFont("timesNewRoman-Regular", 24);

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
        this.elapsedMs += frameTime.ElapsedTime.TotalMilliseconds;

        // If 1,000 milli-seconds(1 second) has passed
        if (this.elapsedMs >= 1000)
        {
            this.elapsedMs = 0; // Reset back to zero to restart the timer

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
        this.batcher.Begin();

        var x = (int)(Width / 2); // Center of the window horizontally
        var y = (int)(Height / 2); // Center of the window vertically

        // Render the string to the screen with the randomized color
        this.fontRenderer.Render(this.font, "Hello Velaptor!!", x, y, this.textColor);

        this.batcher.End();

        base.OnDraw(frameTime);
    }
}
