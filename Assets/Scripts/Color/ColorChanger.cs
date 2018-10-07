using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public abstract class ColorChanger : MonoBehaviour
{
    public abstract void changeColor(Color color);
    public abstract Color getColor();
}
