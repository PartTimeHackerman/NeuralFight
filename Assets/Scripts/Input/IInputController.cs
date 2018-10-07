using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class InputController : MonoBehaviour
{
    public abstract void Began(Touch touch);
    public abstract void Moved(Touch touch);
    public abstract void Ended(Touch touch);
}
