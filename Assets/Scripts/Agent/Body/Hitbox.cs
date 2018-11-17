using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public BodyPart BodyPart;
    //public static Pool<HitNumber> HitNumberPool;

    private void Start()
    {
        //HitNumberPool = ObjectsPool.getPool<HitNumber>((item) => { }, (item) => { });
    }

    public void OnDamagingCollision(Collision collision, float damage)
    {
        ContactPoint contact = collision.contacts[0];
        float collisionVel = Mathf.Min(collision.relativeVelocity.sqrMagnitude / 10f, 1f);
        Vector3 contactPoint = contact.point;

        //Debug.Log("Vel: " + collisionVel + " Contacts: " + collision.contacts.Length + " Point:" + contactPoint);
        //Debug.DrawRay(contact.point, contact.normal, Color.white);
        float hitDamage = (collisionVel * damage);
        //HitNumber hitNumber = HitNumberPool.Pop();
        //hitNumber.Animate(hitDamage, contactPoint, gameObject.layer == LayerMask.NameToLayer("Player2"));
        //Debug.Log(hitDamage);
        BodyPart.HealthPoints -= hitDamage;
    }
}