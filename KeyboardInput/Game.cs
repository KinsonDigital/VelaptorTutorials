// <copyright file="Game.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace KeyboardInput;

using System.Drawing;
using System.Numerics;
using Velaptor;
using Velaptor.Content;
using Velaptor.Factories;
using Velaptor.Graphics.Renderers;
using Velaptor.Input;
using Velaptor.UI;

/// <summary>
/// The main game class.
/// </summary>
public class Game : Window
{
    private readonly ITextureRenderer textureRenderer;
    private readonly IAppInput<KeyboardState> keyboard;
    private const float VelocityX = 50;
    private const float VelocityY = 50;
    private const float MaxVel = 350;
    private ITexture? shipTexture;
    private ITexture laserTexture;
    private KeyboardState currentKeyState;
    private KeyboardState prevKeyState;
    private Vector2 shipPos;
    private List<Laser> lasers = new ();
    private Vector2 velocity;
    private bool leftKeyDown;
    private bool rightKeyDown;
    private bool upKeyDown;
    private bool downKeyDown;
    private bool fireLazerKeyPressed;
    private bool isNotMovingHorizontally;
    private bool isNotMovingVertically;

    /// <summary>
    /// Initializes a new instance of the <see cref="Game"/> class.
    /// </summary>
    public Game()
    {
        Title = "Keyboard Input";
        Width = Height;
        var renderFactory = new RendererFactory();
        this.textureRenderer = renderFactory.CreateTextureRenderer();

        this.keyboard = InputFactory.CreateKeyboard();

        // Set the starting position of the ship to the center of the window
        this.shipPos = new Vector2(Width / 2f, Height - (Height / 4f));
    }

    /// <summary>
    /// Loads game content.
    /// </summary>
    protected override void OnLoad()
    {
        this.shipTexture = ContentLoader.LoadTexture("ship");
        this.laserTexture = ContentLoader.LoadTexture("orange-lazer.png");

        base.OnLoad();
    }

    /// <summary>
    /// Updates the application. Executes one time for every iteration of the game loop
    /// and always BEFORE the <see cref="OnDraw"/> method.
    /// </summary>
    /// <param name="frameTime">The amount of time that has passed for the current frame.</param>
    protected override void OnUpdate(FrameTime frameTime)
    {
        UpdateKeyStates();
        MoveShip(frameTime);

        if (this.fireLazerKeyPressed)
        {
            var noLaserExists = true;

            for (var i = 0; i < this.lasers.Count; i++)
            {
                if (this.lasers[i].IsVisible)
                {
                    continue;
                }

                noLaserExists = false;

                this.lasers[i] = SetupLaser(this.lasers[i]);

                break;
            }

            if (noLaserExists)
            {
                var laser = CreateLaser();

                laser = SetupLaser(laser);
                this.lasers.Add(laser);
            }

            this.fireLazerKeyPressed = false;
        }

        foreach (var laser in this.lasers)
        {
            laser.Update(frameTime);
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

        // Render the ship image in the center of the window
        this.textureRenderer.Render(this.shipTexture, (int)this.shipPos.X, (int)this.shipPos.Y);

        // Render all of the lasers
        for (var i = 0; i < this.lasers.Count; i++)
        {
            Laser laser = this.lasers[i];

            if (laser.IsVisible)
            {
                this.textureRenderer.Render(this.laserTexture, (int)laser.Position.X, (int)laser.Position.Y);
            }
        }

        IRenderer.End();

        base.OnDraw(frameTime);
    }

    private Laser CreateLaser()
    {
        foreach (var laser in this.lasers)
        {
            if (!laser.IsVisible)
            {
                return laser;
            }
        }

        var newLaser = new Laser(
            new Rectangle(0, 0, (int)Width, (int)Height),
            new Vector2(this.laserTexture.Width, this.laserTexture.Height));
        this.lasers.Add(newLaser);

        return newLaser;
    }

    private Laser SetupLaser(Laser laser)
    {
        laser.IsVisible = true;

        const int laserVerticalOffset = 15;
        var shipHalfHeight = this.shipTexture.Height / 2f;
        var shipTop = this.shipPos.Y - shipHalfHeight;
        var laserHalfHeight = this.laserTexture.Height / 2f;
        var laserPosY = shipTop - laserHalfHeight + laserVerticalOffset;

        laser.Position = new Vector2(this.shipPos.X, laserPosY);

        return laser;
    }

    private void UpdateKeyStates()
    {
        this.currentKeyState = this.keyboard.GetState();

        if (this.prevKeyState.IsKeyDown(KeyCode.Space) && this.currentKeyState.IsKeyUp(KeyCode.Space))
        {
            this.fireLazerKeyPressed = true;
        }

        this.leftKeyDown = this.currentKeyState.IsKeyDown(KeyCode.Left);
        this.rightKeyDown = this.currentKeyState.IsKeyDown(KeyCode.Right);
        this.upKeyDown = this.currentKeyState.IsKeyDown(KeyCode.Up);
        this.downKeyDown = this.currentKeyState.IsKeyDown(KeyCode.Down);
        this.isNotMovingHorizontally = this.currentKeyState.IsKeyUp(KeyCode.Right) && this.currentKeyState.IsKeyUp(KeyCode.Left);
        this.isNotMovingVertically = this.currentKeyState.IsKeyUp(KeyCode.Up) && this.currentKeyState.IsKeyUp(KeyCode.Down);

        this.prevKeyState = this.currentKeyState;
    }

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
        this.velocity = Clamp(this.velocity, -MaxVel, MaxVel);

        // Calculate the movement distance for this frame
        var displacement = this.velocity * (float)frameTime.ElapsedTime.TotalSeconds;

        // Apply the movement distance to the ship's position
        this.shipPos += displacement;
    }

    private static Vector2 Clamp(Vector2 value, float min, float max)
    {
        value.X = value.X < min ? min : value.X;
        value.X = value.X > max ? max : value.X;

        value.Y = value.Y < min ? min : value.Y;
        value.Y = value.Y > max ? max : value.Y;

        return value;
    }
}
