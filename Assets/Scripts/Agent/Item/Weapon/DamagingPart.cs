using UnityEngine;

public class DamagingPart : MonoBehaviour
{
    public float damage;
    public bool damaging = true;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("hitbox") && damaging)
        {
            Hitbox hitbox = collision.collider.GetComponent<Hitbox>();
            if (hitbox != null)
            {
                hitbox.OnDamagingCollision(collision, damage);
            }
        }
    }
}