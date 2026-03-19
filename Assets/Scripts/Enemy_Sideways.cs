using UnityEngine;

public class Enemy_Sideways : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float moveDistance;

    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;
    private bool isStopped;
    private float stopTimer;

    private void Start()
    {
        leftEdge = transform.position.x - moveDistance;
        rightEdge = transform.position.x + moveDistance;
    }

    private void Update()
    {
        if (isStopped)
        {
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0)
                isStopped = false;
            return;
        }

        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
                transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            else
                movingLeft = false;
        }
        else
        {
            if (transform.position.x < rightEdge)
                transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            else
                movingLeft = true;
        }
    }

    public void StopEnemy()
    {
        isStopped = true;
        stopTimer = 4f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isStopped)
            collision.GetComponent<Health>().TakeDamage(damage);
    }
}