using UnityEngine;

public class BallController : MonoBehaviour
{
    public float ballSpeed = 5f;
    private Vector3 direction;

    public void Init()
    {
        direction = new Vector3(1f, 1f, 0f).normalized;
    }

    private void Update()
    {
        transform.Translate(direction * ballSpeed * Time.deltaTime);
    }
}