using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildSpawner : MonoBehaviour
{
   public GameObject objectToMonitor;
   public GameObject objectToPlace;

   [Tooltip("Space from the borders")]
   public float margin = 30.0f;
   [Tooltip("Space between elements")]
   public float padding = 20.0f;

   Renderer mRenderer;
   Bounds mBounds;

   public List<GameObject> horizontalObjects = new List<GameObject>();
   public List<GameObject> verticalObjects = new List<GameObject>();

   // Use this for initialization
   void Start()
   {
      if (objectToMonitor)
         mRenderer = objectToMonitor.GetComponent<Renderer>();
      if(objectToPlace)
      {
         Renderer rend = objectToPlace.GetComponent<Renderer>();
         if (objectToPlace != null)
            mBounds = rend.bounds;
      }

#if DEBUG //Let's assume that when the release is built, theese checks are passed
      if (objectToMonitor == null || mRenderer == null || objectToPlace == null || mBounds == null)
      {
         Debug.LogError("ChildSpawner " + name + ": component non correctly initialized.");
         enabled = false;
         return;
      }
#endif
   }

   // Update is called once per frame
   void Update()
   {
      Bounds bounds = mRenderer.bounds;
      //Rect rect = mRenderer.bounds;


      //Available width
      float width = bounds.size.x - 2 * margin;
      if(width > 0)
      {
         int numChairs = (int)(width / (mBounds.size.x + padding));
      }
   }
}
