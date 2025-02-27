public interface IMessageHandler
{
    void OnReceiveMessage(Message message);

    void OnConnectionFail(bool isMain);

    void OnDisconnected(bool isMain);

    void OnConnectOK(bool isMain);

}
