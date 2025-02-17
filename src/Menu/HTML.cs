﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Menu;
using CounterStrikeSharp.API.Modules.Utils;
using static Plugin;

public static class MenuHTML
{
    public static void OpenMenu(CCSPlayerController player)
    {
        CenterHtmlMenu MainMenu = new("Block Builder", Instance);

        MainMenu.AddMenuOption("Create Block", (player, menuOption) =>
        {
            Instance.Command_CreateBlock(player);
        });

        MainMenu.AddMenuOption("Delete Block", (player, menuOption) =>
        {
            Instance.Command_DeleteBlock(player);
        });

        MainMenu.AddMenuOption("Rotate Block", (player, menuOption) =>
        {
            float[] rotateValues = Instance.Config.Settings.Building.RotationValues;
            string[] rotateOptions = { "Reset", "X-", "X+", "Y-", "Y+", "Z-", "Z+" };

            RotateMenuOptions(player, rotateOptions, rotateValues);
        });

        MainMenu.AddMenuOption($"Block Settings", (player, menuOption) =>
        {
            CenterHtmlMenu BlockMenu = new("Block Settings", Instance);

            BlockMenu.AddMenuOption($"Size: " + Instance.playerData[player.Slot].BlockSize, (player, menuOption) =>
            {
                string[] sizeValues = { "Small", "Medium", "Large", "Pole" };

                SizeMenuOptions(player, MainMenu, sizeValues);
            });

            BlockMenu.AddMenuOption($"Grid: {Instance.playerData[player.Slot].GridValue} Units", (player, menuOption) =>
            {
                float[] gridValues = Instance.Config.Settings.Building.GridValues;

                GridMenuOptions(player, gridValues);
            });

            BlockMenu.AddMenuOption($"Type: {Instance.playerData[player.Slot].BlockType}", (player, menuOption) =>
            {
                TypeMenuOptions(player);
            });

            BlockMenu.AddMenuOption($"Color: {Instance.playerData[player.Slot].BlockColor}", (player, menuOption) =>
            {
                ColorMenuOptions(player);
            });

            BlockMenu.AddMenuOption("Convert Block", (player, menuOption) =>
            {
                Instance.Command_ConvertBlock(player);
            });

            BlockMenu.AddMenuOption("Copy Block", (player, menuOption) =>
            {
                Instance.Command_CopyBlock(player);
                MenuManager.OpenCenterHtmlMenu(Instance, player, MainMenu);
            });

            MenuManager.OpenCenterHtmlMenu(Instance, player, BlockMenu);
        });

        MainMenu.AddMenuOption("Build Settings", (player, menuOption) =>
        {
            SettingsOptions(player);
        });

        MenuManager.OpenCenterHtmlMenu(Instance, player, MainMenu);
    }

    private static void RotateMenuOptions(CCSPlayerController player, string[] rotateOptions, float[] rotateValues)
    {
        CenterHtmlMenu RotateMenu = new($"Rotate Block ({Instance.playerData[player.Slot].RotationValue} Units)", Instance);

        RotateMenu.AddMenuOption($"Select Units", (p, option) =>
        {
            RotateValuesMenuOptions(player, rotateOptions, rotateValues);
        });

        foreach (string rotateOption in rotateOptions)
        {
            RotateMenu.AddMenuOption(rotateOption, (p, option) =>
            {
                Instance.Command_RotateBlock(p, rotateOption);
            });
        }

        MenuManager.OpenCenterHtmlMenu(Instance, player, RotateMenu);
    }

    private static void RotateValuesMenuOptions(CCSPlayerController player, string[] rotateOptions, float[] rotateValues)
    {
        CenterHtmlMenu RotateValuesMenu = new($"Rotate Values", Instance);

        foreach (float rotateValueOption in rotateValues)
        {
            RotateValuesMenu.AddMenuOption(rotateValueOption.ToString() + " Units", (p, option) =>
            {
                Instance.playerData[p.Slot].RotationValue = rotateValueOption;

                Instance.PrintToChat(player, $"Selected Rotation Value: {ChatColors.White}{rotateValueOption} Units");

                RotateMenuOptions(player, rotateOptions, rotateValues);
            });
        }

        MenuManager.OpenCenterHtmlMenu(Instance, player, RotateValuesMenu);
    }

