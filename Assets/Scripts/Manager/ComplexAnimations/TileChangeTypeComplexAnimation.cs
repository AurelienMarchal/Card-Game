using GameLogic;
using GameLogic.GameState;
using UnityEngine;

public class TileChangeTypeComplexAnimation : ComplexAnimation
{
    TileManager tileManager;

    TileType tileType;
    
    [SerializeField]
    ParticleSystem particleSystem_; 

    [SerializeField]
    ParticleSystemRenderer particleSystemRenderer; 

    [SerializeField]
    Material natureTileMat;

    [SerializeField]
    Material cursedTileMat;


    public override bool Init(ActionState actionState) 
    {
        base.Init(actionState);
        finalStep = 2;
        var tileChangeTypeActionState = (TileChangeTypeActionState)actionState;

        if(tileChangeTypeActionState == null)
        {
            return false;
        }

        tileType = tileChangeTypeActionState.newType;

        tileManager = gameObject.GetComponentInParent<TileManager>();

        particleSystemRenderer = gameObject.GetComponent<ParticleSystemRenderer>();
        
        return tileManager != null && particleSystemRenderer != null;
    }
    public override void PlayStep()
    {
        switch (step)
        {
            case 0:
                currentlyAffecting.Add(tileManager);
                switch (tileType)
                {
                    case TileType.Nature:
                        particleSystemRenderer.sharedMaterial = natureTileMat;
                        particleSystem_.Play();
                        break;
                    case TileType.CurseSource:
                        particleSystemRenderer.sharedMaterial = cursedTileMat;
                        particleSystem_.Play();
                        break;
                    default:
                        break;
                }

                break;

            case 1:
                tileManager.tileState.tileType = tileType;
                tileManager.UpdateVisuals();
                currentlyAffecting.Remove(tileManager);
                break;
            case 2: 
                currentlyAffecting.Remove(tileManager);
                break;
        }
        
    }

    public override bool StepFinished()
    {
        switch (step)
        {
            case 0 :
                return !particleSystem_.isEmitting;
            case 1 :
                return !particleSystem_.isPlaying;
        }

        return true;
    }
}
