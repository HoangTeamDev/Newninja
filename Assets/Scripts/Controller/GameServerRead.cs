using System;
using System.Threading;

using UnityEngine;


public partial class GameServer
{

    private Character _character;
    private GameObject _player;
    private Enemy _enemy;
    private Npc _npc;
    private ItemMap _itemMap;

    private void HandleMessage(Message message)
    {
        Debug.Log("Mess:"+message.command);
        switch (message.command)
        {
            case 0://login
                LoginSceneManager.Instance.LoginSuccess();
                break;
            case 1://Create char
                LoginSceneManager.Instance.ActiveCreateCharacter();
                Debug.Log("create");
                break;
            case 2://Charinfo
                LoadCharInfo(message);
                break;
            case 3://infoBody
                LoadInfoBody(message);
                break;
            case 4://infoBag
                LoadInfoBag(message);
                break;
            case 5://LoadMiss
                LoadMission(message);
                break;
            case 6://UpdateMissison
                GameData.Instance._DataMain._taskMaint._count = message.reader().readShort();
                break;
            case 7://updatemap
                UpdateMap(message);
                break;
            case 8://MessSUb
                MessageSubCommand(message);
                GameController.Instance._mainCharacter.SetActive(true);
                break;
            case 9://CharMove
                MoveChar(message);
                break;
            case 10://playerdie
                PlayerDie(message);
                break;
            case 11://playerLive
                PlayerLive(message);
                break;
            case 12://Add PlayerInmap
                PlayerJoinMap(message);
                break;
            case 13://setpos
                SetPos(message.reader().readInt(), message.reader().readFloat(), message.reader().readFloat());
                break;
            case 14://updatehpcharinmap
                UpdateHPCharInMap(message);
                break;
            case 15://activenpc
                ActiveNPC(message);            
                break;
            case 16://congyen
                if (GameData.Instance._DataMain != null)
                {
                       long yen= message.reader().readLong();//yen
                    GameData.Instance._DataMain._yen += yen;
                    GameData.Instance._DataMain.CreateTextPopUp(yen, "+", 2);
                }
                break;
            case 17://changemap
                //int iditemdelete= message.reader().readShort();//index
                break;
            case 18://updatehpQuaiMain
                UpdateHpQuai(message);             
                break;
            case 19://quai hoi hinh
                MonsterRespawn(message);              
                break;
            case 20://quaidie
                int index= message.reader().readByte();//index quai
                _enemy=null;
                _enemy = GameData.Instance.FindEnemyInMap(index);
                if (_enemy != null)
                {
                    _enemy.EnemyDie();
                }
                break;
            case 21://thachdau
                message.reader().readInt();//id thachdau
                break;
            case 22://tudongluyentap
                bool value = message.reader().readBoolean();
                break;
            case 23://quaidanh
                EnemyAttack(message);              
                break;
            case 24://updatekinhnghiem
                int exp= message.reader().readInt();//exp
                GameData.Instance._DataMain._exp += exp;
                GameData.Instance._DataMain.CreateTextPopUp(exp, "+", 5);
                break;
            case 25://Quaihoimau
                EnemyHealth(message);             
                break;
            case 26://itemmap
                AddItemMap(message);
                break;
            case 27://cleanmap
                GameController.Instance.CleanMap();
                break;
            case 28://Chatzone
                ChatZone(message);             
                break;
            case 29://CharleaveMap
                CharLeaveMap(message);             
                break;
            case 30://Chattodung
                ServerNoti(message);              
                break;
            case 31://Chatkhi lam gi do
                ChatNoting(message);              
                break;
            case 32://thanh chat chay duoi
                ServerChat(message);
                break;
            case 33://npc chat
                NPCChat(message);                
                break;
            case 34://world
                WorldChat(message);           
                break;
            case 35://privateChat
                PrivateChat(message);               
                break;
            case 36://itemtime
                ItemTime(message);               
                break;
            case 37:
                break;
            case 38://sellitem
                SellItem(message);             
                break;
            case 39://moshop
                OpenShop(message);              
                break;
            case 40:
                break;
            case 41:
                break;
            case 42://deleteItem
                DeleteItem(message);              
                break;
            case 43:
                break;
            case 44://openUIZone
                OpenUIZone(message);             
                break;
            case 45:
                break;
            case 46://ConfirmMenu
                ConfirmMenu(message);               
                break;
            case 47://npcchat pop
                short npcid = message.reader().readShort();
                message.reader().readUTF();
                break;
            case 48://closeUI
                sbyte close = message.reader().readByte();
                break;
            case 49://Showinput;
                ShowInput(message);               
                break;
            case 50:
                break;
            case 51:
                break;
            case 52://OpenChest
                OpenChest(message);             
                break;
            case 53:
                break;
            case 54://die mainpk
                MainDie(message);             
                break;
            case 55:
                TradeItem(message);              
                break;
            case 60://uplevel
                message.reader().readInt();//idplayer
                message.reader().readLong();//expCurrent
                message.reader().readLong();//expToUp
                break;



        }


    }
    public void MessageSubCommand(Message message)
    {
        sbyte b = message.reader().readByte();
        switch (b)
        {
            case 0:
                GameData.Instance._DataMain._playerId = message.reader().readInt();//character.Id
                GameData.Instance._DataMain._taskMaint._taskId = message.reader().readByte();//character.InfoTask.Id
                GameData.Instance._DataMain._gender = message.reader().readByte();//character.InfoChar.Gender
                GameData.Instance._DataMain._hair = message.reader().readShort();//character.InfoChar.Hair
                GameData.Instance._DataMain._level = message.reader().readByte();//character.InfoChar.Level
                GameData.Instance._DataMain._namePlayer = message.reader().readUTF();//character.Name
                GameData.Instance._DataMain._pk = message.reader().readByte();//character.InfoChar.Pk
                GameData.Instance._DataMain._typePK = message.reader().readByte();//character.InfoChar.TypePk
                GameData.Instance._DataMain._yen = message.reader().readLong();//yen
                GameData.Instance._DataMain._xu = message.reader().readLong();//xu
                GameData.Instance._DataMain._luong = message.reader().readLong(); //lượng
                int count0s = message.reader().readByte();//countSkill
                Debug.Log("skill:" + count0s);
                for (int i = 0; i < count0s; i++)
                {
                    Skill skillchar = new Skill();
                    skillchar._Id = message.reader().readShort();
                    skillchar._SkillId = message.reader().readShort();
                    skillchar._Point = message.reader().readShort();
                    skillchar._Type = message.reader().readByte();
                    skillchar._DamageInfo = message.reader().readUTF();
                    skillchar._Description = message.reader().readUTF();
                    skillchar._Name = message.reader().readUTF();
                    skillchar._PowRequire = message.reader().readLong();
                    skillchar._manaUse = message.reader().readInt();
                    skillchar._coolDown = message.reader().readInt();
                    skillchar._Damage = message.reader().readInt();
                    skillchar._MoreInfo = message.reader().readUTF();
                    GameData.Instance._DataMain._Skills.Add(skillchar);
                }

                break;
            case 1:
                GameData.Instance._DataMain._yen = message.reader().readLong();//yen
                GameData.Instance._DataMain._xu = message.reader().readLong();//xu
                GameData.Instance._DataMain._luong = message.reader().readLong();//luong

                break;
            case 2:
                long chp = message.reader().readLong();
                break;
            case 3:
                long cmp = message.reader().readLong();
                break;
            case 4:
                int speed = message.reader().readInt();
                break;
            case 5:
                int idplayer = message.reader().readInt();
                long hp = message.reader().readLong();
                long cHPFull = message.reader().readLong();
               
                break;
            case 6:
                int id = message.reader().readInt();
                int typePk = message.reader().readByte();
                break;
            case 7:
                Skill skillAdd = new Skill();
                skillAdd._Id = message.reader().readShort();
                skillAdd._SkillId = message.reader().readShort();
                skillAdd._Point = message.reader().readShort();
                skillAdd._Type = message.reader().readByte();
                skillAdd._DamageInfo = message.reader().readUTF();
                skillAdd._Description = message.reader().readUTF();
                skillAdd._Name = message.reader().readUTF();
                skillAdd._PowRequire = message.reader().readLong();
                skillAdd._manaUse = message.reader().readInt();
                skillAdd._coolDown = message.reader().readInt();
                skillAdd._Damage = message.reader().readInt();
                skillAdd._MoreInfo = message.reader().readUTF();
                useSkill(skillAdd);
                break;
            case 8:
                try
                {
                    short id8 = message.reader().readShort();
                    short SkillId = message.reader().readShort();
                    short point = message.reader().readShort();
                    int type = message.reader().readByte();
                    string damageInfo = message.reader().readUTF();
                    string description = message.reader().readUTF();
                    string name = message.reader().readUTF();
                    long powRequire = message.reader().readLong();
                    int manaUse = message.reader().readInt();
                    int coolDown = message.reader().readInt();
                    int damage = message.reader().readInt();
                    string moreInfo = message.reader().readUTF();

                    for (int i = 0; i < GameData.Instance._DataMain._Skills.Count; i++)
                    {
                        Skill skill = (Skill)GameData.Instance._DataMain._Skills[i];
                        if (skill._Id == id8)
                        {
                            skill._SkillId = SkillId;
                            skill._Point = point;
                            skill._Type = type;
                            skill._DamageInfo = damageInfo;
                            skill._Description = description;
                            skill._Name = name;
                            skill._PowRequire = powRequire;
                            skill._manaUse = manaUse;
                            skill._coolDown = coolDown;
                            skill._Damage = damage;
                            skill._MoreInfo = moreInfo;
                            Main.Instance.LogError(skill._Point);
                            break;

                        }
                    }
                }
                catch (Exception)
                {
                    Main.Instance.LogError("aaaaaaaaaaaaaaaaaaaa");
                }
                break;
            case 13:
                message.reader().readInt();
                message.reader().readLong();
                message.reader().readLong();
                break;
        }
    }
    private void useSkill(Skill skill)
    {
        if (skill == null) return;
        if (GameData.Instance._DataMain._myskill == null)
        {
            GameData.Instance._DataMain._myskill = skill;
        }
        GameData.Instance._DataMain._Skills.Add(skill);
    }

