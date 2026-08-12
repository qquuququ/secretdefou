from typing import Dict, Set, List
from enum import IntEnum


class RewardType(IntEnum):
    fragment = 1
    blueprint = 2
    resource = 3


# Tous les items disponibles comme récompenses pour les locations
AVAILABLE_REWARDS: Dict[str, RewardType] = {
    # FRAGMENTS (progression scan)
    "SeaglideFragment": RewardType.fragment,
    "PropulsionCannonFragment": RewardType.fragment,
    "LaserCutterFragment": RewardType.fragment,
    "ExosuitFragment": RewardType.fragment,
    "SeaTruckFragment": RewardType.fragment,
    "BuilderFragment": RewardType.fragment,
    "ExosuitDrillArmFragment": RewardType.fragment,
    "ExosuitGrapplingArmFragment": RewardType.fragment,
    "ExosuitPropulsionArmFragment": RewardType.fragment,
    "ExosuitTorpedoArmFragment": RewardType.fragment,
    "SeaTruckDockingModuleFragment": RewardType.fragment,
    "SeaTruckStorageModuleFragment": RewardType.fragment,
    "SeaTruckFabricatorModuleFragment": RewardType.fragment,
    "SeaTruckAquariumModuleFragment": RewardType.fragment,
    "SeaTruckSleeperModuleFragment": RewardType.fragment,
    "SeaTruckUpgradeHorsePowerFragment": RewardType.fragment,
    "SeaTruckUpgradeAfterburnerFragment": RewardType.fragment,
    "NuclearReactorFragment": RewardType.fragment,
    "ThermalPlantFragment": RewardType.fragment,
    "RadioTowerPPUFragment": RewardType.fragment,
    "RadioTowerTOMFragment": RewardType.fragment,
    "MetalDetectorFragment": RewardType.fragment,
    "HydraulicFluidFragment": RewardType.fragment,
    "ColdSuitFragment": RewardType.fragment,
    "HighCapacityTankFragment": RewardType.fragment,
    "ReinforcedDiveSuitFragment": RewardType.fragment,
    "GravSphereFragment": RewardType.fragment,
    "SpyPenguinFragment": RewardType.fragment,
    "LEDLightFragment": RewardType.fragment,

    # BLUEPRINTS (unlock direct craft)
    "Rebreather": RewardType.blueprint,
    "SwimChargeFins": RewardType.blueprint,
    "BatteryCharger": RewardType.blueprint,
    "PowerCellCharger": RewardType.blueprint,
    "WaterFiltrationSuit": RewardType.blueprint,
    "SeaTruckTeleportationModule": RewardType.blueprint,
    "BaseMoonpool": RewardType.blueprint,
    "BaseControlRoom": RewardType.blueprint,
    "BaseMapRoom": RewardType.blueprint,
    "Beacon": RewardType.blueprint,
    "DiveReel": RewardType.blueprint,
    "SuitBoosterTank": RewardType.blueprint,
    "FlashlightHelmet": RewardType.blueprint,
    "DoubleTank": RewardType.blueprint,
    "Recyclotron": RewardType.blueprint,
    "TeleportationTool": RewardType.blueprint,
    "Thumper": RewardType.blueprint,
    "QuantumLocker": RewardType.blueprint,
    "HoverbikeJumpModule": RewardType.blueprint,
    "HoverbikeIceWormReductionModule": RewardType.blueprint,

    # DIRECT ITEMS (no fragment logic)
    "Diamond": RewardType.resource,
    "Battery": RewardType.resource,
    "PowerCell": RewardType.resource,
    "FilteredWater": RewardType.resource,
    "Titanium": RewardType.resource,
    "Copper": RewardType.resource,
    "Gold": RewardType.resource,
    "Silver": RewardType.resource,
    "Lead": RewardType.resource,
    "Kyanite": RewardType.resource,
    "Lithium": RewardType.resource,
    "Magnetite": RewardType.resource,
    "Nickel": RewardType.resource,
    "Quartz": RewardType.resource,
    "AluminumOxide": RewardType.resource,
    "SnowBall": RewardType.resource,
}

