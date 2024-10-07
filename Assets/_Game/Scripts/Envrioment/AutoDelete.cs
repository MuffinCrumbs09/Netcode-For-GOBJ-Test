using UnityEngine;

// Delete gameObject after lifeTime
public class AutoDelete : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float lifeTime;
    private float _curTime;

    private void Update()
    {
        if(_curTime >= lifeTime)
        {
            Destroy(gameObject);
        }

        _curTime += Time.deltaTime;
    }
}
