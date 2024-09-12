
[System.Serializable]
public struct BlockClear
{
    public BlockLayers Layer;
    public NewTagObjParams Params;

    public bool IsValidClear { get { return Layer != BlockLayers.NONE; } }
    public bool IsLayer(BlockLayers layer) { return (Layer & layer) != BlockLayers.NONE; }

    public BlockClear(BlockLayers layer)
    {
        Layer = layer;
        Params = NewTagObjParams.Default;
    }

    public BlockClear(BlockLayers layer,
                      bool spawnClearItems)
    {
        Layer = layer;
        Params = new NewTagObjParams()
        {
            DisableRemoveSpawns =  !spawnClearItems
        };
    }
}
