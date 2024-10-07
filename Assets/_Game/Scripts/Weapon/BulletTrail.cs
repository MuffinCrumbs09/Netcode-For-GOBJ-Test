using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Color color;
    [SerializeField] private float speed = 10f;

    private LineRenderer _lr;

    private void Start()
    {
        _lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        // Gradually decreases alpha to 0
        color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * speed);

        // Sets the start and end color
        _lr.startColor = color;
        _lr.endColor = color;
    }
}