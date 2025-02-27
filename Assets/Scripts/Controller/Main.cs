using System.Net;
using System.Threading;
using UnityEngine;
public class Main : Singleton<Main>
{
    [SerializeField] private GameObject _Controller;
    public static Main main;
    public static bool started;
    public static string mainThreadName;
    private int count;
    private bool isRun;
    protected override void Awake()
    {
        if (started) return;
        if (Thread.CurrentThread.Name != "Main")
        {
            Thread.CurrentThread.Name = "Main";
        }
        mainThreadName = Thread.CurrentThread.Name;
        started = true;
        base.enabled = true;
        base.Awake();
        main = this;
        DontDestroyOnLoad(this);
        Application.runInBackground = true;
        Input.multiTouchEnabled = false;

    }
    public void Log(object text)
    {
#if UNITY_EDITOR
        Debug.Log(text);
#endif
    }
    public void LogError(object text)
    {
#if UNITY_EDITOR
        Debug.LogError(text);
#endif
    }
    private void Start()
    {
        StartGame();
    }
    public bool IsIphone()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            return true;
        }
        return false;
    }
    private void StartGame()
    {              
        Instantiate(_Controller);
        Session.gI().SetHandler(GameServer.gI());
        Session.gI().Connect("192.168.123.186", 14446);
       
    }
    public void setsizeChange()
    {
        if (!isRun)
        {
            Screen.orientation = ScreenOrientation.AutoRotation;
            Application.runInBackground = true;
            Application.targetFrameRate = 60;
            base.useGUILayout = false;
            if (main == null)
            {
                main = this;
            }
            isRun = true;
            Screen.fullScreen = false;

        }
    }
    private void FixedUpdate()
    {
        //GameController.Instance.DoFixedUpdate();
    }
    private void Update()
    {
        count++;
        if (count >= 10)
        {
            GameServer.gI().DoUpdate();
            GameController.Instance.DoUpdate();
            InputController.Instance.DoUpdate();
            setsizeChange();
        }
    }
    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    public static string connectHTTP(string link)
    {
        using (WebClient webClient = new WebClient())
        {
            return webClient.DownloadString(link);
        }
    }
    private void OnApplicationQuit()
    {
        GameServer.gI().Shutdown();
        if (!IsIphone())
        {
            Application.Quit();
        }
    }
}