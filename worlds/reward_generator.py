import json
import os
import random
from typing import Dict, List, Tuple
from worlds.subnautica_bz.rewards import AVAILABLE_REWARDS, CUSTOM_NAMES, get_display_name


def generate_location_rewards(
    location_names: List[str],
    seed: int,
    reward_weights: Dict[str, float] = None
) -> Dict[str, str]:
    """
    Génère un mapping aléatoire : location_name -> reward_tech_type
    
    Args:
        location_names: Liste de tous les noms de locations (PDAs, Databoxes, Créatures)
        seed: Seed aléatoire pour la reproductibilité
        reward_weights: (Optionnel) Poids pour chaque récompense {tech_type: weight}
                       Par défaut, toutes les récompenses ont le même poids
    
    Returns:
        Dict[str, str]: {location_name: tech_type}
    """
    rng = random.Random(seed)
    all_rewards = list(AVAILABLE_REWARDS.keys())
    
    if reward_weights is None:
        # Sans poids : distribution uniforme
        reward_list = all_rewards
    else:
        # Avec poids : filtrer et créer une liste pondérée
        reward_list = [r for r in all_rewards if r in reward_weights]
        if not reward_list:
            reward_list = all_rewards
    
    location_rewards = {}
    for location_name in location_names:
        if reward_weights is None:
            # Choix uniforme
            reward = rng.choice(all_rewards)
        else:
            # Choix pondéré
            reward = rng.choices(
                reward_list,
                weights=[reward_weights.get(r, 1.0) for r in reward_list],
                k=1
            )[0]
        location_rewards[location_name] = reward
    
    return location_rewards


