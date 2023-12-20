using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

using UnityEngine;

[System.Serializable]
public class TileManagerEvent : UnityEvent<TileManager>
{
}

[RequireComponent(typeof(Highlight))]
public class TileManager : MonoBehaviour
{
    private Tile tile_;

    public Tile tile{
        get{
            return tile_;
        }

        set{
            tile_ = value;
            UpdateAccordingToTile();
        }
    }

    private bool selected_;

    public bool selected{
        get{
            return selected_;
        }
        set{
            selected_ = value;

            if(selected){
                selectedEvent.Invoke(this);
            }
            /*
            var highlight = GetComponent<Highlight>();
            if(highlight != null){
                highlight.ToggleHighlight(selected, true);
            }
            */
        }
    }

    private bool hovered_;

    public bool hovered{
        get{
            return hovered_;
        }
        private set{
            hovered_ = value;
            var highlight = GetComponent<Highlight>();
            if(highlight != null){
                highlight.ToggleHighlight(hovered);
            }
            
        }
    }

    [SerializeField]
    Renderer tileRenderer; 

    [SerializeField]
    Material standardTileMat;

    [SerializeField]
    Material natureTileMat;

    [SerializeField]
    Material cursedTileMat;

    [SerializeField]
    Material willGetCursedTileMat;

    [SerializeField]
    Material curseSourceTileMat;



    public TileManagerEvent selectedEvent = new TileManagerEvent();

    public TileManagerEvent clickedEvent = new TileManagerEvent();
    
    // Start is called before the first frame update
    void Start()
    {
        hovered = false;
        selected = false;
    }

    // Update is called once per frame
    void Update(){
    }

    void OnMouseDown(){
        clickedEvent.Invoke(this);
    }

    
    void OnMouseOver(){
        hovered = true;
    }

    void OnMouseExit(){
        hovered = false;
    }


    public static void UnselectEveryTile(){
        foreach(GameObject tile in GameObject.FindGameObjectsWithTag("Tile")){
            var tileManager = tile.GetComponent<TileManager>();
            if(tileManager != null){
                tileManager.selected = false;
            }
        }
    }

    public void UpdateAccordingToTile(){
        Material[] matArray = tileRenderer.sharedMaterials;
        
        switch (tile.tileType){
            case TileType.Standard: matArray[1] = standardTileMat ; break;
            case TileType.Nature: matArray[1] = natureTileMat; break;
            case TileType.Cursed: matArray[1] = cursedTileMat; break;
            case TileType.CurseSource: matArray[1] = curseSourceTileMat; break;
            case TileType.WillGetCursed: matArray[1] = willGetCursedTileMat; break;
            default: matArray[1] = standardTileMat; break;
        }

        tileRenderer.sharedMaterials = matArray;
    }
}
