using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace LightObject
{
    public class LightManager: MonoBehaviour
    {
        private static LightManager _instance;

        public static LightManager Instance 
        { 
            get { return _instance; } 
        } 

        private void Awake() 
        { 
            if (_instance != null && _instance != this) 
            { 
                Destroy(this.gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void TransferLight(LightObject from, LightObject to)
        {
            Debug.Assert(from.GetCurrentLight() > to.GetCurrentLight());
            
            var tmp1 = math.min(from.GetMaxLight() - from.GetCurrentLight(),
                to.GetCurrentLight() - to.GetMinLight());
            var tmp2 = math.min(from.GetLightStep(), to.GetLightStep());
            var lightAmount = math.min(tmp1, tmp2);
            from.LoseLight(lightAmount);
            to.GainLight(lightAmount);
        }
    }
}