def generate_weighted_location_rewards(
    location_names: List[str],
    seed: int,
    progression_ratio: float = 0.3,
    useful_ratio: float = 0.4,
    filler_ratio: float = 0.3
) -> Dict[str, str]:
    """
    Génère des récompenses avec des ratios de progression/utile/remplissage
    
    Args:
        location_names: Liste de tous les noms de locations
        seed: Seed aléatoire
        progression_ratio: Pourcentage de récompenses de progression (0.0-1.0)
        useful_ratio: Pourcentage de récompenses utiles (0.0-1.0)
        filler_ratio: Pourcentage de remplissage (0.0-1.0)
    
    Returns:
        Dict[str, str]: {location_name: tech_type}
    """
    from worlds.subnautica_bz.rewards import RewardType, get_rewards_by_type
    
    rng = random.Random(seed)
    
    # Récupérer les récompenses par type
    progression_rewards = [
        r for r in get_rewards_by_type(RewardType.fragment)
        if r in ["ColdSuitFragment", "BuilderFragment", "LaserCutterFragment", 
                 "ExosuitFragment", "SeaTruckFragment"]  # Items clés
    ]
    useful_rewards = get_rewards_by_type(RewardType.blueprint)
    filler_rewards = get_rewards_by_type(RewardType.resource)
    
    # Créer un pool pondéré
    pool = []
    num_locations = len(location_names)
    
    num_progression = max(1, int(num_locations * progression_ratio))
    num_useful = max(1, int(num_locations * useful_ratio))
    num_filler = num_locations - num_progression - num_useful
    
    pool.extend(progression_rewards * ((num_progression // len(progression_rewards)) + 1))
    pool.extend(useful_rewards * ((num_useful // len(useful_rewards)) + 1))
    pool.extend(filler_rewards * ((num_filler // len(filler_rewards)) + 1))
    
    # Mélanger et découper au nombre exact
    rng.shuffle(pool)
    pool = pool[:num_locations]
    
    # Mélanger à nouveau pour l'ordre des locations
    rng.shuffle(pool)
    
    location_rewards = {
        location_name: reward
        for location_name, reward in zip(location_names, pool)
    }
    
    return location_rewards


def export_rewards_as_json(
    location_rewards: Dict[str, str],
    output_dir: str
) -> None:
    """
    Exporte les récompenses aléatoires en JSON pour le mod C#
    
    Args:
        location_rewards: Dict {location_name: tech_type}
        output_dir: Répertoire de sortie (généralement output_dir de generate_output)
    """
    rewards_data = {
        "location_rewards": location_rewards,
        "custom_names": {
            tech: get_display_name(tech)
            for tech in AVAILABLE_REWARDS.keys()
        }
    }
    
    # Créer le répertoire s'il n'existe pas
    os.makedirs(output_dir, exist_ok=True)
    
    output_file = os.path.join(output_dir, "rewards.json")
    with open(output_file, 'w', encoding='utf-8') as f:
        json.dump(rewards_data, f, indent=2, ensure_ascii=False)
    
    print(f"✓ Rewards exported to {output_file}")


def verify_rewards_coverage(
    location_rewards: Dict[str, str],
    expected_reward_count: int = None
) -> Tuple[bool, str]:
    """
    Vérifie que toutes les récompenses sont valides et présentes
    
    Args:
        location_rewards: Dict {location_name: tech_type}
        expected_reward_count: (Optionnel) Nombre attendu de récompenses
    
    Returns:
        Tuple[bool, str]: (is_valid, message)
    """
    invalid_rewards = []
    
    for location, reward in location_rewards.items():
        if reward not in AVAILABLE_REWARDS:
            invalid_rewards.append(f"  ✗ {location} -> {reward} (INVALID)")
    
    if invalid_rewards:
        msg = f"Invalid rewards found:\n" + "\n".join(invalid_rewards)
        return False, msg
    
    if expected_reward_count and len(location_rewards) != expected_reward_count:
        msg = f"Expected {expected_reward_count} rewards, got {len(location_rewards)}"
        return False, msg
    
    msg = f"✓ All {len(location_rewards)} rewards are valid"
    return True, msg


def get_reward_statistics(location_rewards: Dict[str, str]) -> Dict:
    """
    Génère des statistiques sur la distribution des récompenses
    
    Args:
        location_rewards: Dict {location_name: tech_type}
    
    Returns:
        Dict avec les statistiques
    """
    from worlds.subnautica_bz.rewards import RewardType, AVAILABLE_REWARDS
    
    stats = {
        "total_locations": len(location_rewards),
        "total_unique_rewards": len(set(location_rewards.values())),
        "by_type": {
            "fragments": 0,
            "blueprints": 0,
            "resources": 0
        },
        "most_common": {},
        "never_used": []
    }
    
    # Compter par type
    for reward in location_rewards.values():
        reward_type = AVAILABLE_REWARDS[reward]
        if reward_type == RewardType.fragment:
            stats["by_type"]["fragments"] += 1
        elif reward_type == RewardType.blueprint:
            stats["by_type"]["blueprints"] += 1
        elif reward_type == RewardType.resource:
            stats["by_type"]["resources"] += 1
    
    # Trouver les plus courants
    reward_counts = {}
    for reward in location_rewards.values():
        reward_counts[reward] = reward_counts.get(reward, 0) + 1
    stats["most_common"] = sorted(
        reward_counts.items(),
        key=lambda x: x[1],
        reverse=True
    )[:5]
    
    # Trouver ceux jamais utilisés
    used_rewards = set(location_rewards.values())
    stats["never_used"] = [r for r in AVAILABLE_REWARDS.keys() if r not in used_rewards]
    
    return stats


def print_reward_statistics(stats: Dict) -> None:
    """
    Affiche les statistiques de distribution des récompenses
    
    Args:
        stats: Dict retourné par get_reward_statistics()
    """
    print("\n" + "="*60)
    print("REWARD DISTRIBUTION STATISTICS")
    print("="*60)
    print(f"Total Locations: {stats['total_locations']}")
    print(f"Unique Rewards Used: {stats['total_unique_rewards']}")
    print(f"\nBy Type:")
    print(f"  Fragments: {stats['by_type']['fragments']}")
    print(f"  Blueprints: {stats['by_type']['blueprints']}")
    print(f"  Resources: {stats['by_type']['resources']}")
    
    print(f"\nMost Common Rewards:")
    for reward, count in stats['most_common']:
        percentage = (count / stats['total_locations']) * 100
        print(f"  {reward}: {count} ({percentage:.1f}%)")
    
    if stats['never_used']:
        print(f"\nNever Used Rewards ({len(stats['never_used'])}):")
        for reward in stats['never_used'][:10]:
            print(f"  - {reward}")
        if len(stats['never_used']) > 10:
            print(f"  ... and {len(stats['never_used']) - 10} more")
    
    print("="*60 + "\n")
