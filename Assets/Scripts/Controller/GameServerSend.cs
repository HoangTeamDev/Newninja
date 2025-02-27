using System;
using System.Collections.Generic;
using UnityEngine;
public partial class GameServer
{
  
    public void Login(string username, string pass, string version, sbyte type)
    {
        Message message = new Message((short)0);
        message.writer().writeShort(0);
        message.writer().writeUTF(username);
        message.writer().writeUTF(pass);
        message.writer().writeUTF(version);
        message.writer().writeByte(type);
        session.SendMessage(message);
    }

    public void CreateChar(string name, int gender, int hair)
    {
        Message message = new Message((short)(1));
        message.writer().writeShort((sbyte)2);
        message.writer().writeUTF(name);
        message.writer().writeByte(gender);
        message.writer().writeByte(hair);
        session.SendMessage(message);
    }
    public void clientOk()
    {
        Message message = Message.CreateMessNotMap((sbyte)2);
        session.SendMessage(message);
    }
   

    public void BuyItem(int buyType, int itemID, int quantity)
    {
        Message message = null;
        try
        {
            message = new Message((short)(37));
            message.writer().writeByte(buyType);
            message.writer().writeShort(itemID);
            message.writer().writeShort(quantity);
            if (message != null)
            {
                session.SendMessage(message);
            }
        }
        catch (Exception)
        {
        }
        finally
        {
            message.cleanup();
        }
    }

    public void SellItem(sbyte saleAction, byte itemType, short itemIndexUI)//0 hoi co banko, 1 dong y ban
    {
        Message message = null;
        try
        {
            message = new Message((short)38);
            message.writer().writeByte(saleAction);
            message.writer().writeByte(itemType);
            message.writer().writeShort(itemIndexUI);
            session.SendMessage(message);
        }
        catch (Exception)
        {
        }
        finally
        {
            message.cleanup();
        }
    }
    public void PickItem(int itemMapId)
    {
        try
        {
            Message message = new Message((short)(41));
            message.writer().writeShort(itemMapId);
            session.SendMessage(message);
        }
        catch
        {

        }
       
    }
    public void ThrowItem(short indexUI)
    {
        Message message = null;
        try
        {
            message = new Message((short)40);
            message.writer().writeByte(1);          
            message.writer().writeShort(indexUI);
            session.SendMessage(message);
        }
        catch (Exception)
        {

        }
    }
    public void UseItem(short indexUI)
    {
        Message message = null;
        try
        {
            message = new Message((short)40);
            message.writer().writeByte(2);
            message.writer().writeShort(indexUI);
            session.SendMessage(message);
        }
        catch (Exception)
        {

        }
    }
    public void Charmove(byte key, short dir)
    {
        Message message = null;
        try
        {
            message = new Message((short)9);
            message.writer().writeByte(key);
            message.writer().WriteFloat(GameController.Instance._mainCharacter.transform.position.x);
            message.writer().WriteFloat(GameController.Instance._mainCharacter.transform.position.y);
            message.writer().writeShort(dir);
            session.SendMessage(message);
        }
        catch (Exception)
        {

        }
    }
    public void RequestChangeMap()
    {
        Message message = new Message((short)(43));
        session.SendMessage(message);
        message.cleanup();
       
    }
    public void CallZone()
    {
        Message message = new Message((short)(44));
    }
    public void RequestChangeZone(byte zoneId)
    {
        Message message = new Message((short)(45));
        message.writer().writeByte(zoneId);
        session.SendMessage(message);
        message.cleanup();

    }
    public void OpenMenu(int npcId)
    {
        Message message = null;
        try
        {
            message = new Message((short)46);
            message.writer().writeShort(npcId);
            session.SendMessage(message);
        }
        catch (Exception)
        {

        }
    }
    public void ConfirmMenu(short npcID, sbyte select)
    {
        //Res.outz("confirme menu" + select);
        Message message = null;
        try
        {
            message = new Message((short)51);
            message.writer().writeShort(npcID);
            message.writer().writeByte(select);
            session.SendMessage(message);
        }
        catch (Exception)
        {

        }
        finally
        {
            message.cleanup();
        }
    }
    public void LiveFromDead()//hoi sinh ngoc
    {
        Message message = null;
        try
        {
            message = new Message((short)11);         
            session.SendMessage(message);
        }
        catch (Exception)
        {

        }
    }
    public void BackHome()
    {
        Message message = null;
        try
        {
            message = new Message((short)50);
            session.SendMessage(message);
        }
        catch (Exception)
        {

        }
    }
    public void ChatZone(string text)
    {
        Message message = null;
        try
        {
            message = new Message((short)28);
            message.writer().writeUTF(text);
            session.SendMessage(message);

        }
        catch (Exception)
        {

        }
        finally
        {
            message.cleanup();
        }
    }

    public void SendUp(int typePotential, int num)//0 is hp, 1 is mp, 2 is atk, 3 is nhanh nhẹn
    {
        Message message = null;
        try
        {
            message = new Message((short)8);
            message.writer().writeShort(9);
            message.writer().writeByte(typePotential);
            message.writer().writeShort(num);
            session.SendMessage(message);

        }
        catch { }
    }
    public void WorldChat(string text)
    {
        Message message = null;
        try
        {
            message = new Message((short)34);
            message.writer().writeUTF(text);
            session.SendMessage(message);

        }
        catch (Exception)
        {

        }
        finally
        {
            message.cleanup();
        }
    }
    public void RequestEquipItem(int type, int indexUI)
    {
        Message message = null;
        try
        {
            message = new Message(51);
            message.writer().writeByte(type);
            message.writer().writeShort(indexUI);
            session.SendMessage(message);
        }
        catch (Exception)
        {
        }
        finally
        {
            message.cleanup();
        }
    }
    public void GetItem(byte type,int indexUI)//0 from chest to bag,1 from bag to chest/ 3 lay do tu body xuong/
    {
        Message message = null;
        try
        {
            message = new Message(53);
            message.writer().writeByte(type);
            message.writer().writeShort(indexUI);
            session.SendMessage(message);
        }
        catch (Exception)
        {
        }
    }
    public void TradeItem(sbyte action, int playerID, sbyte index, int num)
    {
        Message message = null;
        try
        {
            message = new Message((short)(55));
            message.writer().writeByte(action);
            switch (action)
            {
                case 0 or 1://o gui loi moi, 1 dong y
                    message.writer().writeInt(playerID);
                    break;
                case 2:// them item vao giao dich
                    message.writer().writeShort(index);//indexUI
                    message.writer().writeInt(num);//quality
                    break;
                case 3://huy

                    break;
                case 4:// huy item luc giao dixh
                    message.writer().writeShort(index);//indexui
                    break;
                case 5://khoa giao dich
                    break;
                case 6://hoan thanh giao dich
                    break;
            }
           
            session.SendMessage(message);
        }
        catch (Exception)
        {
        }
        finally
        {
            message.cleanup();
        }
    }


   





}
