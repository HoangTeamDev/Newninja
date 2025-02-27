using System.Diagnostics;

public class ItemTemplate
{
    public short _id;

    public sbyte _type;

    public sbyte _gender;

    public string _name;

    public string[] _subName;

    public string _description;

    public int _level;

    public short _iconID;

    public short _part;

    public bool _isUpToUp;

    public int _w;

    public int _h;

    public long _strRequire;

    public ItemTemplate()
    {

    }

    public ItemTemplate(short templateID, sbyte type, sbyte gender, string name, string description, sbyte level, int strRequire, short iconID, short part, bool isUpToUp)
    {
        _id = templateID;
        this._type = type;
        this._gender = gender;
        this._name = name;
        this._description = description;
        this._level = level;
        this._strRequire = strRequire;
        this._iconID = iconID;
        this._part = part;
        this._isUpToUp = isUpToUp;
    }
}
