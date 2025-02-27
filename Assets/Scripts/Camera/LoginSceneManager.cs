

using DG.Tweening;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoginSceneManager : Singleton<LoginSceneManager>
{
    [Header("Login")]
    private Tween cameraMapMove;// tween di chuyển camera khi login
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject LoginMain;
    [SerializeField] private GameObject ChangeAccCount;//màn hình thay đổi tài khoản
    [SerializeField] private GameObject CreateCharacter;//màn hình tạo nhân vật
    [SerializeField] public GameObject SelectSever;//UI chọn nhân vật
    [Header("AnimatorCharacter")]
    [SerializeField] private RuntimeAnimatorController[] _animatorControllerTD;//các animation của nhân vật trong mà hình
    [SerializeField] private RuntimeAnimatorController[] _animatorControllerNM;// dùng ddeeerr change các ani khi chọn các nhân vật khác nhau
    [SerializeField] private RuntimeAnimatorController[] _animatorControllerXD;
    [SerializeField] private RuntimeAnimatorController[] _animatorControllerDM;
    [SerializeField] private Animator _animator;// nằm trong gameobject Player: Background/Player
    [Header("BackGround")]
    [SerializeField] private GameObject backgroundMain;// gameobject  chính chứa background chọn nhân vật
    [SerializeField] private GameObject[] background;// list gameobject background
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI txt_Status;
    [SerializeField] private TextMeshProUGUI txt_SerVer;
    [SerializeField] private TextMeshProUGUI txt_StatusConnect;
    [SerializeField] private TextMeshProUGUI txt_UserContinue;
    [SerializeField] private TextMeshProUGUI nameCharacterSelect; //name nhân vật trong mà hình chọn nhân vật
    [Header("Button")]
    [SerializeField] private Image sever_Status;
    [SerializeField] private Button btn_KeepContinune, btn_ChangeAccount, btn_Server;
    [SerializeField] private Button btn_Back;
    [SerializeField] private Button btn_Login;
    [SerializeField] private List<Button> lstBtnPlance;// list chọn hành tinh
    [SerializeField] private Button btn_Create;
    [SerializeField] private List<Button> btn_ChooseServer;
    [Header("Input")]
    public TMP_InputField txt_User;
    public TMP_InputField txt_Password;
    public TMP_InputField currentInputField;// ô nhập hiện tại đc chọn
    public TMP_InputField[] inputla; // name nhân vật trong ô nhập

    [Header("Image")]
    [SerializeField] private List<Sprite> lstImgOff;// list sprite select hành tinh
    [SerializeField] private List<Sprite> lstImgOn;
    [Header("Color")]
    private readonly Color defaultColor = Color.white; 
    private readonly Color highlightColor = new(0.6549f, 0.9490f, 0.6745f, 1);// color khi nhấn vô ô nhập
    
    [Header("Map")]
    [SerializeField] private GameObject[] _movingBg; //list map login
    public List<Vector2> left = new List<Vector2>();
    public List<Vector2> right = new List<Vector2>();
    public GameObject mapcurrent;
    private Tween map;
    public GameObject pannel;
    public TextMeshProUGUI texttp;
    public static List<string> ListServer = new List<string> { "Vũ trụ 1:103.249.158.70:14445", "Vũ trụ 2:192.168.123.91:14445", "Vũ trụ 3:192.168.123.186:14445" };

    protected override void Awake()
    {
       /* if (!string.IsNullOrEmpty(PlayerPrefs.GetString("Host")))
        {
            txt_SerVer.text = "Máy Chủ: " + PlayerPrefs.GetString("NameServer");
            GameMidlet.IP = PlayerPrefs.GetString("Host");
            GameMidlet.PORT = PlayerPrefs.GetInt("Port");
        }
        else
        {
            ActiveServer(2);
        }*/
        _mainCamera = Camera.main;
        btn_KeepContinune.onClick.AddListener(StartGame);// gán các sự kiện khi nhấn các nút   
        btn_ChangeAccount.onClick.AddListener(ShowLogin);
        btn_Back.onClick.AddListener(BackToSelection);
        btn_Login.onClick.AddListener(Login);
        btn_Server.onClick.AddListener(ChooseSerVer);
        txt_User.onSelect.AddListener(delegate { ChangeInputFieldColor(txt_User, highlightColor); });// gán sự kiện khi nhấn vào nút nhập text
        txt_User.onDeselect.AddListener(delegate { ChangeInputFieldColor(txt_User, defaultColor); });
        txt_Password.onSelect.AddListener(delegate { ChangeInputFieldColor(txt_Password, highlightColor); });
        txt_Password.onDeselect.AddListener(delegate { ChangeInputFieldColor(txt_Password, defaultColor); });
        currentInputField = txt_User;
        txt_Status.text = "Version:" + GameMidlet.VERSION;
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("TK")))
        {
            txt_UserContinue.text = "Chơi Tiếp: " + PlayerPrefs.GetString("TK");
        }
        btn_Create.onClick.AddListener(() =>// gán sự kiện tạo nhân vật
        {         
            if (string.IsNullOrEmpty(inputla[0].text))
            {
                ActiveThongBao("Bạn cần nhập tên nhân vật!");
                return;
            }
            if (inputla[0].text.Length > 10 || inputla[0].text.Length < 5)
            {
                ActiveThongBao("Tên nhân vật từ 5 đến 10 ký tự!");
                return;
            }
            else
            {
                GameServer.gI().CreateChar(inputla[0].text, int.Parse(inputla[1].text), int.Parse(inputla[2].text));

            }
        });

    }
    private void FixedUpdate()
    {
       
    }
    private void Update()
    {
        txt_User.text = SanitizeInput(txt_User.text);
        txt_Password.text = SanitizeInput(txt_Password.text);
        ShowStatusGameSever();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchInputField();
        }


    }
    public void ActiveLogin()
    {
        //GameController.Instance.loading.SetActive(true);
        LoginMain.SetActive(true);
        CreateCharacter.SetActive(false);
        ChangeAccCount.SetActive(false);
        int x = UnityEngine.Random.Range(0, _movingBg.Length);
       // _mainCamera.transform.position=new Vector3(right[x].x,0,-10);
        cameraMapMove.Kill();
        GameObject newMap = Instantiate(_movingBg[x], new Vector2(0, 0), Quaternion.identity);
        cameraMapMove = _mainCamera.transform.DOMoveX(left[x].x, 15f, false).SetEase(Ease.Linear).OnComplete(() => { cameraMapMove.Kill(); MoveBacground(x); });
        if (mapcurrent != null) { Destroy(mapcurrent); }
        mapcurrent = newMap;
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("TK")))
        {
            txt_UserContinue.text = "Chơi Tiếp: " + PlayerPrefs.GetString("TK");
            btn_KeepContinune.gameObject.SetActive(true);
        }
        else
        {
            btn_KeepContinune.gameObject.SetActive(false);
        }
    }
    void MoveBacground(int x)
    {
        float left1 = Mathf.Abs(_mainCamera.transform.position.x - left[x].x);
        float right1 = Mathf.Abs(_mainCamera.transform.position.x - right[x].x);
        if (left1 < 2)
        {
            cameraMapMove= _mainCamera.transform.DOMoveX(right[x].x, 15f, false).SetEase(Ease.Linear).OnComplete(() =>
            {
                cameraMapMove.Kill();
                MoveBacground(x);
            });
            return;
        }
        if (right1 < 2)
        {
            cameraMapMove=_mainCamera.transform.DOMoveX(left[x].x, 15f, false).SetEase(Ease.Linear).OnComplete(() =>
            {
                cameraMapMove.Kill();
                MoveBacground(x);
            });
            return;
        }
       
    }
    void ChangeBackground(int id)
    {
        for (int i = 0; i < background.Length; i++)
        {
            if (i == id) { background[i].SetActive(true); } else { background[i].SetActive(false); }
        }
    }
    public void ActiveThongBao(string text)
    {
        texttp.text = text;
        pannel.SetActive(true);
    }
    public void SetServer(int i)
    {
        SelectSever.SetActive(false);
        GameServer.gI().Shutdown();
        ActiveServer(i);
    }
    public void ActiveServer(int i)
    {
        string[] array = ListServer[i].Split(':');
        txt_SerVer.text = "Máy Chủ: " + array[0];
        GameMidlet.IP = array[1];
        GameMidlet.PORT = int.Parse(array[2]);
        PlayerPrefs.SetString("NameServer", array[0]);
        PlayerPrefs.SetString("Host", GameMidlet.IP);
        PlayerPrefs.SetInt("Port", GameMidlet.PORT);
    }    
    public void ChooseSerVer()
    {
        SelectSever.SetActive(true);
        foreach(var data in btn_ChooseServer)
        {
            data.gameObject.SetActive(false);
        }
        for (int i = 0; i < ListServer.Count; i++)
        {
            btn_ChooseServer[i].gameObject.SetActive(true);
        }
    }
    // đổi màu ô nhập text khi nhấn 
    private void ChangeInputFieldColor(TMP_InputField inputField, Color color)
    {
        var colors = inputField.colors;
        colors.normalColor = color;
        colors.selectedColor = color;
        inputField.colors = colors;
    }


    // chuyển đổi giữa 2 ô nhập text
    public void SwitchInputField()
    {
        if (currentInputField == txt_User)
        {
            currentInputField = txt_Password;
            txt_Password.Select();
        }
        else
        {
            currentInputField = txt_User;
            txt_User.Select();
        }
    }
    // hàm được gọi khi login thành công
    public void LoginSuccess()
    {
       // GameController.Instance.loading.SetActive(true);
        LoginMain.SetActive(false);      
        CreateCharacter.SetActive(false);
        cameraMapMove.Kill();
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        cameraFollow.enabled = true;      
        Destroy(mapcurrent);

    }




    private void ShowStatusGameSever()
    {
        DateTime utcNow = DateTime.UtcNow;
        DateTime timeZone7 = utcNow.AddHours(7);
        if (Session.gI().Status is 0 or 4)
        {

          
            sever_Status.color = Color.red;
        }
        else if (Session.gI().Status == 1)
        {
           
            sever_Status.color = Color.yellow;

        }
        else if (Session.gI().Status == 2)
        {
          
            sever_Status.color = Color.green;
        }
        else
        {
          
            sever_Status.color = Color.grey;

        }
    }
    //hàm active giao diện tạo nhân vật
    public void ActiveCreateCharacter()
    {
        //cameraMapMove.Kill();
        _mainCamera.transform.position=new Vector3(0,0,-10);
        CreateCharacter.SetActive(true);
        LoginMain.SetActive(false);
        ChangeAccCount.SetActive(false);

    }
    private void ShowLogin()
    {
        LoginMain.SetActive(false);
        ChangeAccCount.SetActive(true);
        if (currentInputField != null)
        {
            currentInputField.Select();
            EventSystem.current.SetSelectedGameObject(currentInputField.gameObject, null);
        }      
    }


    public void BackToSelection()
    {

        txt_User.text = null;
        txt_Password.text = null;
    }

    private void Login()
    {
        if (txt_User.text.Length > 10 && txt_User.text.Length < 5 || txt_Password.text.Length > 10 && txt_Password.text.Length < 5)
        {
            ActiveThongBao("Tài khoản hoặc mật khẩu chỉ được từ 5-10 ký tự.");
            txt_User.text = null;
            txt_Password.text = null;
            return;
        }
        LoginMain.SetActive(true);
        ChangeAccCount.SetActive(false);
        PlayerPrefs.SetString("TK", txt_User.text);
        PlayerPrefs.SetString("MK", txt_Password.text);
        txt_UserContinue.text = "Chơi Tiếp: " + PlayerPrefs.GetString("TK");
        btn_KeepContinune.gameObject.SetActive(true);

    }
    //PlayerPrefs.GetString("TK") dùng luu tài khoản+ mật khẩu trong máy
    private void StartGame()
    {
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("TK")))
        {
            GameServer.gI().Login(PlayerPrefs.GetString("TK"), PlayerPrefs.GetString("MK"), GameMidlet.VERSION, (sbyte)0);
            GameServer.gI().clientOk();
        }
        else
        {
            ShowLogin();
        }


    }
    private string SanitizeInput(string input)
    {
        char[] sanitized = input.ToLower().ToCharArray();
        sanitized = Array.FindAll(sanitized, c => char.IsLetterOrDigit(c) || c == '.' || c == '@');
        return new string(sanitized);
    }

    //CreateCharacter
   

}
