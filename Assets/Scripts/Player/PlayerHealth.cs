using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : Singleton<PlayerHealth>
{
    public bool isDead {  get; private set; }

    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float knockBackThrustAmount = 10f;
    [SerializeField] private float damageRecoveryTime = 1f;

    private Slider healthSlider;
    private int curentHealth;
    private bool canTakeDamage = true;
    private Knockback knockback;
    private Flash flash;

    const string HEALTH_SLIDER_TEXT = "Health Slider";
    //const string TOWN_TEXT = "Scene1";
    const string TOWN_TEXT = "Menu";
    readonly int DEATH_HASH = Animator.StringToHash("Death");

    protected override void Awake()
    {
        base.Awake();

        flash = GetComponent<Flash>();
        knockback = GetComponent<Knockback>();
        //codethem: ResetHealth();
        if(curentHealth == 0) // Chi khoi tao lan dau
        {
            curentHealth = maxHealth;
            isDead = false;
        }
    }

    private void Start()
    {
        //codegoc:
        //isDead = false;
        // curentHealth = maxHealth;

        // UpdateHealthSlider();

        //codesua:
        InitializeHealthSlider();
        UpdateHealthSlider();
    }

    //codethem: codegoc khong co OnEnable()
    private void OnEnable()
    {
        if (healthSlider != null)
        {
            //UpdateHealthSlider(); // Cap nhat UI khi bat lai
            StartCoroutine(UpdateHealthUIAfterSceneLoad());
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();

        if (enemy) {
            TakeDamage(1, other.transform);
        }
    }

    public void HealPlayer()
    {
        if (curentHealth < maxHealth)
        {
            curentHealth += 1;
            UpdateHealthSlider();
        }
    }

    public void TakeDamage(int damageAmount, Transform hitTransform)
    {
        if (!canTakeDamage) { return; }

        ScreenShakeManager.Instance.ShakeScreen();
        knockback.GetKnockedBack(hitTransform, knockBackThrustAmount);
        StartCoroutine(flash.FlashRoutine());

        canTakeDamage = false;
        curentHealth -= damageAmount;
        StartCoroutine(DamageRecoveryRoutine());
        UpdateHealthSlider();
        CheckIfPlayerDeath();
    }

    private void CheckIfPlayerDeath()
    {
        if (curentHealth <= 0 && !isDead)
        {
            isDead = true;
            //codegoc: Destroy(ActiveWeapon.Instance.gameObject);

            //codesua:
            if (ActiveWeapon.Instance != null && ActiveWeapon.Instance.CurrentActiveWeapon != null)
            {
                Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
                ActiveWeapon.Instance.WeaponNull(); // Dat lai trang thai
            }
            //codegoc:
            curentHealth = 0;
            //Debug.Log("Player Death");
            GetComponent<Animator>().SetTrigger(DEATH_HASH);
            StartCoroutine(DeathLoadSceneRoutine());
        }
    }

    private IEnumerator DeathLoadSceneRoutine()
    {
        yield return new WaitForSeconds(2f);
        //codegoc: Destroy(gameObject);
        SceneManager.LoadScene(TOWN_TEXT);
    }

    private IEnumerator DamageRecoveryRoutine()
    {
        yield return new WaitForSeconds(damageRecoveryTime);
        canTakeDamage = true;
    }

    //codethem: codegoc khong co UpdateHealthUIAfterSceneLoad()
    private IEnumerator UpdateHealthUIAfterSceneLoad()
    {
        yield return new WaitForEndOfFrame(); // Cho scene load hoan tat
        InitializeHealthSlider();
        UpdateHealthSlider();
    }

    private void UpdateHealthSlider()
    {
        //codegoc:
        //if (healthSlider == null)
        // {
        //     healthSlider = GameObject.Find(HEALTH_SLIDER_TEXT).GetComponent<Slider>();
        // }

        //  healthSlider.maxValue = maxHealth;
        // healthSlider.value = curentHealth;

        //tu cho nay tro xuong la codesua:
        if (healthSlider == null)
        {
            InitializeHealthSlider();
        }

        if (healthSlider != null && gameObject.activeInHierarchy)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = curentHealth;
        }
    }

    private void InitializeHealthSlider()
    {
        GameObject sliderObj = GameObject.Find(HEALTH_SLIDER_TEXT);
        if (sliderObj != null)
        {
            healthSlider = sliderObj.GetComponent<Slider>();
        }
        else
        {
            Debug.LogError("Khong tim thay 'Health Slider'");
        }
    }

    public void ResetHealth()
    {
        isDead = false;
        curentHealth = maxHealth;
        canTakeDamage = true;
        UpdateHealthSlider();
    }

    public int curentHealthValue => curentHealth;
}
