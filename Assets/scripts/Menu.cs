using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Menu : MonoBehaviour, IPointerDownHandler
{
    Manager manager;

    public List<GameObject> buys;
    public List<GameObject> upgrades;
    public List<GameObject> upgradeHolders;
    public List<GameObject> buildings;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<Manager>();

        buildings = GameObject.FindGameObjectsWithTag("machine").ToList();

        buys = GameObject.FindGameObjectsWithTag("buy").ToList();
        upgrades = GameObject.FindGameObjectsWithTag("turret").Concat(GameObject.FindGameObjectsWithTag("shield")).ToList();
        upgradeHolders = GameObject.FindGameObjectsWithTag("upgradeHolder").ToList();


        foreach (var item in buildings) item.SetActive(false);

        foreach (var item in buys)
        {
            Button button = item.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => Buy(button));
        }

        foreach (var item in upgrades)
        {
            Button button = item.GetComponentInChildren<Button>();
            button.onClick.AddListener(() => Upgrade(button));
            item.SetActive(false);
        }

        foreach (var item in upgradeHolders) item.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (manager.UIIsOpen()) manager.UIChange();
    }

    public bool AnyBuildingBought()
    {
        bool bought = false;

        foreach (var item in buildings) bought |= item.activeSelf;

        return bought;
    }

    public void Buy(Button button)
    {
        bool canBuy = false;

        string sign = new string(button.name.Skip(1).Take(1).ToArray());

        switch (sign) 
        {
            case "1":
            case "2":
                if(manager.cc >= 1) {
                    canBuy = true;
                    manager.cc -= 1;
                }
                break;
            case "3":
                if (manager.cc >= 1) {
                    canBuy = true;
                    manager.cc -= 1;
                }
                break;
        }

        if (canBuy) {
            Debug.Log("Canbuy");
            button.gameObject.SetActive(false);
            
            foreach(var item in buildings)
            {
                if (item.name == button.name + "game") item.SetActive(true);
            }

            foreach (var item in upgrades)
            {
                if(item.name == button.name)
                {
                    item.SetActive(true);
                }
            }

            foreach (var item in upgradeHolders)
            {
                string position = new string(item.name.Take(2).ToArray());

                if (position == button.name && !item.activeSelf)
                {
                    item.SetActive(true);
                }
            }
        }
    }

    public void Upgrade(Button button)
    {
        string position = new string(button.name.Take(2).ToArray());

        bool canUpgrade = false;

        switch (position[1])
        {
            case '1':
            case '2':
                if(button.tag == "turret")
                {
                    if (manager.cc >= 12)
                    {
                        canUpgrade = true;
                        manager.cc -= 12;
                    }
                } else
                {
                    if (manager.cc >= 24)
                    {
                        canUpgrade = true;
                        manager.cc -= 24;
                    }
                }
                break;
            case '3':
                if (button.tag == "turret")
                {
                    if (manager.cc >= 48)
                    {
                        canUpgrade = true;
                        manager.cc -= 48;
                    }
                }
                else
                {
                    if (manager.cc >= 96)
                    {
                        canUpgrade = true;
                        manager.cc -= 96;
                    }
                }
                break;
        }

        if (canUpgrade) {
            foreach (var item in buildings)
            {
                if (item.name == button.name + "game") {
                    Generator gen = item.GetComponent<Generator>();
                    if (button.tag == "turret") gen.Upgraded(Generator.Upgrades.turret);
                    else gen.Upgraded(Generator.Upgrades.firewall);
                }
            }
        }
    }
}