    private static void SizeMenuOptions(CCSPlayerController player, CenterHtmlMenu openMainMenu, string[] sizeValues)
    {
        CenterHtmlMenu SizeMenu = new($"Select Size ({Instance.playerData[player.Slot].BlockSize})", Instance);

        foreach (string sizeValue in sizeValues)
        {
            SizeMenu.AddMenuOption(sizeValue, (p, option) =>
            {
                Instance.playerData[player.Slot].BlockSize = sizeValue;

                Instance.PrintToChat(p, $"Selected Size: {ChatColors.White}{sizeValue}");

                SizeMenuOptions(player, openMainMenu, sizeValues);
            });
        }

        MenuManager.OpenCenterHtmlMenu(Instance, player, SizeMenu);
    }

    private static void GridMenuOptions(CCSPlayerController player, float[] gridValues)
    {
        CenterHtmlMenu GridMenu = new($"Select Grid ({(Instance.playerData[player.Slot].Grid ? "ON" : "OFF")} - {Instance.playerData[player.Slot].GridValue})", Instance);

        GridMenu.AddMenuOption($"Toggle Grid", (p, option) =>
        {
            Instance.Command_Grid(player, "");

            GridMenuOptions(player, gridValues);
        });

        foreach (float gridValue in gridValues)
        {
            GridMenu.AddMenuOption(gridValue.ToString() + " Units", (p, option) =>
            {
                Instance.Command_Grid(player, gridValue.ToString());

                GridMenuOptions(player, gridValues);
            });
        }

        MenuManager.OpenCenterHtmlMenu(Instance, player, GridMenu);
    }

    private static void TypeMenuOptions(CCSPlayerController player)
    {
        CenterHtmlMenu TypeMenu = new($"Select Type ({Instance.playerData[player.Slot].BlockType})", Instance);

        foreach (var property in typeof(BlockModels).GetProperties())
        {
            var block = (BlockSizes)property.GetValue(Plugin.BlockModels)!;

            string blockName = block.Title;

            TypeMenu.AddMenuOption(blockName, (player, menuOption) =>
            {
                Instance.Command_SelectBlockType(player, blockName);

                TypeMenuOptions(player);
            });
        }
        MenuManager.OpenCenterHtmlMenu(Instance, player, TypeMenu);
    }

    private static void ColorMenuOptions(CCSPlayerController player)
    {
        CenterHtmlMenu ColorMenu = new($"Select Color ({Instance.playerData[player.Slot].BlockColor})", Instance);

        foreach (var color in ColorMapping.Keys)
        {
            ColorMenu.AddMenuOption(color, (player, menuOption) =>
            {
                Instance.Command_SelectBlockColor(player, color);

                TypeMenuOptions(player);
            });
        }

        MenuManager.OpenCenterHtmlMenu(Instance, player, ColorMenu);
    }

    private static void SettingsOptions(CCSPlayerController player)
    {
        CenterHtmlMenu SettingsMenu = new("Settings", Instance);

        SettingsMenu.AddMenuOption("Build Mode: " + (Instance.buildMode ? "ON" : "OFF"), (player, menuOption) =>
        {
            Instance.Command_BuildMode(player);

            SettingsOptions(player);
        });

        SettingsMenu.AddMenuOption("Godmode: " + (Instance.playerData[player.Slot].Godmode ? "ON" : "OFF"), (player, menuOption) =>
        {
            Instance.Command_Godmode(player);

            SettingsOptions(player);
        });

        SettingsMenu.AddMenuOption("Noclip: " + (Instance.playerData[player.Slot].Noclip ? "ON" : "OFF"), (player, menuOption) =>
        {
            Instance.Command_Noclip(player);

            SettingsOptions(player);
        });

        SettingsMenu.AddMenuOption("Save Blocks", (player, menuOption) =>
        {
            Instance.Command_SaveBlocks(player);

            SettingsOptions(player);
        });

        SettingsMenu.AddMenuOption("Clear Blocks", (player, menuOption) =>
        {
            CenterHtmlMenu ConfirmMenu = new("Confirm", Instance);

            ConfirmMenu.AddMenuOption("NO - keep blocks", (player, menuOption) =>
            {
                SettingsOptions(player);
            });

            ConfirmMenu.AddMenuOption("YES - remove blocks", (player, menuOption) =>
            {
                Instance.Command_ClearBlocks(player);

                SettingsOptions(player);
            });

            MenuManager.OpenCenterHtmlMenu(Instance, player, ConfirmMenu);
        });

        MenuManager.OpenCenterHtmlMenu(Instance, player, SettingsMenu);
    }
}