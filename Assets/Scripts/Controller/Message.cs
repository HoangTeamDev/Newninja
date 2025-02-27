public class Message
{
    public static Message CreateMessNotLogin(short comment)
    {
        Message message = new Message((short)0);
        message.writer().writeShort(comment);
        return message;
    }

    public static Message CreateMessNotMap(short comment)
    {
        Message message = new Message((short)-28);
        message.writer().writeShort(comment);
        return message;
    }

    public short command;

    private myReader dis;

    private myWriter dos;

    public Message(int command)
    {
        this.command = (short)command;
        dos = new myWriter(this.command);
    }

    public Message(short command)
    {
        this.command = command;
        dos = new myWriter(command);
    }

    public Message(short command, sbyte[] data)
    {
        this.command = command;
        dis = new myReader(data);
    }

    public sbyte[] getData()
    {
        return dos.getData();
    }

    public myReader reader()
    {
        return dis;
    }

    public myWriter writer()
    {
        return dos;
    }
    public int readInt3Byte()
    {
        return dis.readInt();
    }

    public void cleanup()
    {

    }
}
