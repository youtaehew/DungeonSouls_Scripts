using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Blood : MonoBehaviour
{
    public GameObject bloodSplatFx;

    public void PlayBloodSplat(Vector3 bloodSplatPos)
    {
        GameObject blood = Instantiate(bloodSplatFx, bloodSplatPos, quaternion.identity);
    }
}
