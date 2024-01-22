using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    [SerializeField] private MeshRenderer bottleRenderer;
    [SerializeField] private DisposableEffect pickEffect;

    private Color bottleColor = Color.white;

    private void Start()
    {
        GetComponent<PickableObject>().actions.Add(Pick);
        bottleColor = ColorManager.manager.GetColor();
        bottleRenderer.materials[0].color = bottleColor;
    }

    private void Pick(Player player)
    {
        DisposableEffect effect = Instantiate(pickEffect, transform.position, pickEffect.transform.rotation);
        ParticleSystem.MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = bottleColor;
        //IncrementScore;
    }
}
