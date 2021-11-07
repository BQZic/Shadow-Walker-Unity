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

        public void GainLight(int lightAmount)
        {
            if (currentLight + lightAmount > maxLight) return;
            currentLight += lightAmount;
        }

        public void LoseLight(int lightAmount)
        {
            if (currentLight - lightAmount < minLight) return;
            currentLight -= lightAmount;
        }
        
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