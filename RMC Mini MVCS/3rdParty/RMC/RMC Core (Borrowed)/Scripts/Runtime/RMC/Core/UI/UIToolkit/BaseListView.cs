using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace RMC.Core.UI.UIToolkit
{

    /// <summary>
    /// Each item in the list
    /// </summary>
    public class BaseListView<T> where T: BaseListViewEntry<string>   
    {
        //  Classes ----------------------------------------
        public class BaseListViewEntryUnityEvent : UnityEvent<T>{}
        
        //  Events ------------------------------------
        public BaseListViewEntryUnityEvent OnSelectionChange = new BaseListViewEntryUnityEvent();

        //  Properties ------------------------------------

        //  Fields ----------------------------------------
        private ListView _listView;
        
        //  Initialization  -------------------------------
        public BaseListView(ListView listView)
        {
            _listView = listView;
            
            _listView.fixedItemHeight = 45;
            _listView.selectionChanged += ListView_OnSelectionChange;
            
            _listView.makeItem = () =>
            {
                //Structure
                VisualElement visualElement = new VisualElement();
                Label label = new Label();
                visualElement.Add(label);
                
                //Style
                visualElement.style.flexGrow = 1;
                label.style.flexGrow = 1;
                label.style.unityTextAlign = TextAnchor.MiddleLeft;
                
                return visualElement;
            };
            
            _listView.bindItem = (element, i) =>
            {
                BindItem(element, ItemsSource[i] as T);
            };

            GetRootMostParent().RegisterCallback<ClickEvent>((evt) =>
            {
                // Check if the target is not the ListView or any of its children
                if (!listView.worldBound.Contains(evt.position))
                {
                    listView.ClearSelection();
                }
            }, TrickleDown.NoTrickleDown);
            
            // TEmp
            var list = new List<BaseListViewEntry<object>>();
            for (var i = 1; i <= 10; i++)
            {
                string title = $"Empty List Entry {i}";
                object data = null;
                list.Add(new BaseListViewEntry<object>(title, data));
            }
            ItemsSource = list;
            
        }

        private void BindItem(VisualElement visualElement, T entry)
        {
            if (entry == null)
            {
                //Probably called too early in scene. Ok to return;
                return;
            }
            visualElement.Q<Label>().text = $"{entry.Title}";
        }

        //  Unity Methods ---------------------------------
        public IList ItemsSource
        {
            get
            {
                return _listView.itemsSource;
            }
            set
            {
                _listView.itemsSource = value;
                
                //Remove the selection when data changes
                _listView.selectedIndex = -1;
            }
        }

        private VisualElement GetRootMostParent()
        {
            VisualElement current = _listView;
            while (current.hierarchy.parent != null)
            {
                current = current.hierarchy.parent;
            }
            return current;
        }
        
        //  Methods ---------------------------------------
        
        //  Event Handlers --------------------------------
        private void ListView_OnSelectionChange(IEnumerable<object> selectedItems)
        {
            //This may return null when no selection exists
            OnSelectionChange.Invoke(selectedItems.FirstOrDefault() as T);

        }
    }
}