using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataDefinition;
using System.IO;

public class ResourceManager : Singleton<ResourceManager>
{
    private Dictionary<ePrefabType, List<GameObject>> prefabResource;
    private Dictionary<eSpriteType, List<Sprite>> spriteResource;

    public ResourceManager()
    {
        prefabResource = new Dictionary<ePrefabType, List<GameObject>>();
        spriteResource = new Dictionary<eSpriteType, List<Sprite>>();

        LoadPrefabs();
        LoadSprites();
    }

    private void LoadPrefabs()
    {
        DirectoryInfo prefabsDirectory = new DirectoryInfo(Application.dataPath + "/Resources/Prefabs/");
        DirectoryInfo[] directories = prefabsDirectory.GetDirectories();
        foreach (DirectoryInfo directory in directories)
        {
            ePrefabType type = (ePrefabType)Enum.Parse(typeof(ePrefabType), directory.Name);
            GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/" + directory.Name);
            foreach (GameObject prefab in prefabs)
            {
                if (!prefabResource.TryGetValue(type, out List<GameObject> list))
                {
                    list = new List<GameObject>();
                    prefabResource[type] = list;
                }
                if (!list.Contains(prefab))
                {
                    list.Add(prefab);
                    prefabResource[type] = list;
                }
            }
        }
    }

    private void LoadSprites()
    {
        DirectoryInfo spritesDirectory = new DirectoryInfo(Application.dataPath + "/Resources/Sprites/");
        DirectoryInfo[] directories = spritesDirectory.GetDirectories();
        foreach (DirectoryInfo directory in directories)
        {
            eSpriteType type = (eSpriteType)Enum.Parse(typeof(eSpriteType), directory.Name);
            Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/" + directory.Name);
            foreach (Sprite sprite in sprites)
            {
                if (!spriteResource.TryGetValue(type, out List<Sprite> list))
                {
                    list = new List<Sprite>();
                    spriteResource[type] = list;
                }
                if (!list.Contains(sprite))
                {
                    list.Add(sprite);
                    spriteResource[type] = list;
                }
            }
        }
    }

    public GameObject GetPrefabResource(ePrefabType prefabType, string resourceName)
    {
        prefabResource.TryGetValue(prefabType, out List<GameObject> resourceList);
        return resourceList.Find(o => o.name.Equals(resourceName));
    }

    public Sprite GetSpriteResource(eSpriteType spriteType, string resourceName)
    {
        spriteResource.TryGetValue(spriteType, out List<Sprite> resourceList);
        return resourceList.Find(o => o.name.Equals(resourceName));
    }
}
