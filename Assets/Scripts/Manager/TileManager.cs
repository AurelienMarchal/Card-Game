using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

using UnityEngine;
using UnityEngine.UI;

using GameLogic;
using GameLogic.GameState;
using System;

[System.Serializable]
public class TileManagerEvent : UnityEvent<TileManager>
{
}

[RequireComponent(typeof(Highlight))]
public class TileManager : MonoBehaviour
{   
    [Obsolete]
    private Tile tile_;

    [Obsolete]
    public Tile tile
    {
        get
        {
            return tile_;
        }

        set
        {
            tile_ = value;
            UpdateAccordingToTile();
        }
    }

    private TileState tileState_;

    public TileState tileState{
        get{
            return tileState_;
        }

        set{
            tileState_ = value;
            UpdateAccordingToTileState();
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
                tileSelectedEvent.Invoke(this);
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

    public bool displayInfoUI { 
        set{
            infoQuad.SetActive(value);
        }}

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

    [SerializeField]
    GameObject infoQuad;


    public TileManagerEvent tileSelectedEvent = new TileManagerEvent();

    public TileManagerEvent tileMouseDownEvent = new TileManagerEvent();

    public TileManagerEvent tileMouseUpEvent = new TileManagerEvent();

    public TileManagerEvent tileHoverEnterEvent = new TileManagerEvent();

    public TileManagerEvent tileHoverExitEvent = new TileManagerEvent();
    
    // Start is called before the first frame update
    void Start()
    {
        hovered = false;
        selected = false;
        displayInfoUI = false;
    }

    // Update is called once per frame
    void Update(){
    }

    private void UpdateAccordingToTileState()
    {
        if (tileState == null)
        {
            return;
        }

        //la pos est gere par le board 

        
    }

    public void UpdateVisuals()
    {
        Material[] matArray = tileRenderer.sharedMaterials;
        
        switch (tileState.tileType){
            case TileType.Standard: matArray[1] = standardTileMat ; break;
            case TileType.Nature: matArray[1] = natureTileMat; break;
            case TileType.Cursed: matArray[1] = cursedTileMat; break;
            case TileType.CurseSource: matArray[1] = curseSourceTileMat; break;
            case TileType.WillGetCursed: matArray[1] = willGetCursedTileMat; break;
            default: matArray[1] = standardTileMat; break;
        }

        tileRenderer.sharedMaterials = matArray;
    }

    void OnMouseDown(){
        tileMouseDownEvent.Invoke(this);
    }

    void OnMouseUp(){
        tileMouseUpEvent.Invoke(this);
    }

    
    void OnMouseOver(){
        tileHoverEnterEvent.Invoke(this);
        hovered = true;
    }

    void OnMouseExit(){
        tileHoverExitEvent.Invoke(this);
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

    [Obsolete]
    public void UpdateAccordingToTile()
    {
        Material[] matArray = tileRenderer.sharedMaterials;

        switch (tile.tileType)
        {
            case TileType.Standard: matArray[1] = standardTileMat; break;
            case TileType.Nature: matArray[1] = natureTileMat; break;
            case TileType.Cursed: matArray[1] = cursedTileMat; break;
            case TileType.CurseSource: matArray[1] = curseSourceTileMat; break;
            case TileType.WillGetCursed: matArray[1] = willGetCursedTileMat; break;
            default: matArray[1] = standardTileMat; break;
        }

        tileRenderer.sharedMaterials = matArray;
    }
}
