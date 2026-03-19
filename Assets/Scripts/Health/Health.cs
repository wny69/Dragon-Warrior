using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] public float startingHealth;
    public float currentHealth { get; internal set; }

    private Animator anim;
    private bool dead;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
            anim.SetTrigger("hurt");
        else
        {
            if (!dead)
            {
                dead = true;
                anim.SetTrigger("die");
                GetComponent<Collider2D>().enabled = false;

                GameOverScreen gameOver = FindFirstObjectByType<GameOverScreen>(FindObjectsInactive.Include);
                if (gameOver != null)
                    gameOver.ShowGameOver();
            }
        }
    }

    public void Respawn()
    {
        dead = false;
        currentHealth = startingHealth;
        anim.ResetTrigger("die");
        anim.SetTrigger("respawn");
        GetComponent<Collider2D>().enabled = true;
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
}