using UnityEngine;

public class DamagingPart : MonoBehaviour
{
    public float damage;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("hitbox"))
        {
            Hitbox hitbox = collision.collider.GetComponent<Hitbox>();
            if (hitbox != null)
            {
                hitbox.OnDamagingCollision(collision, damage);
            }
        }
    }
}