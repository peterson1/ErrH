using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;

namespace ErrH.WhiteShim.Extensions
{
public static class UIItemContainerExtensions
{
	public static bool TryGet<T>(this UIItemContainer lookHere, 
		SearchCriteria criteria, out T matchingItem) where T:IUIItem
	{
		if (lookHere.Exists<T>(criteria))
		{
			matchingItem = lookHere.Get<T>(criteria);
			return true;
		}
		
		matchingItem = default(T);
		return false;
	}
}
}
