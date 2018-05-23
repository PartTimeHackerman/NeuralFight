public interface IConnectionClient
{
    void send(string message);
    void receive();
    void destroy();
}