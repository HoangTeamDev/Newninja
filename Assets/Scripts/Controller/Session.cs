using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
public class Session : ISession
{
    public static int count;
    [Header("Status")]
    private string host;
    private int port;
    private IMessageHandler messageHandler;
    private sbyte[] key = null;
    private sbyte curR;
    private sbyte curW;
    protected static Session instance = new Session();
    private readonly ConcurrentQueue<Message> SendingMessage = new ConcurrentQueue<Message>();
    [Header("Connect")]
    private TcpClient client;
    private NetworkStream networkStream;
    private BinaryReader reader;
    private BinaryWriter writer;
    public bool getKeyComplete = false;
    public static bool connected;
    public static bool connecting;
    public int Status;
    public static Thread initThread;
    public static Thread readerThread;
    public static Thread senderThread;
    public static Session gI()
    {
        if (instance == null)
        {
            instance = new Session();
        }
        return instance;
    }
    public void SetHandler(IMessageHandler msgHandler)
    {
        messageHandler = msgHandler;
    }
    public void Connect(string host, int port)
    {
        Debug.Log("okela:" + host);
        if (!connected && !connecting)
        {
            this.host = host;
            this.port = port;
            Close();
            getKeyComplete = false;
            initThread = new Thread(NetworkInit) { IsBackground = true }; ;
            initThread.Start();
        }
    }
    private void NetworkInit()
    {
        connecting = true;
        Thread.CurrentThread.Priority = System.Threading.ThreadPriority.Highest;
        connected = true;
        DoConnect(host, port);
    }

    private void DoConnect(string host, int port)
    {
        try
        {
            client = new TcpClient();
            client.Connect(host, port);
            if (client.Connected)
            {
                networkStream = client.GetStream();
                reader = new BinaryReader(networkStream, new UTF8Encoding());
                writer = new BinaryWriter(networkStream, new UTF8Encoding());
                readerThread = new Thread(RunReader) { IsBackground = true }; ;
                readerThread.Start();
                senderThread = new Thread(RunSender) { IsBackground = true }; ;
                senderThread.Start();
                connecting = false;
                Send(new Message(-27258));           
                Status = 2;
                count = 0;
                connected = true;
                messageHandler.OnConnectOK(true);
            }
            else
            {
                Close();
            }
        }
        catch
        {
            messageHandler.OnDisconnected(true);
            Close();
        }

    }

    public bool IsConnected()
    {
        return connected && client.Connected;
    }

