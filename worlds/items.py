from BaseClasses import ItemClassification as IC # type: ignore
from typing import NamedTuple, Dict, Set, List
from enum import IntEnum


class ItemType(IntEnum):
    technology = 1
    resource = 2
    group = 3


class ItemData(NamedTuple):
    classification: IC
    count: int
    name: str
    tech_type: str
    type: ItemType = ItemType.technology


def make_resource_bundle_data(display_name: str, internal_name: str = "") -> ItemData:
    if not internal_name:
        internal_name = display_name
    return ItemData(IC.filler, 0, display_name, internal_name, ItemType.resource)


item_table: Dict[int, ItemData] = {
    35000: ItemData(IC.filler, 1, "Pathfinder Tool", "DiveReel"),
    35001: ItemData(IC.useful, 1, "Booster Tank", "SuitBoosterTank"),
    35002: ItemData(IC.filler, 1, "Control Room", "BaseControlRoom"),
    35003: ItemData(IC.useful, 1, "Swim Charge Fins", "SwimChargeFins"),
    35004: ItemData(IC.useful, 1, "Thumper", "Thumper"),
    35005: ItemData(IC.filler, 1, "Reinforced Dive Suit", "ReinforcedDiveSuit"),
    35006: ItemData(IC.useful, 1, "Nuclear Reactor Fragment", "NuclearReactorFragment"),
    35007: ItemData(IC.filler, 1, "Water Filtration Suit", "WaterFiltrationSuit"),
    35008: ItemData(IC.progression, 1, "Alien Containment", "BaseWaterPark"),
    35009: ItemData(IC.useful, 1, "Recyclotron", "Recyclotron"),
    35010: ItemData(IC.useful, 2, "Seatruck Horsepower Upgrade Fragment", "SeaTruckUpgradeHorsePowerFragment"),
    35011: ItemData(IC.useful, 2, "Seatruck Afterburner Upgrade Fragment", "SeaTruckUpgradeAfterburnerFragment"),
    35012: ItemData(IC.useful, 2, "Propulsion Cannon Fragment", "PropulsionCannonFragment"),
    35013: ItemData(IC.filler, 1, "Snowfox Jump Module", "HoverbikeJumpModule"),
    35014: ItemData(IC.filler, 1, "Snowfox Ice Worm Attack Reduction Module", "HoverbikeIceWormReductionModule"),
    35015: ItemData(IC.filler, 1, "Headlamp", "FlashlightHelmet"),
    35016: ItemData(IC.progression, 1, "High Capacity O2 Tank", "DoubleTank"),
    35017: ItemData(IC.useful, 1, "Power Cell Charger", "PowerCellCharger"),
    35018: ItemData(IC.filler, 2, "Beacon", "Beacon"),
    35019: ItemData(IC.progression, 4, "Parallel Processing Unit Fragment", "RadioTowerPPUFragment"),
    35020: ItemData(IC.progression, 2, "Test Override Module Fragment", "RadioTowerTOMFragment"),
    35021: ItemData(IC.progression, 2, "Hydraulic Fluid Fragment", "HydraulicFluidFragment"),
    35022: ItemData(IC.progression, 2, "Habitat Builder Fragment", "BuilderFragment"),
    35023: ItemData(IC.filler, 2, "Grav Trap Fragment", "GravSphereFragment"),
    35024: ItemData(IC.progression, 1, "Laser Cutter Fragment", "LaserCutterFragment"),
    35025: ItemData(IC.filler, 1, "Light Stick Fragment", "LEDLightFragment"),
    35026: ItemData(IC.progression, 5, "Mobile Vehicle Bay Fragment", "ConstructorFragment"),
    35027: ItemData(IC.progression, 2, "Modification Station Fragment", "WorkbenchFragment"),
    35028: ItemData(IC.progression, 2, "Moonpool", "BaseMoonpool"),
    35029: ItemData(IC.useful, 1, "Tether Tool", "TeleportationTool"),
    35030: ItemData(IC.useful, 1, "Quantum Locker", "QuantumLocker"),
    35031: ItemData(IC.progression, 1, "Cold Suit Fragment", "ColdSuitFragment"),
    35032: ItemData(IC.progression, 6, "Prawn Suit Fragment", "ExosuitFragment"),
    35033: ItemData(IC.useful, 2, "Prawn Suit Drill Arm Fragment", "ExosuitDrillArmFragment"),
    35034: ItemData(IC.useful, 2, "Prawn Suit Grappling Arm Fragment", "ExosuitGrapplingArmFragment"),
    35035: ItemData(IC.useful, 2, "Prawn Suit Propulsion Cannon Fragment", "ExosuitPropulsionArmFragment"),
    35036: ItemData(IC.useful, 2, "Prawn Suit Torpedo Arm Fragment", "ExosuitTorpedoArmFragment"),
    35037: ItemData(IC.useful, 1, "Scanner Room Fragment", "BaseMapRoomFragment"),
    35038: ItemData(IC.progression, 5, "Seatruck Fragment", "SeaTruckFragment"),
    35039: ItemData(IC.useful, 1, "Spy Pengling Fragment", "SpyPenguinFragment"),
    35040: ItemData(IC.useful, 2, "Thermal Plant Fragment", "ThermalPlantFragment"),
    35041: ItemData(IC.progression, 4, "Seaglide Fragment", "SeaglideFragment"),
    35042: ItemData(IC.useful, 3, "Seatruck Docking Module Fragment", "SeaTruckDockingModuleFragment"),
    35043: ItemData(IC.useful, 3, "Seatruck Storage Module Fragment", "SeaTruckStorageModuleFragment"),
    35044: ItemData(IC.useful, 3, "Seatruck Fabricator Module Fragment", "SeaTruckFabricatorModuleFragment"),
    35045: ItemData(IC.useful, 3, "Seatruck Aquarium Module Fragment", "SeaTruckAquariumModuleFragment"),
    35046: ItemData(IC.useful, 3, "Seatruck Sleeper Module Fragment", "SeaTruckSleeperModuleFragment"),
    35047: ItemData(IC.filler, 1, "Picture Frame", "PictureFrameFragment"),
    35048: ItemData(IC.filler, 1, "Bench", "Bench"),
    35049: ItemData(IC.filler, 1, "Basic Plant Pot", "PlanterPot"),
    35050: ItemData(IC.filler, 1, "Interior Growbed", "PlanterBox"),
    35051: ItemData(IC.filler, 1, "Plant Shelf", "PlanterShelf"),
    35052: ItemData(IC.filler, 1, "Observatory", "BaseObservatory"),
    35053: ItemData(IC.progression, 1, "Multipurpose Room", "BaseRoom"),
    35054: ItemData(IC.useful, 1, "Bulkhead", "BaseBulkhead"),
    35055: ItemData(IC.filler, 1, "Snowfox Hoverpad", "HoverpadFragment"),
    35056: ItemData(IC.filler, 1, "Desk", "StarshipDesk"),
    35057: ItemData(IC.filler, 1, "Swivel Chair", "StarshipChair"),
    35058: ItemData(IC.filler, 1, "Office Chair", "StarshipChair2"),
    35059: ItemData(IC.filler, 1, "Command Chair", "StarshipChair3"),
    35060: ItemData(IC.filler, 1, "Counter", "LabCounter"),
    35061: ItemData(IC.filler, 1, "Single Bed", "NarrowBed"),
    35062: ItemData(IC.filler, 1, "Basic Double Bed", "Bed1"),
    35063: ItemData(IC.filler, 1, "Quilted Double Bed", "Bed2"),
    35064: ItemData(IC.filler, 1, "Coffee Vending Machine", "CoffeeVendingMachine"),
    35065: ItemData(IC.filler, 1, "Trash Can", "Trashcans"),
    35066: ItemData(IC.filler, 1, "Floodlight", "Techlight"),
    35067: ItemData(IC.filler, 1, "Bar Table", "BarTable"),
    35068: ItemData(IC.filler, 1, "Vending Machine", "VendingMachine"),
    35069: ItemData(IC.filler, 1, "Single Wall Shelf", "SingleWallShelf"),
    35070: ItemData(IC.filler, 1, "Wall Shelves", "WallShelves"),
    35071: ItemData(IC.filler, 1, "Composite Plant Pot", "PlanterPot2"),
    35072: ItemData(IC.filler, 1, "Chic Plant Pot", "PlanterPot3"),
    35073: ItemData(IC.filler, 1, "Nuclear Waste Disposal", "LabTrashcan"),
    35074: ItemData(IC.filler, 1, "Wall Planter", "BasePlanter"),
    35075: ItemData(IC.useful, 1, "Ion Battery", "PrecursorIonBattery"),
    35076: ItemData(IC.useful, 1, "Ion Power Cell", "PrecursorIonPowerCell"),
    35077: ItemData(IC.filler, 1, "Exterior Growbed", "FarmingTray"),
    35078: ItemData(IC.filler, 1, "Spotlight", "Spotlight"),
    35079: ItemData(IC.filler, 1, "Snowman", "Snowman"),
    35080: ItemData(IC.filler, 1, "Water Filtration Machine", "BaseFiltrationMachine"),
    35081: ItemData(IC.progression, 1, "Ultra High Capacity Tank", "HighCapacityTank"),
    35082: ItemData(IC.progression, 1, "Large Room", "BaseLargeRoom"),
    # awarded with their rooms, keeping that as-is as they"re cosmetic
    35083: ItemData(IC.filler, 0, "Large Room Glass Dome", "BaseLargeGlassDome"),
    35084: ItemData(IC.filler, 0, "Multipurpose Room Glass Dome", "BaseGlassDome"),
    35085: ItemData(IC.filler, 0, "Partition", "BasePartition"),
    35086: ItemData(IC.filler, 0, "Partition Door", "BasePartitionDoor"),

    35087: ItemData(IC.progression, 1, "Architect Organs", "PrecursorNPCOrgansFragment"),
    35088: ItemData(IC.progression, 1, "Architect Skeleton", "PrecursorNPCSkeletonFragment"),
    35089: ItemData(IC.progression, 1, "Architect Tissues", "PrecursorNPCTissueFragment"),
    35090: ItemData(IC.progression, 1, "Locker", "Locker"),
    35091: ItemData(IC.filler, 1, "Jukebox", "Jukebox"),
    35092: ItemData(IC.filler, 1, "Executive Desk", "ExecutiveDesk"),
    35093: ItemData(IC.filler, 1, "Picture Frame", "PictureFrame"),
    35094: ItemData(IC.filler, 1, "Fridge", "Fridge"),
    35095: ItemData(IC.filler, 1, "Shower", "Shower"),
    35096: ItemData(IC.filler, 1, "Sink", "Sink"),
    35097: ItemData(IC.filler, 1, "Small Stove", "SmallStove"),
    35098: ItemData(IC.filler, 1, "Toilet", "Toilet"),
    35099: ItemData(IC.filler, 1, "Aromatherapy Lamp", "AromatherapyLamp"),
    35103: ItemData(IC.filler, 1, "Executive Toy", "EmmanuelPendulum"),
    35104: ItemData(IC.useful, 3, "Snowfox Fragment", "HoverbikeFragment"),
    35105: ItemData(IC.useful, 2, "Prawn Suit Thermal Reactor", "ExosuitThermalReactorModuleFragment"),
    35106: ItemData(IC.useful, 1, "Mineral Detector Fragment", "MetalDetectorFragment"),

    # Bundles of items
    # Awards all furniture as a bundle
    35100: ItemData(IC.filler, 0, "Furniture", "AP_Furniture", ItemType.group),
    # Awards all farming blueprints as a bundle
    35101: ItemData(IC.filler, 0, "Farming", "AP_Farming", ItemType.group),

    # Awards multiple resources as a bundle
    35102: ItemData(IC.filler, 0, "Resources Bundle", "AP_Resources", ItemType.group),

    # resource bundles, as convenience/filler

    # ores
    35200: make_resource_bundle_data("Titanium"),
    35201: make_resource_bundle_data("Copper Ore", "Copper"),
    35202: make_resource_bundle_data("Silver Ore", "Silver"),
    35203: make_resource_bundle_data("Gold"),
    35204: make_resource_bundle_data("Lead"),
    35205: make_resource_bundle_data("Diamond"),
    35206: make_resource_bundle_data("Lithium"),
    35207: make_resource_bundle_data("Ruby", "AluminumOxide"),
    35208: make_resource_bundle_data("Nickel Ore", "Nickel"),
    35209: make_resource_bundle_data("Crystalline Sulfur", "Sulphur"),
    35210: make_resource_bundle_data("Salt Deposit", "Salt"),
    35211: make_resource_bundle_data("Kyanite"),
    35212: make_resource_bundle_data("Magnetite"),
    35213: make_resource_bundle_data("Reactor Rod", "ReactorRod"),
}


items_by_type: Dict[ItemType, List[int]] = {item_type: [] for item_type in ItemType}
for item_id, item_data in item_table.items():
    items_by_type[item_data.type].append(item_id)
item_names_by_type: Dict[ItemType, List[str]] = {
    item_type: sorted(item_table[item_id].name for item_id in item_ids) for item_type, item_ids in items_by_type.items()
}

group_items: Dict[int, Set[int]] = {
    35100: {35025, 35047, 35048, 35056, 35057, 35058, 35059, 35060, 35061, 35062, 35063, 35064, 35065, 35067, 35068,
            35069, 35070, 35073, 35074, 35078, 35091, 35092, 35093, 35094, 35095, 35096, 35097, 35098, 35099, 35103},
    35101: {35049, 35050, 35051, 35071, 35072, 35074},
    35102: set(items_by_type[ItemType.resource]),
}
