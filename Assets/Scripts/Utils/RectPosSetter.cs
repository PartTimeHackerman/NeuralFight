using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class RectPosSetter : MonoBehaviour
{
    public RectTransform referenceRect;
    private RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    void Update()
    {
        rect.anchoredPosition = referenceRect.anchoredPosition;
    }
}
