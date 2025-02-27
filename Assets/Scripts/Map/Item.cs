

public class Item 
{
    public ItemOption[] _itemOption;
    public string[] _itemFixedStats;

    public ItemTemplate _template;

    public int _itemId;

    public int _playerId;

    public int _indexUI;

    public int _quantity;
    
    public string _info;  
    public int _quantilyToBuy;

    public long _powerRequire;

    public bool _isLock;

    public int _sys;

    public int _buyClanContribute;

    public short _buyClanLevelRequire;

    public int _upgrade;

    public int _buyCoin;

    public int _buyCoinLock;


    public int _buyGold;
    public long _buypower;


    public int _buyGoldLock;

    public int _saleCoinLock;

    public int _buySpec;

    public int _buyRuby;

    public short _iconSpec = -1;

    public sbyte _buyType = -1;

    public int _typeUI;

    public bool _isExpires;
    public bool _isClanBox;

    public bool _isBuySpec;
    public bool _isMuaNhieu;


    //public EffectCharPaint eff;

    public int _indexEff;

    //public Image img;

   
    public string _content;

    public string _reason = string.Empty;

    public int _compare;

    public sbyte _isMe;

    public bool _newItem;

    public int _headTemp = -1;

    public int _bodyTemp = -1;

    public int _legTemp = -1;

    public int _bagTemp = -1;

    public int _wpTemp = -1;

    public long _buyFood;

    public string _nameNguoiKyGui = string.Empty;

    private int[] _color = new int[18]
    {
        0, 0, 0, 0, 600841, 600841, 667658, 667658, 3346944, 3346688,
        4199680, 5052928, 3276851, 3932211, 4587571, 5046280, 6684682, 3359744
    };

    private int[][] _colorBorder = new int[5][]
    {
        new int[6] { 18687, 16869, 15052, 13235, 11161, 9344 },
        new int[6] { 45824, 39168, 32768, 26112, 19712, 13056 },
        new int[6] { 16744192, 15037184, 13395456, 11753728, 10046464, 8404992 },
        new int[6] { 13500671, 12058853, 10682572, 9371827, 7995545, 6684800 },
        new int[6] { 16711705, 15007767, 13369364, 11730962, 10027023, 8388621 }
    };

    private int[] _size = new int[6] { 2, 1, 1, 1, 1, 1 };
}
