using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EconomyManager : Singleton<EconomyManager>
{
    private TMP_Text goldText;
    private int currentGold = 0;

    const string COIN_AMOUNT_TEXT = "Gold Amount Text";

    //codegoc:
    // void UpdateCurrentGold()
    // {
    //   currentGold += 1;
    //
    //  if (goldText == null)
    //  {
    //      goldText = GameObject.Find(COIN_AMOUNT_TEXT).GetComponent<TMP_Text>();
    //  }

    //   goldText.text = currentGold.ToString("D3");
    // }

    //tu cho nay tro di la code them:
    protected override void Awake()
    {
        base.Awake();
        // Khoi tao currentGold neu can (tuy yeu cau)
        if (currentGold == 000)
        {
            currentGold = 000; // Gia tri mac dinh
        }
    }

    private void Start()
    {
        InitializeGoldText();
        UpdateGoldUI();
    }

    private void OnEnable()
    {
        StartCoroutine(UpdateGoldUIAfterSceneLoad()); // Dong bo UI sau scene load
    }

    public void UpdateCurrentGold(int amount = 1)
    {
        currentGold += amount;
        UpdateGoldUI();
    }

    public void ResetGold()
    {
        currentGold = 0;
        UpdateGoldUI();
    }

    private IEnumerator UpdateGoldUIAfterSceneLoad()
    {
        yield return new WaitForEndOfFrame(); // Cho scene load hoan tat
        InitializeGoldText();
        UpdateGoldUI();
    }

    private void UpdateGoldUI()
    {
        if (goldText == null)
        {
            InitializeGoldText();
        }

        if (goldText != null)
        {
            goldText.text = currentGold.ToString("D3");
        }
    }

    private void InitializeGoldText()
    {
        GameObject goldTextObj = GameObject.Find(COIN_AMOUNT_TEXT);
        if (goldTextObj != null)
        {
            goldText = goldTextObj.GetComponent<TMP_Text>();
        }
    }

    // Getter cho currentGold (neu can truy cap tu script khac)
    public int CurrentGold => currentGold;
}