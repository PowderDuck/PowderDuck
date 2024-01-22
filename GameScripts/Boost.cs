using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public delegate void BoostAction();
    public List<BoostAction> actions = new List<BoostAction>();


}