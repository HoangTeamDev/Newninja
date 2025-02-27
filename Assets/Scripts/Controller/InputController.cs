using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class InputController : Singleton<InputController>
{
    [Header("Input")]
    public Joystick Joystick;
  
    public bool _canPress;
    public float _firstLeftClickTime;
    public bool _isDoubleClick;
    public bool _isAuto;
    public bool _isAttack;
    public Vector2 _clickPosition;
    public LayerMask _groundLayer;
    private const float _dragSpeed = 5.0f;
    private Vector3 _dragOrigin;
    [SerializeField] private int _key;
    public Character _character;
    public GameObject _paneTarget;
    public bool _valueDrag;
    public float _ve = 0;
    public float _ho = 0;
    //Get,set input
   
    private float _doublePressTime = 0.5f;
    private float _lastPressTime = -1f;
    public bool _isDoubleEnter;

    private void Start()
    {
        _isDoubleEnter = false;
        _valueDrag = false;
        _canPress = true;
        _isAuto = false;
      
    }
    public void DoUpdate()
    {

        GetHorizontal();
        GetVertical();
        if (_canPress)
        {
            CheckInput();
            //// Kiểm tra nếu chạm vào UI trên thiết bị di động
            //if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
            //    return;
            //}
            //if (EventSystem.current.IsPointerOverGameObject()) return;



            if (Main.main.IsIphone())
            {
                HandleTouchInput();
            }
            else
            {
                HandleMouseInput();
            }
            CheckInputKeyBoard();

            if (Input.GetKeyDown(KeyCode.Return))
            {
                KeyReturnHandle();
            }
        }


    }

    public void KeyReturnHandle()
    {
        if (Time.time - _lastPressTime <= _doublePressTime)
        {
            _isDoubleEnter = true;
            _lastPressTime = -1f;
        }
        else
        {
            _isDoubleEnter = false;
            _lastPressTime = Time.time;
        }
        if (TargetController.Instance._targetNow != null)
        {
            _clickPosition = TargetController.Instance._targetNow.transform.position;
        }
        else
        {
            if (TargetController.Instance._targetMe != null)
            {
                TargetController.Instance.TarGetNow(TargetController.Instance._targetMe);
                TargetController.Instance._targetMe = null;
                _clickPosition = TargetController.Instance._targetNow.transform.position;
            }



        }
        if (TargetController.Instance._targetNow != null)
        {
            _isAuto = true;
            _isDoubleClick = true;
            _isAttack = true;
        }

    }

    public void CheckInput()
    {
        


    }

    public float GetHorizontal()
    {
        return _ho= Input.GetAxisRaw("Horizontal");
    }
    public float GetVertical()
    {
        return _ve= Input.GetAxisRaw("Vertical");
    }
    bool CheckMP()
    {

        return GameData.Instance._DataMain._mp >= (GameData.Instance._DataMain._mpFull * 1 / 100);
    }
    public int CheckInputKeyBoard()
    {
        if (Input.anyKeyDown)
        {

            string buttonPressed = Input.inputString;


            if (!string.IsNullOrEmpty(buttonPressed) && "123456789".Contains(buttonPressed))
            {
                _key = int.Parse(buttonPressed);

                return int.Parse(buttonPressed);
            }
        }


        return 0;
    }

   
    private void OnEnable()
    {
        //OnSingleClick += SingleClickHandle;
        //OnDoubleClick += DoubleClickHandle;
    }

    private void OnDisable()
    {
        //OnSingleClick -= SingleClickHandle;
        //OnDoubleClick -= DoubleClickHandle;
    }

    void SingleClickHandle()
    {
    }

    void DoubleClickHandle()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 clickPosition1 = new Vector2(worldPosition.x, worldPosition.y);
        RaycastHit2D hit = Physics2D.Raycast(clickPosition1, Vector2.zero);


        if (hit.collider == null || hit.collider.tag == "Ground")
        {
            if (_isDoubleClick)
            {
                if (GameController.Instance._mainCharacter != null && GameController.Instance._mainCharacter.activeInHierarchy)
                {
                    _isAuto = true;
                    _clickPosition = new Vector2(clickPosition1.x / 100 * 100, clickPosition1.y / 100 * 100);
                    GameObject click = PoolingContronller.Instance.GetClickPool();
                    click.transform.position = _clickPosition;
                    TargetController.Instance.TarGetNow(null);
                   //GameController.Instance._mainCharacter.GetComponent<Character>().isStartTuDongLuyenTap = false;
                }

            }


        }
    }

    // Các sự kiện click và double click
    //public Action OnSingleClick;
    //public Action OnDoubleClick;

    // Thời gian tối đa giữa hai lần click để tính là double-click
    [SerializeField] private float doubleClickThreshold = 0.3f;

    private float lastClickTime = 0f; // Lưu thời gian của lần click gần nhất

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // Kiểm tra click chuột trái
        {

            //Bỏ qua nếu click lên UI
            if (IsPointerOverUI())
                return;
           /* if (LoginSceneManager.Instance.SelectSever.activeInHierarchy)
            {
                LoginSceneManager.Instance.SelectSever.SetActive(false);
            }*/
            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick <= doubleClickThreshold)
            {
                // Xử lý double-click
                _isDoubleClick = true;
                DoubleClickHandle();
                //OnDoubleClick?.Invoke();
            }
            else
            {
                // Xử lý click đơn (sau một khoảng thời gian)
                _isDoubleClick = false;
                Invoke(nameof(TriggerSingleClick), doubleClickThreshold);
            }

            lastClickTime = Time.time;
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1) // Chỉ xử lý khi có 1 ngón tay chạm
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                // Bỏ qua nếu chạm vào UI
                if (IsPointerOverUI())
                    return;

                float timeSinceLastClick = Time.time - lastClickTime;

                if (timeSinceLastClick <= doubleClickThreshold)
                {
                    // Xử lý double-click
                    _isDoubleClick = true;
                    //OnDoubleClick?.Invoke();
                }
                else
                {
                    // Xử lý click đơn (sau một khoảng thời gian)
                    _isDoubleClick = false;
                    Invoke(nameof(TriggerSingleClick), doubleClickThreshold);
                }

                lastClickTime = Time.time;
            }
        }
    }

    private void TriggerSingleClick()
    {
        if (!_isDoubleClick) // Đảm bảo rằng sự kiện single click chỉ được gọi khi không có double click
        {
            //OnSingleClick?.Invoke();
        }
    }

    public bool IsPointerOverUI()
    {
        if (Main.main.IsIphone())
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                PointerEventData eventData = new PointerEventData(EventSystem.current)
                {
                    position = touch.position
                };

                var results = new System.Collections.Generic.List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);

                return results.Count > 0; // Nếu có kết quả, nghĩa là chạm vào UI
            }
            return false;
        }
        else
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

    }
}
