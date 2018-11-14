using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleGizmo : MonoBehaviour
{
   public enum HandlePosition
   {
      TopLeft,
      TopRight,
      BottomLeft,
      BottomRight
   }

   public GameObject objectToDecorate;
   public HandlePosition position;

   Renderer[] mRenderers;

   //////////////////////////////////////////////////////////
   void Start()
   {
      if (objectToDecorate != null)
         mRenderers = objectToDecorate.GetComponentsInChildren<Renderer>();
#if DEBUG //Let's assume that when the release is built, theese checks are passed
      if (objectToDecorate == null || mRenderers.Length == 0)
      {
         Debug.LogError("HandleGizmo " + name + ": component non correctly initialized.");
         enabled = false;
         return;
      }
#endif
   }

   //////////////////////////////////////////////////////////
   void Update()
   {
      Bounds bounds = mRenderers[0].bounds;
      for (int i = 1; i < mRenderers.Length; i++)
         bounds.Encapsulate(mRenderers[i].bounds);

      Vector3 offset = bounds.extents;
      switch (position)
      {
         case HandlePosition.TopLeft:
            offset = new Vector3(-offset.x, 0.0f, offset.z);
            break;
         case HandlePosition.TopRight:
            offset = new Vector3(offset.x, 0.0f, offset.z);
            break;
         case HandlePosition.BottomLeft:
            offset = new Vector3(-offset.x, 0.0f, -offset.z);
            break;
         case HandlePosition.BottomRight:
            offset = new Vector3(offset.x, 0.0f, -offset.z);
            break;
      }
      transform.position = bounds.center + offset;
   }
}
