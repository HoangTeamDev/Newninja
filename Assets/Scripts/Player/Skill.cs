[System.Serializable]
public class Skill
{
  
    public SkillTemplate template;

    public short _Id;

    public short _SkillId;

    public int _Point;
    public int _Type;


    public long _PowRequire;
    public long l_astTimeUseThisSkill;
    public int _coolDown;
    public int _Damage;

    public int _dx;

    public int _dy;

    public int _maxFight;

    public int _manaUse;

    public SkillOption[] _options;

    public bool _paintCanNotUseSkill;

    public short _damage;

    public string _DamageInfo;
    public string _Description;
    public string _Name;
    public string _MoreInfo;



    public short _price;

    public short _curExp;

    public int _timeBar = 0;

    public int CoolDownBar()
    {
        if (_Id == 1)
            return _coolDown;
        return _coolDown;
    }
    public string strCurExp()
    {
        if (_curExp / 10 >= 100)
        {
            return "MAX";
        }
        if (_curExp % 10 == 0)
        {
            return _curExp / 10 + "%";
        }
        int num = _curExp % 10;
        return _curExp / 10 + "." + num % 10 + "%";
    }

    public string strTimeReplay()
    {
        if (_coolDown % 1000 == 0)
        {
            return _coolDown / 1000 + string.Empty;
        }
        int num = _coolDown % 1000;
        return _coolDown / 1000 + "." + ((num % 100 != 0) ? (num / 10) : (num / 100));
    }
}
