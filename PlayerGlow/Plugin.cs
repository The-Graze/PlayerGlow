using System;
using BepInEx;
using UnityEngine;
using Utilla;

namespace PlayerGlow
{
    [BepInDependency("com.brokenstone.gorillatag.phosphorite")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        void Start()
        {
            GorillaTagger.OnPlayerSpawned(InitBruv);
        }

        private void InitBruv()
        {
            var baseLight = GorillaTagger.Instance.mainCamera.GetComponentInChildren<GameLight>(includeInactive: true);
            foreach (VRRig rig in Resources.FindObjectsOfTypeAll<VRRig>())
            {
                GameLight PlayerLight = Instantiate(baseLight);
                GameLightingManager.instance.AddGameLight(PlayerLight);
                PlayerLight.AddComponent<PlayerLightHandler>();
                PlayerLight.transform.SetParent(rig.transform, false);
                PlayerLight.transform.localPosition = Vector3.zero;
                PlayerLight.gameObject.SetActive(true);
                PlayerLight.name = "PlayerLight";
            }
            baseLight.gameObject.SetActive(true);
        }
    }
    class PlayerLightHandler : MonoBehaviour
    {
        VRRig rig;
        GameLight light;
        Color LightColor;

        void OnEnable()
        {
            rig = transform.parent.GetComponent<VRRig>();
            light = GetComponent<GameLight>();
            name = "PlayerLight";
            GameLightingManager.instance.AddGameLight(light);
        }

        void FixedUpdate()
        {
            if (rig && light)
            {
                LightColor = rig.playerColor;
                light.light.color = LightColor;
            }
            else
            {
                OnEnable();
            }
        }
    }
}
