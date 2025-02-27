using System;
using System.Collections.Generic;

[System.Serializable]
public class Clan
{
    public int _ID;

    public int _imgID;

    public string _name = string.Empty;

    public string _slogan = string.Empty;

    public int _date;

    public string _powerPoint;

    public int _currMember;

    public int _maxMember = 50;

    public int _leaderID;

    public string _leaderName;

    public int _level;
    public int _playerRole;

    public long _clanPoint;
    public List<Member> _members;

    public long _gold;
    public long _exp;
    public long _ClanFee;
    public long _ClanRank;


    public string GetMemberByID(int memberID)
    {
        foreach (var member in _members)
        {
            if (member != null && member._ID == memberID)
            {
                return member.GetRrole();
            }
        }

        return null;
    }
}
