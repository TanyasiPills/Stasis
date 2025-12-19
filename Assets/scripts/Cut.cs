using UnityEngine;

public class Cut : MonoBehaviour
{
    public float timeLeft = 0.1f;

    private void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0) Destroy(gameObject);
    }
}
