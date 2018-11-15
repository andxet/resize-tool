using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSpawner : MonoBehaviour
{
   enum Edge
   {
      NORTH,
      SOUTH,
      EAST,
      WEST
   }

   public GameObject objectToMonitor;
   public GameObject objectToPlace;

   [Tooltip("Space from the borders")]
   public float margin = 0.30f;
   [Tooltip("Space between elements")]
   public float padding = 0.20f;


   Renderer mFollowingRenderer;
   float mChildWidth;
   Bounds mLastBounds;


   Dictionary<Edge, List<GameObject>> mChilds = new Dictionary<Edge, List<GameObject>>();
   //List<GameObject> horizontalObjects = new List<GameObject>();
   //List<GameObject> verticalObjects = new List<GameObject>();

   //////////////////////////////////////////////////////////
   void Start()
   {
      Renderer[] rends = {};
      if (objectToMonitor)
         mFollowingRenderer = objectToMonitor.GetComponent<Renderer>();
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
      if (objectToMonitor == null || mFollowingRenderer == null || objectToPlace == null || rends.Length == 0)
      {
         Debug.LogError("ChildSpawner " + name + ": component non correctly initialized.");
         enabled = false;
         return;
      }
      #endif
      mChilds.Add(Edge.NORTH, new List<GameObject>());
      mChilds.Add(Edge.SOUTH, new List<GameObject>());
      mChilds.Add(Edge.WEST,  new List<GameObject>());
      mChilds.Add(Edge.EAST,  new List<GameObject>());
   }

   //////////////////////////////////////////////////////////
   void Update()
   {
      Bounds bounds = mFollowingRenderer.bounds;

      //Counterclockwise
      Vector3 startPoint = bounds.center;
      if (Mathf.Abs(bounds.size.x - mLastBounds.size.x) > 0.01f || Mathf.Abs(bounds.size.z - mLastBounds.size.z) > 0.01f)
      {
         FitChilds(mChilds[Edge.NORTH], bounds.size.x, Vector3.left, new Vector3(startPoint.x + bounds.extents.x, 0, startPoint.z + bounds.extents.z));
         FitChilds(mChilds[Edge.SOUTH], bounds.size.x, Vector3.right, new Vector3(startPoint.x - bounds.extents.x, 0, startPoint.z - bounds.extents.z));
         FitChilds(mChilds[Edge.WEST], bounds.size.z, Vector3.back, new Vector3(startPoint.x - bounds.extents.x, 0, startPoint.z + bounds.extents.z));
         FitChilds(mChilds[Edge.EAST], bounds.size.z, Vector3.forward, new Vector3(startPoint.x + bounds.extents.x, 0, startPoint.z - bounds.extents.z));
         mLastBounds = bounds;
      }
   }

   //////////////////////////////////////////////////////////
   void FitChilds(List<GameObject> currentList, float length, Vector3 direction, Vector3 startPoint)
   {
      //Available width
      float width = length - 2 * margin;
      if (width > 0)
      {
         //Number of child
         float childWithPadding = mChildWidth + padding;
         int numChairs = (int)(width / childWithPadding);
         float remain = width - numChairs * childWithPadding;
         if (remain > mChildWidth)
         {
            numChairs++;
            remain -= mChildWidth;
         }
         else
            remain += padding;

         //Create and delete gameobjects to match the number
         int childDifference = numChairs - currentList.Count;
         if (childDifference < 0)
         {
            List<GameObject> objectToDelete = currentList.GetRange(numChairs, Mathf.Abs(childDifference));
            foreach (GameObject go in objectToDelete)
               Destroy(go);
            currentList.RemoveRange(numChairs, Mathf.Abs(childDifference));
         }
         else if (childDifference > 0)
         {
            for (int i = 0; i < childDifference; i++)
               currentList.Add(Instantiate(objectToPlace));
         }

         //Place all the objects
         Quaternion rotation = Quaternion.FromToRotation(Vector3.right, direction);

         startPoint += direction * (margin + remain / 2 + mChildWidth / 2);
         foreach (GameObject go in currentList)
         {
            go.transform.position = startPoint;
            startPoint += direction * (mChildWidth + padding);

            //Rotate the object
            go.transform.localRotation = rotation;
         }
      }
   }
}
