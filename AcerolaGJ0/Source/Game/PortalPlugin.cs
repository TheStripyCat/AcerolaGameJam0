using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// PortalPlugin Script.
/// </summary>
public class PortalPlugin : GamePlugin
{
    public bool inPortalSpace, inOtherWorld;
    public Vector3 portalPosition, portalExit;
    public Actor portalSpawnEmpty;
    public List<int> odditiesMapSpawns;
    public List<int> odditiesTilePlacement;
   
    public override void Initialize()
    {
        base.Initialize();
        odditiesMapSpawns = new List<int>();
        odditiesTilePlacement = new List<int>();

}

    /// <inheritdoc/>
    public override void Deinitialize()
    {
        base.Deinitialize();
        odditiesMapSpawns.Clear();
        odditiesTilePlacement.Clear();
    }

}
