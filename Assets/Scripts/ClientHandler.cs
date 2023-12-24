using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

public class ClientHandler
{
    private TcpClient tcpClient;
    private StreamReader reader;
    private StreamWriter writer;
    private Server server;

    public ClientHandler(TcpClient clientSocket, Server serverReference)
    {
        tcpClient = clientSocket;
        server = serverReference;
        reader = new StreamReader(clientSocket.GetStream());
        writer = new StreamWriter(clientSocket.GetStream());
    }

    public void HandleClient()
    {
        try
        {
            while (true)
            {
                string message = reader.ReadLine();

                if (message == null)
                    break;

                server.BroadcastMessage(message, this);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error handling client: {e.Message}");
        }
        finally
        {
            server.RemoveClient(this);
            tcpClient.Close();
        }
    }

    public void SendMessage(string message)
    {
        writer.WriteLine(message);
        writer.Flush();
    }
}
