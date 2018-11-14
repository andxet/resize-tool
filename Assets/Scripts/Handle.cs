#define DEBUG
using UnityEngine;
using UnityEngine.EventSystems;


public class Handle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
   Mesh mMesh;
   Plane mPlane;
   public Resizer objectToResize;

   //////////////////////////////////////////////////////////
   void Start()
   {
      MeshFilter meshFilter = GetComponent<MeshFilter>();
      if (meshFilter != null)
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
   public void OnBeginDrag(PointerEventData eventData)
   {
      mPlane = new Plane(Vector3.up, transform.position);
      objectToResize.BeginResize(transform.position);
   }

   //////////////////////////////////////////////////////////
   public void OnDrag(PointerEventData eventData)
   {
      Camera cam = eventData.pressEventCamera;
      Ray ray = cam.ScreenPointToRay(Input.mousePosition);
      float distance;
      mPlane.Raycast(ray, out distance);
      //transform.position = ray.GetPoint(distance);
      objectToResize.Resize(ray.GetPoint(distance));
   }

   //////////////////////////////////////////////////////////
   public void OnEndDrag(PointerEventData eventData)
   {
   }
}
