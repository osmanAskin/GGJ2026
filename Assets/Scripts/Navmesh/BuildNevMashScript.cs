using System;
using UnityEngine;
using Unity.AI.Navigation;
using System.Collections;

[RequireComponent(typeof(NavMeshSurface))]
public class BuildNevMashScript : MonoBehaviour
{
    private NavMeshSurface m_NavMeshSurface;

    public GameObject[] Characters;
    public Transform[] SpawnPoints;

    private void Awake()
    {
        m_NavMeshSurface = GetComponent<NavMeshSurface>();
    }

    private void Start()
    {
        m_NavMeshSurface.BuildNavMesh();
        StartCoroutine(SpawnCharacters());
    }
    
    IEnumerator SpawnCharacters()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < Characters.Length; i++)
        {
            Instantiate(Characters[i], SpawnPoints[i].position, SpawnPoints[i].rotation);
        }
    }
}
