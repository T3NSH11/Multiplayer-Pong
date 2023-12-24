// GameController.cs

using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject paddlePrefab;
    public GameObject ballPrefab;
    private NetworkManager networkManager;

    public static GameController Instance { get; private set; }

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
        networkManager = GetComponent<NetworkManager>();

        networkManager.ConnectToServer("localhost", 12345);

        GameObject leftPaddle = Instantiate(paddlePrefab, new Vector3(-5f, 0f, 0f), Quaternion.identity);
        GameObject rightPaddle = Instantiate(paddlePrefab, new Vector3(5f, 0f, 0f), Quaternion.identity);
        GameObject ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);

        PaddleController leftPaddleController = leftPaddle.GetComponent<PaddleController>();
        PaddleController rightPaddleController = rightPaddle.GetComponent<PaddleController>();
        BallController ballController = ball.GetComponent<BallController>();

        leftPaddleController.Init(true);
        rightPaddleController.Init(false);
        ballController.Init();
    }

    private void Update()
    {
        float inputY = Input.GetAxis("Vertical");
        networkManager.SendPlayerInput(inputY * Time.deltaTime);

        networkManager.ReadMessage();
    }

    public void ProcessNetworkMessage(string message)
    {
        string[] data = message.Split('|');

        switch (data[0])
        {
            case "Position":
                UpdatePaddlePosition(data[1], float.Parse(data[2]));
                break;
        }
    }

    private void UpdatePaddlePosition(string paddleTag, float posY)
    {
        GameObject paddle = GameObject.FindGameObjectWithTag(paddleTag);

        if (paddle != null)
        {
            PaddleController paddleController = paddle.GetComponent<PaddleController>();
            paddleController.UpdatePosition(posY);
        }
    }
}
