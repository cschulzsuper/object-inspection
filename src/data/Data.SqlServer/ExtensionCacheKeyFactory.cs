namespace ChristianSchulz.ObjectInspection.Data;

public class ExtensionCacheKeyFactory
{
    private readonly ObjectInspectionContextState _state;

    public ExtensionCacheKeyFactory(ObjectInspectionContextState state)
    {
        _state = state;
    }

    public string Create(string aggregateType)
        => $"{_state.CurrentOrganization}|{aggregateType}";
}