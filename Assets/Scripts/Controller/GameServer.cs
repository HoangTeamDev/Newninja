using UnityEngine;

public partial class GameServer : MonoBehaviour, IMessageHandler
{
    protected static GameServer Instance;
    public Session session;
    public void DoUpdate()
    {
        session?.DoUpdate();
       
    }
    public static GameServer gI()
    {
        if (Instance == null)
        {
            GameObject obj = new GameObject("GameServer");
            Instance = obj.AddComponent<GameServer>();
            DontDestroyOnLoad(obj);
        }
        return Instance;
    }
    public GameServer()
    {
        session = Session.gI();
    }    
 

    public void Init()
    {
        Session.gI().SetHandler(GameServer.gI());
        Session.gI().Connect(GameMidlet.IP, GameMidlet.PORT);
    }
    #region Interface
    public void OnReceiveMessage(Message message)
    {
       HandleMessage(message);
    }
    public void OnConnectionFail(bool isMain)
    {
        Session.gI().Status = 1;
    }

    public void OnDisconnected(bool isMain)
    {
        Session.gI().Status = 0;
    }

    public void OnConnectOK(bool isMain)
    {
        Session.gI().Status = 2;
    }

    #endregion
    public void Shutdown()
    {
        session?.Close();
    }
   
}
