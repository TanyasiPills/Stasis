using UnityEngine;
using UnityEngine.EventSystems;

public class User : MonoBehaviour
{
    [SerializeField]
    public float damage = 50f;

    Manager manager;

    private void Start()
    {
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<Manager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (!manager.UIIsOpen()) manager.UIChange();
                }
            }
        }
    }
}
