using System.Collections.Generic;
using UnityEngine;

public static class RewardDB
{
    // ==================== CUSTOM NAMES ====================
    public static Dictionary<TechType, string> CustomNames = new Dictionary<TechType, string>()
    {
        // FRAGMENTS
        { TechType.SeaglideFragment, "Seaglide Fragment" },
        { TechType.PropulsionCannonFragment, "Propulsion Cannon Fragment" },
        { TechType.LaserCutterFragment, "Laser Cutter Fragment" },
        { TechType.ExosuitFragment, "Prawn Suit Fragment" },
        { TechType.SeaTruckFragment, "Seatruck Fragment" },
        { TechType.BuilderFragment, "Habitat Builder Fragment" },
        { TechType.ExosuitDrillArmFragment, "Prawn Suit Drill Arm Fragment" },
        { TechType.ExosuitGrapplingArmFragment, "Prawn Suit Grappling Arm Fragment" },
        { TechType.ExosuitPropulsionArmFragment, "Prawn Suit Propulsion Arm Fragment" },
        { TechType.ExosuitTorpedoArmFragment, "Prawn Suit Torpedo Arm Fragment" },
        { TechType.SeaTruckDockingModuleFragment, "Seatruck Docking Module Fragment" },
        { TechType.SeaTruckStorageModuleFragment, "Seatruck Storage Module Fragment" },
        { TechType.SeaTruckFabricatorModuleFragment, "Seatruck Fabricator Module Fragment" },
        { TechType.SeaTruckAquariumModuleFragment, "Seatruck Aquarium Module Fragment" },
        { TechType.SeaTruckSleeperModuleFragment, "Seatruck Sleeper Module Fragment" },
        { TechType.SeaTruckUpgradeHorsePowerFragment, "Seatruck Horsepower Upgrade Fragment" },
        { TechType.SeaTruckUpgradeAfterburnerFragment, "Seatruck Afterburner Upgrade Fragment" },
        { TechType.NuclearReactorFragment, "Nuclear Reactor Fragment" },
        { TechType.ThermalPlantFragment, "Thermal Plant Fragment" },
        { TechType.RadioTowerPPUFragment, "Radio Tower PPU Fragment" },
        { TechType.RadioTowerTOMFragment, "Radio Tower TOM Fragment" },
        { TechType.MetalDetectorFragment, "Mineral Detector Fragment" },
        { TechType.HydraulicFluidFragment, "Hydraulic Fluid Fragment" },
        { TechType.ColdSuitFragment, "Cold Suit Fragment" },
        { TechType.HighCapacityTankFragment, "Ultra High Capacity Tank Fragment" },
        { TechType.ReinforcedDiveSuitFragment, "Reinforced Dive Suit Fragment" },
        { TechType.GravSphereFragment, "Grav Trap Fragment" },
        { TechType.LEDLightFragment, "Light Stick" },
        { TechType.SpyPenguinFragment, "Spy Pengling Fragment" },

        // BLUEPRINTS
        { TechType.Rebreather, "Rebreather" },
        { TechType.SwimChargeFins, "Swim Charge Fins" },
        { TechType.BatteryCharger, "Battery Charger" },
        { TechType.PowerCellCharger, "Power Cell Charger" },
        { TechType.WaterFiltrationSuit, "Water Filtration Suit" },
        { TechType.SeaTruckTeleportationModule, "Seatruck Teleportation Module" },
        { TechType.BaseMoonpool, "Moonpool" },
        { TechType.BaseControlRoom, "Control Room" },
        { TechType.BaseMapRoom, "Scanner Room" },
        { TechType.Beacon, "Beacon" },
        { TechType.DiveReel, "Pathfinder Tool" },
        { TechType.SuitBoosterTank, "Booster Tank" },
        { TechType.FlashlightHelmet, "Headlamp" },
        { TechType.DoubleTank, "High Capacity O2 Tank" },
        { TechType.Recyclotron, "Recyclotron" },
        { TechType.TeleportationTool, "Tether Tool" },
        { TechType.Thumper, "Thumper" },
        { TechType.QuantumLocker, "Quantum Locker" },
        { TechType.HoverbikeJumpModule, "Snowfox Jump Module" },
        { TechType.HoverbikeIceWormReductionModule, "Snowfox Ice Worm Attack Reduction Module" },
        
        // ITEMS
        { TechType.Diamond, "Diamond" },
        { TechType.Battery, "Battery" },
        { TechType.PowerCell, "Power Cell" },
        { TechType.FilteredWater, "Filtered Water" },
        { TechType.DisinfectedWater, "Disinfected Water" },
        { TechType.Titanium, "Titanium" },
        { TechType.Copper, "Copper" },
        { TechType.Gold, "Gold" },
        { TechType.Silver, "Silver" },
        { TechType.Lead, "Lead" },
        { TechType.Kyanite, "Kyanite" },
        { TechType.Lithium, "Lithium" },
        { TechType.Magnetite, "Magnetite" },
        { TechType.Nickel, "Nickel" },
        { TechType.Quartz, "Quartz" },
        { TechType.AluminumOxide, "Ruby" },
        { TechType.SnowBall, "Snowball" }
    };

