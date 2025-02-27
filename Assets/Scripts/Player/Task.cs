[System.Serializable]
public class Task
{
    public int _index;

    public int _max;

    public short[] _counts;

    public short _taskId;

    public string _names;

    public string _details;

    public string[] _subNames;

    public string[] _contentInfo;

    public short _count;
    public int[] _idnpc;
    public int[] _idmap;

    public Task(short taskId, sbyte index, string name, string detail, string[] subNames, short[] counts, short count, string[] contentInfo, int[] idnpc, int[] idmap)
    {
        this._taskId = taskId;
        this._index = index;
        this._names = name;
        this._details = detail;
        this._subNames = subNames;
        this._counts = counts;
        this._count = count;
        this._contentInfo = contentInfo;
        this._idnpc = idnpc;
        this._idmap = idmap;

    }
}