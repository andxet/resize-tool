using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSpawner : MonoBehaviour
{
   public GameObject objectToMonitor;
   public GameObject objectToPlace;

   [Tooltip("Space from the borders")]
   public float margin = 0.30f;
   [Tooltip("Space between elements")]
   public float padding = 0.20f;

   Renderer mRenderer;
   float mChildWidth;

   List<GameObject> horizontalObjects = new List<GameObject>();
   List<GameObject> verticalObjects = new List<GameObject>();

   //////////////////////////////////////////////////////////
   void Start()
   {
      Renderer[] rends = {};
      if (objectToMonitor)
         mRenderer = objectToMonitor.GetComponent<Renderer>();
      if (objectToPlace)
      {
         
         //GameObject go = Instantiate(objectToPlace);
         //Renderer rend = go.GetComponent<Renderer>();
         rends = objectToPlace.GetComponentsInChildren<Renderer>();
         if (rends.Length != 0)
         {
            Bounds bound = rends[0].bounds;
            for (int i = 1; i < rends.Length; i++)
               bound.Encapsulate(rends[i].bounds);
            mChildWidth = bound.size.x;
         }
      }

#if DEBUG //Let's assume that when the release is built, theese checks are passed
      if (objectToMonitor == null || mRenderer == null || objectToPlace == null || rends.Length == 0)
      {
         Debug.LogError("ChildSpawner " + name + ": component non correctly initialized.");
         enabled = false;
         return;
      }
#endif
   }

   //////////////////////////////////////////////////////////
   void Update()
   {
      Bounds bounds = mRenderer.bounds;
      //Rect rect = mRenderer.bounds;


      //Available width
      float width = bounds.size.x - 2 * margin;
      if (width > 0)
      {
         
         float chairWithPadding = mChildWidth + padding;
         int numChairs = (int)(width / chairWithPadding);
         float remain = width - numChairs * chairWithPadding;
         if (remain > mChildWidth)
         {
            numChairs++;
            remain -= mChildWidth;
         }
         else
            remain += padding;

         //Be sure that the right amount of child are present
         int childDifference = numChairs - horizontalObjects.Count;
         if(childDifference < 0)
         {
            List<GameObject> objectToDelete = horizontalObjects.GetRange(numChairs, Mathf.Abs(childDifference));
            foreach (GameObject go in objectToDelete)
               Destroy(go);
            horizontalObjects.RemoveRange(numChairs, Mathf.Abs(childDifference));
         }
         else if(childDifference > 0)
         {
            for (int i = 0; i < childDifference; i++)
               horizontalObjects.Add(Instantiate(objectToPlace));
         }


         Vector3 startingPoint = bounds.center;
         startingPoint.x += -bounds.extents.x + margin + remain/2 + mChildWidth/2;
         startingPoint.y = 0;
         startingPoint.z += -bounds.extents.z;
         foreach(GameObject go in horizontalObjects)
         {
            go.transform.position = startingPoint;
            startingPoint.x += mChildWidth + padding;
         }
      }
   }
}
