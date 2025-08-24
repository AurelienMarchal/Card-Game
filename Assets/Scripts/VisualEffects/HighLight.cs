using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    //we assign all the renderers here through the inspector
    [SerializeField]
    private List<Renderer> renderers;
    
    [SerializeField]
    private Color color = Color.white;

    [SerializeField]
    private Color selectedColor = Color.magenta;

    public void ToggleHighlight(bool val, bool selected = false)
    {

        foreach (var renderer in renderers)
        {
            foreach (var material in renderer.materials)
            {
                if (val)
                {
                    //We need to enable the EMISSION
                    material.EnableKeyword("_EMISSION");
                    //before we can set the color
                    material.SetColor("_EmissionColor", selected ? selectedColor : color);
                }
                else
                {
                    material.DisableKeyword("_EMISSION");
                }
                
                
            }
            //A single child-object might have mutliple materials on it
            //that is why we need to all materials with "s"
        }
    }


}