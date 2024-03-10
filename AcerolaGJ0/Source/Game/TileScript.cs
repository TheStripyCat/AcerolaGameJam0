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
    private bool inPortalSpace;
    
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
    public override void OnUpdate()
    {
        // Here you can add code that needs to be called every frame
    }
    private void InPortalSpace(PhysicsColliderActor other)
    {
        if (other.HasTag("Player"))
        {
            inPortalSpace = true;
            Debug.Log("portal space");
        }
    }
    private void NotInPortalSpace(PhysicsColliderActor other)
    {
        if (other.HasTag("Player"))
        {
            inPortalSpace = false;
            Debug.Log("not in portal space");
        }
    }
    public bool PortalSpace()
    {
        return inPortalSpace;
    }
}
