using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private float swordAttackCD = .5f;
    [SerializeField] private WeaponInfo weaponInfo;

private Transform weaponCollider;
private Animator myAnimator;

    private GameObject slashAnim;

private void Awake()
{
  myAnimator = GetComponent<Animator>();
}

    private void Start()
    {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("SlashSpawnPoint").transform;
    }

    //codesua: codegoc khong co OnEnable()
    private void OnEnable()
    {
        InitializeComponents();
        if (myAnimator != null)
        {
            myAnimator.Rebind(); // Reset Animator ve trang thai ban dau
        }
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    public WeaponInfo GetWeaponInfo()
    {
        return weaponInfo;
    }

    public void Attack()
    {
        //codegoc:
        // myAnimator.SetTrigger("Attack");
        // weaponCollider.gameObject.SetActive(true);
        //slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        // slashAnim.transform.parent = this.transform.parent;

        //codesua:
        myAnimator.SetTrigger("Attack");
        weaponCollider.gameObject.SetActive(true);

        // Kiem tra va gan lai slashAnimSpawnPoint neu can
        if (slashAnimSpawnPoint == null || slashAnimSpawnPoint.gameObject == null)
        {
            InitializeComponents();
        }

        if (slashAnimSpawnPoint != null)
        {
            slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
            slashAnim.transform.parent = this.transform.parent;
        }
        else
        {
            Debug.LogWarning("Attack failed: slashAnimSpawnPoint is null or destroyed");
        }
    }

    public void DoneAttackingAnimEvent()
    {
        //codegoc: weaponCollider.gameObject.SetActive(false);

        //codesua: 
        if (weaponCollider != null)
        {
            weaponCollider.gameObject.SetActive(false);
        }
    }

    public void SwingUpFlipAnimEvent()
    {
        //codegoc:
        //slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);

        //if (PlayerController.Instance.FacingLeft)
        // {
        //    slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        //}

        //codesua:
        if (slashAnim != null) // Kiem tra null
        {
            slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);
            if (PlayerController.Instance != null && PlayerController.Instance.FacingLeft)
            {
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            Debug.LogWarning("SwingUpFlipAnimEvent: slashAnim is null");
        }
    }

    public void SwingDownFlipAnimEvent()
    {
        //codegoc:
        //slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        //if (PlayerController.Instance.FacingLeft)
        //{
        //    slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        //}

        //codesua:
        if (slashAnim != null) // Kiem tra null
        {
            slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (PlayerController.Instance != null && PlayerController.Instance.FacingLeft)
            {
                slashAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        else
        {
            Debug.LogWarning("SwingDownFlipAnimEvent: slashAnim is null");
        }
    }

    private void MouseFollowWithOffset()
    {
        //codethem:
        if (PlayerController.Instance == null || ActiveWeapon.Instance == null || weaponCollider == null)
        {
            return;
        }
        //codegoc:
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    //codesua: codegoc khong co InitializeComponents()
    private void InitializeComponents()
    {
        if (weaponCollider == null && PlayerController.Instance != null)
        {
            weaponCollider = PlayerController.Instance.GetWeaponCollider();
        }
        if (slashAnimSpawnPoint == null)
        {
            GameObject spawnPoint = GameObject.Find("SlashSpawnPoint");
            if (spawnPoint != null)
            {
                slashAnimSpawnPoint = spawnPoint.transform;
            }
            else
            {
                Debug.LogError("Not Found SlashSpawnPoint");
            }
        }
    }
}