    private void LoadCharInfo(Message message)
    {
        try
        {
            GameData.Instance._DataMain._originHP = message.reader().readLong();//OriginalHp
            GameData.Instance._DataMain._originMP = message.reader().readLong();//OriginalMp
            GameData.Instance._DataMain._originDamege = message.reader().readLong();//OriginalDamage
            GameData.Instance._DataMain._hpFull = message.reader().readLong();//HpFull
            GameData.Instance._DataMain._mpFull = message.reader().readLong(); // MpFull               
            GameData.Instance._DataMain._hp = message.reader().readLong();//Hp
            GameData.Instance._DataMain._mp = message.reader().readLong();//Mp
            GameData.Instance._DataMain._damageFull = message.reader().readLong();//DamageFull
            GameData.Instance._DataMain._defFull = message.reader().readInt();//DefenceFull                 
            GameData.Instance._DataMain._KhangBang = message.reader().readInt();//KhangBang
            GameData.Instance._DataMain._KhangGio = message.reader().readInt();//KhangGio
            GameData.Instance._DataMain._KhangHoa = message.reader().readInt();//KhangHoa
            GameData.Instance._DataMain._Precision = message.reader().readInt();//Precision
            GameData.Instance._DataMain._InternalForce = message.reader().readLong();//InternalForce                 
            GameData.Instance._DataMain._critFull = message.reader().readByte();//CritFull
            GameData.Instance._DataMain._level = message.reader().readInt();
            GameData.Instance._DataMain._exp = message.reader().readLong();//Exp
            GameData.Instance._DataMain._expCurent = message.reader().readLong();
            GameData.Instance._DataMain._expToUp = message.reader().readLong();
            GameData.Instance._DataMain._originDef = message.reader().readShort();//OriginalDefence
            GameData.Instance._DataMain._originCrit = message.reader().readByte();//OriginalCrit
            GameData.Instance._DataMain._mounthid = message.reader().readShort();//MountId 
            GameData.Instance._DataMain._Speed = message.reader().readInt();
        }
        catch (Exception)
        {

        }

    }