    public void SendMessage(Message message)
    {
        SendingMessage.Enqueue(message);
    }
    private void Send(Message m)
    {      
        if (m == null  || !client.Connected) return;
        if ( m.command != -91)
        try
        {
            if (getKeyComplete)
            {
                short value = WriteKey(m.command);
                writer.Write(value);
                   
                }
            else
            {
                writer.Write(m.command);
            }
            sbyte[] data = m.getData();
            if (data != null)
            {
                int num = data.Length;
                if (getKeyComplete)
                {
                    int num2 = WriteKey((sbyte)(num >> 8));
                    writer.Write((sbyte)num2);
                    int num3 = WriteKey((sbyte)(num & 0xFF));
                    writer.Write((sbyte)num3);
                }
                else
                {
                    writer.Write((ushort)num);
                }
                if (getKeyComplete)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        sbyte value2 = WriteKey(data[i]);
                        writer.Write(value2);
                    }
                }
            }
            else
            {
                if (getKeyComplete)
                {
                    int num4 = 0;
                    int num5 = WriteKey((sbyte)(num4 >> 8));
                    writer.Write((sbyte)num5);
                    int num6 = WriteKey((sbyte)(num4 & 0xFF));
                    writer.Write((sbyte)num6);
                }
                else
                {
                    writer.Write((ushort)0);
                }
            }
            writer.Flush();
               
            }
        catch
        {
            writer.Flush();
        }
    }
    public sbyte WriteKey(short b)
    {
        sbyte[] array = key;
        sbyte num = curW;
        curW = (sbyte)(num + 1);
        sbyte result = (sbyte)((array[num] & 0xFF) ^ (b & 0xFF));
        if (curW >= key.Length)
        {
            curW %= (sbyte)key.Length;
        }
        return result;
    }
    public void RunReader()
    {
        Debug.Log("messVe:" + connected);
        try
        {
            while (connected)
            {
                Message message = ReadMessage();
               
                if (message == null) continue;
                try
                {
                    if (message.command == -27)
                    {
                        GetKey(message);
                    }
                    else
                    {
                        onRecieveMsg(message);
                    }
                }
                catch (Exception ex)
                {
                    
                }
                Thread.Sleep(2);
            }
            Status = 0;
            CleanNetwork();
        }
        catch
        {
            /*MainThreadDispatcher.Enqueue(() =>
            {
                GameController.Instance.ActiveMatKetNoi();

            });*/
            Status = 0;
            Close();
        }
    }
    public void RunSender()
    {
        while (connected)
        {
            try
            {
                if (getKeyComplete)
                {
                    while (SendingMessage.Count > 0)
                    {
                        Message message;
                        if (SendingMessage.TryDequeue(out message))
                        {
                            Send(message);
                        }
                    }
                }
                try
                {
                    Thread.Sleep(5);
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
        }
    }
    public void onRecieveMsg(Message message)
    {
        if (message == null) return;
        switch (message.command)
        {
            case -7:
                {
                    messageHandler.OnReceiveMessage(message);
                    break;
                }
            default:
                {
                   
                    MainThreadDispatcher.Enqueue(() =>
                    {
                        messageHandler.OnReceiveMessage(message);
                    });
                    break;
                }
        }
    }
    public void DoUpdate()
    {
        if (IsConnected()) count = 0;
        else
        {
            count++;
            if (count is 1)
            {
                //GameController.Instance.ActiveMatKetNoi();
                Close();
            }
            if (Status is 0 or 4 && count > 1000)
            {
                //GameServer.gI().Init();
                count = 2;
            }
        }
    }
    private void GetKey(Message message)
    {
        try
        {
            sbyte b = message.reader().readSByte();
            key = new sbyte[b];
            for (int i = 0; i < b; i++)
            {
                key[i] = message.reader().readSByte();
            }
            for (int j = 0; j < key.Length - 1; j++)
            {
                ref sbyte reference = ref key[j + 1];
                reference ^= key[j];
            }
            getKeyComplete = true;
        }
        catch (Exception)
        {

        }
    }

    private Message ReadMessage()
    {
        try
        {
            short b = reader.ReadInt16();
            if (getKeyComplete)
            {
                b = readKey(b);
            }
            int num;
            if (getKeyComplete)
            {
                num = ((readKey(reader.ReadSByte()) & 0xFF) << 8) | (readKey(reader.ReadSByte()) & 0xFF);
            }
            else
            {
                num = ((reader.ReadSByte() & 0xFF) << 8) | (reader.ReadSByte() & 0xFF);
            }
            if (b == -98)
            {
                num = 320000;
            }
            sbyte[] array = new sbyte[num];
            byte[] src = reader.ReadBytes(num);
            Buffer.BlockCopy(src, 0, array, 0, num);
            if (getKeyComplete)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = readKey(array[i]);
                }
            }
            return new Message(b, array);
        }
        catch (Exception ex)
        {
            Status = 0;
            Close();
           
        }
        return null;
    }
    public sbyte readKey(short b)
    {
        sbyte[] array = key;
        sbyte num = curR;
        curR = (sbyte)(num + 1);
        sbyte result = (sbyte)((array[num] & 0xFF) ^ (b & 0xFF));
        if (curR >= key.Length)
        {
            curR %= (sbyte)key.Length;
        }
        return result;
    }
    public void Close()
    {
        CleanNetwork();
    }

    public void CleanNetwork()
    {
        key = null;
        curR = 0;
        curW = 0;
        try
        {
            connected = false;
            connecting = false;
            if (client != null)
            {
                client.Close();
                client = null;
            }
            if (networkStream != null)
            {
                networkStream.Close();
                networkStream = null;
            }
            if (writer != null)
            {
                writer.Close();
                writer = null;
            }
            if (reader != null)
            {
                reader.Close();
                reader = null;
            }
            if (initThread != null)
            {
                initThread.Abort();
            }
            initThread = null;
            if (initThread != null)
            {
                initThread.Abort();
            }
            initThread = null;
            if (readerThread != null)
            {
                readerThread.Abort();
            }
            readerThread = null;
            messageHandler.OnDisconnected(true);
        }
        catch (Exception)
        {
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    protected virtual void Dispose(bool readerposing)
    {
        Close();
    }
}