    // ==================== DATABOX & PDA NAMES (par ClassID) ====================
    public static Dictionary<string, string> DataboxPDANames = new Dictionary<string, string>()
    {
        // DATABOXES
        { "5fa317d3-0421-48de-beb9-1ab73bce2bf9", "Twisty Bridges Seabase Tube - Databox" },
        { "9e142596-4324-4216-a41f-df8470839d5d", "Twisty Bridges Alterra Crane - Databox" },
        { "7898b32c-cf2b-4034-a02d-b40e1c28554d", "Sparse Arctic Alterra Crane - Platform Databox" },
        { "d3b4be43-9a2f-45f4-85c1-c0c10aadef61_253_-139_-420", "Water Analysis Station - Purple Vents - Outside Databox" },
        { "9b2996a8-caa4-4f7f-ba85-8fb1f59981af_226_-104_-621", "Purple Vents Small Debris Wreck - Databox" }, //FAKE PDA
        { "ab0cbd59-d0be-43ed-b917-bc0bb925ad18", "Marguerit Seabase - Purple Vents - Outside Databox" },
        { "229090ab-43b7-435b-aa15-c590df34c75e", "Delta Station - Outside Databox" }, //FAKE PDA
        { "bdc52b41-8b8c-47a8-ad91-24bba4f74a22", "Omega Lab - Lilypads Islands - Outside Databox" },
        { "9b2996a8-caa4-4f7f-ba85-8fb1f59981af_565_-203_-1074", "Omega Lab - Lilypads Islands - Greenhouse Databox" }, //FAKE PDA
        { "d3b4be43-9a2f-45f4-85c1-c0c10aadef61_2_-453_-1117", "Tree Spires Fissure Platform - Databox" },
        { "97499e6c-bc0d-4990-beda-c73de0d59081_-1337_37_-266", "West Mining Platform - Arctic Spires - Databox" },
        { "97499e6c-bc0d-4990-beda-c73de0d59081", "Diamond Mining Platform - Arctic Spires - Databox" },
        { "f917f47b-ff2d-4a1d-b6a8-f94d28aabca9", "Alterra Platform - Arctic Spires - Databox" },
        { "fa863c0d-d661-4cf0-b016-3d676fbfc917", "Alterra Platform - Glacial Basin - Databox" },
        { "9b2996a8-caa4-4f7f-ba85-8fb1f59981af_-98_14_330", "Outpost Zero - Laboratory Databox" }, //FAKE PDA
        { "d8b348e2-d62c-4c85-ba18-659091128c5b", "Koppa Mining Site - Entrance Databox" }, // FAKE PDA
        { "229090ab-43b7-435b-aa15-c590df34c75e_-197_-274_-708", "Deep Koppa Mining Site - Crate Databox" }, //FAKE PDA
        { "ec2aca07-56d9-4498-b943-4993fe437c5d", "Deep Koppa Mining Site - Lift Databox" },

        // PDAs
        { "07a03220-3708-7c4f-b38e-3fff32ccc548", "Twisty Bridges Seabase Tube - Opposite Ledge PDA" },
        { "2f02466f-8182-1542-2989-8f95cfb5a661", "Sparse Arctic Alterra Crane - Above Platform PDA" },
        { "ed6bd77a-461b-ec55-8399-5824017f7733", "Kelp Forest Emergency Supply Cache - PDA" },
        { "a4e9daaa-af18-b10b-5122-78d0bcdec061", "Twisty Bridges Seatruck Wreck - PDA 1" },
        { "fbe1f0b6-0fe4-abf8-e3f8-c58eebd83c4e", "Twisty Bridges Seatruck Wreck - PDA 2" },
        { "522f77ae-6251-5ec3-e27d-44558c55d499", "Purple Vents Small Debris Wreck - PDA" },
        { "9f23b303-1ea9-ff5b-8390-3e710b891d6c", "Marguerit Seabase - Purple Vents - Workshop PDA" },
        { "bb256dcc-88ba-2524-4e6c-edcae5121478", "Marguerit Seabase - Purple Vents - Bedroom PDA" },
        { "fca00606-eaeb-b11a-dc65-7ca4bcd9b874", "Mercury II Stern - Purple Vents - Engine Room PDA" },
        { "59bf53a7-85f6-5254-b4d7-1195f55a40d7", "Mercury II Stern - Purple Vents - Hidden Room PDA" },
        { "3a3a33d0-2e55-644f-08aa-7e8097bb2228", "Delta Station - Break Room PDA" },
        { "acfd50d8-9d67-7c0b-3277-90c6f89aef9e", "Delta Station - Bedroom Bed PDA" },
        { "bfb446d3-15f5-ccc9-6d3f-4f569d5ab347", "Delta Station - Bedroom Desk PDA" },
        { "6ef51b95-b6be-240b-23af-533545dbf2bc", "Delta Station - Office Desk PDA" },
        { "1e198596-a784-edab-d2b7-2ba4f5537e93", "Delta Station - Office Table PDA" },
        { "5453d300-9554-3b48-44fd-0e665c59df30", "Communications Tower - Outside Crate PDA" },
        { "adbafd5e-984d-e488-6401-bf48846f1ad6", "Communications Tower - Near Panel PDA" },
        { "cf510408-76d8-a3b0-536e-33f42b17620f", "Delta Island Cave - Desk PDA" },
        { "675b64e3-f3ae-a69d-d6af-3e6eab0b93ea", "Delta Island Dock - PDA" },
        { "6db4a465-0450-b36a-0e47-f2d30f9252ad", "Marguerit Greenhouse - East Arctic - Counter PDA" },
        { "7a6a9ef3-220f-f4ab-ddd6-8cbf89579711", "Marguerit Greenhouse - East Arctic - Desk PDA" },
        { "e54c9ef4-c5d3-9009-ddbe-1314beb65127", "Omega Lab - Lilypads Islands - Greenhouse PDA" },
        { "ad417604-3407-914b-6092-b1e10780f71c", "Omega Lab - Lilypads Islands - Greenhouse Desk PDA" },
        { "f1994d48-a29e-af75-d860-f21a99c9cbf6", "Omega Lab - Lilypads Islands - Danielle's Bed PDA" },
        { "e2e7836a-ca0c-3d06-8692-3b3c97a5212e", "Omega Lab - Lilypads Islands - Vinh's Bed PDA" },
        { "8ffdd0b7-46cd-6817-a833-9814877df15d", "Omega Lab - Lilypads Islands - Lab PDA" },
        { "7e73e368-8258-27ce-4adf-aad818871230", "Mercury II Bow - Lilypads Islands - Growbed PDA" },
        { "a336d97c-73a5-4c17-df48-eeb42d637a75", "Mercury II Bow - Lilypads Islands - Alien Containment PDA" },
        { "66accba7-869a-9b26-40fd-09de251c032c", "Mercury II Bow - Lilypads Islands - Bridge PDA" },
        { "fd075e39-6d2a-6a7d-609b-fd1cd3977823", "Mercury II Bow - Lilypads Islands - Bunk Bed PDA 1" },
        { "3ab0771b-0761-77e6-0a84-bafe25caf611", "Mercury II Bow - Lilypads Islands - Bunk Bed PDA 2" },
        { "85b7775e-bae7-eb77-5597-5d64c6adcb50", "Mercury II Bow - Lilypads Islands - Bunk Bed PDA 3" },
        { "64be8dac-3ae6-e10f-6534-cd5685c14cd9", "Mercury II Bow - Lilypads Islands - Nuclear Reactor PDA" },
        { "066511d9-9323-8aea-866b-62fc9fb26014", "Tree Spires Fissure Platform - PDA" },
        { "7e19a66c-44ea-3cbb-65a7-6037381b28d1", "Glacial Bay Dock - PDA" },
        { "f95bd98e-fee8-dc56-882a-f7b9ac58e629", "West Mining Platform - Arctic Spires - Platform PDA" },
        { "7a8061d3-71b4-76d0-afed-919c006fecef", "West Mining Platform - Arctic Spires - Crate PDA" },
        { "efcb720b-f4d9-829a-3f82-afed032c851e", "Diamond Mining Platform - Arctic Spires - PDA" },
        { "7d0bc123-9a89-c1d5-3bcc-404004e87187", "Phi Robotics Center - Outside PDA" },
        { "bb5b8444-c29e-1006-4cc9-8f46d8456d32", "Phi Robotics Center - Office PDA" },
        { "ab8fd00d-00cb-e136-9357-bfe553c54b48", "Phi Robotics Center - Upstairs Counter PDA" },
        { "b66b6ec8-9074-5ece-c123-1cd959140787", "Phi Robotics Center - Bottom Floor Crate PDA" },
        { "13b69a3a-6444-5855-d8c4-3a7572223820", "Phi Robotics Center - Zeta's Bedroom PDA" },
        { "50f11116-d050-9227-ede4-31e0b4016018", "Phi Robotics Center - Sam's Bedroom PDA" },
        { "bd7fe66d-129d-3330-f884-33a3a8e486b0", "Alterra Cache Cave - Glacial Basin - PDA" },
        { "477e0a9f-f932-b096-09a1-a439e9ae05fd", "Small Lake - Glacial Basin - Crates Above Lake PDA" },
        { "a9c0f675-92a2-1500-ab39-6589bae62f77", "Big Basin Stalker Cavern - Deep Inside PDA" },
        { "9232eaaf-9b03-7a2d-01f7-efe6afabbf92", "Southern Glacial Basin - Spy Pengling Access Point PDA" },
        { "9fac3715-4e68-f925-61da-396ca09a45b9", "Parvan's Bunker - Bed PDA" },
        { "b72a456d-701e-564c-cb8b-c4164954d02b", "Parvan's Bunker - Crate PDA" },
        { "4f81fb12-0606-f30e-ba06-21c4a6e8214f", "Path To Excavation Site - Ground PDA" },
        { "c1ba6f26-a5c4-abd7-b007-6870c782bb4b", "Path To Excavation Site - Glacial Basin - Table PDA" },
        { "594a7288-f6c9-b7d1-6560-7cf2f4fce773", "Ledge Over Excavation Site - Glacial Basin - PDA" },
        { "6cd2480f-cfd5-abb9-69c0-219aab103a97", "Outpost Zero - Lab Middle Counter PDA" },
        { "0c2004c5-a9ba-a27b-e214-6bc08f2f1722", "Outpost Zero - Lab Left Counter PDA" },
        { "e1272d0a-6b9e-9203-9011-b3162af23e95", "Outpost Zero - Kitchen Corner Counter PDA" },
        { "76a1e2d4-313c-c037-fc63-78a423c68fba", "Outpost Zero - Kitchen Bench PDA" },
        { "acfe37ed-524b-ffd9-d066-95f9a5c15451", "Outpost Zero - Sam's Bed PDA" },
        { "936b3419-4f35-a71d-180f-b92689b5a16d", "Outpost Zero - Lillian's Bed PDA" },
        { "ec445c72-5252-d6c4-bae0-0b8dba202867", "Outpost Zero - Greenhouse PDA" },
        { "43d0b9b7-5284-53e0-741f-d417cf20d5f0", "Kelp Forest - Near Sea Monkey Nest PDA" },
        { "b1cbe311-6af6-ceee-e155-7c40e8a37a26", "Deep Twisty Bridges - Entrance PDA" },
        { "c4fe357d-5871-5424-7783-889c752fc3cb", "Deep Twisty Bridges - Deep Near Fragment PDA" },
        { "b679a5d9-6148-9188-7805-502dae221428", "Deep Koppa Mining Site - Near Closed Door PDA" },
        { "2c9eb71d-9ac0-31b5-816f-f1785d20a1fa", "Deep Koppa Mining Site - Table PDA" },
        { "96fcca9b-3b39-2c04-4ca1-6178d9b180c3", "Deep Koppa Mining Site - Desk Near Lift PDA" },

        // FAKE PDAs
        { "570db791-3d49-563b-c510-b4201c2f86ec", "PDA: ControlRoom" },
        { "9b2996a8-caa4-4f7f-ba85-8fb1f59981af", "Omega Lab - Lilypads Islands - Greenhouse Databox" },
        { "ca1a2d90-45bf-cfeb-6152-24dac14f490c", "PDA: MoonpoolExpansion"},
        { "5944b743-619a-7e96-a1d0-08ee5467be39", "Delta Station - Outside Databox" }
    };

