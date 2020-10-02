using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServerTemplate
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var server = new ServerTCP();
      server.Listen();
      Console.WriteLine("Hello World!");
    }
  }

  public class ServerTCP
  {
    const string ip = "127.0.0.1";
    const short port = 8080;
    private IPEndPoint _tcpEndPoint;
    private Socket _tcpSocket;

    public void Listen()
    {
      _tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      _tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

      _tcpSocket.Bind(_tcpEndPoint);
      _tcpSocket.Listen(5);

      while (true)
      {
        var listener = _tcpSocket.Accept();
        var buffer = new byte[256];

        var sizeOfBuffer = 0;
        var data = new StringBuilder();

        do
        {
          sizeOfBuffer = listener.Receive(buffer);
          data.Append(Encoding.ASCII.GetString(buffer, 0, sizeOfBuffer));

        } while (listener.Available > 0);

        Console.WriteLine(data.ToString());

        listener.Send(Encoding.ASCII.GetBytes("Dоne!"));

        listener.Shutdown(SocketShutdown.Both);
        listener.Close();
      }
    }
  }

  public class ServerUDP
  {
    const string ip = "127.0.0.1";
    const short port = 8081;
    private IPEndPoint _udpEndPoint;
    private Socket _udpSocket;

    public void Listen()
    {
      _udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
      _udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

      _udpSocket.Bind(_udpEndPoint);
      _udpSocket.Listen(5);

      while (true)
      {
        var buffer = new byte[256];

        var sizeOfBuffer = 0;
        var data = new StringBuilder();

        EndPoint senderEndPoint = new IPEndPoint(IPAddress.Any, 0);

        do
        {
          sizeOfBuffer = _udpSocket.ReceiveFrom(buffer, ref senderEndPoint);
          data.Append(Encoding.ASCII.GetString(buffer));

        } while (_udpSocket.Available > 0);

        _udpSocket.SendTo(Encoding.ASCII.GetBytes("ewq"), senderEndPoint);

        Console.WriteLine(data.ToString());

      }
    }
  }
}
