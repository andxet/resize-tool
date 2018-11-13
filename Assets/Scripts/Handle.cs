#define DEBUG
using UnityEngine;
using UnityEngine.EventSystems;

public class Handle : MonoBehaviour, IDragHandler, IEndDragHandler 
{
    Mesh mMesh;
    Plane mPlane;

    //////////////////////////////////////////////////////////
    void Start () 
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if(meshFilter != null)
            mMesh = meshFilter.mesh;
#if DEBUG //Let's assume that when the release is built, theese checks are passed
        if (meshFilter == null || mMesh == null)
        {
        Debug.LogError("Handle " + name + ": component non correctly initialized.");
            enabled = false;
            return;
        }
#endif
        mPlane = new Plane(Vector3.up, transform.position.y);
	}

    //////////////////////////////////////////////////////////
    void Update()
    {

    }

    //////////////////////////////////////////////////////////
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("DRAG");
        Camera cam = eventData.pressEventCamera;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //mPlane.distance = transform.position.y;
        float distance;
        mPlane.Raycast(ray, out distance);
        Debug.Log(ray.GetPoint(distance));
        transform.position = ray.GetPoint(distance);
    }

    //////////////////////////////////////////////////////////
    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("END DRAG");
    }

    //////////////////////////////////////////////////////////
    void OnDrawGizmos()
    {
        Gizmos.DrawLine(mPlane.normal * mPlane.distance, mPlane.normal * mPlane.distance + mPlane.normal);
    }
}
