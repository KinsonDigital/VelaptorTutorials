// <copyright file="Events.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace SpaceShooter;

/// <summary>
/// Holds event IDs for the game.
/// </summary>
public static class Events
{
    /// <summary>
    /// Gets an ID used to send notifications that the ship's position has been updated to the weapon system.
    /// The weapons uses this information to update the position of the bullets so that they
    /// start out in front of the ship when fired.
    /// </summary>
    public static Guid UpdatePosition { get; } = Guid.NewGuid();
}
