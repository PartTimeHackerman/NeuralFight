using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

class Ramp : Obstacle
{
    public CapsuleCollider horizontal;
    public CapsuleCollider vertical;
    public GameObject endPointSprite;
    public GameObject coverSprite;
    private SpriteRenderer horizontalSpriteRenderer;
    private SpriteRenderer verticalSpriteRenderer;
    public BoxCollider ground;

    public float width = 0f, height = 0f;
    public bool setRamp = false;

    void OnEnable()
    {
        type = ObstacleType.RAMP;
        horizontalSpriteRenderer = horizontal.GetComponent<SpriteRenderer>();
        verticalSpriteRenderer = vertical.GetComponent<SpriteRenderer>();
        
        setRampWH();
    }

    public void FixedUpdate()
    {
        if (setRamp)
        {
            setRandom();
            setRamp = false;
        }
    }

    public override void setRandom()
    {
        width = Random.Range(1f, 10f);
        height = Random.Range(0.5f, width * .66f);
        setRampWH();
        base.setRandom();
    }

    public void setRampWH()
    {

        totalWidth = width + vertical.radius;
        setWidth();
        setHeight();
        setHorizontal();
        Vector3 endPointPos = new Vector3(width, height - Mathf.Abs(horizontal.transform.localPosition.y), 0f);
        endPointSprite.transform.localPosition = endPointPos;

        Vector3 coverPos = vertical.transform.localPosition;
        coverSprite.transform.localPosition = coverPos;
        Vector3 coverSize = new Vector3(width, height, 0f);
        coverSprite.GetComponent<SpriteRenderer>().size = coverSize;
        setGround();
    }

    public void setWidth()
    {
        
        Vector3 verLocPos = vertical.transform.localPosition;
        verLocPos.x = width;
        vertical.transform.localPosition = verLocPos;

    }

    public void setHeight()
    {
        Vector3 verticalSpriteRendererSize = verticalSpriteRenderer.size;
        verticalSpriteRendererSize.y = height;
        verticalSpriteRenderer.size = verticalSpriteRendererSize;
        vertical.height = height + .1f;
        Vector3 verCenter = vertical.center;
        verCenter.y = (height + .1f) / 2f;
        vertical.center = verCenter;

    }

    public void setHorizontal()
    {

        Vector3 widthHeight = new Vector3(width, height, 0f);
        float horizontalHeight = Vector3.Distance(Vector3.zero, widthHeight);
        horizontal.height = horizontalHeight + .1f;
        Vector3 horCenter = horizontal.center;
        horCenter.x = (horizontalHeight + .1f) / 2f;
        horizontal.center = horCenter;

        Vector3 horizontalSpriteRendererSize = horizontalSpriteRenderer.size;
        horizontalSpriteRendererSize.x = horizontalHeight;
        horizontalSpriteRenderer.size = horizontalSpriteRendererSize;

        float horizontalAngle = Vector2.Angle(new Vector3(width, 0f, 0f), widthHeight);
        Vector3 horAng = horizontal.transform.rotation.eulerAngles;
        horAng.z = horizontalAngle;
        horizontal.transform.rotation = Quaternion.Euler(horAng);
    }

    private void setGround()
    {
        float width = this.width + .2f;
        Vector3 size = ground.size;
        size.x = width;
        ground.size = size;
        Vector3 center = ground.center;
        center.x = width / 2f;
        ground.center = center;
        SpriteRenderer sprite = ground.GetComponent<SpriteRenderer>();
        Vector2 spriteSize = sprite.size;
        spriteSize.x = width;
        sprite.size = spriteSize;
    }

}
