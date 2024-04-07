using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// PlayerInteractions Script.
/// </summary>
public class PlayerInteractions : Script
{
    [Serialize, ShowInEditor] Actor rayCastOrigin, camera;
    [Serialize, ShowInEditor] MaterialBase highlightedTile, tileMaterial;
    [Serialize, ShowInEditor] UIControl pickUpPrompt, placePrompt, victoryPrompt;
    [Serialize, ShowInEditor] AudioSource victorySoundAS, otherWorldSound;
    private Actor currentTile, lastTile, oddity;
    private RayCastHit hit;
    private Vector3 viewDir;
    private bool handsFull, youWon;
    private int portalTileNumber, listsMatch;
    private float timer;

    public override void OnStart()
    {
        
    }    
    
    public override void OnUpdate()
    {
        viewDir = rayCastOrigin.Position - camera.Parent.Position;
        viewDir = viewDir.Normalized;

        if (Physics.RayCast(rayCastOrigin.Position, viewDir, out hit, 350f))
        {            
            //get current tile
            if (hit.Collider.Parent.TryGetScript<PortalTiles>(out PortalTiles portalTiles))
            {
                currentTile = hit.Collider.Parent;
                portalTileNumber = currentTile.GetScript<PortalTiles>().portalTileNumber;
                currentTile.As<StaticModel>().SetMaterial(0, highlightedTile);
                //reset old tile's material
                if ((lastTile != null) && (lastTile != currentTile))
                {
                    lastTile.As<StaticModel>().SetMaterial(0, tileMaterial);                    
                }
                lastTile = currentTile;
                if ((handsFull) && (currentTile.GetScript<PortalTiles>().occupied == false))
                {
                    placePrompt.IsActive = true;
                    //place oddity on the tile
                    if (Input.GetKeyDown(KeyboardKeys.E))
                    {
                        oddity.Position = currentTile.Position + Vector3.Up * 25f;
                        oddity.IsActive = true;
                        oddity.SetParent(currentTile, worldPositionsStays: true, canBreakPrefabLink: false);
                        handsFull = false;
                        currentTile.GetScript<PortalTiles>().occupied = true;
                        PluginManager.GetPlugin<PortalPlugin>().odditiesTilePlacement[portalTileNumber] = 
                            oddity.GetScript<OdditiesScript>().oddityListNumber;
                        oddity.GetScript<OdditiesScript>().tileOccupied = portalTileNumber;

                        for (int i = 0; i<9;  i++)
                        {
                            if (PluginManager.GetPlugin<PortalPlugin>().odditiesTilePlacement[i] == PluginManager.GetPlugin<PortalPlugin>().odditiesMapSpawns[i])
                            {
                                listsMatch++;
                            }
                            
                        }
                        
                        if (listsMatch == 9)
                        {
                            victoryPrompt.IsActive = true;                            
                            victorySoundAS.Play();
                            PluginManager.GetPlugin<PortalPlugin>().inOtherWorld = false;
                            PluginManager.GetPlugin<PortalPlugin>().youDidIt = true;
                            Actor.Position = PluginManager.GetPlugin<PortalPlugin>().playerSpawnPosition;
                            youWon = true;
                            Debug.Log("victory");
                            
                        }
                        else
                        {
                            listsMatch = 0;
                        }                       
                        
                    }
                }
            }
            else
            {
                placePrompt.IsActive = false;
                //reset old tile's material if we are looking elsewhere
                if (lastTile != null)
                {
                    lastTile.As<StaticModel>().SetMaterial(0, tileMaterial);
                    lastTile = null;
                    currentTile = null;
                }
            }
            //looking at an oddity while not holding another oddity
            if ((hit.Collider.HasTag("oddity"))&&(!handsFull))
            {
                pickUpPrompt.IsActive = true;
                if (Input.GetKeyDown(KeyboardKeys.E))
                {
                    oddity = hit.Collider.Parent;                    
                    oddity.IsActive = false;
                    if (oddity.GetScript<OdditiesScript>().tileOccupied != 9)
                    {
                        oddity.Parent.GetScript<PortalTiles>().occupied = false;
                        
                        PluginManager.GetPlugin<PortalPlugin>().odditiesTilePlacement[oddity.GetScript<OdditiesScript>().tileOccupied] = 3;
                        oddity.GetScript<OdditiesScript>().tileOccupied = 9;

                    }
                    handsFull = true;
                }
            }
            else
            {
                pickUpPrompt.IsActive = false;
            }
            
        }
        if (youWon)
        {
            timer += Time.DeltaTime;
        }
        if (timer > 5)
        {
            Engine.RequestExit();
        }
    }
}
