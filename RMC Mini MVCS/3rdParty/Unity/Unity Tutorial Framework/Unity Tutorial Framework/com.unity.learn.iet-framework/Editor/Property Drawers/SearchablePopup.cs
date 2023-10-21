// ----------------------------------------------------------------------------
// Based on https://github.com/roboryantron/UnityEditorJunkie made by Ryan Hipple
// ----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Unity.Tutorials.Core.Editor
{
    /// <summary>
    /// A popup window that displays a list of options and may use a search
    /// string to filter the displayed content.
    /// </summary>
    internal class SearchablePopup : PopupWindowContent
    {
        /// <summary>
        /// Height of each element in the popup list.
        /// </summary>
        const float k_RowHeight = 16.0f;

        /// <summary>
        /// How far to indent list entries.
        /// </summary>
        const float k_RowIndent = 8.0f;

        /// <summary>
        /// Name to use for the text field for search.
        /// </summary>
        const string k_SearchControlName = "EnumSearchText";

        /// <summary>
        /// Show a new SearchablePopup.
        /// </summary>
        /// <param name="activatorRect"> Rectangle of the button that triggered the popup. </param>
        /// <param name="options">List of strings to choose from.</param>
        /// <param name="current"> Index of the currently selected string. </param>
        /// <param name="onSelectionMade"> Callback to trigger when a choice is made. </param>
        public static void Show(Rect activatorRect, string[] options, int current, Action<int> onSelectionMade)
        {
            SearchablePopup win = new SearchablePopup(options, current, onSelectionMade);
            PopupWindow.Show(activatorRect, win);
        }

        /// <summary>
        /// Show a new SearchablePopup.
        /// </summary>
        /// <param name="activatorRect"> Rectangle of the button that triggered the popup. </param>
        /// <param name="options">List of GUIContent to choose from.</param>
        /// <param name="current"> Index of the currently selected string. </param>
        /// <param name="onSelectionMade"> Callback to trigger when a choice is made. </param>
        public static void Show(Rect activatorRect, GUIContent[] options, int current, Action<int> onSelectionMade)
        {
            Show(activatorRect, options.Select(o => o.text).ToArray(), current, onSelectionMade);
        }

        /// <summary>
        /// Force the focused window to redraw. This can be used to make the
        /// popup more responsive to mouse movement.
        /// </summary>
        static void Repaint() { EditorWindow.focusedWindow.Repaint(); }

        /// <summary>
        /// Draw a generic box.
        /// </summary>
        /// <param name="rect">Where to draw.</param>
        /// <param name="tint">Color to tint the box.</param>
        static void DrawBox(Rect rect, Color tint)
        {
            Color c = GUI.color;
            GUI.color = tint;
            GUI.Box(rect, "", s_Selection);
            GUI.color = c;
        }

        /// <summary>
        /// Stores a list of strings and can return a subset of that list that
        /// matches a given filter string.
        /// </summary>
        class FilteredList
        {
            /// <summary>
            /// An entry in the filtererd list, mapping the text to the
            /// original index.
            /// </summary>
            public struct Entry
            {
                public int Index;
                public string Text;
            }

            /// <summary>
            /// All posibile items in the list.
            /// </summary>
            readonly string[] m_AllItems;

            /// <summary>
            /// Create a new filtered list.
            /// </summary>
            /// <param name="items">All The items to filter.</param>
            public FilteredList(string[] items)
            {
                m_AllItems = items;
                Entries = new List<Entry>();
                UpdateFilter("");
            }

            /// <summary>
            /// The current string filtering the list.
            /// </summary>
            public string Filter { get; set; }

            /// <summary> All valid entries for the current filter. </summary>
            public List<Entry> Entries { get; set; }

            /// <summary>
            /// Total possible entries in the list.
            /// </summary>
            public int MaxLength { get { return m_AllItems.Length; } }

            /// <summary>
            /// Sets a new filter string and updates the Entries that match the
            /// new filter if it has changed.
            /// </summary>
            /// <param name="filter">String to use to filter the list.</param>
            /// <returns>
            /// True if the filter is updated, false if newFilter is the same
            /// as the current Filter and no update is necessary.
            /// </returns>
            public bool UpdateFilter(string filter)
            {
                if (Filter == filter) { return false; }

                Filter = filter;
                Entries.Clear();

                for (int i = 0; i < m_AllItems.Length; i++)
                {
                    if (string.IsNullOrEmpty(Filter) || m_AllItems[i].ToLower().Contains(Filter.ToLower()))
                    {
                        Entry entry = new Entry
                        {
                            Index = i,
                            Text = m_AllItems[i]
                        };
                        if (string.Equals(m_AllItems[i], Filter, StringComparison.CurrentCultureIgnoreCase))
                        {
                            Entries.Insert(0, entry);
                        }
                        else
                        {
                            Entries.Add(entry);
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// Callback to trigger when an item is selected.
        /// </summary>
        readonly Action<int> m_OnSelectionMade;

        /// <summary>
        /// Index of the item that was selected when the list was opened.
        /// </summary>
        readonly int m_CurrentIndex;

        /// <summary>
        /// Container for all available options that does the actual string
        /// filtering of the content.
        /// </summary>
        readonly FilteredList m_List;

        /// <summary>
        /// Scroll offset for the vertical scroll area.
        /// </summary>
        Vector2 m_Scroll;

        /// <summary>
        /// Index of the item under the mouse or selected with the keyboard.
        /// </summary>
        int m_HoverIndex;

        /// <summary>
        /// An item index to scroll to on the next draw.
        /// </summary>
        int m_ScrollToIndex;

        /// <summary>
        /// An offset to apply after scrolling to scrollToIndex. This can be
        /// used to control if the selection appears at the top, bottom, or
        /// center of the popup.
        /// </summary>
        float m_ScrollOffset;

        // GUIStyles implicitly cast from a string. This triggers a lookup into
        // the current skin which will be the editor skin and lets us get some
        // built-in styles.
        static GUIStyle s_SearchBox = "ToolbarSeachTextField";
        static GUIStyle s_CancelButton = "ToolbarSeachCancelButton";
        static GUIStyle s_DisabledCancelButton = "ToolbarSeachCancelButtonEmpty";
        static GUIStyle s_Selection = "SelectionRect";

        SearchablePopup(string[] names, int currentIndex, Action<int> onSelectionMade)
        {
            m_List = new FilteredList(names);
            m_CurrentIndex = currentIndex;
            m_OnSelectionMade = onSelectionMade;

            m_HoverIndex = currentIndex;
            m_ScrollToIndex = currentIndex;
            m_ScrollOffset = GetWindowSize().y - k_RowHeight * 2;
        }

        /// <summary>
        /// Called when the Popup opens
        /// </summary>
        public override void OnOpen()
        {
            base.OnOpen();
            // Force a repaint every frame to be responsive to mouse hover.
            EditorApplication.update += Repaint;
        }

        /// <summary>
        /// Called when the Popup closes
        /// </summary>
        public override void OnClose()
        {
            base.OnClose();
            EditorApplication.update -= Repaint;
        }

        /// <summary>
        /// Gets the size of the window
        /// </summary>
        /// <returns>The size of the window</returns>
        public override Vector2 GetWindowSize()
        {
            return new Vector2(base.GetWindowSize().x * 2,
                Mathf.Min(600, m_List.MaxLength * k_RowHeight +
                    EditorStyles.toolbar.fixedHeight));
        }

        /// <summary>
        /// Draws the popup using IMGUI
        /// </summary>
        /// <param name="rect">The rect of the window to be drawn</param>
        public override void OnGUI(Rect rect)
        {
            Rect searchRect = new Rect(0, 0, rect.width, EditorStyles.toolbar.fixedHeight);
            Rect scrollRect = Rect.MinMaxRect(0, searchRect.yMax, rect.xMax, rect.yMax);

            HandleKeyboard();
            DrawSearch(searchRect);
            DrawSelectionArea(scrollRect);
        }

        void DrawSearch(Rect rect)
        {
            if (Event.current.type == EventType.Repaint)
            {
                EditorStyles.toolbar.Draw(rect, false, false, false, false);
            }

            Rect searchRect = new Rect(rect);
            searchRect.xMin += 6;
            searchRect.xMax -= 6;
            searchRect.y += 2;
            searchRect.width -= s_CancelButton.fixedWidth;

            GUI.FocusControl(k_SearchControlName);
            GUI.SetNextControlName(k_SearchControlName);
            string newText = GUI.TextField(searchRect, m_List.Filter, s_SearchBox);

            if (m_List.UpdateFilter(newText))
            {
                m_HoverIndex = 0;
                m_Scroll = Vector2.zero;
            }

            searchRect.x = searchRect.xMax;
            searchRect.width = s_CancelButton.fixedWidth;

            if (string.IsNullOrEmpty(m_List.Filter))
            {
                GUI.Box(searchRect, GUIContent.none, s_DisabledCancelButton);
            }
            else if (GUI.Button(searchRect, "x", s_CancelButton))
            {
                m_List.UpdateFilter("");
                m_Scroll = Vector2.zero;
            }
        }

        void DrawSelectionArea(Rect scrollRect)
        {
            Rect contentRect = new Rect(0, 0,
                scrollRect.width - GUI.skin.verticalScrollbar.fixedWidth,
                m_List.Entries.Count * k_RowHeight);

            m_Scroll = GUI.BeginScrollView(scrollRect, m_Scroll, contentRect);

            Rect rowRect = new Rect(0, 0, scrollRect.width, k_RowHeight);

            for (int i = 0; i < m_List.Entries.Count; i++)
            {
                if (m_ScrollToIndex == i
                    && (Event.current.type == EventType.Repaint || Event.current.type == EventType.Layout))
                {
                    Rect r = new Rect(rowRect);
                    r.y += m_ScrollOffset;
                    GUI.ScrollTo(r);
                    m_ScrollToIndex = -1;
                    m_Scroll.x = 0;
                }

                if (rowRect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.type == EventType.MouseMove
                        || Event.current.type == EventType.ScrollWheel)
                    {
                        m_HoverIndex = i;
                    }
                    if (Event.current.type == EventType.MouseDown)
                    {
                        m_OnSelectionMade(m_List.Entries[i].Index);
                        EditorWindow.focusedWindow.Close();
                    }
                }

                DrawRow(rowRect, i);

                rowRect.y = rowRect.yMax;
            }

            GUI.EndScrollView();
        }

        void DrawRow(Rect rowRect, int i)
        {
            if (m_List.Entries[i].Index == m_CurrentIndex)
            {
                DrawBox(rowRect, Color.cyan);
            }
            else if (i == m_HoverIndex)
            {
                DrawBox(rowRect, Color.white);
            }

            Rect labelRect = new Rect(rowRect);
            labelRect.xMin += k_RowIndent;

            GUI.Label(labelRect, m_List.Entries[i].Text);
        }

        /// <summary>
        /// Process keyboard input to navigate the choices or make a selection.
        /// </summary>
        void HandleKeyboard()
        {
            if (Event.current.type != EventType.KeyDown)
                return;

            if (Event.current.keyCode == KeyCode.DownArrow)
            {
                m_HoverIndex = Mathf.Min(m_List.Entries.Count - 1, m_HoverIndex + 1);
                Event.current.Use();
                m_ScrollToIndex = m_HoverIndex;
                m_ScrollOffset = k_RowHeight;
            }

            if (Event.current.keyCode == KeyCode.UpArrow)
            {
                m_HoverIndex = Mathf.Max(0, m_HoverIndex - 1);
                Event.current.Use();
                m_ScrollToIndex = m_HoverIndex;
                m_ScrollOffset = -k_RowHeight;
            }

            if (Event.current.keyCode == KeyCode.Return)
            {
                if (m_HoverIndex >= 0 && m_HoverIndex < m_List.Entries.Count)
                {
                    m_OnSelectionMade(m_List.Entries[m_HoverIndex].Index);
                    EditorWindow.focusedWindow.Close();
                }
            }

            if (Event.current.keyCode == KeyCode.Escape)
            {
                EditorWindow.focusedWindow.Close();
            }
        }
    }
}
