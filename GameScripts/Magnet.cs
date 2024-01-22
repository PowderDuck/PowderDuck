using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField] private float effectRadius = 5f;
    [SerializeField] private Vector2 magnetDurationRange = new Vector2(10f, 30f);

    [SerializeField] private MeshRenderer magnetRenderer;
    [HideInInspector] public float currentMagnetTime = 0f;

    private const int bottleLayer = 64;

    private void Start()
    {
        PickableObject boost = GetComponent<PickableObject>();

        if(boost)
        {
            boost.actions.Add(ActivateMagnet);
        }

        if(magnetRenderer)
        {
            magnetRenderer.enabled = false;
        }
    }

    private void ActivateMagnet(Player player)
    {
        Magnet[] magnets = FindObjectsOfType<Magnet>();

        for (int i = 0; i < magnets.Length; i++)
        {
            if(magnets[i] != this)
            {
                magnets[i].currentMagnetTime = Mathf.Lerp(magnetDurationRange.x, magnetDurationRange.y, PlayerPrefs.GetFloat("MagnetUpgrade"));
            }
        }
    }

    private void Update()
    {
        if(currentMagnetTime > 0f)
        {
            currentMagnetTime -= Time.deltaTime;
            currentMagnetTime = Mathf.Max(0f, currentMagnetTime);

            AttractBottles();

            magnetRenderer.enabled = currentMagnetTime > 0f;
        }
    }
    private void AttractBottles()
    {
        Collider[] affectedBottles = Physics.OverlapSphere(transform.position, effectRadius, bottleLayer);

        for (int i = 0; i < affectedBottles.Length; i++)
        {
            Bottle bottle = affectedBottles[i].GetComponent<Bottle>();

            if(bottle)
            {

            }
        }
    }
}
