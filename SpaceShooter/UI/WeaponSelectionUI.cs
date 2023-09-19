// <copyright file="WeaponSelectionUI.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace SpaceShooter.UI;

using System.Drawing;
using System.Numerics;
using Carbonate.Fluent;
using Signals;
using Velaptor;
using Velaptor.Content;
using Velaptor.Factories;
using Velaptor.Graphics.Renderers;

/// <summary>
/// Shows an indicator of the currently selected weapon.
/// </summary>
public sealed class WeaponSelectionUI
{
    private const int ReticleMargin = 0;
    private readonly ITextureRenderer textureRenderer;
    private readonly ITexture selectionReticle;
    private readonly ITexture laser;
    private readonly float selectionHalfWidth;
    private readonly float selectionHalfHeight;
    private Vector2 whitePos;
    private Vector2 redPos;
    private Vector2 greenPos;
    private Vector2 bluePos;

    public WeaponSelectionUI(ISwapWeaponSignal swapWeaponSignal)
    {
        var swapSubscription = ISubscriptionBuilder.Create()
            .WithId(SignalIds.SwapWeapon)
            .BuildOneWayReceive<WeaponType>(weaponType => TypeOfWeapon = weaponType);

        swapWeaponSignal.Subscribe(swapSubscription);

        var contentLoader = ContentLoaderFactory.CreateTextureLoader();
        this.selectionReticle = contentLoader.Load("weapon-selection");
        this.laser = contentLoader.Load("laser.png");

        this.selectionHalfWidth = this.selectionReticle.Width / 2f;
        this.selectionHalfHeight = this.selectionReticle.Height / 2f;

        var renderFactory = new RendererFactory();
        this.textureRenderer = renderFactory.CreateTextureRenderer();
    }


    public Vector2 Position { get; set; }

    public WeaponType TypeOfWeapon { get; private set; }

    public void Update(FrameTime frameTime)
    {
        // Calculate the position for all of the lasers and reticles
        var redX = Position.X - this.selectionHalfWidth - ReticleMargin;
        var whiteX = redX - this.selectionHalfWidth - ReticleMargin;
        var greenX = Position.X + this.selectionHalfWidth + ReticleMargin;
        var blueX = greenX + this.selectionHalfWidth + ReticleMargin;

        this.whitePos = new Vector2(whiteX, Position.Y);
        this.redPos = new Vector2(redX, Position.Y);
        this.greenPos = new Vector2(greenX, Position.Y);
        this.bluePos = new Vector2(blueX, Position.Y);
    }

    public void Render()
    {
        // Render the white laser
        this.textureRenderer.Render(this.laser, this.whitePos, Color.White);
        this.textureRenderer.Render(this.laser, this.redPos, Color.Red);
        this.textureRenderer.Render(this.laser, this.greenPos, Color.Green);
        this.textureRenderer.Render(this.laser, this.bluePos, Color.Blue);
    }
}
