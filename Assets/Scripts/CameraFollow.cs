using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject[] locationPoints;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    private GameObject player;

    private void Update()
    {
        player = GameObject.FindWithTag("Player");

        DetectedLocationWithPlayer();
    }
    private void DetectedLocationWithPlayer()
    {
        foreach (var colliderPoint in locationPoints)
        {
            if(colliderPoint.gameObject.GetComponent<PolygonCollider2D>().OverlapPoint(player.gameObject.transform.position))
            {
                virtualCamera.GetComponent<CinemachineVirtualCamera>().Follow = colliderPoint.GetComponent<GetTransform>().m_Transform;
                virtualCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = colliderPoint.GetComponent<PolygonCollider2D>();               
            }
        }
    }
    
}
