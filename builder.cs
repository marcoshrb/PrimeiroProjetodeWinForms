public class StateBuilder
{
    private Enemy enemy = null;
    private State initial = null;
    private State last = null;

    public StateBuilder SetEnemy(Enemy enemy){
        this.enemy = enemy;
        return this;
    }

    public StateBuilder AddState(State state)
    {
        state.enemy = enemy;

        if(last is not null)
            last.NextState = state;

        if(initial is null)
            initial = state;
        
        last = state;
        return this;
    }

    public State Build()
    {
        last.NextState = initial;
        this.enemy.State = initial;
        return initial;
    }
}

public static class StateBuilderExtenxions
{
    public static StateBuilder AddMovingState(
        this StateBuilder builder,
        float targetX, float targetY
    )
    {
        builder.AddState(new MovingState
        {
            XTarget = targetX,
            YTarget = targetY
        });
        return builder;
    }

    public static StateBuilder AddRotateState(
        this StateBuilder builder,
        float angle
    )
    {
        builder.AddState(new RotateState
        {
            AngleTarget = angle
        });
        return builder;
    }
}