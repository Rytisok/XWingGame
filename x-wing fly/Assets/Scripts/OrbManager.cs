using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class OrbManager : MonoBehaviour
{
    public int orbCount;
    public int spawnArea;
    public GameObject orbPrefab;
    public Realtime _realtime;
    public List<GameObject> orbs = new List<GameObject>();

    float nextCheck = 0;
    float interval = 5;
    bool k = false;

    void Update()
    {
        if (_realtime.connected && !k)
        {
            SpawnOrbs();
            k = true;
        }

        if (Time.time > nextCheck)
        {
            for (int i = 0; i < orbCount; i++)
            {
                if (orbs[i] == null)
                {
                    orbs.Remove(orbs[i]);
                    Invoke("SpawnOrb", 4);
                    SpawnOrb(i);
                }
            }
            nextCheck = Time.time + interval;
        }
    }

    void SpawnOrbs()
    {
        for (int i = 0; i < orbCount; i++)
        {
            SpawnOrb(i);
        }
    }

    void SpawnOrb(int n)
    {
        Vector3 pos = (Random.insideUnitSphere * spawnArea) + transform.position;

        GameObject orb = Realtime.Instantiate(orbPrefab.name, pos, transform.rotation, ownedByClient: false, useInstance: _realtime);
        orb.transform.position = pos;
        orbs.Insert(n, orb);
    }
}
