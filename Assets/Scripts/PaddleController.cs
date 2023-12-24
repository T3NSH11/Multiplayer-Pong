using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public float paddleSpeed = 5f;
    private bool isLeftPaddle;

    public void Init(bool isLeft)
    {
        isLeftPaddle = isLeft;
    }

    private void Update()
    {
        float inputY = isLeftPaddle ? Input.GetAxis("Vertical") : 0f;

        transform.Translate(Vector3.up * inputY * paddleSpeed * Time.deltaTime);
    }

    public void UpdatePosition(float posY)
    {
        transform.position = new Vector3(transform.position.x, posY, transform.position.z);
    }
}