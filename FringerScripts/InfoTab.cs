using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoTab : MonoBehaviour
{
    [SerializeField] private List<GameObject> infoPanels = new List<GameObject>();
    [SerializeField] private GameObject[] arrows = new GameObject[2];
    [SerializeField] private int currentPanelIndex = 0;

    private void OnEnable()
    {
        ChangePanel(0);
    }

    public void ChangePanel(int increment)
    {
        int previousPanel = currentPanelIndex;
        int iterations = 1;

        currentPanelIndex += increment;

        iterations += previousPanel == currentPanelIndex ? 0 : 1;

        for (int i = 0; i < iterations; i++)
        {
            int index = i == 0 ? currentPanelIndex : previousPanel;

            infoPanels[index].SetActive(i == 0);
        }

        arrows[0].SetActive(currentPanelIndex > 0);
        arrows[1].SetActive(currentPanelIndex < infoPanels.Count - 1f);

        SoundManager.manager.PlaySound(SoundManager.manager.uiClick, 0);
    }
}
