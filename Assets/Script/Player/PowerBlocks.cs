using UnityEngine;

namespace Script.Player
{
    public class PowerBlocks : MonoBehaviour
    {
        [SerializeField] private Transform block;

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
}
