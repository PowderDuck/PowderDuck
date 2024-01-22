using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintTrap : MonoBehaviour
{
    [SerializeField] private DisposableObject stainPrefab;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if(player && !activated)
        {
            DisposableObject stain = Instantiate(stainPrefab, FindObjectOfType<Canvas>().transform);
            stain.transform.localPosition = Vector3.zero;
            stain.GetComponent<Image>().color = Random.ColorHSV();
            stain.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));

            //SprayAnimation;

            activated = true;
        }
    }
}
