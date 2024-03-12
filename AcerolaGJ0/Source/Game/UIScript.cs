using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// UIScript Script.
/// </summary>
public class UIScript : Script
{
    [Serialize, ShowInEditor] UIControl theNote, startPrompt;
    public override void OnStart()
    {
        Time.TimeScale = 0f;
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
        if (Input.GetKeyDown(KeyboardKeys.E))
        {
            theNote.IsActive = false;
            startPrompt.IsActive = false;
            Time.TimeScale = 1f;
        }
    }
}
