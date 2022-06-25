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
    private bool AutoControlIsActive = false;

    public void Move(BaseEventData baseEventData)
    {
        if (((PointerEventData)baseEventData).pointerCurrentRaycast.isValid && !AutoControlIsActive)//if player touches to screen, moves hole to that position
        {
            transform.position = ((PointerEventData)baseEventData).pointerCurrentRaycast.worldPosition;
        }
    }

    public void ActivateAutoControl()
    {
        AutoControlIsActive = true;
    }

    public void DeactivateAutoControl()
    {
        AutoControlIsActive = false;
    }

    private void OnTriggerEnter(Collider other)//if hole triggers objects, it turns on their triggers to make them fall and changes it's use gravity to prevent any bug related to the gravity
    {
        if (!AutoControlIsActive)
        {
            other.isTrigger = true;
            other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            other.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    private void OnTriggerExit(Collider other)//if hole not triggers objects, it turns off their triggers to make them fall
    {
        if (!AutoControlIsActive)
        {
            other.isTrigger = false;
            other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            other.gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    private void FixedUpdate()
    {
        if (transform.hasChanged == true)
        {
            transform.hasChanged = false;
            hole2DCollider.transform.position = new Vector2(transform.position.x, transform.position.z);//moves hole to the position which player wants
            Make2DHole();
            Make3DMeshCollider();
        }
    }

    private void Make2DHole()//moves ground2DCollider as much as hole2DCollider at the same time
    {
        Vector2[] PointPositions = hole2DCollider.GetPath(0);
        for (int i = 0; i < PointPositions.Length; i++)//changes pointPositions to the holeCollider related points
        {
            PointPositions[i] = hole2DCollider.transform.TransformPoint(PointPositions[i]);
        }
        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, PointPositions);//moves ground2DCollider to new pointPositions
    }

    private void Make3DMeshCollider()
    {
        if (GeneratedMesh != null)//Destroys generatedMesh apply changes
        {
            Destroy(GeneratedMesh);
        }
        GeneratedMesh = ground2DCollider.CreateMesh(true, true);//creates mesh which is related to the ground2DCollider and has it's positions and rotations
        GeneratedMeshCollider.sharedMesh = GeneratedMesh;//applies changes to the generatedMeshCollider
    }
}
