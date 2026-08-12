from typing import Dict, TypedDict, List
from .creatures import creature_locations


class LocationDict(TypedDict, total=False):
    name: str
    position: Dict[str, float]


events: List[str] = [
    "Leave with Al-An",
]

location_table: Dict[int, LocationDict] = {
    # ========== DATABOXES ==========
    34000: {'name': 'Twisty Bridges Seabase Tube - Databox', 'position': {'x': -252.6308, 'y': -127.818, 'z': -242.0844}},
    34001: {'name': 'Twisty Bridges Alterra Crane - Databox', 'position': {'x': 107.467, 'y': -33.983, 'z': -10.09099}},
    34002: {'name': 'Sparse Arctic Alterra Crane - Platform Databox', 'position': {'x': -371.7426, 'y': -172.453, 'z': -314.7984}},
    34003: {'name': 'Purple Vents Water Analysis Station - Outside Databox', 'position': {'x': -1234.3, 'y': -349.7, 'z': -396.0}},
    34004: {'name': 'Purple Vents Small Debris Wreck - Databox', 'position': {'x': 225.72, 'y': -104.125, 'z': -621.39}},
    34005: {'name': 'Marguerit Seabase - Purple Vents - Outside Databox', 'position': {'x': 48.672, 'y': -410.821, 'z': -869.683}},
    34006: {'name': 'Delta Station - Outside Databox', 'position': {'x': -261.115, 'y': 40.169, 'z': -771.852}},
    34007: {'name': 'Omega Lab - Lilypads Islands - Outside Databox', 'position': {'x': 548.336, 'y': -203.589, 'z': -1076.432}},
    34008: {'name': 'Omega Lab - Lilypads Islands - Greenhouse Databox', 'position': {'x': 564.533, 'y': -203.203, 'z': -1074.353}},
    34009: {'name': 'Tree Spires Fissure Platform - Databox', 'position': {'x': 1.976, 'y': -453.015, 'z': -1116.554}},
    34010: {'name': 'West Mining Platform - Arctic Spires - Databox', 'position': {'x': -1337.014, 'y': 36.735, 'z': -265.8504}},
    34011: {'name': 'Diamond Mining Platform - Arctic Spires - Databox', 'position': {'x': -964.342, 'y': 69.718, 'z': -60.39}},
    34012: {'name': 'Alterra Platform - Arctic Spires - Databox', 'position': {'x': -1097.763, 'y': 62.789, 'z': 338.602}},
    34013: {'name': 'Alterra Platform - Glacial Basin - Databox', 'position': {'x': -1515.366, 'y': 11.032, 'z': -1038.79}},
    34014: {'name': 'Outpost Zero - Laboratory Databox', 'position': {'x': -98.275, 'y': 14.046, 'z': 329.966}},
    34015: {'name': 'Koppa Mining Site - Entrance Databox', 'position': {'x': -293.853, 'y': -152.892, 'z': -778.356}},
    34016: {'name': 'Deep Koppa Mining Site - Crate Databox', 'position': {'x': -197.09, 'y': -274.411, 'z': -707.637}},
    34017: {'name': 'Deep Koppa Mining Site - Lift Databox', 'position': {'x': -97.61, 'y': -296.996, 'z': -644.22}},

    # ========== PDAS ==========
    34100: {'name': 'Twisty Bridges Seabase Tube - Opposite Ledge PDA', 'position': {'x': -215.9167, 'y': -121.4516, 'z': -283.3836}},
    34101: {'name': 'Sparse Arctic Alterra Crane - Above Platform PDA', 'position': {'x': 81.72634, 'y': -18.5626, 'z': 1.81261}},
    34102: {'name': 'Kelp Forest Emergency Supply Cache - PDA', 'position': {'x': -588.1724, 'y': -34.06147, 'z': 24.55146}},
    34103: {'name': 'Twisty Bridges Seatruck Wreck - PDA 1', 'position': {'x': -214.9648, 'y': -106.9843, 'z': -200.7542}},
    34104: {'name': 'Twisty Bridges Seatruck Wreck - PDA 2', 'position': {'x': -212.9176, 'y': -108.4616, 'z': -201.0399}},
    34105: {'name': 'Purple Vents Small Debris Wreck - PDA', 'position': {'x': 230.1064, 'y': -99.09208, 'z': -628.6053}},
    34106: {'name': 'Marguerit Seabase - Purple Vents - Workshop PDA', 'position': {'x': 80.52015, 'y': -375.9301, 'z': -916.1251}},
    34107: {'name': 'Marguerit Seabase - Purple Vents - Bedroom PDA', 'position': {'x': 60.95208, 'y': -375.6388, 'z': -918.0844}},
    34108: {'name': 'Mercury II Stern - Purple Vents - Engine Room PDA', 'position': {'x': 70.15193, 'y': -89.40207, 'z': -863.3396}},
    34109: {'name': 'Mercury II Stern - Purple Vents - Hidden Room PDA', 'position': {'x': 55.55484, 'y': -92.00343, 'z': -886.0209}},
    34110: {'name': 'Delta Station - Break Room PDA', 'position': {'x': -253.9896, 'y': 42.33442, 'z': -788.3668}},
    34111: {'name': 'Delta Station - Bedroom Bed PDA', 'position': {'x': -266.7728, 'y': 42.28731, 'z': -779.4988}},
    34112: {'name': 'Delta Station - Bedroom Desk PDA', 'position': {'x': -269.9761, 'y': 42.26775, 'z': -780.1193}},
    34113: {'name': 'Delta Station - Office Desk PDA', 'position': {'x': -282.3356, 'y': 47.635, 'z': -765.4014}},
    34114: {'name': 'Delta Station - Office Table PDA', 'position': {'x': -286.9537, 'y': 47.635, 'z': -757.7156}},
    34115: {'name': 'Communications Tower - Outside Crate PDA', 'position': {'x': -210.3686, 'y': 48.49123, 'z': -739.967}},
    34116: {'name': 'Communications Tower - Near Panel PDA', 'position': {'x': -214.1502, 'y': 55.53, 'z': -721.2535}},
    34117: {'name': 'Delta Island Cave - Desk PDA', 'position': {'x': -235.1454, 'y': 31.2412, 'z': -729.2964}},
    34118: {'name': 'Delta Island Dock - PDA', 'position': {'x': -137.0995, 'y': 2.540001, 'z': -573.1951}},
    34119: {'name': 'Marguerit Greenhouse - East Arctic - Counter PDA', 'position': {'x': 986.9686, 'y': 31.20709, 'z': -892.6783}},
    34120: {'name': 'Marguerit Greenhouse - East Arctic - Desk PDA', 'position': {'x': 988.3533, 'y': 31.20709, 'z': -896.1426}},
    34121: {'name': 'Omega Lab - Lilypads Islands - Greenhouse PDA', 'position': {'x': 562.646, 'y': -203.2453, 'z': -1072.332}},
    34122: {'name': 'Omega Lab - Lilypads Islands - Greenhouse Desk PDA', 'position': {'x': 565.294, 'y': -202.7185, 'z': -1068.671}},
    34123: {'name': 'Omega Lab - Lilypads Islands - Danielle\'s Bed PDA', 'position': {'x': 543.3214, 'y': -203.0938, 'z': -1068.473}},
    34124: {'name': 'Omega Lab - Lilypads Islands - Vinh\'s Bed PDA', 'position': {'x': 545.1058, 'y': -202.2226, 'z': -1063.6}},
    34125: {'name': 'Omega Lab - Lilypads Islands - Lab PDA', 'position': {'x': 552.7621, 'y': -203.6689, 'z': -1057.376}},
    34126: {'name': 'Mercury II Bow - Lilypads Islands - Growbed PDA', 'position': {'x': 262.9326, 'y': -237.4497, 'z': -1287.118}},
    34127: {'name': 'Mercury II Bow - Lilypads Islands - Alien Containment PDA', 'position': {'x': 255.4886, 'y': -235.4721, 'z': -1249.968}},
    34128: {'name': 'Mercury II Bow - Lilypads Islands - Bridge PDA', 'position': {'x': 270.0131, 'y': -235.9435, 'z': -1318.073}},
    34129: {'name': 'Mercury II Bow - Lilypads Islands - Bunk Bed PDA 1', 'position': {'x': 248.9509, 'y': -249.9548, 'z': -1274.931}},
    34130: {'name': 'Mercury II Bow - Lilypads Islands - Bunk Bed PDA 2', 'position': {'x': 232.4889, 'y': -256.5203, 'z': -1283.042}},
    34131: {'name': 'Mercury II Bow - Lilypads Islands - Bunk Bed PDA 3', 'position': {'x': 233.5361, 'y': -256.8545, 'z': -1293.487}},
    34132: {'name': 'Mercury II Bow - Lilypads Islands - Nuclear Reactor PDA', 'position': {'x': 286.5875, 'y': -232.9995, 'z': -1272.352}},
    34133: {'name': 'Tree Spires Fissure Platform - PDA', 'position': {'x': -4.0049, 'y': -452.5248, 'z': -1114.992}},
    34134: {'name': 'Glacial Bay Dock - PDA', 'position': {'x': -1029.996, 'y': 2.347006, 'z': -365.9128}},
    34135: {'name': 'West Mining Platform - Arctic Spires - Platform PDA', 'position': {'x': -1342.291, 'y': 37.28, 'z': -267.8291}},
    34136: {'name': 'West Mining Platform - Arctic Spires - Crate PDA', 'position': {'x': -1330.58, 'y': 32.22618, 'z': -257.8538}},
    34137: {'name': 'Diamond Mining Platform - Arctic Spires - PDA', 'position': {'x': -962.523, 'y': 70.35323, 'z': -60.73063}},
    34138: {'name': 'Phi Robotics Center - Outside PDA', 'position': {'x': -1173.074, 'y': 5.735222, 'z': -599.7462}},
    34139: {'name': 'Phi Robotics Center - Office PDA', 'position': {'x': -1193.984, 'y': 22.57739, 'z': -708.8841}},
    34140: {'name': 'Phi Robotics Center - Upstairs Counter PDA', 'position': {'x': -1182.963, 'y': 18.81464, 'z': -715.9973}},
    34141: {'name': 'Phi Robotics Center - Bottom Floor Crate PDA', 'position': {'x': -1164.955, 'y': 17.93466, 'z': -710.1385}},
    34142: {'name': 'Phi Robotics Center - Zeta\'s Bedroom PDA', 'position': {'x': -1153.918, 'y': 17.99703, 'z': -724.8858}},
    34143: {'name': 'Phi Robotics Center - Sam\'s Bedroom PDA', 'position': {'x': -1162.849, 'y': 18.27229, 'z': -720.6103}},
    34144: {'name': 'Alterra Cache Cave - Glacial Basin - PDA', 'position': {'x': -1110.287, 'y': 9.368934, 'z': -808.0339}},
    34145: {'name': 'Small Lake - Glacial Basin - Crates Above Lake PDA', 'position': {'x': -1292.603, 'y': 11.53529, 'z': -1029.864}},
    34146: {'name': 'Big Basin Stalker Cavern - Deep Inside PDA', 'position': {'x': -1247.08, 'y': 30.98129, 'z': -1226.74}},
    34147: {'name': 'Southern Glacial Basin - Spy Pengling Access Point PDA', 'position': {'x': -1492.676, 'y': 27.00033, 'z': -1202.602}},
    34148: {'name': 'Parvan\'s Bunker - Bed PDA', 'position': {'x': -1600.402, 'y': 18.789, 'z': -819.7835}},
    34149: {'name': 'Parvan\'s Bunker - Crate PDA', 'position': {'x': -1603.437, 'y': 18.789, 'z': -818.2996}},
    34150: {'name': 'Path To Excavation Site - Glacial Basin - Ground PDA', 'position': {'x': -1642.33, 'y': 29.41844, 'z': -772.5251}},
    34151: {'name': 'Path To Excavation Site - Glacial Basin - Table PDA', 'position': {'x': -1618.997, 'y': 28.6251, 'z': -765.2982}},
    34152: {'name': 'Ledge Over Excavation Site - Glacial Basin - PDA', 'position': {'x': -1606.001, 'y': 43.25556, 'z': -731.5477}},
    34153: {'name': 'Outpost Zero - Lab Middle Counter PDA', 'position': {'x': -101.5099, 'y': 14.56332, 'z': 326.0944}},
    34154: {'name': 'Outpost Zero - Lab Left Counter PDA', 'position': {'x': -109.2666, 'y': 15.0017, 'z': 328.079}},
    34155: {'name': 'Outpost Zero - Kitchen Corner Counter PDA', 'position': {'x': -113.3186, 'y': 14.73585, 'z': 312.8183}},
    34156: {'name': 'Outpost Zero - Kitchen Bench PDA', 'position': {'x': -106.239, 'y': 14.33043, 'z': 293.0095}},
    34157: {'name': 'Outpost Zero - Sam\'s Bed PDA', 'position': {'x': -122.3746, 'y': 14.902, 'z': 296.0627}},
    34158: {'name': 'Outpost Zero - Lillian\'s Bed PDA', 'position': {'x': -124.2811, 'y': 14.902, 'z': 296.4579}},
    34159: {'name': 'Outpost Zero - Greenhouse PDA', 'position': {'x': -108.5517, 'y': 14.4567, 'z': 282.3088}},
    34160: {'name': 'Kelp Forest - Near Sea Monkey Nest PDA', 'position': {'x': -169.3967, 'y': -50.14649, 'z': -114.2751}},
    34161: {'name': 'Deep Twisty Bridges - Entrance PDA', 'position': {'x': -309.3701, 'y': -239.6826, 'z': -335.9702}},
    34162: {'name': 'Deep Twisty Bridges - Deep Near Fragment PDA', 'position': {'x': -435.93, 'y': -349.7274, 'z': -352.0994}},
    34163: {'name': 'Deep Koppa Mining Site - Near Closed Door PDA', 'position': {'x': -182.3849, 'y': -273.0319, 'z': -763.1766}},
    34164: {'name': 'Deep Koppa Mining Site - Table PDA', 'position': {'x': -218.8873, 'y': -284.7715, 'z': -682.2697}},
    34165: {'name': 'Deep Koppa Mining Site - Desk Near Lift PDA', 'position': {'x': -105.7205, 'y': -294.9391, 'z': -654.4979}},
}

location_table.update({
    loc_id: {"name": loc_name}
    for loc_name, loc_id in creature_locations.items()
})