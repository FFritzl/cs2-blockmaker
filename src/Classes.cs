﻿using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Utils;
using static BlockMaker.VectorUtils;

public class PlayerData
{
    public bool Builder;
    public string BlockType = "Platform";
    public string BlockSize = "Medium";
    public bool Grid = false;
    public float GridValue = 32f;
    public float RotationValue = 30f;
    public bool Snapping = false;
}

public class Builder
{
    public CBaseProp block = null!;
    public Vector offset = null!;
    public int distance;
}

public class BlockSizes
{
    public string Small { get; set; } = string.Empty;
    public string Medium { get; set; } = string.Empty;
    public string Large { get; set; } = string.Empty;
    public string Pole { get; set; } = string.Empty;
}

public class BlockData
{
    public BlockData(CBaseProp block, string blockName, string blockModel, string blockSize)
    {
        Entity = block;
        Name = blockName;
        Model = blockModel;
        Size = blockSize;
    }

    public CBaseProp Entity;
    public string Name { get; private set; }
    public string Model { get; private set; }
    public string Size { get; private set; }
}

public class SaveBlockData
{
    public string Name { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public VectorDTO Position { get; set; } = new VectorDTO(Vector.Zero);
    public QAngleDTO Rotation { get; set; } = new QAngleDTO(QAngle.Zero);
}