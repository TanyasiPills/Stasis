using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Menu : MonoBehaviour, IPointerDownHandler
{
    Manager manager;

    public List<GameObject> buys;
    public List<GameObject> upgrades;
    public List<GameObject> buildings;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<Manager>();

        buildings = GameObject.FindGameObjectsWithTag("machine").ToList();

        buys = GameObject.FindGameObjectsWithTag("buy").ToList();
        upgrades = GameObject.FindGameObjectsWithTag("turret").Concat(GameObject.FindGameObjectsWithTag("shield")).ToList();

        foreach (var item in buildings)
        {
            item.SetActive(false);
        }

        foreach (var item in buys)
        {
            Button button = item.GetComponentInChildren<Button>();
            string name = item.name;
            string tag = item.tag;

            button.onClick.AddListener(() => Buy(button, name));
        }

        foreach (var item in upgrades)
        {
            item.SetActive(false);
        }
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

    public void Buy(Button button, string name)
    {
        bool canBuy = false;

        string sign = new string(name.Skip(1).Take(1).ToArray());

        Debug.Log(sign);

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
                if (item.name == name + "game") item.SetActive(true);
            }

            foreach (var item in upgrades)
            {
                if(item.name == name){
                    item.SetActive(true);
                }
            }
        }
    }

    public void Upgrade(int index)
    {

    }
}
