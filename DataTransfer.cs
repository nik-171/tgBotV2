public class DataTransfer
{
    Message Message { get; set; }
    public State State { get; set; }
    public object Object { get; set; }

    public DataTransfer(State state, object obj, Message message)
    {
        State = state;
        Object = obj;
        Message = message;
    }
}
