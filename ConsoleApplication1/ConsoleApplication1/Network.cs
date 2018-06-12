using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

using NetComm;
using RLNET;

namespace ConsoleApplication1
{
    public class NetObject
    {
        public bool isChanged = false;

        public string changedInfo = string.Empty;

        public int ID = -1;

        public NetObject()
        {
            NetContainer.Add(this);
        }

        public void Logic()
        {

        }

        public void NetLogic()
        {
            if (this.isChanged)
            {
                if (Program.isServer)
                {
                    this.changedInfo = ID.ToString() + ": " + changedInfo;
                    Server.SendDataToAllUsers(changedInfo);
                }
                else
                {
                    Client.SendData(changedInfo);
                }
                this.changedInfo = string.Empty;
                this.isChanged = false;
            }
        }
    }

    public static class Network
    {
        public static string ConvertBytesToString(byte[] bytes)
        {
            return ASCIIEncoding.ASCII.GetString(bytes);
        }

        public static byte[] ConvertStringToBytes(string str)
        {
            return ASCIIEncoding.ASCII.GetBytes(str);
        }

        public static string MiniParce(ref string str, char border)
        {
            string result = string.Empty;
            //string strCopy = str;
            while (str[0] != border)
            {
                result += str[0];
                str = str.Remove(0, 1);
            }
            str = str.Remove(0, 1);
            return result;
        }
    }

    public class Server
    {
        public static NetComm.Host server;

        private List<ClientPlayer> connectedPlayers = new List<ClientPlayer>();

        public void Init()
        {
            server = new Host(8080);
            server.StartConnection();

            server.NoDelay = true;
            server.ReceiveBufferSize = 100;
            server.SendBufferSize = 100;

            server.onConnection += Server_onConnection;
            server.lostConnection += Server_lostConnection;
            server.DataReceived += Server_DataReceived;

            Console.WriteLine("Сервер запущен. Ожидание подключений...");
        }

        private void Server_DataReceived(string ID, byte[] Data)
        {
            //Console.WriteLine(ID + ": " + Network.ConvertBytesToString(Data));
            string received = Network.ConvertBytesToString(Data);
            if (received == "getAllCreatures")
            {
                string data = NetContainer.GetAllCreatures(ID);
                server.SendData(ID, Network.ConvertStringToBytes(data));
            }
            else
            {
                foreach (var player in connectedPlayers)
                {
                    if (player.playerId == ID)
                    {
                        player.ParceFromServer(Network.ConvertBytesToString(Data));
                        break;
                    }
                }
            }
        }

        private void Server_lostConnection(string id)
        {
            foreach (var player in connectedPlayers)
            {
                if (player.playerId == id)
                {
                    connectedPlayers.Remove(player);
                    CreaturesContainer.Remove(player);
                }
            }
        }

        private void Server_onConnection(string id)
        {
            Console.WriteLine(id + " is connected!");
            //throw new NotImplementedException();

            this.connectedPlayers.Add(new ClientPlayer((char)2, Vector.One * 20, RLColor.Red, id));
        }

        public void Logic()
        {


        }

        public static void SendDataToAllUsers(string data)
        {
            server.Brodcast(Network.ConvertStringToBytes(data));
        }
    }


    public class Client
    {
        public static NetComm.Client client;

        private string name = "_Dismas";

        private List<NetObject> creatures = new List<NetObject>();

        public void Init()
        {
            client = new NetComm.Client();
            client.Connect("127.0.0.1", 8080, name);

            client.NoDelay = true;
            client.ReceiveBufferSize = 100;
            client.SendBufferSize = 100;

            client.DataReceived += Client_DataReceived;
            client.Connected += Client_Connected;
        }

        private void Client_Connected()
        {
            string message = "getAllCreatures";
            client.SendData(Network.ConvertStringToBytes(message));
        }

