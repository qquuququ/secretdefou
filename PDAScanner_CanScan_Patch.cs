using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;

[HarmonyPatch(typeof(PDAScanner), "CanScan", new[] { typeof(PDAScanner.ScanTarget) })]
public class PDAScanner_CanScan_Patch
{
    public static HashSet<TechType> UnscanableTechs = new HashSet<TechType>()
    {
        TechType.ExosuitFragment,
        TechType.LaserCutterFragment,
        TechType.SeaTruckFragment,
        TechType.ConstructorFragment,
        TechType.SeaglideFragment,
        TechType.BeaconFragment,
        TechType.DiveReel,
        TechType.BaseWaterPark,
        TechType.Aquarium,
        TechType.AromatherapyLamp,
        TechType.BarTable,
        TechType.Bed1,
        TechType.PlanterPot,
        TechType.Bench,
        TechType.BaseBulkhead,
        TechType.PlanterPot3,
        TechType.CoffeeVendingMachine,
        TechType.StarshipChair3,
        TechType.PlanterPot2,
        TechType.LabCounter,
        TechType.BedDanielle,
        TechType.ExecutiveDesk,
        TechType.ExecutiveDesk,
        TechType.FarmingTray,
        TechType.Techlight,
        TechType.BedFred,
        TechType.Fridge,
        TechType.PlanterBox,
        TechType.JukeboxFragment,
        TechType.Fabricator,
        TechType.Speaker,
        TechType.BaseLargeRoomFragment,
        TechType.BaseLargeGlassDomeFragment,
        TechType.Locker,
        TechType.WorkbenchFragment,
        TechType.BaseMoonpool,
        TechType.BaseRoom,
        TechType.BaseGlassDome,
        TechType.BaseNuclearReactorFragment,
        TechType.LabTrashcan,
        TechType.BaseObservatory,
        TechType.StarshipChair2,
        TechType.PictureFrame,
        TechType.PlanterShelf,
        TechType.PowerCellChargerFragment,
        TechType.Shower,
        TechType.NarrowBed,
        TechType.SingleWallShelf,
        TechType.Sink,
        TechType.SmallStove,
        TechType.HoverpadFragment,
        TechType.StarshipChair,
        TechType.ThermalPlantFragment,
        TechType.Toilet,
        TechType.Trashcans,
        TechType.VendingMachine,
        TechType.WallShelves,
        TechType.BaseFiltrationMachine,
        TechType.BaseWindow,
        TechType.BuilderFragment,
        TechType.LEDLightFragment,
        TechType.MetalDetectorFragment,
        TechType.PropulsionCannonFragment,
        TechType.SeaTruckAquariumModuleFragment,
        TechType.SeaTruckDockingModuleFragment,
        TechType.SeaTruckFabricatorModuleFragment,
        TechType.SeaTruckSleeperModuleFragment,
        TechType.SeaTruckStorageModuleFragment,
        TechType.HoverbikeFragment,
        TechType.ExosuitDrillArmFragment,
        TechType.ExosuitGrapplingArmFragment,
        TechType.ExosuitPropulsionArmFragment,
        TechType.ExosuitTorpedoArmFragment,
        TechType.SeaTruckUpgradeAfterburnerFragment,
        TechType.SeaTruckUpgradeHorsePowerFragment,
        TechType.ColdSuitFragment,
        TechType.ReinforcedDiveSuitFragment,
        TechType.HighCapacityTankFragment,
        TechType.RadioTowerPPUFragment,
        TechType.RadioTowerTOMFragment,
        TechType.GravSphereFragment,
        TechType.SpyPenguinFragment,
        TechType.HydraulicFluidFragment,
        TechType.StarshipDesk,
        TechType.BedJeremiah,
        TechType.BedEmmanuel,
        TechType.EmmanuelPendulum,
        TechType.BasePlanter,
        TechType.BedSam,
    };

    [HarmonyPrefix]
    public static bool Prefix(PDAScanner.ScanTarget scanTarget, ref PDAScanner.Result __result)
    {
        Debug.Log($"[PATCH CanScan] Called for: {scanTarget.techType}");

        if (scanTarget.techType != TechType.None && UnscanableTechs.Contains(scanTarget.techType))
        {
            Debug.Log($"[PATCH CanScan] BLOCKING {scanTarget.techType}!");
            __result = PDAScanner.Result.None;
            return false;
        }

        return true;
    }
}