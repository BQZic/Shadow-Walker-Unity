using UnityEngine;

namespace LightObject
{
    public class LightObject : MonoBehaviour
    {
        [Header("Light Properties")]
        [SerializeField] protected int currentLight = 0;
        [SerializeField] protected int maxLight = 3;
        [SerializeField] protected int minLight = 0;
        [SerializeField] protected int lightStep = 1;

        
        
        public int GetMaxLight()
        {
            return maxLight;
        }

        public int GetCurrentLight()
        {
            return currentLight;
        }

        public int GetMinLight()
        {
            return minLight;
        }

        public int GetLightStep()
        {
            return lightStep;
        }
    }
}