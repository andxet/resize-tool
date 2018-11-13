using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Resizer : MonoBehaviour
{

   Renderer mRenderer;

   //////////////////////////////////////////////////////////
   void Start()
   {
      mRenderer = GetComponent<Renderer>();
#if DEBUG //Let's assume that when the release is built, theese checks are passed
      if (mRenderer == null)
      {
         Debug.LogError("Resizer " + name + ": component non correctly initialized.");
         enabled = false;
         return;
      }
#endif
   }

   //////////////////////////////////////////////////////////
   void Update()
   {

   }

   //////////////////////////////////////////////////////////
   public void Resize(Vector3 boundPosition)
   {

      Bounds bb = mRenderer.bounds;
      Vector3 previousExtents = bb.extents;
      Vector3 newExtents = boundPosition - transform.position;

      //make the new extents positive


      //transform.localScale = newExtents * 2;
      transform.position -= newExtents / 2;



      //boundPosition.z = 0;
      //Vector3 newBound = boundPosition - transform.position;

      /*Vector3 scale = new Vector3(bb.size.x / newBound.x, bb.size.y / newBound.y, bb.size.z / newBound.z);
      transform.position -= (transform.position -( bb.extents + boundPosition))/2;
      transform.localScale = (boundPosition - (transform.position - bb.extents)) * 2;*/


      //Similar to working
      /*Vector3 scale = new Vector3(bb.size.x / newBound.x, bb.size.y / newBound.y, bb.size.z / newBound.z);
      transform.position += boundPosition - transform.position - bb.extents;
      transform.localScale += transform.position - bb.extents + boundPosition;*/

      /*newBound.x = Mathf.Abs(newBound.x);
      newBound.y = Mathf.Abs(newBound.y);
      newBound.z = Mathf.Abs(newBound.z);
      Vector3 delta = newBound - bb.size;
      delta.Scale(0.5f);
      transform.position += delta;*/
   }

   //////////////////////////////////////////////////////////
   private void OnDrawGizmos()
   {
      if (mRenderer != null)
      {
         Bounds bb = mRenderer.bounds;
         Gizmos.color = Color.blue;
         Gizmos.DrawSphere(transform.position, 0.01f);

         Gizmos.color = Color.red;
         Gizmos.DrawSphere(transform.position - bb.extents, 0.01f);

         Gizmos.color = Color.yellow;
         float xHalf = (bb.extents.x + 0.01f);
         float yHalf = (bb.extents.y + 0.01f);
         float zHalf = (bb.extents.z + 0.01f);

         //Front
         Vector3 fTopLeftCorner = new Vector3(transform.position.x - xHalf, transform.position.y + yHalf, transform.position.z + zHalf);
         Vector3 fTopRightCorner = new Vector3(transform.position.x + xHalf, transform.position.y + yHalf, transform.position.z + zHalf);
         Vector3 fBottomLeftCorner = new Vector3(transform.position.x - xHalf, transform.position.y - yHalf, transform.position.z + zHalf);
         Vector3 fBottomRightCorner = new Vector3(transform.position.x + xHalf, transform.position.y - yHalf, transform.position.z + zHalf);


         //Back
         Vector3 bTopLeftCorner = new Vector3(transform.position.x - xHalf, transform.position.y + yHalf, transform.position.z - zHalf);
         Vector3 bTopRightCorner = new Vector3(transform.position.x + xHalf, transform.position.y + yHalf, transform.position.z - zHalf);
         Vector3 bBottomLeftCorner = new Vector3(transform.position.x - xHalf, transform.position.y - yHalf, transform.position.z - zHalf);
         Vector3 bBottomRightCorner = new Vector3(transform.position.x + xHalf, transform.position.y - yHalf, transform.position.z - zHalf);


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
