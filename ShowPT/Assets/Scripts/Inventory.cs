﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    public WEAPON_TYPE weapon;
    public GameObject[] weaponsInventory;
    private int selectedIdWeapond = -1;

    public enum AMMO_TYPE
    {
        GUNAMMO,
        SHOTGUNAMMO,
        CANONAMMO,

        TOTAL_AMMO,
        NO_AMMO
    }

    public enum WEAPON_TYPE
    {
        NONE = -1,

        GUN,
        SHOTGUN,
        CANON,

        TOTAL_WEAPONS,
        NO_WEAPON
    }

    public Dictionary<AMMO_TYPE, int> ammoInventory = new Dictionary<AMMO_TYPE, int>();
    public Dictionary<AMMO_TYPE, int> totalAmmoInventory = new Dictionary<AMMO_TYPE, int>();
    public bool[] weaponsCarrying;

    public HudController hudController;

    // Use this for initialization
    void Start ()
    {
        weapon = WEAPON_TYPE.NONE;

        totalAmmoInventory.Add(AMMO_TYPE.GUNAMMO, 1);
        totalAmmoInventory.Add(AMMO_TYPE.SHOTGUNAMMO, 0);
        totalAmmoInventory.Add(AMMO_TYPE.CANONAMMO, 0);

        weaponsCarrying = new bool[(int)WEAPON_TYPE.TOTAL_WEAPONS];
        weaponsCarrying[(int)WEAPON_TYPE.GUN] = false;
        weaponsCarrying[(int)WEAPON_TYPE.SHOTGUN] = false;
        weaponsCarrying[(int)WEAPON_TYPE.CANON] = false;

        //test
        //weapon = WEAPON_TYPE.GUN;
        //gameObject.GetComponent<PlayerMovment>().animator = weaponsInventory[(int)weapon].GetComponentInChildren<Animator>();

        //addWeapon(WEAPON_TYPE.GUN);
        //addWeapon(WEAPON_TYPE.SHOTGUN);
        //addWeapon(WEAPON_TYPE.CANON);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            switchWeapon(WEAPON_TYPE.GUN);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            switchWeapon(WEAPON_TYPE.SHOTGUN);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            switchWeapon(WEAPON_TYPE.CANON);
        }

        if (Input.GetButtonDown("ButtonLB") || Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            nextWeapond(-1);
        }
        if (Input.GetButtonDown("ButtonRB") || Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            nextWeapond(+1);
        }
    }

    void nextWeapond(int direction)
    {
        int lastWeapon = selectedIdWeapond;

        if (selectedIdWeapond != -1)
        {
            for (int i = selectedIdWeapond; i < weaponsCarrying.Length; )
            {
                i += direction;

                if (i < 0)
                {
                    i = weaponsCarrying.Length - 1;
                }
                i %= weaponsCarrying.Length;

                Debug.Log(i + " " + weaponsCarrying[i]);
                if (weaponsCarrying[i])
                {
                    if (lastWeapon != i)
                    {
                        switchWeapon((WEAPON_TYPE)i);
                    }
                    break;
                }
            }
        }
    }

    public int getAmmo(AMMO_TYPE typeAmmo)
    {
        return totalAmmoInventory[typeAmmo];
    }

    public void decreaseAmmo(AMMO_TYPE typeAmmo, int value)
    {
        totalAmmoInventory[typeAmmo] -= value;
        if (totalAmmoInventory[typeAmmo] < 0)
        {
            totalAmmoInventory[typeAmmo] = 0;
        }
        hudController.setTotalAmmo(typeAmmo, totalAmmoInventory[typeAmmo]);
    }

    public void increaseAmmo(AMMO_TYPE typeAmmo, int value)
    {
        totalAmmoInventory[typeAmmo] += value;
        hudController.setTotalAmmo(typeAmmo, totalAmmoInventory[typeAmmo]);
    }

    public void setAmmo(AMMO_TYPE typeAmmo, int value)
    {
        if (ammoInventory.ContainsKey(typeAmmo))
        {
            ammoInventory[typeAmmo] = value;
        }
        else
        {
            ammoInventory.Add(typeAmmo, value);
        }

        hudController.setAmmo(ammoInventory[typeAmmo]);
    }

    private void switchWeapon(WEAPON_TYPE type)
    {
        if (weapon != type && weaponsCarrying[(int)type])
        {
            selectedIdWeapond = (int)type;

            if (weapon != WEAPON_TYPE.NONE)
            {
                weaponsInventory[(int)weapon].SetActive(false);
            }
            weapon = type;
            weaponsInventory[(int)weapon].SetActive(true);

            switch (type)
            {
                case WEAPON_TYPE.GUN:
                    hudController.selectWeapon(type, ammoInventory[AMMO_TYPE.GUNAMMO], getAmmo(AMMO_TYPE.GUNAMMO));
                    break;
                case WEAPON_TYPE.SHOTGUN:
                    hudController.selectWeapon(type, ammoInventory[AMMO_TYPE.SHOTGUNAMMO], getAmmo(AMMO_TYPE.SHOTGUNAMMO));
                    break;
                case WEAPON_TYPE.CANON:
                    hudController.selectWeapon(type, ammoInventory[AMMO_TYPE.CANONAMMO], getAmmo(AMMO_TYPE.CANONAMMO));
                    break;
            }
            gameObject.GetComponent<PlayerMovment>().animator = weaponsInventory[(int)weapon].GetComponentInChildren<Animator>();
        }
    }

    public bool hasWeapon(WEAPON_TYPE type)
    {
        return (weaponsCarrying[(int)type]);
    }

    public void addWeapon(WEAPON_TYPE type)
    {
        weaponsCarrying[(int)type] = true;

        int ammunition = weaponsInventory[(int)type].GetComponent<Weapon>().ammunition;
        switch (type)
        {
            case WEAPON_TYPE.GUN:
                ammoInventory.Add(AMMO_TYPE.GUNAMMO, ammunition);
                break;
            case WEAPON_TYPE.SHOTGUN:
                ammoInventory.Add(AMMO_TYPE.SHOTGUNAMMO, ammunition);
                break;
            case WEAPON_TYPE.CANON:
                ammoInventory.Add(AMMO_TYPE.CANONAMMO, ammunition);
                break;
        }
        
        if (selectedIdWeapond == -1)
        {
            switchWeapon(type);
        }
        hudController.addWeapon(type);
        //Launch some animation or sound that has bought weapon
    }
}
