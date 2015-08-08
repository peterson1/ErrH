using TestStack.White.UIItems;

namespace ErrH.WhiteShim.Extensions
{
public static class IUIItemExtensions
{

	public static string LocalType(this IUIItem item)
	{
		return item.AutomationElement.LocalType();
	}

}
}
