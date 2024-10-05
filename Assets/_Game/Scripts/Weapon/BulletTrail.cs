using System;
using UnityEngine;

namespace NameEnvironment
{
    public class BulletTrail : MonoBehaviour
    {
        [SerializeField] private Color color;

        [SerializeField] private float speed = 10f;

        private LineRenderer _lr;


        private void Start()
        {
            _lr = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            color.a = Mathf.Lerp(color.a, 0, Time.deltaTime * speed);

            _lr.startColor = color;
            _lr.endColor = color;
        }
    }
}