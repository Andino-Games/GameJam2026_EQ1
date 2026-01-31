using UnityEngine;

public class PowerBolcks : MonoBehaviour
{
    [SerializeField] Transform block;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Transform blocks in block)
        {
            if(!block.gameObject.activeInHierarchy)
            {
                blocks.gameObject.SetActive(true);
            }
        }
    }
}
