import typing
from dataclasses import dataclass
from functools import cached_property

from Options import (
    Choice,
    Range,
    DeathLink,
    Toggle,
    DefaultOnToggle,
    StartInventoryPool,
    ItemDict,
    PerGameCommonOptions,
)

from .creatures import all_creatures, containment, Definitions
from .items import ItemType, item_names_by_type


class SwimRule(Choice):
    """What logic considers ok swimming distances.
    Easy: +200 depth from any max vehicle depth.
    Normal: +400 depth from any max vehicle depth.
    Warning: Normal can expect you to death run to a location (No viable return trip).
    Hard: +600 depth from any max vehicle depth.
    Warning: Hard may require bases, deaths, glitches, multi-tank inventory or other depth extending means.
    Items: Expected depth is extended by items like seaglide, ultra glide fins and capacity tanks.
    """
    display_name = "Swim Rule"
    option_easy = 0
    option_normal = 1
    option_hard = 2
    option_items_easy = 3
    option_items_normal = 4
    option_items_hard = 5

    @property
    def base_depth(self) -> int:
        return [200, 400, 600][self.value % 3]

    @property
    def consider_items(self) -> bool:
        return self.value > 2


class EarlySeaglide(DefaultOnToggle):
    """Make sure 2 of the Seaglide Fragments are available in or near the Safe Shallows (Sphere 1 Locations)."""
    display_name = "Early Seaglide"


class FreeSamples(Toggle):
    """Get free items with your blueprints.
    Items that can go into your inventory are awarded when you unlock their blueprint through Archipelago."""
    display_name = "Free Samples"


class Goal(Choice):
    """Goal to complete.
    Launch: Leave the planet with Al-An.
    """
    auto_display_name = True
    display_name = "Goal"
    option_launch = 0

    def get_event_name(self) -> str:
        return {
            self.option_launch: "Leave with Al-An",
        }[self.value]


class CreatureScans(Range):
    """Place items on specific, randomly chosen, creature scans.
    Warning: Includes aggressive Leviathans."""
    display_name = "Creature Scans"
    range_end = len(all_creatures)


class IncludeContainmentCreatures(DefaultOnToggle):
    """Include creatures that require Alien Containment (Sea Monkey Baby, Trivalve Blue, Trivalve Yellow) as checks.
    When disabled, these creatures are excluded from the randomizer pool."""
    display_name = "Include Containment Creatures"


class SubnauticaBZDeathLink(DeathLink):
    __doc__ = DeathLink.__doc__ + "\n\n    Note: can be toggled via in-game console command \"deathlink\"."


class FillerItemsDistribution(ItemDict):
    """Random chance weights of various filler resources that can be obtained.
    Available items: """
    __doc__ += ", ".join(f"\"{item_name}\"" for item_name in item_names_by_type[ItemType.resource])
    valid_keys = sorted(item_names_by_type[ItemType.resource])
    default = {item_name: 1 for item_name in item_names_by_type[ItemType.resource]}
    display_name = "Filler Items Distribution"

    @cached_property
    def weights_pair(self) -> typing.Tuple[typing.List[str], typing.List[int]]:
        from itertools import accumulate
        return list(self.value.keys()), list(accumulate(self.value.values()))


class EmptyTanks(DefaultOnToggle):
    """Oxygen Tanks stored in inventory are empty if enabled."""


@dataclass
class SubnauticaBZOptions(PerGameCommonOptions):
    swim_rule: SwimRule
    early_seaglide: EarlySeaglide
    free_samples: FreeSamples
    goal: Goal
    creature_scans: CreatureScans
    include_containment_creatures: IncludeContainmentCreatures
    death_link: SubnauticaBZDeathLink
    start_inventory_from_pool: StartInventoryPool
    filler_items_distribution: FillerItemsDistribution
    empty_tanks: EmptyTanks
