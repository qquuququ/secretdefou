using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class LocationExporter
{
    private static string GetExportDirectory()
    {
        string path = Path.Combine(
            Application.persistentDataPath,
            "ArchipelagoExport"
        );

        Debug.Log($"[AP] Export directory: {path}");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            Debug.Log($"[AP] Directory créé: {path}");
        }

        return path;
    }

    public static void ExportToText()
    {
        try
        {
            string exportDir = GetExportDirectory();
            string filePath = Path.Combine(exportDir, "Locations.txt");

            Debug.Log($"[AP] Tentative d'écriture: {filePath}");

            var locations = ArchipelagoLocationDatabase.GetAllLocations();
            var lines = new List<string>();

            lines.Add($"=== ARCHIPELAGO LOCATIONS ({DateTime.Now}) ===\n");
            lines.Add($"Total découvertes: {locations.Count}\n");
            lines.Add(new string('=', 50));

            foreach (var location in locations)
            {
                lines.Add($"\nID: {location.LocationId}");
                lines.Add($"TechType: {location.TechType}");
                lines.Add($"UID: {location.UID}");
                lines.Add($"Collected: {location.Collected}");
                lines.Add($"Position: {string.Join(", ", location.Position ?? new float[] { 0, 0, 0 })}");
            }

            File.WriteAllLines(filePath, lines);
            Debug.Log($"[AP] ✅ Fichier écrit avec succès: {filePath}");
            ErrorMessage.AddMessage($"✅ Locations exportées: {filePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[AP] ❌ ERREUR EXPORT: {ex.Message}\n{ex.StackTrace}");
            ErrorMessage.AddMessage($"❌ Erreur export: {ex.Message}");
        }
    }

    public static void ExportToCSV()
    {
        try
        {
            string exportDir = GetExportDirectory();
            string filePath = Path.Combine(exportDir, "Locations.csv");

            Debug.Log($"[AP] Tentative CSV: {filePath}");

            var locations = ArchipelagoLocationDatabase.GetAllLocations();
            var lines = new List<string> { "LocationId,TechType,UID,IsFragment,Collected,X,Y,Z" };

            foreach (var loc in locations)
            {
                string line = $"{loc.LocationId},{loc.TechType},{loc.UID},{loc.IsFragment},{loc.Collected}," +
                    $"{loc.Position[0]},{loc.Position[1]},{loc.Position[2]}";
                lines.Add(line);
            }

            File.WriteAllLines(filePath, lines);
            Debug.Log($"[AP] ✅ CSV écrit: {filePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[AP] ❌ ERREUR CSV: {ex.Message}");
        }
    }

    public static void ExportStatusReport()
    {
        try
        {
            string exportDir = GetExportDirectory();
            string filePath = Path.Combine(exportDir, "Status_Report.txt");

            Debug.Log($"[AP] Tentative rapport: {filePath}");

            string report = ArchipelagoLocationDatabase.GetStatusReport();
            File.WriteAllText(filePath, report);
            Debug.Log($"[AP] ✅ Rapport écrit: {filePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[AP] ❌ ERREUR RAPPORT: {ex.Message}");
        }
    }
}