# Noms personnalisés pour l'affichage
CUSTOM_NAMES: Dict[str, str] = {
    # FRAGMENTS
    "SeaglideFragment": "Seaglide Fragment",
    "PropulsionCannonFragment": "Propulsion Cannon Fragment",
    "LaserCutterFragment": "Laser Cutter Fragment",
    "ExosuitFragment": "Prawn Suit Fragment",
    "SeaTruckFragment": "Seatruck Fragment",
    "BuilderFragment": "Habitat Builder Fragment",
    "ExosuitDrillArmFragment": "Prawn Suit Drill Arm Fragment",
    "ExosuitGrapplingArmFragment": "Prawn Suit Grappling Arm Fragment",
    "ExosuitPropulsionArmFragment": "Prawn Suit Propulsion Arm Fragment",
    "ExosuitTorpedoArmFragment": "Prawn Suit Torpedo Arm Fragment",
    "SeaTruckDockingModuleFragment": "Seatruck Docking Module Fragment",
    "SeaTruckStorageModuleFragment": "Seatruck Storage Module Fragment",
    "SeaTruckFabricatorModuleFragment": "Seatruck Fabricator Module Fragment",
    "SeaTruckAquariumModuleFragment": "Seatruck Aquarium Module Fragment",
    "SeaTruckSleeperModuleFragment": "Seatruck Sleeper Module Fragment",
    "SeaTruckUpgradeHorsePowerFragment": "Seatruck Horsepower Upgrade Fragment",
    "SeaTruckUpgradeAfterburnerFragment": "Seatruck Afterburner Upgrade Fragment",
    "NuclearReactorFragment": "Nuclear Reactor Fragment",
    "ThermalPlantFragment": "Thermal Plant Fragment",
    "RadioTowerPPUFragment": "Radio Tower PPU Fragment",
    "RadioTowerTOMFragment": "Radio Tower TOM Fragment",
    "MetalDetectorFragment": "Mineral Detector Fragment",
    "HydraulicFluidFragment": "Hydraulic Fluid Fragment",
    "ColdSuitFragment": "Cold Suit Fragment",
    "HighCapacityTankFragment": "Ultra High Capacity Tank Fragment",
    "ReinforcedDiveSuitFragment": "Reinforced Dive Suit Fragment",
    "GravSphereFragment": "Grav Trap Fragment",
    "LEDLightFragment": "Light Stick",
    "SpyPenguinFragment": "Spy Pengling Fragment",

    # BLUEPRINTS
    "Rebreather": "Rebreather",
    "SwimChargeFins": "Swim Charge Fins",
    "BatteryCharger": "Battery Charger",
    "PowerCellCharger": "Power Cell Charger",
    "WaterFiltrationSuit": "Water Filtration Suit",
    "SeaTruckTeleportationModule": "Seatruck Teleportation Module",
    "BaseMoonpool": "Moonpool",
    "BaseControlRoom": "Control Room",
    "BaseMapRoom": "Scanner Room",
    "Beacon": "Beacon",
    "DiveReel": "Pathfinder Tool",
    "SuitBoosterTank": "Booster Tank",
    "FlashlightHelmet": "Headlamp",
    "DoubleTank": "High Capacity O2 Tank",
    "Recyclotron": "Recyclotron",
    "TeleportationTool": "Tether Tool",
    "Thumper": "Thumper",
    "QuantumLocker": "Quantum Locker",
    "HoverbikeJumpModule": "Snowfox Jump Module",
    "HoverbikeIceWormReductionModule": "Snowfox Ice Worm Attack Reduction Module",

    # ITEMS
    "Diamond": "Diamond",
    "Battery": "Battery",
    "PowerCell": "Power Cell",
    "FilteredWater": "Filtered Water",
    "Titanium": "Titanium",
    "Copper": "Copper",
    "Gold": "Gold",
    "Silver": "Silver",
    "Lead": "Lead",
    "Kyanite": "Kyanite",
    "Lithium": "Lithium",
    "Magnetite": "Magnetite",
    "Nickel": "Nickel",
    "Quartz": "Quartz",
    "AluminumOxide": "Ruby",
    "SnowBall": "Snowball",
}

# Mappings par type
fragments_list: List[str] = [
    "SeaglideFragment",
    "PropulsionCannonFragment",
    "LaserCutterFragment",
    "ExosuitFragment",
    "SeaTruckFragment",
    "BuilderFragment",
    "ExosuitDrillArmFragment",
    "ExosuitGrapplingArmFragment",
    "ExosuitPropulsionArmFragment",
    "ExosuitTorpedoArmFragment",
    "SeaTruckDockingModuleFragment",
    "SeaTruckStorageModuleFragment",
    "SeaTruckFabricatorModuleFragment",
    "SeaTruckAquariumModuleFragment",
    "SeaTruckSleeperModuleFragment",
    "SeaTruckUpgradeHorsePowerFragment",
    "SeaTruckUpgradeAfterburnerFragment",
    "NuclearReactorFragment",
    "ThermalPlantFragment",
    "RadioTowerPPUFragment",
    "RadioTowerTOMFragment",
    "MetalDetectorFragment",
    "HydraulicFluidFragment",
    "ColdSuitFragment",
    "HighCapacityTankFragment",
    "ReinforcedDiveSuitFragment",
    "GravSphereFragment",
    "SpyPenguinFragment",
    "LEDLightFragment",
]

blueprints_list: List[str] = [
    "Rebreather",
    "SwimChargeFins",
    "BatteryCharger",
    "PowerCellCharger",
    "WaterFiltrationSuit",
    "SeaTruckTeleportationModule",
    "BaseMoonpool",
    "BaseControlRoom",
    "BaseMapRoom",
    "Beacon",
    "DiveReel",
    "SuitBoosterTank",
    "FlashlightHelmet",
    "DoubleTank",
    "Recyclotron",
    "TeleportationTool",
    "Thumper",
    "QuantumLocker",
    "HoverbikeJumpModule",a
    "HoverbikeIceWormReductionModule",
]

resources_list: List[str] = [
    "Diamond",
    "Battery",
    "PowerCell",
    "FilteredWater",
    "Titanium",
    "Copper",
    "Gold",
    "Silver",
    "Lead",
    "Kyanite",
    "Lithium",
    "Magnetite",
    "Nickel",
    "Quartz",
    "AluminumOxide",
    "SnowBall",
]


def get_display_name(tech_type: str) -> str:
    """Obtient le nom personnalisé d'un item ou retourne le tech_type"""
    return CUSTOM_NAMES.get(tech_type, tech_type)


def get_all_rewards() -> List[str]:
    """Retourne une liste de toutes les récompenses disponibles"""
    return list(AVAILABLE_REWARDS.keys())


def get_rewards_by_type(reward_type: RewardType) -> List[str]:
    """Retourne toutes les récompenses d'un type spécifique"""
    return [name for name, rtype in AVAILABLE_REWARDS.items() if rtype == reward_type]
