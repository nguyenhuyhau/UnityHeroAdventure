using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour CurrentActiveWeapon {  get; private set; }

    private PlayerControls playerControls;
    private float timeBetweenAttacks;

    private bool attackButtonDown, isAttacking = false;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        //codesua:
        CurrentActiveWeapon = null; // Dam bao trang thai ban dau
    }

    private void OnEnable()
    {
        //codegoc: playerControls.Enable();

        //codesua:
        if (playerControls != null)
        {
            playerControls.Enable();
        }
        // Reset trang thai khi bat lai
        AttackCooldown();
    }

    //codesua: codegoc khong co OnDisable()
    private void OnDisable()
    {
        if (playerControls != null)
        {
            playerControls.Disable();
        }
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

        AttackCooldown();
    }

    private void Update()
    {
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon)
    {
        CurrentActiveWeapon = newWeapon;

        //codegoc:
        //AttackCooldown();
        //timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;

        //codesua:
        if (newWeapon != null)
        {
            AttackCooldown();
            timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
            newWeapon.gameObject.SetActive(true);
        }
    }

    public void WeaponNull()
    {
        //codegoc: CurrentActiveWeapon = null;

        //codesua:
        if (CurrentActiveWeapon != null)
        {
            Destroy(CurrentActiveWeapon.gameObject);
        }
        CurrentActiveWeapon = null;
    }

    private void AttackCooldown()
    {
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(TimeBetweenAttacksRoutine());
    }

    private IEnumerator TimeBetweenAttacksRoutine()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    private void StartAttacking()
    {
        attackButtonDown = true;
    }

    private void StopAttacking()
    {
        attackButtonDown = false;
    }

    private void Attack()
    {
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon)
        {
            AttackCooldown();
            (CurrentActiveWeapon as IWeapon).Attack();
        }
    }

    //codesua: codegoc khong co ResetWeapon()
    public void ResetWeapon()
    {
        attackButtonDown = false;
        isAttacking = false;
        StopAllCoroutines();
        if (CurrentActiveWeapon != null)
        {
            timeBetweenAttacks = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
            AttackCooldown();
        }
    }
}
