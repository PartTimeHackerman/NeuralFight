using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class SpriteShadows : MonoBehaviour
{
    public Color color = Color.black;
    public Vector2 offset = new Vector2();
    public float size = .05f;
    public bool reset = false;
    private Dictionary<SpriteRenderer, SpriteRenderer> outlines = new Dictionary<SpriteRenderer, SpriteRenderer>();

    void Start()
    {
        createOutlines();
    }

    void FixedUpdate()
    {
        if (reset)
        {
            resetShadows();
            reset = false;
        }
    }

    public void createOutlines()
    {
        List<SpriteRenderer> sprites = GetComponentsInChildren<SpriteRenderer>().ToList();
        foreach (SpriteRenderer spriteRenderer in sprites)
        {
            var shadow = new GameObject(spriteRenderer.name + "_shadow");
            CopyComponent(spriteRenderer, shadow);
            shadow.transform.parent = spriteRenderer.transform;
            SpriteRenderer spriteRendererShadow = shadow.GetComponent<SpriteRenderer>();
            spriteRendererShadow.color = color;
            spriteRendererShadow.sortingOrder = -10;
            shadow.transform.localRotation = Quaternion.Euler(Vector3.zero);
            Vector2 pivot = getSpritePivot(spriteRendererShadow.sprite);
            Vector2 scaledSize = new Vector2(size, size) * .5f;
            Vector2 scaledPivotTransform = scaledSize * pivot;
            shadow.transform.localPosition = new Vector3(offset.x + scaledPivotTransform.x, offset.y + scaledPivotTransform.y, 0f);

            /// COLOR CHANGER

            SpriteColorChanger scc = spriteRendererShadow.gameObject.AddComponent<SpriteColorChanger>();
            GroupColorChanger gcc = GroupColorChanger.get(GroupType.OUTLINES, Color.black, Color.white);
            gcc.addChanger(scc);
            /// 

            outlines[spriteRenderer] = spriteRendererShadow;
            setOutlines();
        }
    }

    public void setOutlines()
    {
        foreach (KeyValuePair<SpriteRenderer, SpriteRenderer> outline in outlines)
        {
            SpriteRenderer baseSprite = outline.Key;
            SpriteRenderer outlineSprite = outline.Value;
            Vector2 scaledSpriteSize = baseSprite.size;
            float scaleAdd = size;
            scaledSpriteSize.x += scaleAdd;
            scaledSpriteSize.y += scaleAdd;
            outlineSprite.size = scaledSpriteSize;

        }
    }

    public void removeShadows()
    {
        List<SpriteRenderer> sprites = GetComponentsInChildren<SpriteRenderer>().ToList();
        foreach (SpriteRenderer spriteRenderer in sprites)
        {
            if (spriteRenderer.name.Contains("_shadow"))
            {
                UnityEngine.Object.Destroy(spriteRenderer.gameObject);
            }
        }
    }

    public void resetShadows()
    {
        setOutlines();
    }

    public Vector2 getSpritePivot(Sprite sprite)
    {
        Vector2 spriteSize = new Vector2((float)sprite.texture.width, (float)sprite.texture.height);
        Vector2 pivot = sprite.pivot;
        pivot.x = (pivot.x / spriteSize.x - 0.5f) * 2f;
        pivot.y = (pivot.y / spriteSize.y - 0.5f) * 2f;
        return pivot;
    }

    T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst) dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props)
        {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst as T;
    }

}