    private void LoadInfoBody(Message message)
    {
        try
        {
            GameData.Instance._DataMain._hair = message.reader().readShort();//hair
            sbyte countAll = message.reader().readByte();//so do
            for (int i = 0; i < countAll; i++)
            {
                Item items = new Item();
                items._itemId = message.reader().readShort();//id
                items._quantity = message.reader().readInt();//qualiti
                items._template._name = message.reader().readUTF();//name
                items._template._description = message.reader().readUTF();//des
                items._template._type = message.reader().readByte();//type
                items._template._strRequire = message.reader().readLong();//require
                sbyte count1 = message.reader().readByte();//count
                items._itemOption = new ItemOption[count1];
                for (int j = 0; j < count1; j++)
                {
                    ItemOption itemOption = new ItemOption(message.reader().readShort(), message.reader().readUTF(), message.reader().readInt(), message.reader().readUTF(), message.reader().readByte());
                    items._itemOption[j] = itemOption;
                }
                items._template._level = message.reader().readShort();//level
                if (items._template._type <= 10)
                {
                    message.reader().readByte();//count infoupdate
                    message.reader().readUTF();//info
                }
                GameData.Instance._DataMain._ListItemBody.Add(items);

            }
        }
        catch (Exception e)
        {

        }

    }


    private void LoadInfoBag(Message message)
    {
        try
        {
            GameData.Instance._DataMain._bagLenght = message.reader().readInt();//độ dài hành trang
            int count4 = message.reader().readInt();// số đồ trong hành trang
            for (int i = 0; i < count4; i++)
            {
                Item item = new Item();
                item._itemId = message.reader().readShort();//id item
                item._indexUI = message.reader().readInt();//index UI
                item._quantity = message.reader().readInt();//qulity
                item._template._name = message.reader().readUTF();//name
                item._template._description = message.reader().readUTF();//des
                sbyte type = message.reader().readByte();//type
                item._template._type = type;
                item._template._strRequire = message.reader().readLong();//require
                message.reader().readShort();//element
                sbyte count2 = message.reader().readByte();//countop
                for (int k = 0; k < count2; k++)
                {
                    ItemOption option = new ItemOption(message.reader().readShort(), message.reader().readUTF(), message.reader().readInt(), message.reader().readUTF() ,message.reader().readByte());
                    item._itemOption[k] = option;
                }
                item._template._level = message.reader().readShort();//level
                if (type <= 10)
                {

                    sbyte count41 = message.reader().readByte();// count..
                    for (int j = 0; j < count41; j++)
                    {
                        message.reader().readUTF();//
                    }

                }
                GameData.Instance._DataMain._listItemBag.Add(item);
            }
        }
        catch (Exception e) { }

    }
    private void LoadMission(Message message)
    {
        try
        {
            short taskId = message.reader().readShort();
            sbyte index3 = message.reader().readByte();
            string str3 = message.reader().readUTF();
            string str4 = message.reader().readUTF();
            string[] array12 = new string[message.reader().readByte()];
            string[] array13 = new string[array12.Length];
            short[] array14 = new short[array12.Length];
            int[] array15 = new int[array12.Length];
            int[] array16 = new int[array12.Length];
            short count = -1;
            for (int num159 = 0; num159 < array12.Length; num159++)
            {
                string str5 = message.reader().readUTF();
                int idnpc = message.reader().readByte();
                int idmap = message.reader().readShort();
                string str6 = message.reader().readUTF();
                array14[num159] = -1;
                array12[num159] = str5;
                array15[num159] = idnpc;
                array16[num159] = idmap;
                if (!str6.Equals(string.Empty))
                {
                    array13[num159] = str6;
                }
            }
            try
            {
                count = message.reader().readShort();
                for (int num160 = 0; num160 < array12.Length; num160++)
                {
                    array14[num160] = message.reader().readShort();
                }
            }

            catch (Exception ex23)
            {
                Main.Instance.LogError(ex23);
            }
            GameData.Instance._DataMain._taskMaint = new Task(taskId, index3, str3, str4, array12, array14, count, array13, array15, array16);
        }
        catch (Exception e) { }

    }


