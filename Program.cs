using System;
using System.Drawing;
using System.Drawing.Drawing2D;

using Pamella;

App.Open<joguinhoTrevi>();

public class Enemy
{
    public State State { get; set; }

    public float X { get; set; }

    public float Y { get; set; }

    public float Angle { get; set; }

    public void Act()
    {
        this.State.Act();
    }

}

public abstract class State
{
    
    public Enemy enemy;
    public State NextState { get; set; }
    public abstract void Act();

}

public class MovingState : State
{
    public float XTarget { get; set; }
    public float YTarget { get; set; }

    public override void Act()
    {
        var dx = XTarget - enemy.X;
        var dy = YTarget - enemy.Y;

        var mod = MathF.Sqrt(dx * dx + dy * dy);
        if(mod < 5)
        {
            this.enemy.State = NextState;
            return;
        }
            
        enemy.X += 50 * dx / mod / 20;
        enemy.Y += 50 * dy / mod / 20;
    }
}
public class RotateState : State
{
    public float AngleTarget { get; set; }

    public override void Act()
    {
        var dTheta = AngleTarget - enemy.Angle;

        if(MathF.Abs(dTheta) < 0.05)
        {
            this.enemy.State = NextState;
            return;
        }
            
        enemy.Angle += 0.1f * MathF.Sign(dTheta);
    }
}

public class joguinhoTrevi : View
{
    Enemy enemy1;
    protected override void OnStart(IGraphics g)
    {
        enemy1 = new Enemy();
        enemy1.X = 400;
        enemy1.Y = 200;

        var builder = new StateBuilder();

        builder
            .SetEnemy(enemy1)
            .AddMovingState(400, 200)
            .AddRotateState(MathF.PI / 2)
            .AddMovingState(400, 800)
            .AddRotateState(0)
            .AddMovingState(1600, 800)
            .AddRotateState( 3 * MathF.PI / 2)
            .AddMovingState(1600, 200)
            .AddRotateState( MathF.PI)
            .Build();

        g.SubscribeKeyDownEvent(key =>
        {
            if(key == Input.Escape)
                App.Close();
        });

        AlwaysInvalidateMode();
    }

    protected override void OnFrame(IGraphics g)
    {
        enemy1.Act();
    }

    protected override void OnRender(IGraphics g)
    {
        
        g.Clear(Color.DarkGreen);

        var cos = MathF.Cos(enemy1.Angle);
        var sin = MathF.Sin(enemy1.Angle);
        
        PointF[] points = new PointF[] {
            new PointF(enemy1.X , enemy1.Y ),
            new PointF(
                enemy1.X + 250 * cos - 50 * sin,
                enemy1.Y + 250 * sin + 50 * cos
            ),
            new PointF(
                enemy1.X + 250 * cos + 50 * sin,
                enemy1.Y + 250 * sin - 50 * cos
                )
        };

        Color color = Color.FromArgb(255, 255, 0);

        LinearGradientBrush brush = new LinearGradientBrush(
            points[0], points[1], Color.FromArgb(200, color),
            Color.FromArgb(0, color)
        );

        g.FillPolygon ( points, brush );

        g.FillRectangle (
            enemy1.X - 15, enemy1.Y - 15,
            30, 30, Brushes.Red
        );
    }
}