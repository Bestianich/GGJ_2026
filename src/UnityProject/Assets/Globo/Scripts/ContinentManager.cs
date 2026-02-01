using System;
using System.Collections.Generic;
using UnityEngine;


    public class ContinentManager : MonoBehaviour
    {
        public static ContinentManager Instance;
        [SerializeField] private List<Continent> _continents;

        private void Awake()
        {
            if(Instance != null)
                Destroy(gameObject);
            Instance = this;
        }

        public Continent SearchContinents(Vector3 position)
        {
            float distance = 0;
            float temp;
            Continent cont = null;
            int x = 0;
            foreach (var continent in _continents)
            {
                if(continent.GetMeshRenderer().bounds.Contains(position))
                    return continent;
                temp = continent.FindDistance(position);
                if (distance < temp)
                {
                    distance = temp;
                    cont = continent;
                }
            }
            return cont;
        }

        public Continent FindContinentWithMesh(MeshRenderer mesh)
        {
            foreach (var continent in _continents)
            {
                Debug.Log(mesh.name);
                if(continent.GetMeshRenderer() == mesh)
                    return continent;
            }
            return SearchContinents(mesh.transform.position);
        }
    }