    private void UpdateMap(Message message)
    {
        try
        {
            GameData.Instance._titleMapData._mapID = message.reader().readByte();//mapid                                   
            GameData.Instance._titleMapData._mapName = message.reader().readUTF();//namemap
            GameData.Instance._titleMapData._zoneID = message.reader().readByte();//zone
            float x = message.reader().readFloat();//x
            float y = GameData.Instance._DataMain._y = message.reader().readFloat();//y
            GameData.Instance._DataMain.SetPos(x, y);
            int count1 = message.reader().readByte();//số cổng
            for (int i = 0; i < count1; i++)
            {
                Waypoint waypoint = new Waypoint(message.reader().readFloat(), message.reader().readFloat(), message.reader().readBool(), message.reader().readBool(), message.reader().readUTF());
                GameData.Instance._titleMapData._lstWayPoint.Add(waypoint);
            }
            GameController.Instance.LoadDaTaMap();
            int count2 = message.reader().readByte();// so quai
            for (int j = 0; j < count2; j++)
            {
                _enemy = null;
                GameObject EnemyOb = PoolingContronller.Instance.GetMonsterPool();
                _enemy = EnemyOb.GetComponent<Enemy>();

                _enemy._isDie = message.reader().readBool();//die
                _enemy._isDontMove = message.reader().readBool();//dontmove
                _enemy._KhangLua = message.reader().readBool();//lua
                _enemy._KhangBang = message.reader().readBool();//bang
                _enemy._KhangGio = message.reader().readBool();//gio
                _enemy._idEnemy = message.reader().readByte();//idquai
                _enemy._hpEnemy = message.reader().readLong();//hp
                _enemy._level = message.reader().readByte();//level
                _enemy._maxExp = message.reader().readLong();//maxexp
                _enemy._xEnemy = message.reader().readFloat();//x
                _enemy._yEnemy = message.reader().readFloat();//y
                _enemy._status = message.reader().readByte();//status
                _enemy._levelBoss = message.reader().readByte();//levelboss
                _enemy._nameEnemy = message.reader().readUTF();//name
                _enemy._isBoss = message.reader().readBool();////boss
                _enemy._speed= message.reader().readInt();//speed
                _enemy._distanceMove= message.reader().readFloat();//dítancemove
                _enemy._type= message.reader().readInt();//type
                _enemy.EnemyOnLoad();
                GameData.Instance._enemyInMap.Add(_enemy);
                GameController.Instance._enemyInMap.Add(EnemyOb);
            }



            int count3 = message.reader().readByte();// so npc
            for (int k = 0; k < count3; k++)
            {
                _npc = null;
                GameObject npcs = PoolingContronller.Instance.GetNPCPooling();
                _npc = npcs.GetComponent<Npc>();
                _npc.Onload(k, message.reader().readByte(), message.reader().readFloat(), message.reader().readFloat(),  message.reader().readUTF());
                GameData.Instance._npcInmap.Add(_npc);
                GameController.Instance._npcInMap.Add(npcs);
            }

            int count4 = message.reader().readByte();
            for (int l = 0; l < count4; l++)
            {
                _itemMap = null;
                GameObject itemmap = PoolingContronller.Instance.GetItemPool();
                _itemMap = itemmap.GetComponent<ItemMap>();
                _itemMap.Onload(message.reader().readShort(), l, message.reader().readShort(), message.reader().readFloat(), message.reader().readFloat());
                GameData.Instance._itemInMap.Add(_itemMap);
                GameController.Instance._itemInMap.Add(itemmap);
            }
            message.reader().readByte();
        }
        catch (Exception ex) { }

    }
    private void MoveChar(Message message)
    {
        int num177 = message.reader().readInt();
        int key = message.reader().readByte();
        MovePoint move = new MovePoint(num177, key, message.reader().readFloat(), message.reader().readFloat(), message.reader().readFloat());
        if (!GameController.Instance.MovePoints.TryAdd(num177, move))
        {
            GameController.Instance.MovePoints[num177] = move;
        }
    }

