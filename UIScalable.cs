using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScalable : MonoBehaviour
{
    [SerializeField] private bool relativeScale = false;
    [SerializeField] private bool uiObject = true;
    [SerializeField] private bool isEnabled = true;
    [SerializeField] private float extension = 1f;
    private Vector2 ratio = Vector2.one;

    private void Start()
    {
        AdjustRatio();
    }

    public void AdjustRatio()
    {
        if(isEnabled)
        {
            ratio = UIManager.manager.GetCameraRatio();
            float minValue = Mathf.Min(ratio.x, ratio.y);
            ratio = relativeScale ? ratio : new Vector2(minValue, minValue);
            Vector2 scaleRatio = uiObject ? ratio : new Vector2(ratio.x / ratio.y, 1f);
            transform.localPosition = new Vector3(transform.localPosition.x * scaleRatio.x, transform.localPosition.y * scaleRatio.y, transform.localPosition.z * 1f);
            transform.localScale = new Vector3(transform.localScale.x * scaleRatio.x, transform.localScale.y * scaleRatio.y, transform.localScale.z * 1f) * extension;
        }
    }
}
