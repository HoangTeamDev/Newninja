public class SkillTemplate
{
    public sbyte _id;

    public int _classId;

    public string _name;

    public int _maxPoint;

    public int _manaUseType;

    public int _type;

    public int _iconId;

    public string[] _description;

    public Skill[] _skills;

    public string _damInfo;

    public bool isBuffToPlayer()
    {
        if (_type == 2)
        {
            return true;
        }
        return false;
    }

    public bool isUseAlone()
    {
        if (_type == 3)
        {
            return true;
        }
        return false;
    }

    public bool isAttackSkill()
    {
        if (_type == 1)
        {
            return true;
        }
        return false;
    }

    public bool isSkillSpec()
    {
        if (_type == 4)
        {
            return true;
        }
        return false;
    }
}
