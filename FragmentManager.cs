using System.Collections.Generic;
using UnityEngine;

public static class FragmentManager
{
    private static Dictionary<TechType, int> progress = new Dictionary<TechType, int>();

    private static Dictionary<TechType, int> required = new Dictionary<TechType, int>()
    {
        { TechType.SeaglideFragment, 3 },
        { TechType.PropulsionCannonFragment, 2 },
        { TechType.LaserCutterFragment, 3 },
        { TechType.ExosuitFragment, 4 },
        { TechType.SeaTruckFragment, 3 },
        { TechType.BuilderFragment, 1 },

        { TechType.ExosuitDrillArmFragment, 2 },
        { TechType.ExosuitGrapplingArmFragment, 2 },
        { TechType.ExosuitPropulsionArmFragment, 2 },
        { TechType.ExosuitTorpedoArmFragment, 2 },

        { TechType.SeaTruckDockingModuleFragment, 3 },
        { TechType.SeaTruckStorageModuleFragment, 3 },
        { TechType.SeaTruckFabricatorModuleFragment, 3 },
        { TechType.SeaTruckAquariumModuleFragment, 3 },
        { TechType.SeaTruckSleeperModuleFragment, 3 },
        { TechType.SeaTruckUpgradeHorsePowerFragment, 2 },
        { TechType.SeaTruckUpgradeAfterburnerFragment, 2 },

        { TechType.NuclearReactorFragment, 1 },
        { TechType.ThermalPlantFragment, 2 },

        { TechType.RadioTowerPPUFragment, 3 },
        { TechType.RadioTowerTOMFragment, 1 },
        { TechType.MetalDetectorFragment, 1 },
        { TechType.HydraulicFluidFragment, 1 },
        { TechType.ColdSuitFragment, 1 },

        { TechType.HighCapacityTankFragment, 3 },
        { TechType.ReinforcedDiveSuitFragment, 1 }
    };

    private static Dictionary<TechType, TechType> unlockMap = new Dictionary<TechType, TechType>()
    {
        { TechType.SeaglideFragment, TechType.Seaglide },
        { TechType.PropulsionCannonFragment, TechType.PropulsionCannon },
        { TechType.LaserCutterFragment, TechType.LaserCutter },
        { TechType.ExosuitFragment, TechType.Exosuit },
        { TechType.SeaTruckFragment, TechType.SeaTruck },
        { TechType.BuilderFragment, TechType.Builder },

        { TechType.ExosuitDrillArmFragment, TechType.ExosuitDrillArmModule },
        { TechType.ExosuitGrapplingArmFragment, TechType.ExosuitGrapplingArmModule },
        { TechType.ExosuitPropulsionArmFragment, TechType.ExosuitPropulsionArmModule },
        { TechType.ExosuitTorpedoArmFragment, TechType.ExosuitTorpedoArmModule },

        { TechType.SeaTruckDockingModuleFragment, TechType.SeaTruckDockingModule },
        { TechType.SeaTruckStorageModuleFragment, TechType.SeaTruckStorageModule },
        { TechType.SeaTruckFabricatorModuleFragment, TechType.SeaTruckFabricatorModule },
        { TechType.SeaTruckAquariumModuleFragment, TechType.SeaTruckAquariumModule },
        { TechType.SeaTruckSleeperModuleFragment, TechType.SeaTruckSleeperModule },
        { TechType.SeaTruckUpgradeHorsePowerFragment, TechType.SeaTruckUpgradeHorsePower },
        { TechType.SeaTruckUpgradeAfterburnerFragment, TechType.SeaTruckUpgradeAfterburner },

        { TechType.NuclearReactorFragment, TechType.BaseNuclearReactor },
        { TechType.ThermalPlantFragment, TechType.ThermalPlant },

        { TechType.RadioTowerPPUFragment, TechType.RadioTowerPPU },
        { TechType.RadioTowerTOMFragment, TechType.RadioTowerTOM },
        { TechType.MetalDetectorFragment, TechType.MetalDetector },
        { TechType.HydraulicFluidFragment, TechType.HydraulicFluid },
        { TechType.ColdSuitFragment, TechType.ColdSuit },

        { TechType.HighCapacityTankFragment, TechType.HighCapacityTank },
        { TechType.ReinforcedDiveSuitFragment, TechType.ReinforcedDiveSuit }
    };

    // ===== APPEL PRINCIPAL =====
    public static void AddFragment(TechType fragment)
    {
        if (!required.ContainsKey(fragment))
        {
            Debug.LogWarning("Fragment non configuré: " + fragment);
            return;
        }

        if (!progress.ContainsKey(fragment))
            progress[fragment] = 0;

        progress[fragment]++;

        int current = progress[fragment];
        int needed = required[fragment];

        Debug.Log($"{fragment}: {current}/{needed}");

        if (current >= needed)
        {
            Unlock(fragment);
        }
    }

    private static void Unlock(TechType fragment)
    {
        if (!unlockMap.ContainsKey(fragment))
            return;

        TechType blueprint = unlockMap[fragment];

        if (!KnownTech.Contains(blueprint))
        {
            KnownTech.Add(blueprint, true);
            ErrorMessage.AddMessage($"Blueprint débloqué: {blueprint}");
        }
    }
}