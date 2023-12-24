using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;

public class NetworkManager : MonoBehaviour
{
    private TcpClient socketConnection;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;
    private Server server;

    public static NetworkManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        server = new Server();
        server.StartServer(12345);

        ConnectToServer("localhost", 12345);
    }

    public void ConnectToServer(string host, int port)
    {
        try
        {
            socketConnection = new TcpClient(host, port);
            stream = socketConnection.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            Debug.Log("Connected to the server");

            StartClient();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error: {e.Message}");
        }
    }

    private void StartClient()
    {
        System.Threading.Thread clientThread = new System.Threading.Thread(new System.Threading.ThreadStart(ListenForMessages));
        clientThread.Start();
    }

    private void ListenForMessages()
    {
        try
        {
            while (true)
            {
                string receivedMessage = reader.ReadLine();
                ProcessMessage(receivedMessage);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading message: {e.Message}");
        }
    }

    private void ProcessMessage(string message)
    {
        GameController.Instance.ProcessNetworkMessage(message);
    }

    public void SendPlayerInput(float inputY)
    {
        SendMessage($"Position|{inputY}");
    }

    private void SendMessage(string message)
    {
        if (writer != null)
        {
            writer.WriteLine(message);
            writer.Flush();
        }
    }

    private void OnDestroy()
    {
        writer.Close();
        reader.Close();
        socketConnection.Close();
    }

    public void ReadMessage()
    {
        try
        {
            string receivedMessage = reader.ReadLine();
            // Process received message
            ProcessMessage(receivedMessage);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading message: {e.Message}");
        }
    }
}
