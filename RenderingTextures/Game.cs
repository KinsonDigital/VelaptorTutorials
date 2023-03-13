﻿using Velaptor;
using Velaptor.Content;
using Velaptor.Factories;
using Velaptor.Graphics.Renderers;
using Velaptor.UI;

namespace RenderingTextures;

public class Game : Window
{
    private readonly ITextureRenderer textureRenderer;
    private ITexture mascotTexture;

    public Game()
    {
        Title = "Render Textures";
        var renderFactory = new RendererFactory();
        this.textureRenderer = renderFactory.CreateTextureRenderer();
    }

    /// <summary>
    /// Loads game content.
    /// </summary>
    protected override void OnLoad()
    {
        this.mascotTexture = ContentLoader.LoadTexture("velaptor-mascot");

        base.OnLoad();
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

        // Render the mascot image in the center of the window
        this.textureRenderer.Render(this.mascotTexture, x, y);

        IRenderer.End();

        base.OnDraw(frameTime);
    }
}
