public class UserStateData
{
    public SendableMessageDesign Message {get; set; }
    public State State { get; set; }
    public object? Object { get; set; }

    public UserStateData(State state, object? obj, SendableMessageDesign message)
    {
        State = state;
        Object? = obj;
        Message = message;
    }
}