    private void PlayerDie(Message message)
    {
        _character = null;
        int idplayerdie = message.reader().readInt();
        int typk = message.reader().readByte();//typk
        float x = message.reader().readFloat();//x
        float y = message.reader().readFloat();//y      
        if (idplayerdie == GameData.Instance._DataMain._playerId)
        {
            GameData.Instance._DataMain._isDie = true;
            GameData.Instance._DataMain._SpineController.PlayAnimation(-5);
        }
        else
        {
            _character = GameData.Instance.FindPlayerInMap(idplayerdie);
            if (_character != null)
            {
                _character._isDie = true;
                _character._SpineController.PlayAnimation(-5);
            }
        }
    }

    private void PlayerLive(Message message)
    {
        _character = null;
        int idplayeLive = message.reader().readInt();
        long hp = message.reader().readLong();//hp
        long hpfull = message.reader().readLong();//hpfull
        long mp = message.reader().readLong();//mp
        long mpfull = message.reader().readLong();//mpfull
        float x = message.reader().readFloat();//x
        float y = message.reader().readFloat();//y

        if (idplayeLive == GameData.Instance._DataMain._playerId)
        {
            GameData.Instance._DataMain._isDie = false;
            GameData.Instance._DataMain._hp = hp;
            GameData.Instance._DataMain._hpFull = hpfull;
            GameData.Instance._DataMain._mp = mp;
            GameData.Instance._DataMain._mpFull = mpfull;
            GameData.Instance._DataMain.SetPos(x, y);
        }
        else
        {
            _character = GameData.Instance.FindPlayerInMap(idplayeLive);
            if (_character != null)
            {
                _character._isDie = false;
                _character._hp = hp;
                _character._hpFull = hpfull;
                _character._mp = mp;
                _character._mpFull = mpfull;
                _character.SetPos(x, y);
            }
        }
    }
    private void PlayerJoinMap(Message message)
    {
        try
        {
            _character=null;
            GameObject player1 = PoolingContronller.Instance.GetPlayerPooling();
            _character = player1.GetComponent<Character>();
            Debug.Log("addPlayer:" + player1);
            if (_character != null)
            {
                _character._playerId = message.reader().readInt();//idplayere
                _character._clan._ID = message.reader().readInt();//clanid
                _character._level = message.reader().readByte();     //level
                _character._typePK = message.reader().readByte();    //type pk
                _character._gender = message.reader().readByte();    //gender
                _character._clan._playerRole = message.reader().readShort();//role clan
                _character._namePlayer = message.reader().readUTF();//name
                _character._clan._name = message.reader().readUTF();//nameclan
                _character._hp = message.reader().readLong();    //hp
                _character._hpFull = message.reader().readLong();         //hp full
                _character._mp = message.reader().readLong();    //mp
                _character._mpFull = message.reader().readLong();         //mpfull
                _character._damageFull = message.reader().readLong();     //damefull
                _character._KhangBang = message.reader().readInt();//khangbang
                _character._KhangGio = message.reader().readInt();//khanggio
                _character._KhangHoa = message.reader().readInt();//khanghoa
                _character._Precision = message.reader().readInt();//chinhxac
                _character._InternalForce = message.reader().readLong();//noiluc
                _character._x = message.reader().readFloat();        //x
                _character._y = message.reader().readFloat();        //y
                _character._mounthid = message.reader().readShort();  //mount
                _character._Speed = message.reader().readInt();
            }
            player1.transform.position = new Vector2(_character._x, _character._y);
            GameData.Instance._CharacterInMap.Add(_character);
            GameController.Instance._playerInMap.Add(player1);
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }

    }

