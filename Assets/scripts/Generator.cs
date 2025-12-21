using UnityEngine;
using UnityEngine.Networking;

public class Generator : MonoBehaviour
{
    public enum MachineType
    {
        Chunky,
        Smol,
        Suspicious
    }

    public MachineType type;
    float generatedHeat;
    int health;

    Manager manager;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<Manager>();

        switch (type)
        {
            case MachineType.Chunky:
                generatedHeat = 1;
                health = 100;
                break;
            case MachineType.Smol:
                generatedHeat = 0.1f;
                health = 25;
                break;
            case MachineType.Suspicious:
                generatedHeat = 0.01f;
                health = 200;
                break;
        }
    }

    void Update()
    {
        manager.AddHeat(generatedHeat * Time.deltaTime);
    }
}
