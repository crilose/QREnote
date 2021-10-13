using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory<T> : MonoBehaviour where T : MonoBehaviour
{

    [SerializeField]
    private T prefab;
    // Start is called before the first frame update
    
    public T GetNewInstance(Transform parent)
    {
        return Instantiate(prefab, parent);
    }
}
