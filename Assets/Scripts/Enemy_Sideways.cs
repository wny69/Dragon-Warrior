using UnityEngine;

public class Enemy_Sideways : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float moveDistance;

    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;

    private void Start()
    {
        leftEdge = transform.position.x - moveDistance;
        rightEdge = transform.position.x + moveDistance;
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x > leftEdge) // ← was < (inverted)
                transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            else
                movingLeft = false; // ← was outside the if block
        }
        else
        {
            if (transform.position.x < rightEdge)
                transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            else
                movingLeft = true; // ← braces were completely broken here
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<Health>().TakeDamage(damage);
    }
}