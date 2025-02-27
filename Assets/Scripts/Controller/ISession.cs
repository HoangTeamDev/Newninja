using System;
using UnityEngine;

public interface ISession : IDisposable
{
    bool IsConnected();

    void SetHandler(IMessageHandler messageHandler);

    void Connect(string host, int port);

    void SendMessage(Message message);

    void Close();
    void DoUpdate();
    void CleanNetwork();
}
