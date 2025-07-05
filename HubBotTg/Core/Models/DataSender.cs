public class DataSender
{
    public State State { get; set; }
    public object Object { get; set; }

    public DataSender(State state, object obj)
    {
        State = state;
        Object = obj;
    }
}
