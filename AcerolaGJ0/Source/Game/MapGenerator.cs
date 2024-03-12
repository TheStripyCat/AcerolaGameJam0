using System;
using System.Collections.Generic;
using FlaxEngine;

namespace Game;

/// <summary>
/// MapGenerator Script.
/// </summary>
public class MapGenerator : Script
{
    [Serialize, ShowInEditor] List<Actor> tileSpawns;
    [Serialize, ShowInEditor] List<Prefab> tiles, weirdTiles, oddities;
    [Serialize, ShowInEditor] Actor player;
    private int weirdTileSpawn, playerSpawnTile, tileOption, odditySpawn;
    private Prefab weirdTile, tile;
    private Actor spawnedTile;
    private List<int> angles, tilesWithOdditites;
    private bool isRepeating;

    public override void OnStart()
    {
        angles = new List<int>();
        angles.Add(90);
        angles.Add(180);
        angles.Add(270);
        //choosing where the player will spawn
        //playerSpawnTile = RandomUtil.Random.Next(0, 8);
        playerSpawnTile = 1;

        //choosing a place an spawning the tile with the portal
        weirdTileSpawn = RandomUtil.Random.Next(0, 9);
        Debug.Log(weirdTileSpawn);
        weirdTile = weirdTiles[RandomUtil.Random.Next(0, weirdTiles.Count)];
        spawnedTile = PrefabManager.SpawnPrefab(weirdTile, tileSpawns[weirdTileSpawn]);
        PluginManager.GetPlugin<PortalPlugin>().odditiesMapSpawns.Add(3);
        PluginManager.GetPlugin<PortalPlugin>().odditiesTilePlacement.Add(3);
        //rotate spawned tile to add some randomness
        spawnedTile.RotateAround(spawnedTile.Position, Vector3.Up, angles[RandomUtil.Random.Next(0, 3)]);
        if (playerSpawnTile == weirdTileSpawn)
        {
            player.Position = spawnedTile.GetScript<TileScript>().playerSpawn.Position;
        }
        //choosing which tiles will have oddities
        tilesWithOdditites = new List<int>();
        for (int t = 0; t < 3; t++)
        {
            isRepeating = false;
            tileOption = RandomUtil.Random.Next(0, 9);
            if (tileOption != weirdTileSpawn)
            {
                for (int i = 0; i < tilesWithOdditites.Count; i++)
                {
                    if (tilesWithOdditites[i] == tileOption)
                        isRepeating = true;
                }
                if (isRepeating == false)
                {
                    tilesWithOdditites.Add(tileOption);
                }
                else
                    t--;

            }
            else t--;

        }
        
        //spawning other tiles
        for (int i = 0; i < 9; i++)
        {
            if (i != weirdTileSpawn)
            {
                tile = tiles[RandomUtil.Random.Next(0, tiles.Count)];
                spawnedTile = PrefabManager.SpawnPrefab(tile, tileSpawns[i]);
                spawnedTile.RotateAround(spawnedTile.Position, Vector3.Up, angles[RandomUtil.Random.Next(0,3)]);
                PluginManager.GetPlugin<PortalPlugin>().odditiesMapSpawns.Add(3);
                PluginManager.GetPlugin<PortalPlugin>().odditiesTilePlacement.Add(3);

                //spawn oddities                
                for (int t = 0; t < 3; t++)
                {
                    if (tilesWithOdditites[t] == i)
                    {
                        odditySpawn = RandomUtil.Random.Next(0, 3);
                        PrefabManager.SpawnPrefab(oddities[t], spawnedTile.GetScript<TileScript>().odditySpawns[odditySpawn].Position);
                        PluginManager.GetPlugin<PortalPlugin>().odditiesMapSpawns[i] = t;
                    }
                }

                //spawn player
                if (playerSpawnTile == i)
                {
                    player.Position = spawnedTile.GetScript<TileScript>().playerSpawn.Position;
                }
            }
            
        }
        Debug.Log("tile " + tilesWithOdditites[0]);
        Debug.Log("tile " + tilesWithOdditites[1]);
        Debug.Log("tile " + tilesWithOdditites[2]);
                        
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
        // Here you can add code that needs to be called every frame
    }
}
