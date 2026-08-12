from __future__ import annotations

from typing import TYPE_CHECKING, Dict, Callable, Optional

from worlds.generic.Rules import set_rule, add_rule
from .locations import location_table, LocationDict
from .creatures import all_creatures, suffix, containment
from .options import SwimRule
import math

if TYPE_CHECKING:
    from . import SubnauticaBZWorld
    from BaseClasses import CollectionState, Location


def has_seaglide(state: "CollectionState", player: int) -> bool:
    return state.has("Seaglide Fragment", player, 3)


def has_modification_station(state: "CollectionState", player: int) -> bool:
    return state.has("Modification Station Fragment", player, 1)


def has_mobile_vehicle_bay(state: "CollectionState", player: int) -> bool:
    return state.has("Mobile Vehicle Bay Fragment", player, 3)


def has_moonpool(state: "CollectionState", player: int) -> bool:
    return state.has("Moonpool", player, 1)


def has_vehicle_upgrade_console(state: "CollectionState", player: int) -> bool:
    return state.has("Vehicle Upgrade Console", player) and \
           has_moonpool(state, player)


def has_seatruck(state: "CollectionState", player: int) -> bool:
    return state.has("Seatruck Fragment", player, 3) and \
           has_mobile_vehicle_bay(state, player)


def has_seatruck_depth_module_mk1(state: "CollectionState", player: int) -> bool:
    return has_vehicle_upgrade_console(state, player)


def has_seatruck_depth_module_mk2(state: "CollectionState", player: int) -> bool:
    return has_seatruck_depth_module_mk1(state, player) and \
           has_modification_station(state, player)


def has_seatruck_depth_module_mk3(state: "CollectionState", player: int) -> bool:
    return has_seatruck_depth_module_mk2(state, player) and \
           has_modification_station(state, player)


def has_prawn(state: "CollectionState", player: int) -> bool:
    return state.has("Prawn Suit Fragment", player, 4) and \
           has_mobile_vehicle_bay(state, player)


def has_prawn_propulsion_arm(state: "CollectionState", player: int) -> bool:
    return state.has("Prawn Suit Propulsion Cannon Fragment", player, 2) and \
           has_vehicle_upgrade_console(state, player)


def has_prawn_depth_module_mk1(state: "CollectionState", player: int) -> bool:
    return has_vehicle_upgrade_console(state, player)


def has_prawn_depth_module_mk2(state: "CollectionState", player: int) -> bool:
    return has_prawn_depth_module_mk1(state, player) and \
           has_modification_station(state, player)


def has_containment(state: "CollectionState", player: int) -> bool:
    return state.has("Alien Containment", player) and has_utility_room(state, player)


def has_utility_room(state: "CollectionState", player: int) -> bool:
    return state.has("Large Room", player) or state.has("Multipurpose Room", player)


def has_ultra_high_capacity_tank(state: "CollectionState", player: int) -> bool:
    return has_modification_station(state, player) and state.has("Ultra High Capacity Tank", player)


def has_cyclops_shield(state: "CollectionState", player: int) -> bool:
    return state.has("Cyclops Depth Module MK1", player)

# Swim depth rules:
# Rebreather, high capacity tank and fins are available from the start.
# All tests for those were done without inventory for light weight.
# Fins and ultra Fins are better than charge fins, so we ignore charge fins.

# swim speeds: https://subnautica.fandom.com/wiki/Swimming_Speed


def get_max_swim_depth(state: "CollectionState", player: int) -> int:
    swim_rule: SwimRule = state.multiworld.worlds[player].options.swim_rule
    depth: int = swim_rule.base_depth
    if swim_rule.consider_items:
        if has_seaglide(state, player):
            if has_ultra_high_capacity_tank(state, player):
                depth += 350  # It's about 800m. Give some room
            else:
                depth += 200  # It's about 650m. Give some room
        # seaglide and fins cannot be used together
        elif has_ultra_high_capacity_tank(state, player):
            depth += 100
    return depth


def get_seatruck_max_depth(state: "CollectionState", player: int):
    if has_seatruck(state, player):
        if has_seatruck_depth_module_mk3(state, player):
            return 1000
        elif has_seatruck_depth_module_mk2(state, player): # Will never be the case, 3 is craftable
            return 650
        elif has_seatruck_depth_module_mk1(state, player):
            return 300
        else:
            return 200
    else:
        return 0


def get_prawn_max_depth(state: "CollectionState", player):
    if has_prawn(state, player):
        if has_prawn_depth_module_mk2(state, player):
            return 1100
        elif has_prawn_depth_module_mk1(state, player):
            return 700
        else:
            return 400
    else:
        return 0


def get_max_depth(state: "CollectionState", player: int):
    return get_max_swim_depth(state, player) + max(
        get_seatruck_max_depth(state, player),
        get_prawn_max_depth(state, player)
    )


def can_access_location(state: "CollectionState", player: int, loc: LocationDict) -> bool:
    # Les creature scans n'ont pas de position, retourner True (accessible)
    if "position" not in loc:
        return True
    
    pos = loc["position"]
    pos_x = pos["x"]
    pos_y = pos["y"]
    pos_z = pos["z"]

    # Seaglide doesn't unlock anything specific, but just allows for faster movement. 
    # Otherwise the game is painfully slow.
    map_center_dist = math.sqrt(pos_x ** 2 + pos_z ** 2)
    if (map_center_dist > 800 or pos_y < -200) and not has_seaglide(state, player):
        return False

    depth = -pos_y  # y-up
    return get_max_depth(state, player) >= depth


def set_location_rule(world: "SubnauticaBZWorld", player: int, loc: LocationDict):
    set_rule(world.get_location(loc["name"]), lambda state: can_access_location(state, player, loc))


def can_scan_creature(state: "CollectionState", player: int, creature: str) -> bool:
    if not has_seaglide(state, player):
        return False
    return get_max_depth(state, player) >= all_creatures[creature]


def set_creature_rule(world: "SubnauticaBZWorld", player: int, creature_name: str) -> "Location":
    location = world.get_location(creature_name + suffix)
    set_rule(location,
             lambda state: can_scan_creature(state, player, creature_name))
    return location


def set_rules(subnautica_bz_world: "SubnauticaBZWorld") -> None:
    player = subnautica_bz_world.player

    for loc in location_table.values():
        set_location_rule(subnautica_bz_world, player, loc)

    if subnautica_bz_world.creatures_to_scan:
        for creature_name in subnautica_bz_world.creatures_to_scan:
            location = set_creature_rule(subnautica_bz_world, player, creature_name)
            if creature_name in containment:  # there is no other way, hard-required containment
                add_rule(location, lambda state: has_containment(state, player))

    # Victory locations
    set_rule(subnautica_bz_world.get_location("Leave with Al-An"),
             lambda state:
             state.has("Architect Organs", player) and
             state.has("Architect Skeleton", player) and
             state.has("Architect Tissues", player))

    subnautica_bz_world.multiworld.completion_condition[player] = lambda state: state.has("Victory", player)
