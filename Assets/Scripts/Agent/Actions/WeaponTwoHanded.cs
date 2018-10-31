using UnityEngine;

public class WeaponTwoHanded : MonoBehaviour
{
    private Rigidbody weaponRb;
    public Rigidbody rHandPlaceRb;
    public Rigidbody lHandPlaceRb;

    void Start()
    {
        weaponRb = GetComponent<Rigidbody>();
        foreach (Rigidbody child in GetComponentsInChildren<Rigidbody>())
        {
            if (child.name.Contains("rhand")) rHandPlaceRb = child;
            if (child.name.Contains("lhand")) lHandPlaceRb = child;
        }
    }
}