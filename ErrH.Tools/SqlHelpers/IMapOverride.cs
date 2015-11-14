namespace ErrH.Tools.SqlHelpers
{
    public interface IMapOverride
    {
        bool HasOverride(string propertyName);

        object OverrideValue(string propertyName, 
                             object origValue);
    }
}
