using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// TileScript Script.
/// </summary>
public class TileScript : Script
{
    public Actor playerSpawn;
    public List<Actor> odditySpawns;
    [Serialize, ShowInEditor] Collider portalSpace;
    [Serialize, ShowInEditor] Actor portalExit, portalPosition;

    
    public override void OnEnable()
    {
        if (portalSpace != null)
        {
            portalSpace.TriggerEnter += InPortalSpace;
            portalSpace.TriggerExit += NotInPortalSpace;
        }
        
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        if (portalSpace != null)
        {
            portalSpace.TriggerEnter -= InPortalSpace;
            portalSpace.TriggerExit -= NotInPortalSpace;
        }
    }

    /// <inheritdoc/>
    
    private void InPortalSpace(PhysicsColliderActor other)
    {
        if (other.HasTag("Player"))
        {
            PluginManager.GetPlugin<PortalPlugin>().inPortalSpace = true;
            PluginManager.GetPlugin<PortalPlugin>().portalPosition = portalPosition.Position;
            PluginManager.GetPlugin<PortalPlugin>().portalSpawnEmpty = portalPosition;
            PluginManager.GetPlugin<PortalPlugin>().portalExit = portalExit.Position;            
        }
    }
    private void NotInPortalSpace(PhysicsColliderActor other)
    {
        if (other.HasTag("Player"))
        {
            PluginManager.GetPlugin<PortalPlugin>().inPortalSpace = false;           
        }
    }
    
}
