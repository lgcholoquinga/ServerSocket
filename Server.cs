using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Serialization;
using Auth;

namespace Server
{
    public class Server
    {
        IPHostEntry host;
        IPAddress ipAdress;
        IPEndPoint endPoint;

        Socket s_Server;
        Socket s_Client;
        public Server(string ip,int port)
        {
            host = Dns.GetHostEntry(ip);
            ipAdress = host.AddressList[0];
            endPoint = new IPEndPoint(ipAdress, port);
            //Init Server
            s_Server = new Socket(ipAdress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            s_Server.Bind(endPoint);
            s_Server.Listen(10);
        }
        public void Start()
        {
            Thread t;
            while (true)
            {
                Console.WriteLine("Waiting for Connections...");
                s_Client = s_Server.Accept();
                t = new Thread(ClientConnection);
                t.Start(s_Client);
                Console.WriteLine("New Simulator Connected...");
                //Console.Out.Flush();
            }
        }
        public void ClientConnection(object s)
        {
            Socket s_Client = (Socket)s;
            byte[] buffer;
            User user;
            try
            {
                while (true)
                {
                    buffer = new byte[1024];
                    s_Client.Receive(buffer);
                    user = (User)BinarySerialization.Deserializate(buffer);

                    if(user.user == "admin" && user.password == "admin")
                    {
                        byte[] toSend = Encoding.ASCII.GetBytes("success");
                        s_Client.Send(toSend);
                    }
                    else
                    {
                        byte[] toSend = Encoding.ASCII.GetBytes("fail");
                        s_Client.Send(toSend);
                    }

                    Console.WriteLine("User Recieved:" + user.user);
                    Console.WriteLine("Password Recieved:" + user.password);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Simulator Disconnected : {0}", ex.Message);
            }
        }
        public void Send(string message)
        {
            byte[] response = Encoding.ASCII.GetBytes(message);
            s_Client.Send(response);
        }
        //Convert Byte to String
        public string byte2String(byte[] buffer)
        {
            string message;
            int endIndex;
            message = Encoding.ASCII.GetString(buffer);
            endIndex = message.IndexOf('\0');
            if (endIndex > 0)
                message = message.Substring(0, endIndex);
            return message;
        }
    }
}
