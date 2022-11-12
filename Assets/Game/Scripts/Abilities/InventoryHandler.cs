using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour
{
    [Tooltip("Стартовый набор боеприпасов")]
    [SerializeField]
    private Ammunition[] startAmmunition;
    [Tooltip("Стартовое оружие в инвентаре")]
    [SerializeField]
    private List<Weapon> startWeapons;
    /// <summary>
    /// Количество патронов в инвентаре для каждого типа оружия
    /// </summary>
    private Dictionary<string, int> ammunition;
    /// <summary>
    /// количество заряженных патронов в каждом оружии
    /// </summary>
    private Dictionary<string,(Weapon, int)> loadedAmmoCount;

    private WeaponHandler _weaponHandler;
    // Start is called before the first frame update
    private void Awake()
    {
        _weaponHandler = GetComponent<WeaponHandler>();
        ammunition = new Dictionary<string, int>();
        loadedAmmoCount = new Dictionary<string,(Weapon, int)>();
        for (int i = 0; i < startAmmunition.Length; i++)
        {
            ammunition[startAmmunition[i].name] = startAmmunition[i].count;
        }
        ///В начале все оружие идет с полным боекомплектом
        for (int i = 0; i < startWeapons.Count; i++)
        {
            loadedAmmoCount[startWeapons[i].ItemName] = (startWeapons[i],startWeapons[i].MaxAmmoCount);
        }
        
    }

    void Start()
    {
        _weaponHandler.UpdateAvailableWeapons(startWeapons);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Если оружие с заданным именем имеет достаточное кол-во заряженных патронов для выстрела,
    /// то стреляем и уменьшаем кол-во заряженных патронов
    /// </summary>
    /// <param name="weaponName"></param>
    /// <param name="shotAmmoCount"></param>
    /// <returns></returns>
    public bool Shoot(string weaponName, int shotAmmoCount = 1)
    {
        if (!loadedAmmoCount.ContainsKey(weaponName)) return false;
        if (loadedAmmoCount[weaponName].Item2 < shotAmmoCount) return false;

        var t = loadedAmmoCount[weaponName];
        t.Item2 -= shotAmmoCount;
        loadedAmmoCount[weaponName] = t;

        return true;
    }

    public bool CanShoot(string weaponName, int shotAmmoCount = 1)
    {
        if (!loadedAmmoCount.ContainsKey(weaponName)) return false;
        if (loadedAmmoCount[weaponName].Item2 < shotAmmoCount) return false;

        return true;
    }

    public bool ReloadWeapon(string weaponName)
    {
        if (!loadedAmmoCount.ContainsKey(weaponName)) return false;
        if (loadedAmmoCount[weaponName].Item2 == loadedAmmoCount[weaponName].Item1.MaxAmmoCount) return false;

        var t = loadedAmmoCount[weaponName];
        if (t.Item1.AmmunitionType == "")
        {
            t.Item2 = t.Item1.MaxAmmoCount;
            loadedAmmoCount[weaponName] = t;
        }
        else
        {
            if (ammunition[t.Item1.AmmunitionType] <= 0) return false;
            ///Дополняем столько патронов сколько можем
            var diff = Mathf.Min(t.Item1.MaxAmmoCount - t.Item2, ammunition[t.Item1.AmmunitionType]);

            t.Item2 += diff;
            ///Забираем патроны из инвентаря
            ammunition[t.Item1.AmmunitionType] -= diff;

            loadedAmmoCount[weaponName] = t;
        }

        return true;
    }

    public bool CanReload(string weaponName)
    {
        if (!loadedAmmoCount.ContainsKey(weaponName)) return false;
        if (loadedAmmoCount[weaponName].Item2 == loadedAmmoCount[weaponName].Item1.MaxAmmoCount) return false;
        if (loadedAmmoCount[weaponName].Item1.AmmunitionType != "" && ammunition[loadedAmmoCount[weaponName].Item1.AmmunitionType] <= 0) return false;

        return true;
    }
    /// <summary>
    /// Отвечает на вопрос: есть ли хотя бы 1 патрон для оружия нужного типа в инвентаре?
    /// </summary>
    public bool HasAmmunitionForWeapon(string weaponName)
    {
        if (!loadedAmmoCount.ContainsKey(weaponName)) return false;
        if (!ammunition.ContainsKey(loadedAmmoCount[weaponName].Item1.AmmunitionType)) return false;
        return ammunition[loadedAmmoCount[weaponName].Item1.AmmunitionType] > 0;
    }

    public void AddAmmunition(string ammoName, int count)
    {
        if (ammunition.ContainsKey(ammoName) && count > 0)
        {
            ammunition[ammoName] += count;
        }
    }
}
[Serializable]
public class Ammunition
{
    public string name;
    public int count;
}
