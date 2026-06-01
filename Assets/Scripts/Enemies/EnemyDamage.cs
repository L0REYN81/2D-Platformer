using UnityEngine;

//*Этот код наносит игроку урон от Шипов
public class EnemyDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            collision.GetComponent<Health>().TakeDamage(damage);
    }
}
