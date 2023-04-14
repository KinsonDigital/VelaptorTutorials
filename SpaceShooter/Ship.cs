// <copyright file="Ship.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace SpaceShooter;

using System.Drawing;
using System.Numerics;
using Carbonate.UniDirectional;
using Velaptor;
using Velaptor.Content;
using Velaptor.Factories;
using Velaptor.Graphics.Renderers;
using Velaptor.Input;

/// <summary>
/// The player ship.
/// </summary>
public class Ship : IUpdatable, IDrawable
{
    private const float VelocityX = 50;
    private const float VelocityY = 50;
    private const float MaxVel = 350;
    private readonly ITextureRenderer renderer;
    private readonly ITexture texture;
    private readonly IAppInput<KeyboardState> keyboard;
    private readonly Vector2 minVelocity = new (-MaxVel, -MaxVel);
    private readonly Vector2 maxVelocity = new (MaxVel, MaxVel);
    private readonly PushReactable<Vector2> posUpdater;
    private readonly Weapon weapon;
    private KeyboardState currentKeyState;
    private KeyboardState prevKeyState;
    private bool leftKeyDown;
    private bool rightKeyDown;
    private bool upKeyDown;
    private bool downKeyDown;
    private bool isNotMovingHorizontally;
    private bool isNotMovingVertically;
    private Vector2 shipPos;
    private Vector2 velocity;

    public Ship(Rectangle worldBounds)
    {
        this.posUpdater = new PushReactable<Vector2>();

        var renderFactory = new RendererFactory();
        this.renderer = renderFactory.CreateTextureRenderer();

        var contentLoader = ContentLoaderFactory.CreateTextureLoader();
        this.texture = contentLoader.Load("ship");

        this.keyboard = InputFactory.CreateKeyboard();

        this.weapon = new Weapon(this.posUpdater, worldBounds, new Size((int)this.texture.Width, (int)this.texture.Height));

        // Set the starting position of the ship to the center of the world
        this.shipPos = new Vector2(worldBounds.Width / 2f, worldBounds.Height - (worldBounds.Height / 4f));
    }

    /// <summary>
    /// Updates the ship.
    /// </summary>
    /// <param name="frameTime">The total amount of time for the current frame.</param>
    public void Update(FrameTime frameTime)
    {
        this.currentKeyState = this.keyboard.GetState();

        UpdateKeyStates();
        MoveShip(frameTime);

        var shouldFireWeapon = this.prevKeyState.IsKeyDown(KeyCode.Space) &&
                                  this.currentKeyState.IsKeyUp(KeyCode.Space);

        if (shouldFireWeapon)
        {
            this.weapon.Fire();
        }

        this.weapon.Update(frameTime);

        this.prevKeyState = this.currentKeyState;
    }

    /// <summary>
    /// Renders the ship.
    /// </summary>
    public void Render()
    {
        this.weapon.Render();

        // Render the ship image in the center of the window
        this.renderer.Render(this.texture, (int)this.shipPos.X, (int)this.shipPos.Y);
    }

    /// <summary>
    /// Moves the ship based on keyboard input.
    /// </summary>
    /// <param name="frameTime">The total amount of time for the current frame.</param>
    private void MoveShip(FrameTime frameTime)
    {
        // Increase velocity in each direction of commanded
        this.velocity.X -= this.leftKeyDown ? VelocityX : 0;
        this.velocity.X += this.rightKeyDown ? VelocityX : 0;
        this.velocity.Y -= this.upKeyDown ? VelocityY : 0;
        this.velocity.Y += this.downKeyDown ? VelocityY : 0;

        // Stop moving the ship if the left or right key is no longer being pressed
        this.velocity.X = this.isNotMovingHorizontally ? 0 : this.velocity.X;

        // Stop moving the ship if the up or down key is no longer being pressed
        this.velocity.Y = this.isNotMovingVertically ? 0 : this.velocity.Y;

        // Limit the maximum velocity of the ship in any direction
        this.velocity = Vector2.Clamp(this.velocity, this.minVelocity, this.maxVelocity);

        // Calculate the movement distance for this frame
        var displacement = this.velocity * (float)frameTime.ElapsedTime.TotalSeconds;

        // Apply the movement distance to the ship's position
        this.shipPos += displacement;

        // Update the position of the ship to the weapon for bullet positioning
        this.posUpdater.Push(this.shipPos, Events.UpdatePosition);
    }

    /// <summary>
    /// Updates the state of the keys.
    /// </summary>
    private void UpdateKeyStates()
    {
        this.leftKeyDown = this.currentKeyState.IsKeyDown(KeyCode.Left);
        this.rightKeyDown = this.currentKeyState.IsKeyDown(KeyCode.Right);
        this.upKeyDown = this.currentKeyState.IsKeyDown(KeyCode.Up);
        this.downKeyDown = this.currentKeyState.IsKeyDown(KeyCode.Down);
        this.isNotMovingHorizontally = this.currentKeyState.IsKeyUp(KeyCode.Right) && this.currentKeyState.IsKeyUp(KeyCode.Left);
        this.isNotMovingVertically = this.currentKeyState.IsKeyUp(KeyCode.Up) && this.currentKeyState.IsKeyUp(KeyCode.Down);
    }
}
