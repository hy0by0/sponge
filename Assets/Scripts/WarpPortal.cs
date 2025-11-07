using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPortal : MonoBehaviour
{
    public GameObject warpexit;
    //public GameObject warpG;
    //public GameObject ExitwarpG;
    Vector3 pos;
    float AddY;
    float warp_x;
    float warp_y;
    float warp_z;
    //bool FromWarpG = false;

    // Start is called before the first frame update
    void Start()
    {
        pos = new Vector3(warpexit.transform.position.x, warpexit.transform.position.y, warpexit.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {

        //if (ExitwarpG != null)
        //{

        //pos = new Vector3(warpexit.transform.position.x, warpexit.transform.position.y, warpexit.transform.position.z);
        //pos = new Vector3(warpG_x, warpG_y, warpG_z);
        //}

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //AddY = this.gameObject.transform.position.y - collider.gameObject.transform.position.y;
        //collider.gameObject.transform.position = new Vector3(pos.x, pos.y - AddY, pos.z);
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.transform.position = new Vector3(pos.x, pos.y, pos.z);
        }

    }
}