using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roots
{
    public class GrassSpawner : MonoBehaviour
    {
        public GameObject[] prefabs;
        public Bounds bounds;
        public int count = 10;
        
        void Start()
        {
            for (int i = 0; i < count; i++)
            {
                SpawnRandomGrass();
            }   
        }

        private void SpawnRandomGrass()
        {
            var prefab = prefabs[Random.Range(0, prefabs.Length - 1)];

            var grass = Instantiate(prefab, new Vector3(), Quaternion.Euler(45, Random.Range(0, 180), 0), transform);
            grass.transform.localPosition = new Vector3(
                bounds.center.x + Random.Range(-bounds.extents.x, bounds.extents.x),
                .15f, 
                bounds.center.z + Random.Range(-bounds.extents.z, bounds.extents.z)
            );
        }
    }
}
