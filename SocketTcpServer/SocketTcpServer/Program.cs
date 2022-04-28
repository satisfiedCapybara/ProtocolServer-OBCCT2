using System;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace SocketTcpServer
{
  class Program
  {
    static void Main(string[] args)
    {
      int aPort = 8005;
      string aHost = Dns.GetHostName();
      Console.WriteLine("Host: " + aHost);

      IPAddress[] IPs;
      IPs = Dns.GetHostAddresses(aHost);
      foreach (IPAddress ip1 in IPs)
      {
        Console.WriteLine(ip1);
      }

      IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), aPort);

      Socket aListenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      try
      {
        aListenSocket.Bind(ipPoint);
        aListenSocket.Listen(10);

        StringBuilder aBuilder = new StringBuilder("");
        int aBytes = 0;
        int aNumberBytes = 0;
        byte[] aData = new byte[255];

        Socket aSocketHandler = aListenSocket.Accept();

        while (true)
        {
          if (!aSocketHandler.Connected)
          {
            aSocketHandler = aListenSocket.Accept();
          }


          aBuilder = new StringBuilder("");

          do
          {
            aBytes = aSocketHandler.Receive(aData);
            aBuilder.Append(Encoding.Unicode.GetString(aData, 0, aBytes));
            aNumberBytes += aBytes;
          }
          while (aSocketHandler.Available > 0);

          Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + aBuilder.ToString());
          Console.WriteLine(aNumberBytes + "bytes\n");

          byte[] anArrayBytes = Encoding.UTF8.GetBytes("Server response");

          aSocketHandler.Send(anArrayBytes);

          if (aBuilder.ToString() == "shutdown")
          {
            aSocketHandler.Shutdown(SocketShutdown.Both);
            aSocketHandler.Close();
          }
        }

      }
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }
    }
  }
}
