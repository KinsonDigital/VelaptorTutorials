// <copyright file="WorldSignal.cs" company="KinsonDigital">
// Copyright (c) KinsonDigital. All rights reserved.
// </copyright>

namespace SpaceShooter.Signals;

using Carbonate.OneWay;
using Data;

public interface IWorldSignal : IPushReactable<WorldData>
{
}

public class WorldSignal : PushReactable<WorldData>, IWorldSignal
{
}
