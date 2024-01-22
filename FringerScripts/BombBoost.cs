using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBoost : MonoBehaviour
{
    [SerializeField] private float radius = 3f;
    [SerializeField] private ParticleSystem explosionEffect;

    private Boost boost;

    private void Start()
    {
        boost = GetComponent<Boost>();

        boost.actions.Add(Explode);
    }

    private void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            Mine currentMine = colliders[i].GetComponent<Mine>();

            if(currentMine)
            {
                currentMine.ManipulateMine(true, true);
            }
        }

        ParticleSystem explosion = Instantiate(explosionEffect, new Vector3(transform.position.x, transform.position.y, -5f), Quaternion.identity);
        explosion.transform.eulerAngles = new Vector3(90f, 0f, 0f);
        explosion.Play();
        SoundManager.manager.PlaySound(SoundManager.manager.bombBoost, 0);
        Destroy(gameObject);
        //explosionEffect.Play();
    }
}
