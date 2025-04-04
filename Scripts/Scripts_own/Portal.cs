using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private Loading loading;

    private void Start()
    {
        loading = FindObjectOfType<Loading>();
    }

    private void OnParticleCollision(GameObject other)
    {
        loading.LoadScene(4);
    }
}
