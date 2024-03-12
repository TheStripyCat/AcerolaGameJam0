using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// BackgroundSounds Script.
/// </summary>
public class BackgroundSounds : Script
{
    [Serialize, ShowInEditor] AudioSource crows1, crows2, crows3;
    public override void OnStart()
    {
        // Here you can add code that needs to be called when script is created, just before the first game update
    }
    
    /// <inheritdoc/>
    public override void OnEnable()
    {
        // Here you can add code that needs to be called when script is enabled (eg. register for events)
    }

    /// <inheritdoc/>
    public override void OnDisable()
    {
        // Here you can add code that needs to be called when script is disabled (eg. unregister from events)
    }

    /// <inheritdoc/>
    public override void OnUpdate()
    {
        if (PluginManager.GetPlugin<PortalPlugin>().inOtherWorld)
        {
            crows1.Stop();
            crows2.Stop();
            crows3.Stop();
            Actor.As<AudioSource>().Play();
        }
        else
        {
            Actor.As<AudioSource>().Stop();
            crows1.Play();
            crows2.Play();
            crows3.Play();
        }
    }
}
