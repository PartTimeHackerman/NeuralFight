using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

class Cannon : Obstacle
{

    private ConfigurableJoint joint;
    public Rigidbody target;
    public float shootSpeed = 0f;
    public float shootSpeedMult = .5f;
    public Transform barrelEnd;
    public GameObject barrel;
    private CapsuleCollider barrelCollider;
    public ParticleSystem shootParticles;
    private SpriteRenderer barrelSprite;
    public SpriteRenderer ammoSprite;
    public static Pool<CannonBall> cannonBallPool;
    private float nextTime = 1f;
    public float interval = 1f;
    public float size;
    public bool reset = false;
    public float cannonBallMass = 10f;
    public bool shoot;
    private int ammo = 0;
    private int maxAmmo = 0;
    private float ammoSpriteLen = 0f;
    public bool targetVisible = false;

    private IEnumerator coroutine;
    void OnEnable()
    {
        type = ObstacleType.CANNON;
        joint = barrel.GetComponent<ConfigurableJoint>();
        cannonBallPool = ObjectsPool.getPool<CannonBall>();
        barrelCollider = barrel.GetComponent<CapsuleCollider>();
        barrelSprite = barrel.GetComponent<SpriteRenderer>();
        size = barrelCollider.radius;
        setRandom();
        coroutine = update(.1f);
        //StartCoroutine(coroutine);
    }

    void OnDisable()
    {
        barrel.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    void Start()
    {
    }


    void FixedUpdate()
    {
        if (isVisible() && ammo > 0)
        {
            float trajectoryAngle;
            float distance = Vector3.Distance(transform.position, target.position);
            Vector3 lead = ProjectileLead.FirstOrderIntercept(transform.position, Vector3.zero, shootSpeed * shootSpeedMult, target.position, target.velocity);
            if (ProjectileLead.CalculateTrajectory(distance, shootSpeed, out trajectoryAngle))
            {
                float trajectoryHeight = Mathf.Tan(trajectoryAngle * Mathf.Deg2Rad) * distance * shootSpeedMult;
                lead.y += trajectoryHeight;
            }
            Vector3 relativePos = lead - transform.position;
            Quaternion rotation = LookAt(relativePos);
            //Debug.DrawLine(target.position, lead);
            joint.targetRotation = rotation;

            if (Time.time >= nextTime)
            {
                nextTime += interval;
                if (target.transform.position.x < transform.position.x && Vector3.Distance(target.transform.position, transform.position) < 100f)
                {
                    shootBall();
                }
            }
        }
        else
        {
            nextTime = Time.time;
        }

    }

    IEnumerator update(float wait)
    {
        while (true)
        {
            targetVisible = isVisible();
            yield return new WaitForSeconds(wait);
        }
    }

    private void shootBall()
    {
        shootParticles.Play();
        CannonBall ball = cannonBallPool.Pop();
        ball.target = target;
        ball.setSize(size);
        ball.rigidbody.velocity = Vector3.zero;
        ball.rigidbody.mass = cannonBallMass;
        ball.transform.position = shootParticles.transform.position;
        ball.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
        ball.rigidbody.AddForce(barrel.transform.right * shootSpeed, ForceMode.VelocityChange);
        GetComponent<Rigidbody>().AddForce(-transform.right * 50, ForceMode.Impulse);
        ammo--;
        setAmmoSpriteLen(ammo / (float)maxAmmo);
    }

    public void setSize(float size)
    {
        this.size = size;
        barrelCollider.radius = size;
        barrelSprite.size = new Vector2(barrelSprite.size.x, size * 2f);
    }

    public void setSpeed(float shootSpeed)
    {
        this.shootSpeed = shootSpeed;
        float barrelLen = (shootSpeed / 100f) * 2f;
        barrelLen = Mathf.Clamp(barrelLen, 2f, 10f);
        Vector3 center = barrelCollider.center;
        center.x = barrelLen / 2f;
        barrelCollider.center = center;
        barrelCollider.height = barrelLen;
        barrelSprite.size = new Vector2(barrelLen, barrelSprite.size.y);
        Vector3 endPos = barrelEnd.transform.localPosition;
        endPos.x = barrelLen;
        barrelEnd.transform.localPosition = endPos;
    }

    public override void setRandom()
    {

        setSize(Random.Range(.1f, .5f));
        setSpeed(Random.Range(10f, 100f));
        interval = Mathf.Abs((size / .5f));
        cannonBallMass = (size / .5f) * 10f;
        maxAmmo = (int)Mathf.Abs(((size - 0.5f) * 2 * 10) - 1);
        ammo = maxAmmo;
        ammoSpriteLen = getAmmoSpriteLen();
        setAmmoSpriteLen(1f);

        base.setRandom();
    }

    private float getAmmoSpriteLen()
    {
        Vector3 ammoSpritePos = ammoSprite.transform.localPosition;
        Vector3 barrelEndPos = barrelEnd.transform.localPosition;
        ammoSpritePos.y = 0f;
        barrelEndPos.y = 0f;
        return Vector3.Distance(ammoSpritePos, barrelEndPos);
    }

    private void setAmmoSpriteLen(float len)
    {
        Vector2 size = ammoSprite.size;
        size.x = ammoSpriteLen * len;
        ammoSprite.size = size;
    }

    public override void setXPosition(float xPos)
    {
        Vector3 objPos = transform.position;
        objPos.x = xPos;
        objPos.y = Random.Range(3f, 10f);
        transform.position = objPos;
        colorShifterManager = GetComponent<ColorShifterManager>();
    }

    private bool isVisible()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return (screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1);
    }

    private Quaternion LookAt(Vector3 relativePos)
    {
        float rotationZ = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0.0f, 0.0f, -rotationZ);
    }
}
