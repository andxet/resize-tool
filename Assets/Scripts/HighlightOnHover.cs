#define DEBUG
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
    public Color hoverColor = Color.red;
    Material mMaterial;
    Color mOriginalColor;

    //////////////////////////////////////////////////////////
    void Start () 
    {
        Renderer rend = GetComponent<Renderer>();
        if(rend != null)
            mMaterial = rend.material;
#if DEBUG //Let's assume that when the release is built, theese checks are passed
        if (rend == null || mMaterial == null)
        {
        Debug.LogError("HighlightOnHover " + name + ": component non correctly initialized.");
            enabled = false;
            return;
        }
#endif
        mOriginalColor = mMaterial.color;
	}

    //////////////////////////////////////////////////////////
	void Update () 
    {
		
	}

    //////////////////////////////////////////////////////////
    public void OnPointerEnter(PointerEventData eventData)
    {
        mMaterial.color = hoverColor;
    }

    //////////////////////////////////////////////////////////
    public void OnPointerExit(PointerEventData eventData)
    {
        mMaterial.color = mOriginalColor;
    }

}