        private void Client_DataReceived(byte[] Data, string ID)
        {
            string responce = Network.ConvertBytesToString(Data);
            Console.WriteLine(responce + " is recieved!");
            if (responce.Contains("gac"))
            {
                responce = responce.Replace("gac \n", "");
                var list = responce.Split('\n');
                
                foreach (var item in list)
                {
                    if (item=="")
                    {
                        continue;
                    }
                    string itemCopy = item;
                    bool isMe = false;
                    if (itemCopy[0] == 'p')
                    {
                        isMe = true;
                        itemCopy = itemCopy.Remove(0, 1);
                    }
                    int Id = Convert.ToInt32(Network.MiniParce(ref itemCopy, ':'));
                    int x = Convert.ToInt32(Network.MiniParce(ref itemCopy, ','));
                    int y = Convert.ToInt32(Network.MiniParce(ref itemCopy, ','));
                    itemCopy = itemCopy.Remove(0, 1);
                    char sym = itemCopy[0];//Convert.ToChar(itemCopy);

                    if (isMe)
                    {
                        var player = new Player(sym, new Vector(x, y), RLColor.White);
                        player.ID = Id;
                    }
                    else
                    {
                        var crea = new Creature(sym, new Vector(x, y), RLColor.White);
                        crea.ID = Id;
                    }
                }
            }
            else if (responce.Contains("dead"))
            {
                string copy = responce;
                int Id = Convert.ToInt32(Network.MiniParce(ref copy, ':'));

                var crea = CreaturesContainer.GetCreature(Id);
                if (crea!=null)
                {
                    CreaturesContainer.Remove(crea);
                    crea = null;
                }
            }
            else 
            {
                string copy = responce;
                int Id = Convert.ToInt32(Network.MiniParce(ref copy, ':'));
                int x = Convert.ToInt32(Network.MiniParce(ref copy, ','));
                int y = Convert.ToInt32(Network.MiniParce(ref copy, ';'));

                var crea = CreaturesContainer.GetCreature(Id);
                if (crea!=null)
                {
                    crea.UpdateData(new Vector(x, y));
                }
            }
        }



        public void Logic()
        {
            //string message = Console.ReadLine();

            //if (!string.IsNullOrEmpty(message))
            //{
            //    client.SendData(Network.ConvertStringToBytes(message));
            //}
        }

        public static void SendData(string data)
        {
            if (!string.IsNullOrEmpty(data))
            {
                client.SendData(Network.ConvertStringToBytes(data));
            }
        }
    }

    public class ClientPlayer : Creature
    {
        public string playerId = string.Empty;

        public ClientPlayer(char s, Vector pos, RLColor c, string plrId) : base(s, pos, c)
        {
            this.playerId = plrId;

            this.movingDelay = 2;
        }

        public void ParceFromServer(string data)
        {
            string dataCopy = data;
            while (!string.IsNullOrEmpty(dataCopy))
            {
                string section = string.Empty;
                while (dataCopy.Length != 0)
                {
                    if (dataCopy[0] == ';')
                    {
                        dataCopy = dataCopy.Remove(0, 1);
                        break;
                    }
                    section += dataCopy[0];
                    dataCopy = dataCopy.Remove(0, 1);
                }

                if (section.Contains("ttm"))
                {
                    section = section.Replace("ttm:", "");
                    string a1 = string.Empty;
                    while (section[0] != ',')
                    {
                        a1 += section[0];
                        section = section.Remove(0, 1);
                    }
                    string a2 = section.Remove(0, 1);

                    this.TryToMove(new Vector(Convert.ToInt32(a1), Convert.ToInt32(a2)));
                }
                else if (section.Contains("tta"))
                {
                    section = section.Replace("tta:", "");
                    string a1 = string.Empty;
                    while (section[0] != ',')
                    {
                        a1 += section[0];
                        section = section.Remove(0, 1);
                    }
                    string a2 = section.Remove(0, 1);

                    this.TryToAttack(new Vector(Convert.ToInt32(a1), Convert.ToInt32(a2)));
                }
            }
        }

        public override void TryToAttack(Vector dir)
        {
            this.isChanged = true;
            this.changedInfo += "as;";

            base.TryToAttack(dir);
        }
    }

}

