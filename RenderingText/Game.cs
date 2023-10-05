// <copyright file="Game.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace RenderingText;

using Velaptor;
using Velaptor.Content.Fonts;
using Velaptor.Factories;
using Velaptor.Graphics.Renderers;
using System.Drawing;
using System.Numerics;
using Velaptor.Batching;
using Velaptor.UI;

/// <summary>
/// The main game class.
/// </summary>
public class Game : Window
{
    private const string Text = "Hello Velaptor!";
    private readonly Random random = new (); // Creates random numbers
    private IFontRenderer? fontRenderer; // Renders text
    private IBatcher? batcher;
    private IFont? font; // The type of font
    private Vector2 velocity = new (100, 100);
    private Vector2 position = new (400, 400);
    private Color textColor = Color.White; // The color of the text

    /// <summary>
    /// Initializes a new instance of the <see cref="Game"/> class.
    /// </summary>
    public Game()
    {
        Title = "Render Text Guide";
        Width = 800;
        Height = 800;
    }

    /// <summary>
    /// Loads game content.
    /// </summary>
    protected override void OnLoad()
    {
        var rendererFactory = new RendererFactory();
        this.fontRenderer = rendererFactory.CreateFontRenderer();
        this.batcher = rendererFactory.CreateBatcher();
        this.font = ContentLoader.LoadFont("TimesNewRoman-Regular", 24);

        base.OnLoad();
    }

    /// <summary>
    /// Updates the application. Executes one time for every iteration of the game loop
    /// and always BEFORE the <see cref="OnDraw"/> method.
    /// </summary>
    /// <param name="frameTime">The amount of time that has passed for the current frame.</param>
    protected override void OnUpdate(FrameTime frameTime)
    {
        ProcessCollisionAndColor();
        var displacement = this.velocity * (float)frameTime.ElapsedTime.TotalSeconds;
        this.position += displacement;

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

        this.fontRenderer.Render(this.font, Text, (int)this.position.X, (int)this.position.Y, this.textColor);

        this.batcher.End();

        base.OnDraw(frameTime);
    }

    /// <summary>
    /// Processes collision with the edges of the window and randomly set the color.
    /// </summary>
    private void ProcessCollisionAndColor()
    {
        var textSize = this.font.Measure(Text);
        var halfWidth = textSize.Width / 2;
        var halfHeight = textSize.Height / 2;

        var leftSide = this.position.X - halfWidth;
        var top = this.position.Y - halfHeight;
        var rightSide = this.position.X + halfWidth;
        var bottom = this.position.Y + halfHeight;

        if (leftSide <= 0)
        {
            this.velocity.X *= -1;
            RandomizeColor();
        }

        if (top <= 0)
        {
            this.velocity.Y *= -1;
            RandomizeColor();
        }

        if (rightSide >= Width)
        {
            this.velocity.X *= -1;
            RandomizeColor();
        }

        if (bottom >= Height)
        {
            this.velocity.Y *= -1;
            RandomizeColor();
        }
    }

    /// <summary>
    /// Randomizes the text color.
    /// </summary>
    private void RandomizeColor()
    {
        var red = this.random.Next(0, 255); // Create a random red value
        var green = this.random.Next(0, 255); // Create a random green value
        var blue = this.random.Next(0, 255); // Create a random blue value

        // Set the text color
        this.textColor = Color.FromArgb(255, red, green, blue);
    }
}
