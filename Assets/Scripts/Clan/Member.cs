public class Member
{
    public int _ID;

    public short _head;

    public short _headICON = -1;

    public short _leg;

    public short _body;

    public string _name;

    public sbyte _role;

    public string _powerPoint;

    public int _donate;

    public int _receive_donate;

    public int _curClanPoint;

    public int _clanPoint;

    public int _lastRequest;

    public string _joinTime;

    public long _goldContributePoint;
    public long _expContributePoint;

    public bool _isOnline;

    public int _cgender;
    public int _clevel;

    public static string getRole(int r)
    {
        return r switch
        {
            0 => "Bang Chủ ",
            1 => "Bang Phó ",
            2 => "Thành Viên",
            _ => string.Empty,
        };
    }
    public string GetRrole()
    {
        return _role switch
        {
            0 => "Bang Chủ ",
            1 => "Bang Phó ",
            2 => "Thành Viên",
            _ => string.Empty,
        };
    }
}
