using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SwitchVirtualCameraArea : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera targetVirtualCamera = default; //最初に設定
    private CinemachineVirtualCamera TargetVirtualCamera => targetVirtualCamera; //カメラ切り替えの記述

    private const int EnableVirtualCameraPriority = int.MaxValue;

    private CinemachineBrain cinemachineBrain;

    // Start is called before the first frame update
    void Start()
    {
        this.cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
        //this.GetComponent<Collider2D>.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player"))
        {
            return;
        }

        this.DisableCurrentVirtualCamera();
        this.EnableTargetVirtualCamera();
    }

    private void DisableCurrentVirtualCamera()
    {
        var current = this.cinemachineBrain.ActiveVirtualCamera as CinemachineVirtualCamera;
        current.Priority = 0; //重要！ここで前のマップのカメラのpriorityを０にする
    }

    private void EnableTargetVirtualCamera()
    {
        this.TargetVirtualCamera.enabled = true;
        this.TargetVirtualCamera.Priority = EnableVirtualCameraPriority; //ここで今のマップのカメラのpriorityをマックス
    }


}