using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public BodyPart BodyPart;

    public void OnDamagingCollision(Collision collision, float damage)
    {
        ContactPoint contact = collision.contacts[0];
        float collisionVel = collision.relativeVelocity.sqrMagnitude;
        Vector3 contactPoint = contact.point;
        
        //Debug.Log("Vel: " + collisionVel + " Contacts: " + collision.contacts.Length + " Point:" + contactPoint);
        //Debug.DrawRay(contact.point, contact.normal, Color.white);
        float hitDamage = (collisionVel * damage);
        //Debug.Log(hitDamage);
        BodyPart.HealthPoints -= hitDamage;
    }
}