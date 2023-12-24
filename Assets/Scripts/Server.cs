using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class Server
{
    private TcpListener tcpListener;
    private List<ClientHandler> clients = new List<ClientHandler>();

    public void StartServer(int port)
    {
        tcpListener = new TcpListener(IPAddress.Any, port);
        tcpListener.Start();

        Console.WriteLine($"Server started on port {port}");

        Thread listenerThread = new Thread(new ThreadStart(ListenForClients));
        listenerThread.Start();
    }

    private void ListenForClients()
    {
        while (true)
        {
            TcpClient tcpClient = tcpListener.AcceptTcpClient();

            ClientHandler clientHandler = new ClientHandler(tcpClient, this);
            clients.Add(clientHandler);

            Thread clientThread = new Thread(new ThreadStart(clientHandler.HandleClient));
            clientThread.Start();
        }
    }

    public void BroadcastMessage(string message, ClientHandler excludeClient = null)
    {
        foreach (var client in clients)
        {
            if (client != excludeClient)
            {
                client.SendMessage(message);
            }
        }
    }

    public void RemoveClient(ClientHandler client)
    {
        clients.Remove(client);
    }
}
