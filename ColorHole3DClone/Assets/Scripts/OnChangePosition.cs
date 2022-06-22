using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnChangePosition : MonoBehaviour
{
    public PolygonCollider2D hole2DCollider;
    public PolygonCollider2D ground2DCollider;
    public MeshCollider GeneratedMeshCollider;
    public Collider GroundCollider;
    Mesh GeneratedMesh;

    public void Move(BaseEventData baseEventData)
    {
        if (((PointerEventData)baseEventData).pointerCurrentRaycast.isValid)
        {
            transform.position = ((PointerEventData)baseEventData).pointerCurrentRaycast.worldPosition;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        other.isTrigger = true;
    }

    private void OnTriggerExit(Collider other)
    {
        other.isTrigger = false;
    }

    private void FixedUpdate()
    {
        if (transform.hasChanged == true)
        {
            transform.hasChanged = false;
            hole2DCollider.transform.position = new Vector2(transform.position.x, transform.position.z);
            hole2DCollider.transform.localScale = transform.localScale;
            Make2DHole();
            Make3DMeshCollider();
        }
    }

    private void Make2DHole()
    {
        Vector2[] PointPositions = hole2DCollider.GetPath(0);
        for (int i = 0; i < PointPositions.Length; i++)
        {
            PointPositions[i] = hole2DCollider.transform.TransformPoint(PointPositions[i]);
        }
        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, PointPositions);
    }

    private void Make3DMeshCollider()
    {
        if (GeneratedMesh != null)
        {
            Destroy(GeneratedMesh);
        }
        GeneratedMesh = ground2DCollider.CreateMesh(true, true);
        GeneratedMeshCollider.sharedMesh = GeneratedMesh;
    }
}
