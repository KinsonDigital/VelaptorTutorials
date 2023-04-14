namespace KeyboardInput;

using System.Drawing;
using System.Numerics;
using Velaptor;

public class Laser
{
    private const float VelocityY = -400;
    private readonly Rectangle worldBounds;
    private readonly Vector2 laserSize;

    public Laser(Rectangle worldBounds, Vector2 laserSize)
    {
        this.worldBounds = worldBounds;
        this.laserSize = laserSize;
    }

    public Vector2 Position { get; set; }

    public bool IsVisible { get; set; }

    public void Update(FrameTime frameTime)
    {
        if (!this.IsVisible)
        {
            return;
        }

        var velocity = new Vector2(0, VelocityY);
        var displacement = velocity * (float)frameTime.ElapsedTime.TotalSeconds;

        this.Position += displacement;

        var laserBounds = new Rectangle(
            (int)this.Position.X,
            (int)this.Position.Y,
            (int)this.laserSize.X,
            (int)this.laserSize.Y);

        if (!this.worldBounds.Contains(laserBounds))
        {
            this.IsVisible = false;
        }
    }
}
