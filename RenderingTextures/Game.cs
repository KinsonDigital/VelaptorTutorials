using Velaptor;
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

    protected override void OnLoad()
    {
        this.mascotTexture = ContentLoader.LoadTexture("velaptor-mascot");

        base.OnLoad();
    }

    protected override void OnDraw(FrameTime frameTime)
    {
        IRenderer.Begin();

        var x = (int)(Width / 2);
        var y = (int)(Height / 2);

        this.textureRenderer.Render(this.mascotTexture, x, y);

        IRenderer.End();

        base.OnDraw(frameTime);
    }
}
