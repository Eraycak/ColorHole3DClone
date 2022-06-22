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
    public float initialScale = 0.5f;
    Mesh GeneratedMesh;
    public bool gameIsActive;

    public void Move(BaseEventData baseEventData)
    {
        if (((PointerEventData)baseEventData).pointerCurrentRaycast.isValid)
        {
            transform.position = ((PointerEventData)baseEventData).pointerCurrentRaycast.worldPosition;
        }
    }

    private void Start()
    {
        gameIsActive = false;
        GameObject[] allGameObjects = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (var gameObject in allGameObjects)
        {
            if (gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), GeneratedMeshCollider, true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(other, GroundCollider, true);
        Physics.IgnoreCollision(other, GeneratedMeshCollider, false);
    }

    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, GroundCollider, false);
        Physics.IgnoreCollision(other, GeneratedMeshCollider, true);
    }

    private void FixedUpdate()
    {
        if (transform.hasChanged == true)
        {
            transform.hasChanged = false;
            hole2DCollider.transform.position = new Vector2(transform.position.x, transform.position.z);
            hole2DCollider.transform.localScale = transform.localScale * initialScale;
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