    public static string GetDisplayName(TechType techType)
    {
        if (CustomNames.TryGetValue(techType, out string customName))
            return customName;

        return Language.main.Get(techType) ?? techType.ToString();
    }

    public static string GetDataboxPDAName(string classId, string fallbackName)
    {
        if (DataboxPDANames.TryGetValue(classId, out string customName))
            return customName;

        return fallbackName;
    }

    // ==================== FRAGMENTS (progression scan) ====================
    public static List<TechType> Fragments = new List<TechType>()
    {
        TechType.SeaglideFragment,
        TechType.PropulsionCannonFragment,
        TechType.LaserCutterFragment,
        TechType.ExosuitFragment,
        TechType.SeaTruckFragment,
        TechType.BuilderFragment,
        TechType.ExosuitDrillArmFragment,
        TechType.ExosuitGrapplingArmFragment,
        TechType.ExosuitPropulsionArmFragment,
        TechType.ExosuitTorpedoArmFragment,
        TechType.SeaTruckDockingModuleFragment,
        TechType.SeaTruckStorageModuleFragment,
        TechType.SeaTruckFabricatorModuleFragment,
        TechType.SeaTruckAquariumModuleFragment,
        TechType.SeaTruckSleeperModuleFragment,
        TechType.SeaTruckUpgradeHorsePowerFragment,
        TechType.SeaTruckUpgradeAfterburnerFragment,
        TechType.NuclearReactorFragment,
        TechType.ThermalPlantFragment,
        TechType.RadioTowerPPUFragment,
        TechType.RadioTowerTOMFragment,
        TechType.MetalDetectorFragment,
        TechType.HydraulicFluidFragment,
        TechType.ColdSuitFragment,
        TechType.HighCapacityTankFragment,
        TechType.ReinforcedDiveSuitFragment,
        TechType.GravSphereFragment,
        TechType.SpyPenguinFragment,
        TechType.LEDLightFragment
    };

