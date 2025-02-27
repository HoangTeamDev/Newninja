using System.Collections.Generic;
using UnityEngine;

public class PoolingContronller : Singleton<PoolingContronller>
{
    [Header("DauChan")]
    [SerializeField] private GameObject _DauChan;
    private Queue<GameObject> _DauChanPool;
    [Header("Text")]
    [SerializeField] private GameObject _textpopup;
    private Queue<GameObject> _textPopupPool;
    [SerializeField] private GameObject _textpopupCrit;
    private Queue<GameObject> _textPopupPoolCrit;
    [SerializeField] private GameObject _textpopupKi;
    private Queue<GameObject> _textPopupPoolKI;
    [SerializeField] private GameObject _textpopupTiemNang;
    private Queue<GameObject> _textPopupPoolTiemNang;
    [SerializeField] private GameObject _textpopupMiss;
    private Queue<GameObject> _textPopupPoolMiss;
    [Header("Player")]
    [SerializeField] private GameObject _Player1;
    public Queue<GameObject> _PlayerPool;
    [Header("Skill")]
    [SerializeField] private GameObject _kame;
    private Queue<GameObject> _KamePool;
    [SerializeField] private GameObject _kameno;
    private Queue<GameObject> _KameNoPool;   
    [Header("NPC")]
    [SerializeField] private GameObject _NPC;
    private Queue<GameObject> _NPCPool;
    [Header("Item")]
    [SerializeField] private GameObject _Item;
    private Queue<GameObject> _ItemPool;
    [Header("Teleport")]
    [SerializeField] private GameObject _Town;
    private Queue<GameObject> _TownPool;
    [Header("Monster")]
    [SerializeField] private GameObject _monster;
    private Queue<GameObject> _MonsterPool;
    [Header("Click")]
    [SerializeField] private GameObject _Click;
    private Queue<GameObject> _ClickPool;
    [Header("Click")]
    [SerializeField] private GameObject _bulletEnemy;
    private Queue<GameObject> _BulletEnemyPool;
    [Header("MainPool")]

    [SerializeField] private GameObject _TextMainPool;
    [SerializeField] private GameObject _TextMainPoolCrit;
    [SerializeField] private GameObject _TextMainPoolKi;
    [SerializeField] private GameObject _TextMainPoolTiemNang;
    [SerializeField] private GameObject _TextMainPoolMiss;
    [SerializeField] public GameObject _PlayerMainPool; 
    [SerializeField] private GameObject _NPCMainPool;
    [SerializeField] private GameObject _ItemMainPool;
    [SerializeField] private GameObject _TownMainPool;
    [SerializeField] private GameObject _MonsterMainPool;
    [SerializeField] private GameObject _DauChanMainPool;
    [SerializeField] private GameObject _KameMainPool;
    [SerializeField] private GameObject _KameNoMainPool;
    [SerializeField] private GameObject _ClickMainPool;
    [SerializeField] private GameObject _BulletEnemyMainPool;
    public GameObject boxMainPool;




    public void CreateAllPool()
    {
        _textPopupPool = CreatePool(_textpopup, _TextMainPool.transform, 10);
        _textPopupPoolCrit = CreatePool(_textpopupCrit, _TextMainPoolCrit.transform, 10);
        _textPopupPoolKI = CreatePool(_textpopupKi, _TextMainPoolKi.transform, 10);
        _textPopupPoolTiemNang = CreatePool(_textpopupTiemNang, _TextMainPoolTiemNang.transform, 10);
        _textPopupPoolMiss = CreatePool(_textpopupMiss, _TextMainPoolMiss.transform, 10);
        _PlayerPool = CreatePool(_Player1, _PlayerMainPool.transform, 5);
        _NPCPool = CreatePool(_NPC, _NPCMainPool.transform, 10);
        _ItemPool = CreatePool(_Item, _ItemMainPool.transform, 5);
        _TownPool = CreatePool(_Town, _TownMainPool.transform, 10);
        _MonsterPool = CreatePool(_monster, _MonsterMainPool.transform, 9);
        _DauChanPool = CreatePool(_DauChan,_DauChanMainPool.transform, 20);
        _KamePool= CreatePool(_kame, _KameMainPool.transform, 20);
        _KameNoPool= CreatePool(_kameno, _KameNoMainPool.transform, 20);
        _ClickPool= CreatePool(_Click, _ClickMainPool.transform, 10);
        _BulletEnemyPool= CreatePool(_bulletEnemy, _BulletEnemyMainPool.transform, 10);

    }

