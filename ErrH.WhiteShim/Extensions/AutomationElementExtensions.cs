using System.Windows.Automation;

namespace ErrH.WhiteShim.Extensions
{
    public static class AutomationElementExtensions
{

	//from http://stackoverflow.com/a/23851560/3973863
	public static string GetText(this AutomationElement element)
	{
		if (element.Current.IsPassword) return " ‹ masked password › ";

		object patternObj;
		if (element.TryGetCurrentPattern(ValuePattern.Pattern, out patternObj))
		{
			var valuePattern = (ValuePattern)patternObj;
			return valuePattern.Current.Value;
		}
		else if (element.TryGetCurrentPattern(TextPattern.Pattern, out patternObj))
		{
			var textPattern = (TextPattern)patternObj;
			return textPattern.DocumentRange.GetText(-1).TrimEnd('\r'); // often there is an extra '\r' hanging off the end.
		}
		else
		{
			return element.Current.Name;
		}
	}


	public static string LocalType(this AutomationElement element)
	{
		return element.Current.LocalizedControlType;
	}


}
}
