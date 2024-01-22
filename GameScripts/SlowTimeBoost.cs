using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SlowTimeBoost : MonoBehaviour
{
    [SerializeField] private float slowDuration = 8f;
    [SerializeField] private AnimationCurve slowCurve;
    [SerializeField] private Vector2 gameSpeedRange = new Vector2(1f, 0.6f);

    private float currentSlowTime = 0f;
    private Player player;
    private Boost boost;
    private Multiplier slowTimeMultiplier;

    private void Start()
    {
        boost = GetComponent<Boost>();
        player = GetComponent<Player>();
        slowTimeMultiplier = new Multiplier() { multiplier = gameSpeedRange.x };
        MapController.controller.SetMultiplier(slowTimeMultiplier, true);

        currentSlowTime = 0f;
        if(boost)
        {
            //AddAction;
            currentSlowTime = slowDuration;
        }

        if(player)
        {
            
        }
    }

    private void Update()
    {
        if(currentSlowTime < slowDuration)
        {
            currentSlowTime += Time.deltaTime;
            currentSlowTime = Mathf.Min(currentSlowTime, slowDuration);

            float slowPercentage = currentSlowTime / slowDuration;
            slowTimeMultiplier.multiplier = Mathf.Lerp(gameSpeedRange.x, gameSpeedRange.y, slowCurve.Evaluate(slowPercentage));

            if(currentSlowTime >= slowDuration)
            {
                MapController.controller.SetMultiplier(slowTimeMultiplier, false);
                Destroy(this);
            }
        }
    }
}