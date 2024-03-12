using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// PortalScript Script.
/// </summary>
public class PortalScript : Script
{
    [Serialize, ShowInEditor] Collider portalCollider;

    
    public override void OnEnable()
    {
        portalCollider.TriggerEnter += EnterThePortal;
        PluginManager.GetPlugin<PortalPlugin>().inOtherWorld = false;
    }

    
    public override void OnDisable()
    {
        portalCollider.TriggerEnter -= EnterThePortal;
    }

    
    
    public void EnterThePortal(PhysicsColliderActor other)
    {
        if (other.HasTag("Player"))
        {
            if (PluginManager.GetPlugin<PortalPlugin>().inOtherWorld == false)
            {
                other.Position = new Vector3(0, -3390, -1230);
                PluginManager.GetPlugin<PortalPlugin>().inOtherWorld = true;
            }
            else
            {
                other.Position = PluginManager.GetPlugin<PortalPlugin>().portalExit;
                PluginManager.GetPlugin<PortalPlugin>().inOtherWorld = false;
            }
        }
    }
    
}
