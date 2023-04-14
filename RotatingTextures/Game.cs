// <copyright file="Game.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace RotatingTextures;

using Velaptor;
using Velaptor.Content;
using Velaptor.Factories;
using Velaptor.Graphics.Renderers;
using Velaptor.UI;

/// <summary>
/// The main game class.
/// </summary>
public class Game : Window
{
    private readonly ITextureRenderer textureRenderer;
    private ITexture? textTexture;
    private ITexture? gearTexture;
    private float angle;
    private float angleSpeed = 50;

    /// <summary>
    /// Initializes a new instance of the <see cref="Game"/> class.
    /// </summary>
    public Game()
    {
        Title = "Rotating Textures";
        var renderFactory = new RendererFactory();
        this.textureRenderer = renderFactory.CreateTextureRenderer();
    }

    /// <summary>
    /// Loads game content.
    /// </summary>
    protected override void OnLoad()
    {
        this.textTexture = ContentLoader.LoadTexture("text");
        this.gearTexture = ContentLoader.LoadTexture("gear");

        base.OnLoad();
    }

    /// <summary>
    /// Updates the game state. Executes one time for every iteration of the game loop.
    /// </summary>
    /// <param name="frameTime">The amount of time that has passed for the current frame.</param>
    protected override void OnUpdate(FrameTime frameTime)
    {
        var angleVelocity = this.angleSpeed * (float)frameTime.ElapsedTime.TotalSeconds;

        this.angle += angleVelocity;

        base.OnUpdate(frameTime);
    }

    /// <summary>
    /// Draws to the screen. Executes one time for every iteration of the game loop
    /// and always AFTER the <see cref="OnDraw"/> method has finished.
    /// </summary>
    /// <param name="frameTime">The amount of time that has passed for the current frame.</param>
    protected override void OnDraw(FrameTime frameTime)
    {
        IRenderer.Begin();

        var x = (int)(Width / 2); // Center of the window horizontally
        var y = (int)(Height / 2); // Center of the window vertically

        // Render the gear texture in the center of the window
        this.textureRenderer.Render(this.gearTexture, x, y, this.angle);

        // Render the text texture on top of the gear texture in the center of the window
        this.textureRenderer.Render(this.textTexture, x, y, 2);

        IRenderer.End();

        base.OnDraw(frameTime);
    }
}