    // ==================== BLUEPRINTS (unlock direct craft) ====================
    public static List<TechType> Blueprints = new List<TechType>()
    {
        TechType.Rebreather,
        TechType.SwimChargeFins,
        TechType.BatteryCharger,
        TechType.PowerCellCharger,
        TechType.WaterFiltrationSuit,
        TechType.SeaTruckTeleportationModule,
        TechType.BaseMoonpool,
        TechType.BaseControlRoom,
        TechType.BaseMapRoom,
        TechType.Beacon,
        TechType.DiveReel,
        TechType.SuitBoosterTank,
        TechType.FlashlightHelmet,
        TechType.DoubleTank,
        TechType.Recyclotron,
        TechType.TeleportationTool,
        TechType.Thumper,
        TechType.QuantumLocker,
        TechType.HoverbikeJumpModule,
        TechType.HoverbikeIceWormReductionModule
    };

    // ==================== DIRECT ITEMS (no fragment logic) ====================
    public static List<TechType> DirectItems = new List<TechType>()
    {
        TechType.Diamond,
        TechType.Battery,
        TechType.PowerCell,
        TechType.FilteredWater,
        TechType.DisinfectedWater,
        TechType.Titanium,
        TechType.Copper,
        TechType.Gold,
        TechType.Silver,
        TechType.Lead,
        TechType.Kyanite,
        TechType.Lithium,
        TechType.Magnetite,
        TechType.Nickel,
        TechType.Quartz,
        TechType.AluminumOxide,
        TechType.SnowBall
    };
}