    private Queue<GameObject> CreatePool(GameObject prefab, Transform parent, int initialCount)
    {
        Queue<GameObject> pool = new Queue<GameObject>();
        for (int i = 0; i < initialCount; i++)
        {
            GameObject obj = Instantiate(prefab, parent);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
        return pool;
    }

    private GameObject GetFromPool(Queue<GameObject> pool, GameObject prefab, Transform parent)
    {
        if (pool.Count > 0)
        {
            GameObject Obj = pool.Dequeue();
            Obj.SetActive(true);
            return Obj;
        }
        else
        {
            GameObject obj = Instantiate(prefab, parent);
            return obj;
        }

    }
    public GameObject GetTextPopup() => GetFromPool(_textPopupPool, _textpopup, _TextMainPool.transform);
    public GameObject GetTextPopupCrit() => GetFromPool(_textPopupPoolCrit, _textpopupCrit, _TextMainPoolCrit.transform);
    public GameObject GetTextPopupKi() => GetFromPool(_textPopupPoolKI, _textpopupKi, _TextMainPoolKi.transform);
    public GameObject GetTextPopupTiemNang() => GetFromPool(_textPopupPool, _textpopup, _TextMainPool.transform);
    public GameObject GetTextPopupMiss() => GetFromPool(_textPopupPool, _textpopup, _TextMainPool.transform);
    public GameObject GetPlayerPooling() => GetFromPool(_PlayerPool, _Player1, _PlayerMainPool.transform);
    public GameObject GetNPCPooling() => GetFromPool(_NPCPool, _NPC, _NPCMainPool.transform);
    public GameObject GetItemPool() => GetFromPool(_ItemPool, _Item, _ItemMainPool.transform);
    public GameObject GetTownPool() => GetFromPool(_TownPool, _Town, _TownMainPool.transform);
    public GameObject GetMonsterPool() => GetFromPool(_MonsterPool, _monster, _MonsterMainPool.transform);
    public GameObject GetDauChanPool() => GetFromPool(_DauChanPool, _DauChan, _DauChanMainPool.transform);
    public GameObject GetKamePool() => GetFromPool(_KamePool, _kame, _KameMainPool.transform);
    public GameObject GetKameNoPool() => GetFromPool(_KameNoPool, _kameno, _KameMainPool.transform);
    public GameObject GetClickPool() => GetFromPool(_ClickPool, _Click, _KameMainPool.transform);
    public GameObject GetBulletEnemykPool() => GetFromPool(_BulletEnemyPool, _bulletEnemy, _BulletEnemyMainPool.transform);

    public void ReturnDauChan(GameObject dauchan) => ReturnToPool(_DauChanPool, dauchan);
    public void ReturnTextPopup(GameObject textPopup) => ReturnToPool(_textPopupPool, textPopup, 0.5f);
    public void ReturnTextPopupCrit(GameObject textPopup) => ReturnToPool(_textPopupPoolCrit, textPopup, 0.5f);
    public void ReturnTextPopupKi(GameObject textPopup) => ReturnToPool(_textPopupPoolKI, textPopup, 0.5f);
    public void ReturnTextPopupTiemNang(GameObject textPopup) => ReturnToPool(_textPopupPoolTiemNang, textPopup, 0.5f);
    public void ReturnTextPopupMiss(GameObject textPopup) => ReturnToPool(_textPopupPoolMiss, textPopup, 0.5f);
    public void ReturnPlayer(GameObject player) => ReturnToPool(_PlayerPool, player);
    public void ReturnNPC(GameObject npc) => ReturnToPool(_NPCPool, npc);
    public void ReturnItem(GameObject item) => ReturnToPool(_ItemPool, item);
    public void ReturnTown(GameObject town) => ReturnToPool(_TownPool, town);
    public void ReturnMonster(GameObject monster) => ReturnToPool(_MonsterPool, monster);
    public void ReturnKame(GameObject kame) => ReturnToPool(_KamePool, kame);
    public void ReturnKameNo(GameObject kameno) => ReturnToPool(_KameNoPool, kameno);
    public void ReturnClick(GameObject Click) => ReturnToPool(_ClickPool, Click,0.5f);
    public void ReturnBuleltEnemy(GameObject bullet) => ReturnToPool(_BulletEnemyPool, bullet, 0.5f);
   
    public void SetParent(GameObject gameObject)
    {
        gameObject.transform.SetParent(_KameNoMainPool.transform);
    }

  /*  public BoxLeaveMap GetBoxLeaveMap(int id)
    {
        boxMainPool.transform.GetChild(id).gameObject.SetActive(true);
        return boxMainPool.transform.GetChild(id).gameObject.GetComponent<BoxLeaveMap>();
    }*/
  /*  public BoxLeaveMap1 GetBoxLeaveMap1(int id)
    {
        boxMainPool.transform.GetChild(id).gameObject.SetActive(true);
        return boxMainPool.transform.GetChild(id).gameObject.GetComponent<BoxLeaveMap1>();
    }*/
    private void ReturnToPool(Queue<GameObject> pool, GameObject obj, float time = 0)
    {
        if (time > 0)
        {
           
            //obj.transform.SetParent(parent.transform);
            pool.Enqueue(obj);
        }
        else
        {
           
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }
    
}