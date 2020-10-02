using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClientTepldate
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var tcpClient = new ClientUDP();
      while (true)
      {
        var message = Console.ReadLine();
        if (message == "ex")
          break;
        var answer = tcpClient.Ask(message);
        Console.WriteLine(answer);
      }
      Console.ReadLine();
    }
  }

  public class ClientTCP
  {
    const string ip = "127.0.0.1";
    const short port = 8080;
    private IPEndPoint _tcpEndPoint;
    private Socket _tcpSocket;

    public string Ask(string message)
    {
      _tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      _tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

      var data = Encoding.ASCII.GetBytes(message);
      _tcpSocket.Connect(_tcpEndPoint);
      _tcpSocket.Send(data);

      var buffer = new byte[256];

      var sizeOfBuffer = 0;
      var answer = new StringBuilder();

      do
      {
        sizeOfBuffer = _tcpSocket.Receive(buffer);
        answer.Append(Encoding.ASCII.GetString(buffer, 0, sizeOfBuffer));

      } while (_tcpSocket.Available > 0);

      _tcpSocket.Shutdown(SocketShutdown.Both);
      _tcpSocket.Close();
      return answer.ToString();
    }
  }

  public class ClientUDP
  {
    const string ip = "127.0.0.1";
    const short port = 8082;
    private IPEndPoint _udpEndPoint;
    private Socket _udpSocket;
    private EndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

    public string Ask(string message)
    {
      _udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
      _udpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);

      _udpSocket.SendTo(Encoding.ASCII.GetBytes(message), serverEndPoint);
      var data = Encoding.ASCII.GetBytes(message);
      _udpSocket.Connect(_udpEndPoint);
      _udpSocket.Send(data);

      var buffer = new byte[256];

      var sizeOfBuffer = 0;
      var answer = new StringBuilder();

      do
      {
        sizeOfBuffer = _udpSocket.Receive(buffer);
        answer.Append(Encoding.ASCII.GetString(buffer, 0, sizeOfBuffer));

      } while (_udpSocket.Available > 0);

      _udpSocket.Shutdown(SocketShutdown.Both);
      _udpSocket.Close();
      return answer.ToString();
    }
  }
}