    private void SetPos(int playerid, float x, float y)
    {
        _character = null;
        if (GameData.Instance._DataMain._playerId == playerid)
        {
            _character = GameData.Instance._DataMain;
            if (_character != null)
            {
                _character.SetPos(x, y);
            }
        }
        else
        {
            _character = GameData.Instance.FindPlayerInMap(playerid);
            if (_character != null)
            {
                _character.SetPos(x, y);
            }
        }
        
    }

    private void UpdateHPCharInMap(Message message)
    {
        _character = null;
        try
        {
            int idnhanvat56 = message.reader().readInt();
            long hp1 = message.reader().readLong();
            long hpMinus = message.reader().readLong();
            bool flag4 = message.reader().readBoolean();
            if (GameData.Instance._DataMain._playerId == idnhanvat56)
            {
                _character = GameData.Instance._DataMain;
                if (_character != null)
                {
                    _character._hp = hp1;
                    if (flag4)
                    {
                        _character.TakeDamage(hpMinus, true);
                    }
                    else
                    {
                        _character.TakeDamage(hpMinus, false);
                    }
                }
            }
            else
            {
                _character = GameData.Instance.FindPlayerInMap(idnhanvat56);
                if (_character != null)
                {
                    _character._hp = hp1;
                    if (flag4)
                    {
                        _character.TakeDamage(hpMinus, true);
                    }
                    else
                    {
                        _character.TakeDamage(hpMinus, false);
                    }
                }
            }

        }
        catch
        {

        }

    }

    private void ActiveNPC(Message message)
    {
        int idnpc15 = message.reader().readByte();//id npc
        bool active = message.reader().readBoolean();
        _npc = null;
        _npc = GameData.Instance.FindNpcByIndex(idnpc15);
        if (_npc != null)
        {
            _npc.OnsetActive(active);
        }
    }

    private void UpdateHpQuai(Message message)
    {
        int indexquai= message.reader().readByte();//indexquai
        long hp= message.reader().readLong();//hpcon
        bool iscrit= message.reader().readBoolean();//chimang
        _enemy = null;
        _enemy=GameData.Instance.FindEnemyInMap(indexquai);
        if (_enemy != null)
        {
            if (iscrit)
            {
                _enemy.TakeDamage(_enemy._hpEnemy - hp, true);
            }
            else
            {
                _enemy.TakeDamage(_enemy._hpEnemy - hp, false);
            }
            _enemy._hpEnemy-=hp;
        }
    }

