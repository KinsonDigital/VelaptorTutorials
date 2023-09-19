// <copyright file="Weapon.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace SpaceShooter;

using System.Drawing;
using System.Numerics;
using Carbonate.Fluent;
using Carbonate.OneWay;
using Signals;
using Signals.Data;
using Velaptor;
using Velaptor.Content;
using Velaptor.Factories;
using Velaptor.Graphics.Renderers;

/// <summary>
/// The ship's weapon.
/// </summary>
public class Weapon : IUpdatable, IDrawable
{
    private readonly IPushReactable<WeaponType> swapWeaponReactable;
    private readonly ITextureRenderer renderer;
    private readonly ITexture texture;
    private readonly IDisposable unsubscriber;
    private readonly List<Bullet> bullets = new ();
    private readonly int[] weaponTypeValues;
    private Rectangle worldBounds;
    private SizeF shipSize;
    private Vector2 shipPos;

    /// <summary>
    /// Initializes a new instance of the <see cref="Weapon"/> class.
    /// </summary>
    /// <param name="shipSignal">Provides notifications of the ships position.</param>
    /// <param name="swapWeaponReactable">Sends a signal that a weapon has been swapped.</param>
    /// <param name="worldDataReactable">Receives updates about the world.</param>
    public Weapon(
        IShipSignal shipSignal,
        ISwapWeaponSignal swapWeaponReactable,
        IWorldSignal worldDataReactable)
    {
        this.swapWeaponReactable = swapWeaponReactable;

        var worldUpdateSubscription = ISubscriptionBuilder.Create()
            .WithId(SignalIds.WorldDataUpdate)
            .BuildOneWayReceive<WorldData>(worldData => this.worldBounds = worldData.WorldBounds);

        worldDataReactable.Subscribe(worldUpdateSubscription);

        this.weaponTypeValues = Enum.GetValues(typeof(WeaponType)).Cast<int>().ToArray();

        this.shipSize = shipSize;
        var renderFactory = new RendererFactory();
        this.renderer = renderFactory.CreateTextureRenderer();

        var textureLoader = ContentLoaderFactory.CreateTextureLoader();
        this.texture = textureLoader.Load("laser");

        var shipSignalSubscription = ISubscriptionBuilder.Create()
            .WithId(SignalIds.ShipUpdate)
            .BuildOneWayReceive<ShipData>(shipData =>
            {
                this.shipPos = shipData.ShipPos;
                this.shipSize = shipData.ShipSize;
            });

        shipSignal.Subscribe(shipSignalSubscription);
    }

    public WeaponType TypeOfWeapon { get; private set; } = WeaponType.White;

    /// <summary>
    /// Updates the bullets of the weapon.
    /// </summary>
    /// <param name="frameTime">The total amount of time for the current frame.</param>
    public void Update(FrameTime frameTime)
    {
        foreach (var bullet in this.bullets)
        {
            bullet.Update(frameTime);
        }
    }

    /// <summary>
    /// Renders the bullets of the weapon.
    /// </summary>
    public void Render()
    {
        // Render all of the bullets
        foreach (var bullet in this.bullets)
        {
            if (bullet.IsVisible)
            {
                this.renderer.Render(this.texture, (int)bullet.Position.X, (int)bullet.Position.Y, Color.Orange);
            }
        }
    }

    /// <summary>
    /// Fires the weapon.
    /// </summary>
    public void Fire()
    {
        var noBulletExists = true;

        for (var i = 0; i < this.bullets.Count; i++)
        {
            if (this.bullets[i].IsVisible)
            {
                continue;
            }

            noBulletExists = false;

            this.bullets[i] = SetupBullet(this.bullets[i]);

            break;
        }

        if (!noBulletExists)
        {
            return;
        }

        var bullet = CreateBullet();

        bullet = SetupBullet(bullet);
        this.bullets.Add(bullet);
    }

    /// <summary>
    /// Swaps to the next weapon.
    /// </summary>
    public void SwapWeapon()
    {
        if ((int)TypeOfWeapon > this.weaponTypeValues.Max())
        {
            TypeOfWeapon = (WeaponType)this.weaponTypeValues.Min();
        }
        else
        {
            TypeOfWeapon++;
        }

        // Send a notification to the UI that the weapon has been swapped
        this.swapWeaponReactable.Push(TypeOfWeapon, SignalIds.SwapWeapon);
    }

    /// <summary>
    /// Creates a new bullet.
    /// </summary>
    /// <returns>The new bullet.</returns>
    private Bullet CreateBullet()
    {
        // Check the object pool to see if any bullets are available
        foreach (var bullet in this.bullets)
        {
            // If the bullet is hidden, it is not being used.
            // Return the bullet to put it to use
            if (!bullet.IsVisible)
            {
                return bullet;
            }
        }

        // If this point is reached, then no bullets are available in the pool
        // Create a brand new bullet object to add to the pool and return it
        var newBullet = new Bullet(
            new Rectangle(0, 0, this.worldBounds.Width, this.worldBounds.Height),
            new Vector2(this.texture.Width, this.texture.Height));

        this.bullets.Add(newBullet);

        return newBullet;
    }

    /// <summary>
    /// Sets up the given <paramref name="bullet"/> before it is fired from the weapon.
    /// </summary>
    /// <param name="bullet">The bullet to setup.</param>
    /// <returns>The ready to fire bullet.</returns>
    private Bullet SetupBullet(Bullet bullet)
    {
        bullet.IsVisible = true;

        const int bulletVerticalOffset = 15;
        var shipHalfHeight = this.shipSize.Width / 2f;
        var shipTop = this.shipPos.Y - shipHalfHeight;
        var bulletHalfHeight = this.texture.Height / 2f;
        var bulletPosY = shipTop - bulletHalfHeight + bulletVerticalOffset;

        bullet.Position = this.shipPos with { Y = bulletPosY };

        return bullet;
    }
}
