using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class Manager : MonoBehaviour
{
    List<Transform> spawns;
    Transform main;

    public TMP_Text timeText;
    public TMP_Text moneyText;
    public TMP_Text temperatureText;

    public GameObject menu;

    public GameObject icePrefab;
    public GameObject buyHolder;
    List<GameObject> ice;

    public float time = 0;
    public float temperature = -273.15f;

    public float spawnTime = 1;
    float timeTilSpawn = 0;
    int value = 50;
    int speed = 1;

    public int cc = 0;

    public string TimeToString()
    {
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }

    private void Start()
    {
        spawns = GameObject.FindGameObjectsWithTag("spawner").Select(e => e.transform).ToList();
        main = GameObject.FindGameObjectWithTag("mainSpawner").transform;
        ice = new List<GameObject>();

        menu = GameObject.FindGameObjectWithTag("menu");
        menu.SetActive(false);

        buyHolder = GameObject.Find("BuyHolder");
    }

    private void Update()
    {
        time += Time.deltaTime;
        for (int i = ice.Count - 1; i >= 0; i--)
        {
            if (ice[i].GetComponent<Ice>().health <= 0)
            {
                Destroy(ice[i]);
                ice.RemoveAt(i);
                cc++;
            }
        }

        moneyText.text = $"{cc}";
        timeText.text = TimeToString();
        temperatureText.text = temperature.ToString() + "°C";
    }

    public void UIChange()
    {
        if (!menu.activeSelf && menu.GetComponent<Menu>().AnyBuildingBought())
        {
            menu.SetActive(true);
            buyHolder.SetActive(false);
        }
        else if (menu.activeSelf)
        {
            menu.SetActive(false);
            buyHolder.SetActive(true);
        }
    }

    public bool UIIsOpen()
    {
        return menu.activeSelf;
    }

    private void FixedUpdate()
    {
        if(timeTilSpawn <= 0)
        {
            timeTilSpawn = spawnTime;

            if(Random.Range(0, 11) <= 4) ice.Add(Instantiate(icePrefab, main));
            else ice.Add(Instantiate(icePrefab, spawns[Random.Range(0, spawns.Count)]));
        }

        timeTilSpawn -= Time.fixedDeltaTime;
    }
}