    private void MonsterRespawn(Message message)
    {
        int index= message.reader().readByte();//index quai
        int level= message.reader().readByte();//level
        long hp= message.reader().readLong();//hp
        bool isboss= message.reader().readBoolean();//isboss
        _enemy=null;
        _enemy = GameData.Instance.FindEnemyInMap(index);
        if (_enemy != null)
        {
            _enemy.EnemyRespawn(hp,level,isboss);
            
        }
    }

    private void EnemyAttack(Message message)
    {
        try
        {
           int index=  message.reader().readByte();//indexquai
            int player= message.reader().readInt();//idnguoichoi
            long dame= message.reader().readLong();//dame
            long hp= message.reader().readLong();//hp
            _enemy=null ;
            _enemy = GameData.Instance.FindEnemyInMap(index);
            if (_enemy != null)
            {
                _character=null;
                if (GameData.Instance._DataMain._playerId == player)
                {
                    _enemy.SetAttackTarget(GameData.Instance._DataMain.transform, dame);
                }
                else
                {
                    _character = GameData.Instance.FindPlayerInMap(player);
                    _enemy.SetAttackTarget(_character.gameObject.transform, dame);

                }
            }
        }
        catch (Exception e) { }
       
    }

    private void EnemyHealth(Message message)
    {
        try
        {
            int index= message.reader().readByte();//index
            long hp= message.reader().readLong();//hp
            _enemy=null ;
            _enemy=GameData.Instance.FindEnemyInMap(index);
            if (_enemy != null)
            {
                _enemy._hpEnemy = hp;
            }
        }
        catch (Exception e) { }
    }

    private void AddItemMap(Message message)
    {
        short itemMapID = message.reader().readShort();
        short itemTemplateID = message.reader().readShort();
        float x = message.reader().readFloat();
        float y = message.reader().readFloat();
        int playerid= message.reader().readInt();
        _itemMap = null;
        GameObject itemmap = PoolingContronller.Instance.GetItemPool();
        _itemMap = itemmap.GetComponent<ItemMap>();
        _itemMap.Onload(playerid, itemMapID, itemTemplateID, x, y);
        GameData.Instance._itemInMap.Add(_itemMap);
        itemmap.transform.position=new Vector3(x, y, 0);
        GameController.Instance._itemInMap.Add(itemmap);
        
    }

    private void ChatZone(Message message)
    {
        try
        {
            int playerid= message.reader().readInt();//idplayer
            string textchat= message.reader().readStringUTF();//
            _character = null;
            if (playerid == GameData.Instance._DataMain._playerId)
            {
                GameData.Instance._DataMain.ChatZone(textchat);
            }
            else
            {
                _character=GameData.Instance.FindPlayerInMap(playerid);
                if (_character != null)
                {
                    _character.ChatZone(textchat);
                }
            }
        }
        catch (Exception e) { }
        
    }

    private void CharLeaveMap(Message message)
    {
        int id= message.reader().readInt();//idplayer
        _character=null;
        _character = GameData.Instance.FindPlayerInMap(id);
        if (_character != null)
        {
            GameData.Instance._CharacterInMap.Remove(_character);
            GameController.Instance._playerInMap.Remove(_character.gameObject);
            PoolingContronller.Instance.ReturnPlayer(_character.gameObject);
        }
    }

    private void ServerNoti(Message message)
    {
        message.reader().readStringUTF();
    }

    private void ChatNoting(Message message)
    {
        message.reader().readStringUTF();
    }

    private void ServerChat(Message message)
    {
        message.reader().readStringUTF();
    }
    private void NPCChat(Message message)
    {
        message.reader().readShort();//idnpc
        message.reader().readStringUTF();//
    }
    private void WorldChat(Message message)
    {
        message.reader().readUTF();//name
        message.reader().readUTF();//mess
        message.reader().readInt();//id
        message.reader().readInt();//gender
        message.reader().readInt();//level
    }
    private void PrivateChat(Message message)
    {
        message.reader().readUTF();//name
        message.reader().readUTF();//chat
        message.reader().readInt();//idbanbe
        message.reader().readInt();//idminh
        message.reader().readInt();//gender
        message.reader().readInt();//level
    }

    private void ItemTime(Message message)
    {
        short num32 = message.reader().readShort();//id
        int num33 = message.reader().readInt();//time
    }

    private void SellItem(Message message)
    {
        var itemSaleType = message.reader().readByte();
        var itemSaleIndex = message.reader().readShort();
        var itemSaleInfo = message.reader().readUTF();
    }

