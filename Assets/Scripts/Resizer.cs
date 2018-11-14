using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Resizer : MonoBehaviour
{
   Renderer mRenderer; //This could be a vector
   GameObject mChild;
   Vector3 mPivot;
   Vector3 mInitialSize;
   Vector3 mStartScale;

   //////////////////////////////////////////////////////////
   void Start()
   {
      mRenderer = GetComponentInChildren<Renderer>();
      mChild = mRenderer.gameObject;
#if DEBUG //Let's assume that when the release is built, theese checks are passed
      if (mRenderer == null || mChild == null)
      {
         Debug.LogError("Resizer " + name + ": component non correctly initialized.");
         enabled = false;
         return;
      }
#endif
   }

   //////////////////////////////////////////////////////////
   public void BeginResize(Vector3 position)
   {
      Bounds bb = mRenderer.bounds;

      mStartScale = transform.localScale;
      mInitialSize = bb.size;

      //Find the new pivot
      mPivot = bb.center + (bb.center - position);

      //Change the pivot
      Vector3 childPosition = mChild.transform.position;
      transform.position = mPivot;
      mChild.transform.position = childPosition;
   }


   //Vector3 Dest;
   //////////////////////////////////////////////////////////
   public void Resize(Vector3 boundPosition)
   {
      //move into BoundingBox space
      Vector3 newExtents = boundPosition - mPivot;
      newExtents.x = Mathf.Abs(newExtents.x);
      newExtents.y = Mathf.Abs(newExtents.y);
      newExtents.z = Mathf.Abs(newExtents.z);
      //Dest = newExtents;
      
      //Calculate the ratio between the new BB and the initial
      Vector3 increment = new Vector3(newExtents.x / mInitialSize.x, newExtents.y / mInitialSize.y, newExtents.z / mInitialSize.z);

      //Multiply the initial scale with the ratio
      Vector3 finalScale = mStartScale;
      finalScale.Scale(increment);

      //Assign the scale
      transform.localScale = finalScale;
   }

   //////////////////////////////////////////////////////////
   private void OnDrawGizmos()
   {
      if (mRenderer != null && mChild != null)
      {
         Bounds bb = mRenderer.bounds;
         Gizmos.color = Color.blue;
         Gizmos.DrawSphere(mChild.transform.position, 0.01f);

         //Origin of the BoundingBox
         Gizmos.color = Color.red;
         Gizmos.DrawSphere(mChild.transform.position - bb.extents, 0.01f);
         //Gizmos.DrawLine(mChild.transform.position - bb.extents, mChild.transform.position - bb.extents + Dest);

         //Draw the pivot
         Gizmos.color = Color.cyan;
         Gizmos.DrawSphere(mPivot, 0.01f);

         //Draw the bounding box
         Gizmos.color = Color.yellow;
         float xHalf = (bb.extents.x + 0.01f);
         float yHalf = (bb.extents.y + 0.01f);
         float zHalf = (bb.extents.z + 0.01f);

         //Front
         Vector3 fTopLeftCorner = new Vector3(mChild.transform.position.x - xHalf, mChild.transform.position.y + yHalf, mChild.transform.position.z + zHalf);
         Vector3 fTopRightCorner = new Vector3(mChild.transform.position.x + xHalf, mChild.transform.position.y + yHalf, mChild.transform.position.z + zHalf);
         Vector3 fBottomLeftCorner = new Vector3(mChild.transform.position.x - xHalf, mChild.transform.position.y - yHalf, mChild.transform.position.z + zHalf);
         Vector3 fBottomRightCorner = new Vector3(mChild.transform.position.x + xHalf, mChild.transform.position.y - yHalf, mChild.transform.position.z + zHalf);


         //Back
         Vector3 bTopLeftCorner = new Vector3(mChild.transform.position.x - xHalf, mChild.transform.position.y + yHalf, mChild.transform.position.z - zHalf);
         Vector3 bTopRightCorner = new Vector3(mChild.transform.position.x + xHalf, mChild.transform.position.y + yHalf, mChild.transform.position.z - zHalf);
         Vector3 bBottomLeftCorner = new Vector3(mChild.transform.position.x - xHalf, mChild.transform.position.y - yHalf, mChild.transform.position.z - zHalf);
         Vector3 bBottomRightCorner = new Vector3(mChild.transform.position.x + xHalf, mChild.transform.position.y - yHalf, mChild.transform.position.z - zHalf);


         Gizmos.DrawLine(fTopLeftCorner, fTopRightCorner);
         Gizmos.DrawLine(fTopRightCorner, fBottomRightCorner);
         Gizmos.DrawLine(fBottomRightCorner, fBottomLeftCorner);
         Gizmos.DrawLine(fBottomLeftCorner, fTopLeftCorner);

         Gizmos.DrawLine(bTopLeftCorner, bTopRightCorner);
         Gizmos.DrawLine(bTopRightCorner, bBottomRightCorner);
         Gizmos.DrawLine(bBottomRightCorner, bBottomLeftCorner);
         Gizmos.DrawLine(bBottomLeftCorner, bTopLeftCorner);


         Gizmos.DrawLine(fTopLeftCorner, bTopLeftCorner);
         Gizmos.DrawLine(fTopRightCorner, bTopRightCorner);
         Gizmos.DrawLine(fBottomLeftCorner, bBottomLeftCorner);
         Gizmos.DrawLine(fBottomRightCorner, bBottomRightCorner);
      }
   }
}
