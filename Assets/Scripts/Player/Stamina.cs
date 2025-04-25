using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : Singleton<Stamina>
{
    public int CurrentStamina { get; private set; }

    [SerializeField] private Sprite fullStaminaImage, emptyStaminaImage;
    [SerializeField] private int timeBetweenStaminaRefresh = 3;

    private Transform staminaContainer;
    private int startingStamina = 3;
    private int maxStamina;
    const string STAMINA_CONTAINER_TEXT = "Stamina Container";

    protected override void Awake()
    {
        //codegoc:
        //base.Awake();

        //maxStamina = startingStamina;
        //CurrentStamina = startingStamina;

        //codesua:
        base.Awake();

        maxStamina = startingStamina;
        ResetStamina();
    }

    private void Start()
    {
        //codegoc: staminaContainer = GameObject.Find(STAMINA_CONTAINER_TEXT).transform;

        // Tim staminaContainer va kiem tra null
        GameObject container = GameObject.Find(STAMINA_CONTAINER_TEXT);
        if (container != null)
        {
            staminaContainer = container.transform;
        }
        else
        {
            Debug.LogError("Not Found 'Stamina Container'");
        }

        // Cap nhat UI lan dau
        if (staminaContainer != null && gameObject.activeInHierarchy)
        {
            UpdateStaminaImages();
        }
    }

    //codesua: codegoc khong co OnEnable()
    private void OnEnable()
    {
        // Cap nhat UI va khoi dong lai coroutine khi bat lai
        if (staminaContainer != null)
        {
            UpdateStaminaImages();
            if (CurrentStamina < maxStamina)
            {
                StopAllCoroutines();
                StartCoroutine(RefreshStaminaRoutine());
            }
        }
    }

    public void UseStamina()
    {
        //codegoc:
        // CurrentStamina--;
        // UpdateStaminaImages();

        //codesua:
        if(CurrentStamina > 0) // Them kiem tra de tranh loi khi stamina am
        {
            CurrentStamina--;
            UpdateStaminaImages();
        }

    }

    public void RefreshStamina()
    {
        if (CurrentStamina < maxStamina)
        {
            CurrentStamina++;
        }
        UpdateStaminaImages();
    }

    private IEnumerator RefreshStaminaRoutine()
    {
        //codegoc:
        //while (true)
        // {
        //    yield return new WaitForSeconds(timeBetweenStaminaRefresh);
        //    RefreshStamina();
        // }

        //codesua:
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenStaminaRefresh);
            if (gameObject.activeInHierarchy) // Chi refresh neu GameObject active
            {
                RefreshStamina();
            }
        }
    }

    private void UpdateStaminaImages()
    {
        //codegoc:
        //for (int i = 0; i < maxStamina; i++)
        //  {
        //  if (i <= CurrentStamina - 1)
        //  {
        //   staminaContainer.GetChild(i).GetComponent<Image>().sprite = fullStaminaImage;
        //}
        //  else
        //  {
        //      staminaContainer.GetChild(i).GetComponent<Image>().sprite = emptyStaminaImage;
        //  }
        //  }

        // if (CurrentStamina < maxStamina)
        //  {
        //     StopAllCoroutines();
        //    StartCoroutine(RefreshStaminaRoutine());
        // }

        //codesua:
        // Kiem tra xem staminaContainer co ton tai va GameObject co active khong
        if (staminaContainer == null || !gameObject.activeInHierarchy)
        {
            return; // Thoat neu khong hop le
        }

        for (int i = 0; i < maxStamina; i++)
        {
            // Kiem tra so luong con de tranh loi IndexOutOfRange
            if (i < staminaContainer.childCount)
            {
                Image staminaImage = staminaContainer.GetChild(i).GetComponent<Image>();
                if (staminaImage != null) // Kiem tra Image co ton tai khong
                {
                    staminaImage.sprite = (i <= CurrentStamina - 1) ? fullStaminaImage : emptyStaminaImage;
                }
            }
        }

        // Chi khoi dong coroutine neu can va GameObject active
        if (CurrentStamina < maxStamina && gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            StartCoroutine(RefreshStaminaRoutine());
        }
    }

    //codesua: codegoc khong co ResetStamina
    public void ResetStamina()
    {
        CurrentStamina = startingStamina;
        UpdateStaminaImages();
    }

}
