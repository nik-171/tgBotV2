public class UserStateData
{
    public Message Message {get; set; }
    public State State { get; set; }
    public object Object { get; set; }

    public UserStateData(State state, object obj, Message message)
    {
        State = state;
        Object = obj;
        Message = message;
    }
}
