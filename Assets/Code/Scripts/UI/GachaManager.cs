using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using System;

#if UNITY_EDITOR
using TMPro.EditorUtilities; // Editor-only reference to avoid runtime errors
#endif

public class GachaManager : MonoBehaviour
{
    public GameObject bannerPrefab;
    public List<BannerData> banners;
    public Transform bannerPanel;
    public SummonManager summonManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateBannerDisplay();
        summonManager.SetSelectedBanner(banners[1]);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void UpdateBannerDisplay()
    {
        foreach (Transform child in bannerPanel)
        {
            if (child.GetComponent<Scrollbar>() == null)
            {
                Destroy(child.gameObject);
            }
        }
        foreach (BannerData data in banners)
        {
            GameObject bnr = Instantiate(bannerPrefab, bannerPanel);

            // Set Icon
            Image panelImage = bnr.GetComponent<Image>();
            if (panelImage != null)
                panelImage.sprite = data.bannerIcon;

            // Assign Click Event
            Button btn = bnr.GetComponent<Button>();
            if (btn != null)
            {
                BannerData capturedData = data; // Capture variable to avoid closure issues
                btn.onClick.AddListener(() => summonManager.SetSelectedBanner(capturedData));
            }
        }


        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(bannerPanel.GetComponent<RectTransform>());
    }
}
