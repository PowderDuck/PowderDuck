using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class AdManager : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener, IUnityAdsInitializationListener
{
    [SerializeField] private float adPeriod = 5f;
    [SerializeField] private string[] gameIDs = new string[2];
    [SerializeField] private bool testMode = true;
    [SerializeField] private List<Button> disableButtons = new List<Button>();

    private float currentADPeriod = 0f;
    private int deviceIndex = 0;

    private Dictionary<string, List<string>> placementIDs = new Dictionary<string, List<string>>()
    {
        { "Interstitial", new List<string>() { "Interstitial_Android", "Interstitial_iOS" } },
        { "Rewarded", new List<string>() { "Rewarded_Android", "Rewarded_iOS" } },
        { "Banner", new List<string>() { "Banner_Android", "Banner_iOS" } }
    };

    private void Start()
    {
        deviceIndex = Application.platform == RuntimePlatform.IPhonePlayer ? 1 : 0;
        Advertisement.Initialize(gameIDs[deviceIndex], testMode, this);
    }

    public void ShowAD(string adType)
    {
        if(currentADPeriod >= adPeriod)
        {
            ManipulateButtons(false);
            Advertisement.Load(placementIDs[adType][deviceIndex], this);
        }
        
        currentADPeriod = (currentADPeriod + 1f) % (adPeriod + 1f);
    }

    public void OnInitializationComplete()
    {
        
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        
    }

    void IUnityAdsShowListener.OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        ManipulateButtons(true);
    }

    void IUnityAdsShowListener.OnUnityAdsShowStart(string placementId)
    {
        
    }

    void IUnityAdsShowListener.OnUnityAdsShowClick(string placementId)
    {
        
    }

    private void ManipulateButtons(bool enable)
    {
        for (int i = 0; i < disableButtons.Count; i++)
        {
            disableButtons[i].interactable = enable;
        }
    }

    void IUnityAdsShowListener.OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        ManipulateButtons(true);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        //ManipulateButtons(false);
        Advertisement.Show(placementId, new ShowOptions(), this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        ManipulateButtons(true);
    }
}
