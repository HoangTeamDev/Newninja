
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class GameData : Singleton<GameData>
{
    [SerializeField] public TitleMapData _titleMapData = new TitleMapData();
    [SerializeField] public Character _DataMain=new Character();
    [SerializeField]  public List<Character>  _CharacterInMap;
    public List<Enemy> _enemyInMap;
    public List<ItemMap> _itemInMap;
    public List<Npc> _npcInmap;

    public Character FindPlayerInMap(int id)
    {
        return _CharacterInMap.FirstOrDefault(x => x._playerId == id);
    }

    public Enemy FindEnemyInMap(int id)
    {
        return _enemyInMap.FirstOrDefault(x => x._indexEnemy == id);
    }
    public ItemMap FindItembyIndex(int id)
    {
        return _itemInMap.FirstOrDefault(x=>x._itemMapID == id); 
    }
    public ItemMap FindItemById(int id) { return _itemInMap.FirstOrDefault(x => x._idTemplateID == id); }

    public Npc FindNpcByIndex(int id) { return (Npc)_npcInmap.FirstOrDefault(x => x._npcId == id); }
    public Npc FindNpcByID(int id) { return _npcInmap.FirstOrDefault(x => x._templateId == id); }










}


