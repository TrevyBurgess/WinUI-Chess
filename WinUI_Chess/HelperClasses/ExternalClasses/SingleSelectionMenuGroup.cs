// 
// 
namespace TrevyBurgess.WPF
{
    using System;

    public static class SingleSelectionMenuGroup
    {
        #region IsSingleSelection
        /// <summary>
        /// IsSingleSelection Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsSingleSelectionProperty =
            DependencyProperty.RegisterAttached
            (
                "IsSingleSelection",
                typeof(bool),
                typeof(SingleSelectionMenuGroup),
                new PropertyMetadata
                (
                    (bool)false,
                    new PropertyChangedCallback(OnIsSingleSelectionChanged)
                )
            );

        /// <summary>
        /// Gets the IsSingleSelection property.
        /// </summary>
        public static bool GetIsSingleSelection(DependencyObject d)
        {
            return (bool)d.GetValue(IsSingleSelectionProperty);
        }

        /// <summary>
        /// Sets the IsSingleSelection property.
        /// </summary>
        public static void SetIsSingleSelection(DependencyObject d, bool value)
        {
            d.SetValue(IsSingleSelectionProperty, value);
        }

        /// <summary>
        /// Handles changes to the IsSingleSelection property.
        /// </summary>
        private static void OnIsSingleSelectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuItem item = d as MenuItem;
            if (item == null)
                return;
            if (e.NewValue is bool == false)
                return;

            if ((bool)e.NewValue)
            {
                item.IsHitTestVisible = !item.IsChecked;
                item.Checked += OnItemChecked;
            }
            else
            {
                item.Checked -= OnItemChecked;
                item.IsHitTestVisible = true;
            }
        }

        private static void OnItemChecked(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            if (item != null)
            {
                MenuItem parentMenu = item.Parent as MenuItem;
                if (parentMenu != null)
                {
                    // disable checked item to prevent unchecking
                    item.IsHitTestVisible = false;

                    ItemCollection children = parentMenu.Items;
                    for (int i = 0; i < children.Count; i++)
                    {
                        MenuItem child = children[i] as MenuItem;
                        if (child != null)
                        {
                            if (child == item)
                            {
                                SetSingleSelectionIndex(parentMenu as DependencyObject, i);
                                SetSingleSelectionByTag(parentMenu as DependencyObject, child.Tag);
                            }
                            else
                            {
                                child.IsChecked = false;
                            }

                            child.IsHitTestVisible = true;
                        }
                    }
                }
            }
        }
        #endregion

        #region SingleSelectionIndex
        /// <summary>
        /// SingleSelectionIndex Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty SingleSelectionIndexProperty =
            DependencyProperty.RegisterAttached
            (
                "SingleSelectionIndex",
                typeof(int),
                typeof(SingleSelectionMenuGroup),
                new PropertyMetadata
                (
                    (int)-1,
                    new PropertyChangedCallback(OnSingleSelectionIndexChanged)
                )
            );

        /// <summary>
        /// Gets the IsSingleSelection property.
        /// </summary>
        public static int GetSingleSelectionIndex(DependencyObject d)
        {
            return (int)d.GetValue(SingleSelectionIndexProperty);
        }

        /// <summary>
        /// Sets the IsSingleSelection property.
        /// </summary>
        public static void SetSingleSelectionIndex(DependencyObject d, int value)
        {
            d.SetValue(SingleSelectionIndexProperty, value);
        }

        /// <summary>
        /// Handles changes to the IsSingleSelection property.
        /// </summary>
        private static void OnSingleSelectionIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuItem menuItem = d as MenuItem;
            if (menuItem == null)
                return;

            // Boundary checking
            int index = GetSingleSelectionIndex(d);
            if (index < 0 || index > menuItem.Items.Count)
            {
                throw new IndexOutOfRangeException();
            }
            else
            {
                // disable checked item to prevent unchecking
                menuItem.IsHitTestVisible = false;

                ItemCollection children = menuItem.Items;
                for (int i = 0; i < children.Count; i++)
                {
                    MenuItem childMenu = children[i] as MenuItem;
                    if (childMenu != null)
                    {
                        if (i == index)
                        {
                            childMenu.IsChecked = true;
                        }
                        else
                        {
                            childMenu.IsChecked = false;
                        }
                    }
                }

                // disable checked item to prevent unchecking
                menuItem.IsHitTestVisible = true;
            }
        }
        #endregion

        #region SingleSelectionByTag
        /// <summary>
        /// SingleSelectionByTag Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty SingleSelectionByTagProperty =
            DependencyProperty.RegisterAttached
            (
                "SingleSelectionByTag",
                typeof(object),
                typeof(SingleSelectionMenuGroup),
                new PropertyMetadata
                (
                    new PropertyChangedCallback(OnSingleSelectionByTagChanged)
                )
            );

        /// <summary>
        /// Gets the SingleSelectionByTag property.
        /// </summary>
        public static object GetSingleSelectionByTag(DependencyObject d)
        {
            return d.GetValue(SingleSelectionByTagProperty);
        }

        /// <summary>
        /// Sets the SingleSelectionByTag property.
        /// </summary>
        public static void SetSingleSelectionByTag(DependencyObject d, object value)
        {
            d.SetValue(SingleSelectionByTagProperty, value);
        }

        /// <summary>
        /// Handles changes to the IsSingleSelection property.
        /// </summary>
        private static void OnSingleSelectionByTagChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MenuItem menuItem = d as MenuItem;
            if (menuItem == null)
                return;

            // disable checked item to prevent unchecking
            menuItem.IsHitTestVisible = false;

            // Find menu item
            object tagObject = GetSingleSelectionByTag(d);
            string tag = tagObject as string;
            if (tag == null)
            {
                ItemCollection children = menuItem.Items;
                for (int i = 0; i < children.Count; i++)
                {
                    MenuItem childMenu = children[i] as MenuItem;
                    if (childMenu != null && childMenu.Tag != null)
                    {
                        if (childMenu.Tag.Equals(tagObject))
                        {
                            childMenu.IsChecked = true;
                        }
                        else
                        {
                            childMenu.IsChecked = false;
                        }
                    }
                }
            }
            else
            {
                ItemCollection children = menuItem.Items;
                for (int i = 0; i < children.Count; i++)
                {
                    MenuItem childMenu = children[i] as MenuItem;
                    string menuTag = childMenu.Tag as string;
                    if (childMenu != null || menuTag != null)
                    {
                        if (menuTag.Equals(tag, StringComparison.CurrentCultureIgnoreCase))
                        {
                            childMenu.IsChecked = true;
                        }
                        else
                        {
                            childMenu.IsChecked = false;
                        }
                    }
                }
            }

            // enable checked item
            menuItem.IsHitTestVisible = true;
        }
        #endregion
    }
}