using System.Collections.Generic;

public static class ArchipelagoLocationMapping
{
    /// <summary>
    /// Mapping: Nom Subnautica BZ -> ID Archipelago
    /// Basé sur la liste de TargetRewards.cs
    /// </summary>
    public static readonly Dictionary<string, long> LocationIdMap = new Dictionary<string, long>()
    {
        // ========== DATABOXES ==========
        { "Twisty Bridges Seabase Tube - Databox", 34000 },
        { "Twisty Bridges Alterra Crane - Databox", 34001 },
        { "Sparse Arctic Alterra Crane - Platform Databox", 34002 },
        { "Purple Vents Water Analysis Station - Outside Databox", 34003 },
        { "Purple Vents Small Debris Wreck - Databox", 34004 },
        { "Marguerit Seabase - Purple Vents - Outside Databox", 34005 },
        { "Delta Station - Outside Databox", 34006 },
        { "Omega Lab - Lilypads Islands - Outside Databox", 34007 },
        { "Omega Lab - Lilypads Islands - Greenhouse Databox", 34008 },
        { "Tree Spires Fissure Platform - Databox", 34009 },
        { "West Mining Platform - Arctic Spires - Databox", 34010 },
        { "Diamond Mining Platform - Arctic Spires - Databox", 34011 },
        { "Alterra Platform - Arctic Spires - Databox", 34012 },
        { "Alterra Platform - Glacial Basin - Databox", 34013 },
        { "Outpost Zero - Laboratory Databox", 34014 },
        { "Koppa Mining Site - Entrance Databox", 34015 },
        { "Deep Koppa Mining Site - Crate Databox", 34016 },
        { "Deep Koppa Mining Site - Lift Databox", 34017 },

        // ========== PDAS ==========
        { "Twisty Bridges Seabase Tube - Opposite Ledge PDA", 34100 },
        { "Sparse Arctic Alterra Crane - Above Platform PDA", 34101 },
        { "Kelp Forest Emergency Supply Cache - PDA", 34102 },
        { "Twisty Bridges Seatruck Wreck - PDA 1", 34103 },
        { "Twisty Bridges Seatruck Wreck - PDA 2", 34104 },
        { "Purple Vents Small Debris Wreck - PDA", 34105 },
        { "Marguerit Seabase - Purple Vents - Workshop PDA", 34106 },
        { "Marguerit Seabase - Purple Vents - Bedroom PDA", 34107 },
        { "Mercury II Stern - Purple Vents - Engine Room PDA", 34108 },
        { "Mercury II Stern - Purple Vents - Hidden Room PDA", 34109 },
        { "Delta Station - Break Room PDA", 34110 },
        { "Delta Station - Bedroom Bed PDA", 34111 },
        { "Delta Station - Bedroom Desk PDA", 34112 },
        { "Delta Station - Office Desk PDA", 34113 },
        { "Delta Station - Office Table PDA", 34114 },
        { "Communications Tower - Outside Crate PDA", 34115 },
        { "Communications Tower - Near Panel PDA", 34116 },
        { "Delta Island Cave - Desk PDA", 34117 },
        { "Delta Island Dock - PDA", 34118 },
        { "Marguerit Greenhouse - East Arctic - Counter PDA", 34119 },
        { "Marguerit Greenhouse - East Arctic - Desk PDA", 34120 },
        { "Omega Lab - Lilypads Islands - Greenhouse PDA", 34121 },
        { "Omega Lab - Lilypads Islands - Greenhouse Desk PDA", 34122 },
        { "Omega Lab - Lilypads Islands - Danielle's Bed PDA", 34123 },
        { "Omega Lab - Lilypads Islands - Vinh's Bed PDA", 34124 },
        { "Omega Lab - Lilypads Islands - Lab PDA", 34125 },
        { "Mercury II Bow - Lilypads Islands - Growbed PDA", 34126 },
        { "Mercury II Bow - Lilypads Islands - Alien Containment PDA", 34127 },
        { "Mercury II Bow - Lilypads Islands - Bridge PDA", 34128 },
        { "Mercury II Bow - Lilypads Islands - Bunk Bed PDA 1", 34129 },
        { "Mercury II Bow - Lilypads Islands - Bunk Bed PDA 2", 34130 },
        { "Mercury II Bow - Lilypads Islands - Bunk Bed PDA 3", 34131 },
        { "Mercury II Bow - Lilypads Islands - Nuclear Reactor PDA", 34132 },
        { "Tree Spires Fissure Platform - PDA", 34133 },
        { "Glacial Bay Dock - PDA", 34134 },
        { "West Mining Platform - Arctic Spires - Platform PDA", 34135 },
        { "West Mining Platform - Arctic Spires - Crate PDA", 34136 },
        { "Diamond Mining Platform - Arctic Spires - PDA", 34137 },
        { "Phi Robotics Center - Outside PDA", 34138 },
        { "Phi Robotics Center - Office PDA", 34139 },
        { "Phi Robotics Center - Upstairs Counter PDA", 34140 },
        { "Phi Robotics Center - Bottom Floor Crate PDA", 34141 },
        { "Phi Robotics Center - Zeta's Bedroom PDA", 34142 },
        { "Phi Robotics Center - Sam's Bedroom PDA", 34143 },
        { "Alterra Cache Cave - Glacial Basin - PDA", 34144 },
        { "Small Lake - Glacial Basin - Crates Above Lake PDA", 34145 },
        { "Big Basin Stalker Cavern - Deep Inside PDA", 34146 },
        { "Southern Glacial Basin - Spy Pengling Access Point PDA", 34147 },
        { "Parvan's Bunker - Bed PDA", 34148 },
        { "Parvan's Bunker - Crate PDA", 34149 },
        { "Path To Excavation Site - Ground PDA", 34150 },
        { "Path To Excavation Site - Glacial Basin - Table PDA", 34151 },
        { "Ledge Over Excavation Site - Glacial Basin - PDA", 34152 },
        { "Outpost Zero - Lab Middle Counter PDA", 34153 },
        { "Outpost Zero - Lab Left Counter PDA", 34154 },
        { "Outpost Zero - Kitchen Corner Counter PDA", 34155 },
        { "Outpost Zero - Kitchen Bench PDA", 34156 },
        { "Outpost Zero - Sam's Bed PDA", 34157 },
        { "Outpost Zero - Lillian's Bed PDA", 34158 },
        { "Outpost Zero - Greenhouse PDA", 34159 },
        { "Kelp Forest - Near Sea Monkey Nest PDA", 34160 },
        { "Deep Twisty Bridges - Entrance PDA", 34161 },
        { "Deep Twisty Bridges - Deep Near Fragment PDA", 34162 },
        { "Deep Koppa Mining Site - Near Closed Door PDA", 34163 },
        { "Deep Koppa Mining Site - Table PDA", 34164 },
        { "Deep Koppa Mining Site - Desk Near Lift PDA", 34165 },

        // ========== CREATURE SCANS ========== (✅ Commençant à 34200)
        { "Arctic Peeper Scan", 34200 },
        { "Arctic Ray Scan", 34201 },
        { "Arrow Ray Scan", 34202 },
        { "Bladderfish Scan", 34203 },
        { "Boomerang Scan", 34204 },
        { "Brinewing Scan", 34205 },
        { "Brute Shark Scan", 34206 },
        { "Chelicerate Scan", 34207 },
        { "Crash Scan", 34208 },
        { "Cryptosuchus Scan", 34209 },
        { "Discus Fish Scan", 34210 },
        { "Jellyfish Scan", 34211 },
        { "Feather Fish Scan", 34212 },
        { "Feather Fish Red Scan", 34213 },
        { "Glow Whale Scan", 34214 },
        { "Hive Plant Scan", 34215 },
        { "Hoopfish Scan", 34216 },
        { "Ice Worm Scan", 34217 },
        { "Small Vent Garden Scan", 34218 },
        { "Lily Paddler Scan", 34219 },
        { "Noot Fish Scan", 34220 },
        { "Penguin Baby Scan", 34221 },
        { "Penguin Scan", 34222 },
        { "Pinnacarid Scan", 34223 },
        { "Rock Puncher Scan", 34224 },
        { "Rockgrub Scan", 34225 },
        { "Sea Monkey Scan", 34226 },
        { "Sea Monkey Baby Scan", 34227 },
        { "Shadow Leviathan Scan", 34228 },
        { "Skyray Scan", 34229 },
        { "Snow Stalker Scan", 34230 },
        { "Snow Stalker Baby Scan", 34231 },
        { "Spikey Trap Scan", 34232 },
        { "Spinefish Scan", 34233 },
        { "Spinner Fish Scan", 34234 },
        { "Squid Shark Scan", 34235 },
        { "Symbiote Scan", 34236 },
        { "Titan Holefish Scan", 34237 },
        { "Triops Scan", 34238 },
        { "Trivalve Blue Scan", 34239 },
        { "Trivalve Yellow Scan", 34240 },
        { "Large Vent Garden Scan", 34241 },
    };

    public static string GetLocationName(string subnauticaName)
    {
        return $"Subnautica BZ: {subnauticaName}";
    }

    public static long? GetLocationId(string subnauticaName)
    {
        if (LocationIdMap.TryGetValue(subnauticaName, out var id))
            return id;
        return null;
    }

    public static bool IsMapped(string subnauticaName)
    {
        return LocationIdMap.ContainsKey(subnauticaName);
    }
}