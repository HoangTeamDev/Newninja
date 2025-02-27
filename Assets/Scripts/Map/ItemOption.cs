public class ItemOption
{
    public int _param;

    public sbyte _active;

    public sbyte _activeCard;
    public int _optionColor;

    public ItemOptionTemplate _optionTemplate;

    public ItemOption()
    {
    }

    public ItemOption(int optionTemplateId, string name, int param, string des,int color)
    {
        _optionTemplate = new ItemOptionTemplate();
        if (optionTemplateId == 22)
        {
            optionTemplateId = 6;
            param *= 1000;
        }
        if (optionTemplateId == 23)
        {
            optionTemplateId = 7;
            param *= 1000;
        }

        _optionTemplate.id = optionTemplateId;
        _optionTemplate.name = name;
        this._param = param;
        this._optionColor = color;
        //optionTemplate = GameScr.gI().iOptionTemplates[optionTemplateId];
    }

    public string getOptionString()
    {
        return NinjaUtil.replace(_optionTemplate.name, "#", _param + string.Empty);
    }

    public string getOptionName()
    {
        return NinjaUtil.replace(_optionTemplate.name, "+#", string.Empty);
    }

    public string getOptiongColor()
    {
        return NinjaUtil.replace(_optionTemplate.name, "$", string.Empty);
    }
}
