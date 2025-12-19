using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Ice : MonoBehaviour
{
    Vector3 mouseEnterPos;
    Vector3 mouseExitPos;

    bool timerOn = false;
    float time;

    [Range(0f, 1f)]
    public float cutTime = 0.2f;

    public float health = 100.0f;

    public User user;
    public GameObject cutPrefab;
    Manager manager;

    int rotSpeed;
    float fallSpeed;
    bool rotNegative;

    private void Start()
    {
        rotSpeed = Random.Range(10, 181);
        rotNegative = Random.value < 0.5f;
        fallSpeed = Random.Range(1f, 3f);

        user = GameObject.FindGameObjectWithTag("Player").GetComponent<User>();
        manager = GameObject.FindGameObjectWithTag("manager").GetComponent<Manager>();
    }

    private void OnMouseEnter()
    {
        if (manager.UIIsOpen()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            mouseEnterPos = hit.point;
            mouseEnterPos.z = -1f;
        }

        timerOn = true;
    }

    private void Update()
    {
        if (timerOn) time += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(oldPos.x, oldPos.y - fallSpeed * Time.fixedDeltaTime, oldPos.z);

        transform.position = newPos;

        float deltaRot = (rotNegative ? -1 : 1) * rotSpeed * Time.fixedDeltaTime;
        transform.Rotate(0f, deltaRot, 0f, Space.Self);
    }

    private void OnMouseExit()
    {
        if (manager.UIIsOpen()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            mouseExitPos = hit.point;
            mouseExitPos.z = -1f;
        }

        timerOn = false;

        if (time <= cutTime)
        {
            health -= user.damage;
            //
            Vector3 diff = mouseExitPos - mouseEnterPos;
            Quaternion rotation = Quaternion.LookRotation(diff, Vector3.up) * Quaternion.Euler(
                (mouseEnterPos.x < mouseExitPos.x) ? -90 : 90, -90, 0);
            Instantiate(cutPrefab, mouseEnterPos + diff / 2f, rotation);
        }

        time = 0;
    }
}
