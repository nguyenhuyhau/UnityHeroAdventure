using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance;
    public static T Instance {  get { return instance; } }

    //codethem: codegoc khong co previousScene
    private static string previousScene = ""; // Theo doi scene truoc do

    protected virtual void Awake()
    {
        if (instance != null && this.gameObject != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = (T)this;
        }

        if (!gameObject.transform.parent)
        {
            DontDestroyOnLoad(gameObject);
        }

        //Tu cho nay tro xuong la code sua:
        SceneManager.sceneLoaded += OnSceneLoaded;

        UpdateUIState();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateUIState();
        // Chi reset Stamina va PlayerHealth khi chuyen tu "Menu" sang Scene 1
        if (previousScene == "Menu" && scene.buildIndex == 1) // Scene 1 co buildIndex = 1
        {
            if (typeof(T) == typeof(Stamina))
            {
                Stamina.Instance?.ResetStamina();
            }
            if (typeof(T) == typeof(PlayerHealth))
            {
                PlayerHealth.Instance?.ResetHealth();
            }
            if (typeof(T) == typeof(EconomyManager))
            {
                EconomyManager.Instance?.ResetGold();
            }
        }

        // Luon gan lai vu khi khi vao scene gameplay
        if (scene.name != "Menu")
        {
            if (typeof(T) == typeof(ActiveInventory))
            {
                ActiveInventory.Instance?.EquipStartingWeapon();
            }
            if (typeof(T) == typeof(ActiveWeapon))
            {
                ActiveWeapon.Instance?.ResetWeapon();
            }
        }

        // Cap nhat previousScene sau khi load
        previousScene = scene.name;
    }

    private void UpdateUIState()
    {
        if (SceneManager.GetActiveScene().name == "Menu" || SceneManager.GetActiveScene().name == "Scene3")
        {
            gameObject.SetActive(false); 
        }
        else
        {
            gameObject.SetActive(true); 
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