    private void OpenShop(Message message)
    {
        int type = message.reader().readByte();//type shop
        int cout39 = message.reader().readByte();// số shop con

        for (int i = 0; i < cout39; i++)
        {
            var nameshop = message.reader().readUTF().Replace("\r\n", " ");
            int countItemInShop = message.reader().readByte();
            for (int j = 0; j < countItemInShop; i++)
            {
                message.reader().readShort();//iditem
                message.reader().readByte();//idtemplate
                message.reader().readUTF();//name
                message.reader().readUTF();//des
                message.reader().readLong(); //power//power
                message.reader().readInt();//yen
                message.reader().readInt();//xu
                message.reader().readInt();//luong
                message.reader().readBool();//muanhieu
                int countItemOp = message.reader().readByte();//countitemop
                for (int k = 0; k < countItemOp; k++)
                {
                    message.reader().readShort();//opId
                    message.reader().readUTF();//nameop
                    message.reader().readInt();//param
                    message.reader().readByte();//color
                }
                message.reader().readShort();//level
            }
        }
    }

    private void DeleteItem(Message message)
    {
        message.reader().readShort();//indexitem
    }

    private void OpenUIZone(Message message)
    {
        int countZone = message.reader().readByte();//zonecount
        for (int i = 0; i < countZone; i++)
        {
            message.reader().readByte();//ZoneId
            message.reader().readByte();//typeID
            message.reader().readByte();//numCharZOne
            message.reader().readByte();//Charmax
            message.reader().readByte();//rank
        }
    }

    private void ConfirmMenu(Message message)
    {
        short npcIDDialog = message.reader().readShort();
        string mainDescription = message.reader().readUTF();
        string[] subDialogOption = new string[message.reader().readByte()];
        for (int j = 0; j < subDialogOption.Length; j++)
        {
            subDialogOption[j] = message.reader().readUTF();
        }
    }

    private void ShowInput(Message message)
    {
        string textTitle = message.reader().readUTF();
        var textIpCount = message.reader().readByte();
        for (int i = 0; i < textIpCount; i++)
        {
            var ipPlaceHolder = message.reader().readUTF();
            var contentType = message.reader().readByte();
        }
    }

    private void OpenChest(Message message)
    {
        try
        {
            message.reader().readInt();//boxLenght
            int countBox = message.reader().readInt();//So itembox
            for (int i = 0; i < countBox; i++)
            {
                message.reader().readShort();//idtem
                message.reader().readInt();//indexUI
                message.reader().readInt();//quaility
                message.reader().readUTF();//name
                message.reader().readUTF();//des
                int type52 = message.reader().readByte();//type
                message.reader().readLong();//requore
                int countop52 = message.reader().readByte();//countop
                for (int ie = 0; ie < countop52; ie++)
                {
                    message.reader().readShort();//idop
                    message.reader().readUTF();//nameop
                    message.reader().readInt();//paramop
                    message.reader().readByte();//colorOP
                }
                int level52 = message.reader().readShort();//level
                if (type52 <= 10)
                {

                    int countlevel = message.reader().readByte();//count
                    for (int ia = 0; ia < countlevel; i++)
                    {
                        message.reader().readUTF();//info
                    }

                }
            }
        }
        catch (Exception e) { }
        
    }

    private void MainDie(Message message)
    {
        try
        {
            message.reader().readByte();
            message.reader().readFloat();
            message.reader().readFloat();
            message.reader().readLong();
        }
        catch (Exception e) { }
        
    }

    private void TradeItem(Message message)
    {
        try
        {
            int action = message.reader().readByte();
            switch (action)
            {
                case 0:// loi moi

                    int charid = message.reader().readInt();
                    break;
                case 1:// hien UI giao dich 2 nguoi
                    int charid1 = message.reader().readInt();
                    break;
                case 2://vatj pham ko giao dich
                    message.reader().readShort();// index ui
                    message.reader().readInt();// Quantity
                    break;
                case 3://hien all vat pham
                    message.reader().readInt();//gold
                    int count55 = message.reader().readByte();//so vat pham
                    for (int i = 0; i < count55; i++)
                    {
                        message.reader().readShort();//id
                        message.reader().readUTF();//name
                        message.reader().readUTF();//des
                        message.reader().readInt();//quantity
                        int countop = message.reader().readByte();//countOP
                        for (int j = 0; j < countop; j++)
                        {
                            message.reader().readShort();//idop
                            message.reader().readUTF();//nameop
                            message.reader().readInt();//param
                            message.reader().readByte();//color
                        }

                    }
                    break;
                default:
                    break;


            }
        }
        catch (Exception e) { }
       
    }
}