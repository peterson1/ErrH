﻿/*   Copyright 2010 Gishu Pillai

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using ErrH.Tools.ErrorConstructors;

namespace ErrH.WpfTools.Extensions
{
    static class DependencyObjExtensions
    {
        public static T Find<T>(this DependencyObject parent, string name)
            where T : FrameworkElement
        {
            T match;
            var ok = parent.TryFindChild<T>(x => x.Name == name, out match);
            if (ok) return match;

            throw Error.NoMember<T>(name);
        }





        /// <summary>
        /// Find the first child of the specified type (the child must exist)
        /// by walking down the logical/visual trees
        /// Will throw an exception if a matching child does not exist. If you're not sure, use the TryFindChild method instead.
        /// </summary>
        /// <typeparam name="T">The type of child you want to find</typeparam>
        /// <param name="parent">The dependency object whose children you wish to scan</param>
        /// <returns>The first descendant of the specified type</returns>
        /// <remarks> usage: myWindow.FindChild<StackPanel>() </StackPanel></remarks>
        public static T FindChild<T>(this DependencyObject parent)
            where T : DependencyObject
        {
            return parent.FindChild<T>(child => true);
        }

        /// <summary>
        /// Find the first child of the specified type (the child must exist)
        /// by walking down the logical/visual trees, which meets the specified criteria
        /// Will throw an exception if a matching child does not exist. If you're not sure, use the TryFindChild method instead.
        /// </summary>
        /// <typeparam name="T">The type of child you want to find</typeparam>
        /// <param name="parent">The dependency object whose children you wish to scan</param>
        /// <param name="predicate">The child object is selected if the predicate evaluates to true</param>
        /// <returns>The first matching descendant of the specified type</returns>
        /// <remarks> usage: myWindow.FindChild<StackPanel>( child => child.Name == "myPanel" ) </StackPanel></remarks>
        public static T FindChild<T>(this DependencyObject parent, Func<T, bool> predicate)
            where T : DependencyObject
        {
            return parent.FindChildren<T>(predicate).First();
        }

        /// <summary>
        /// Use this overload if the child you're looking may not exist.
        /// </summary>
        /// <typeparam name="T">The type of child you're looking for</typeparam>
        /// <param name="parent">The dependency object whose children you wish to scan</param>
        /// <param name="foundChild">out param - the found child dependencyobject, null if the method returns false</param>
        /// <returns>True if a child was found, else false</returns>
        public static bool TryFindChild<T>(this DependencyObject parent, out T foundChild)
            where T : DependencyObject
        {
            return parent.TryFindChild<T>(child => true, out foundChild);
        }

        /// <summary>
        /// Use this overload if the child you're looking may not exist.
        /// </summary>
        /// <typeparam name="T">The type of child you're looking for</typeparam>
        /// <param name="parent">The dependency object whose children you wish to scan</param>
        /// <param name="predicate">The child object is selected if the predicate evaluates to true</param>
        /// <param name="foundChild">out param - the found child dependencyobject, null if the method returns false</param>
        /// <returns>True if a child was found, else false</returns>
        public static bool TryFindChild<T>(this DependencyObject parent, Func<T, bool> predicate, out T foundChild)
            where T : DependencyObject
        {
            foundChild = null;
            var results = parent.FindChildren<T>(predicate);
            if (results.Count() == 0)
                return false;

            foundChild = results.First();
            return true;
        }
        
        /// <summary>
        /// Get a list of descendant dependencyobjects of the specified type and which meet the criteria
        /// as specified by the predicate
        /// </summary>
        /// <typeparam name="T">The type of child you want to find</typeparam>
        /// <param name="parent">The dependency object whose children you wish to scan</param>
        /// <param name="predicate">The child object is selected if the predicate evaluates to true</param>
        /// <returns>The first matching descendant of the specified type</returns>
        /// <remarks> usage: myWindow.FindChildren<StackPanel>( child => child.Name == "myPanel" ) </StackPanel></remarks>
        public static IEnumerable<T> FindChildren<T>(this DependencyObject parent, Func<T, bool> predicate)
            where T : DependencyObject
        {
            var children = new  List<DependencyObject>();

            if ((parent is Visual) || (parent is Visual3D))
            {
                var visualChildrenCount = VisualTreeHelper.GetChildrenCount(parent);
                for (int childIndex = 0; childIndex < visualChildrenCount; childIndex++)
                    children.Add(VisualTreeHelper.GetChild(parent, childIndex));
            }
            foreach (var logicalChild in LogicalTreeHelper.GetChildren(parent).OfType<DependencyObject>())
                if (!children.Contains(logicalChild))
                    children.Add(logicalChild);

            foreach (var child in children)
            {
                var typedChild = child as T;
                if ((typedChild != null) && predicate.Invoke(typedChild))
                    yield return typedChild;

                foreach (var foundDescendant in FindChildren(child, predicate))
                    yield return foundDescendant;
            }
            yield break;
        }
    }
}
