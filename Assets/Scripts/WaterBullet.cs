using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBullet : MonoBehaviour
{
    public float LimitTime = 1.0f;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //transform.Translate (speed, 0, 0);

        if (time > LimitTime)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Fire")
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
    }
}