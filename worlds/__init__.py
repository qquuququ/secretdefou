from __future__ import annotations

import itertools
from typing import List, Dict, Any, cast

from BaseClasses import Region, Location, Item, Tutorial, ItemClassification
from worlds.AutoWorld import World, WebWorld
from . import items
from . import locations
from . import creatures
from . import options
from .items import item_table, group_items
from .rules import set_rules


class SubnauticaBZWeb(WebWorld):
    tutorials = [Tutorial(
        "Multiworld Setup Guide",
        "A guide to setting up the Subnautica BZ randomizer connected to an Archipelago Multiworld",
        "English",
        "setup_en.md",
        "setup/en",
        [""]
    )]


all_locations = {data["name"]: loc_id for loc_id, data in locations.location_table.items()}
all_locations.update(creatures.creature_locations)


class SubnauticaBZWorld(World):
    """
    Subnautica: Below Zero is an undersea exploration game. Stranded on an alien world, you become infected by
    an unknown bacteria. The planet's automatic quarantine will shoot you down if you try to leave.
    You must find a cure for yourself, build an escape rocket, and leave the planet.
    """
    game = "Subnautica: Below Zero"
    web = SubnauticaBZWeb()

    item_name_to_id = {data.name: item_id for item_id, data in items.item_table.items()}
    location_name_to_id = all_locations
    options_dataclass = options.SubnauticaBZOptions
    options: options.SubnauticaBZOptions
    required_client_version = (0, 6, 3)
    origin_region_name = "Planet 4546B"
    creatures_to_scan: List[str]

    def generate_early(self) -> None:
        if not self.options.filler_items_distribution.weights_pair[1][-1]:
            raise Exception("Filler Items Distribution needs at least one positive weight.")
        if self.options.early_seaglide:
            self.multiworld.local_early_items[self.player]["Seaglide Fragment"] = 2

        # Get creature pool based on containment option
        if self.options.include_containment_creatures:
            creature_pool = list(creatures.all_creatures.keys())
        else:
            creature_pool = creatures.definitions.all_creatures_presorted_without_containment

        self.options.creature_scans.value = min(
            len(creature_pool),
            self.options.creature_scans.value
        )

        self.creatures_to_scan = self.random.sample(
            creature_pool, self.options.creature_scans.value)

    def create_regions(self):
        # Create Region
        planet_region = Region("Planet 4546B", self.player, self.multiworld)

        # Create regular locations
        location_names = itertools.chain((location["name"] for location in locations.location_table.values()),
                                         (creature + creatures.suffix for creature in self.creatures_to_scan))
        for location_name in location_names:
            loc_id = self.location_name_to_id[location_name]
            location = SubnauticaBZLocation(self.player, location_name, loc_id, planet_region)
            planet_region.locations.append(location)

        # Create events
        goal_event_name = self.options.goal.get_event_name()

        for event in locations.events:
            location = SubnauticaBZLocation(self.player, event, None, planet_region)
            planet_region.locations.append(location)
            location.place_locked_item(
                SubnauticaBZItem(event, ItemClassification.progression, None, player=self.player))
            if event == goal_event_name:
                # make the goal event the victory "item"
                location.item.name = "Victory"

        # Register region to multiworld
        self.multiworld.regions.append(planet_region)

    # refer to rules.py
    set_rules = set_rules

    def create_items(self):
        # Generate item pool
        pool: List[SubnauticaBZItem] = []
        extras = self.options.creature_scans.value

        grouped = set(itertools.chain.from_iterable(group_items.values()))

        for item_id, item in item_table.items():
            if item_id in grouped:
                extras += item.count
            else:
                for i in range(item.count):
                    subnautica_bz_item = self.create_item(item.name)
                    pool.append(subnautica_bz_item)

        group_amount: int = 2
        assert len(group_items) * group_amount <= extras
        for item_id in group_items:
            name = item_table[item_id].name
            for _ in range(group_amount):
                pool.append(self.create_item(name))
            extras -= group_amount

        for item_name in self.random.sample(
                # list of high-count important fragments as priority filler
                [
                    "Seatruck Fragment",
                    "Prawn Suit Fragment",
                    "Parallel Processing Unit Fragment",
                    "Test Override Module Fragment",
                    "Hydraulic Fluid Fragment",
                    "Mobile Vehicle Bay Fragment",
                    "Modification Station Fragment",
                    "Moonpool",
                    "Laser Cutter Fragment",
                ],
                k=min(extras, 9)):
            item = self.create_item(item_name)
            pool.append(item)
            extras -= 1

        # resource bundle filler
        for _ in range(extras):
            item = self.create_filler()
            item = cast(SubnauticaBZItem, item)
            pool.append(item)

        self.multiworld.itempool += pool

    def fill_slot_data(self) -> Dict[str, Any]:
        vanilla_tech: List[str] = []
        pickup_rewards = {}
        scan_rewards = {}
        location_id_to_item = {}  # ✅ AJOUTER CETTE LIGNE

        for location in self.multiworld.get_locations(self.player):
            if location.item is None:
                continue

            if location.name in self.location_name_to_id:
                pickup_rewards[location.name] = location.item.name
                # ✅ AJOUTER CES 3 LIGNES
                loc_id = self.location_name_to_id[location.name]
                location_id_to_item[loc_id] = location.item.name

            for creature in self.creatures_to_scan:
                if location.name == f"{creature} Scan":
                    scan_rewards[creature] = location.item.name
                    break

        # ✅ AJOUTER CES LIGNES
        # 1. Créer item_code_to_tech_type
        item_code_to_tech_type = {}
        for item_id, item_data in item_table.items():
            item_code_to_tech_type[item_id] = item_data.tech_type
        
        # 2. Créer item_code_to_item_type
        item_code_to_item_type = {}
        for item_id, item_data in item_table.items():
            # Convertir ItemClassification en string
            classification = item_data.classification.name  # "progression", "useful", "filler"
            if classification == "progression":
                item_type = "progression"
            elif classification == "useful":
                item_type = "useful"
            else:
                item_type = "resource"  # filler -> resource
            item_code_to_item_type[item_id] = item_type
        
        # 3. Ajouter group_items
        slot_group_items = {
            str(group_id): list(item_ids)
            for group_id, item_ids in group_items.items()
        }

        slot_data: Dict[str, Any] = {
            "goal": "AlAn_WarpAway",
            "swim_rule": self.options.swim_rule.current_key,
            "vanilla_tech": vanilla_tech,
            "creatures_to_scan": self.creatures_to_scan,
            "death_link": self.options.death_link.value,
            "free_samples": self.options.free_samples.value,
            "empty_tanks": self.options.empty_tanks.value,
            "pickup_rewards": pickup_rewards,
            "scan_rewards": scan_rewards,
            # ✅ AJOUTER CES 4 LIGNES
            "item_code_to_tech_type": item_code_to_tech_type,
            "item_code_to_item_type": item_code_to_item_type,
            "group_items": slot_group_items,
            "location_id_to_item": location_id_to_item,
        }

        return slot_data

    def create_item(self, name: str) -> SubnauticaBZItem:
        item_id: int = self.item_name_to_id[name]

        return SubnauticaBZItem(name,
                                item_table[item_id].classification,
                                item_id, player=self.player)

    def get_filler_item_name(self) -> str:
        item_names, cum_item_weights = self.options.filler_items_distribution.weights_pair
        return self.random.choices(item_names,
                                   cum_weights=cum_item_weights,
                                   k=1)[0]


class SubnauticaBZLocation(Location):
    game: str = "Subnautica: Below Zero"


class SubnauticaBZItem(Item):
    game: str = "Subnautica: Below Zero"


__all__ = ["SubnauticaBZWorld"]
