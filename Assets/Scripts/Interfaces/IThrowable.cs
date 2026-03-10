public interface IThrowable
{
    public enum State
    {
        Idle,
        Thrown
    }

    public void SetState(State state);
    public State GetState();
}
