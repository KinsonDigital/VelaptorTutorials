// <copyright file="Game.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

// ReSharper disable RedundantOverriddenMember
// ReSharper disable ArrangeMethodOrOperatorBody
// ReSharper disable ArrangeConstructorOrDestructorBody
namespace EmptyProject;

using Velaptor;
using Velaptor.UI;

/// <summary>
/// The main game class.
/// </summary>
public class Game : Window
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Game"/> class.
    /// </summary>
    public Game()
    {
        Title = "Empty Project";
        // TODO: Add other non content creation code here
    }

    /// <summary>
    /// Loads game content.
    /// </summary>
    protected override void OnLoad()
    {
        // TODO: Load game content here
    }

    /// <summary>
    /// Updates the application. Executes one time for every iteration of the game loop
    /// and always BEFORE the <see cref="OnDraw"/> method.
    /// </summary>
    /// <param name="frameTime">The amount of time that has passed for the current frame.</param>
    protected override void OnUpdate(FrameTime frameTime)
    {
        // TODO: Update game state here
        base.OnUpdate(frameTime);
    }

    /// <summary>
    /// Invoked when the window is resized.  This includes minimizing, maximizing, and restoring the window.
    /// </summary>
    /// <param name="size">The new size the window.</param>
    protected override void OnResize(SizeU size)
    {
        // TODO: React to the window resizing here
        base.OnResize(size);
    }

    /// <summary>
    /// Draws to the screen. Executes one time for every iteration of the game loop
    /// and always AFTER the <see cref="OnUpdate"/> method has finished.
    /// </summary>
    /// <param name="frameTime">The amount of time that has passed for the current frame.</param>
    protected override void OnDraw(FrameTime frameTime)
    {
        // TODO: Render game content here
        base.OnDraw(frameTime);
    }

    /// <summary>
    /// Unload game content. Invoked when the window is closed.
    /// </summary>
    protected override void OnUnload()
    {
        // TODO: Unload game content here
        base.OnUnload();
    }
}
