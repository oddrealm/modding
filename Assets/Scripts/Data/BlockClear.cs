
[System.Serializable]
public struct BlockClear
{
    public BlockLayers Layer;
    public bool DisableRemoveSpawns;

    public bool IsValidClear { get { return Layer != BlockLayers.NONE; } }
    public bool IsLayer(BlockLayers layer) { return (Layer & layer) != BlockLayers.NONE; }

    public BlockClear(BlockLayers layer, bool disableRemoveSpawns)
    {
        Layer = layer;
        DisableRemoveSpawns = disableRemoveSpawns;
    }
}
