import functools
from typing import Dict, List, Set

# Subnautica BZ Creatures
all_creatures: Dict[str, int] = {
    "Arctic Peeper": 0,
    "Arctic Ray": 0,
    "Arrow Ray": 400,
    "Bladderfish": 0,
    "Boomerang": 0,
    "Brinewing": 0,
    "Brute Shark": 0,
    "Chelicerate": 0,
    "Crash": 100,
    "Cryptosuchus": 0,
    "Discus Fish": 300,
    "Jellyfish": 0,
    "Feather Fish": 0,
    "Feather Fish Red": 300,
    "Glow Whale": 0,
    "Hive Plant": 200,
    "Hoopfish": 0,
    "Ice Worm": 0,
    "Small Vent Garden": 200,
    "Lily Paddler": 50,
    "Noot Fish": 150,
    "Penguin Baby": 0,
    "Penguin": 0,
    "Pinnacarid": 0,
    "Rock Puncher": 0,
    "Rockgrub": 150,
    "Sea Monkey": 0,
    "Sea Monkey Baby": 100,
    "Shadow Leviathan": 550,
    "Skyray": 0,
    "Snow Stalker": 0,
    "Snow Stalker Baby": 0,
    "Spikey Trap": 350,
    "Spinefish": 300,
    "Spinner Fish": 100,
    "Squid Shark": 150,
    "Symbiote": 0,
    "Titan Holefish": 0,
    "Triops": 150,
    "Trivalve Blue": 0,
    "Trivalve Yellow": 500,
    "Large Vent Garden": 300,
}

containment: Set[str] = {  # creatures that have to be raised from eggs
    "Sea Monkey Baby",
    "Trivalve Blue",
    "Trivalve Yellow",
}

suffix: str = " Scan"

# ✅ FIX: Commencer à 34200 pour éviter les collisions avec databoxes et PDAs
creature_locations: Dict[str, int] = {
    creature + suffix: creature_id for creature_id, creature in enumerate(all_creatures, start=34200)
}

class Definitions:
    """Only compute lists if needed and then cache them."""

    @functools.cached_property
    def all_creatures_presorted(self) -> List[str]:
        return sorted(all_creatures)

    @functools.cached_property
    def all_creatures_presorted_without_containment(self) -> List[str]:
        return [name for name in self.all_creatures_presorted if name not in containment]

# only singleton needed
definitions: Definitions = Definitions()