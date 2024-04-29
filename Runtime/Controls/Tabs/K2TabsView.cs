

using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using KTools;


namespace K2UI.Tabs
{
    /// <summary>
    /// K2TabsView is the main class to use tabs feature. 
    /// 
    /// It create a tabsBar that will accept tabbed buttons
    /// 
    /// It change the content in #Content element depending on the current Tab
    /// 
    /// Adds K2TabButton to the Element they will be finally added in the #TabBar
    /// </summary>
    public class K2TabsView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<K2TabsView, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {    
                get
                {
                    // we can add only tabButton here
                    yield return new UxmlChildElementDescription(typeof(VisualElement));
                }
            }

            UxmlStringAttributeDescription m_SelectedTabName =
                new() { name = "selected-tab-Name", defaultValue = "" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as K2TabsView;

                ate.SelectedTabName = m_SelectedTabName.GetValueFromBag(bag, cc);
            }
        }

        string _selected_tab_name = "";
        public string SelectedTabName
        {
            get {return _selected_tab_name;}
            set { _selected_tab_name = value; }
        }

        K2TabsBar tabsbar_el;
        VisualElement content_el;

        public override VisualElement contentContainer 
        {
            get{
                 if (content_el != null) 
                    return content_el;
                 else 
                    return this;
            }      
        }

        public K2TabsView()
        {       
            Setup();
            InitializeUI();
        }

        void Setup()
        {
            tabsbar_el = new K2TabsBar() {name = "K2TabsBar"};
            var content = new VisualElement() {name = "Content"};

            Add(tabsbar_el);
            Add(content);
            // setup main content after adding to parent
            content_el = content;
            tabsbar_el.RegisterCallback<ChangeEvent<string>>(onTabChanged);
        }

        private void InitializeUI()
        {
            content_el.RegisterCallback<GeometryChangedEvent>(HandleContentChanged);
            
        }

        private void HandleContentChanged(GeometryChangedEvent evt)
        {
            BuildButtons();

        }

        bool hasChanged()
        {
            var buttons = tabsbar_el.Query<K2TabButton>().ToList();
            var pages = content_el.Query<K2PageElement>().ToList();

            if (buttons.Count != pages.Count)
                return true;

            for (int i = 0; i < buttons.Count ; i++)
            {
                if (buttons[i].name != pages[i].name)
                    return true;
            }

            return false;
        }

        void BuildButtons()
        {
            if (!hasChanged())
                return;

            tabsbar_el.Clear();
            var pages = content_el.Query<K2PageElement>().ToList();
            foreach (var page in pages)
            {
                var bt = new K2TabButton();
                page.setButton(bt);
                tabsbar_el.Add(bt);
            }
            tabsbar_el.updateList();
        }

        private void onTabChanged(ChangeEvent<string> evt)
        {
            if (evt.target != tabsbar_el)
                return;

            // Debug.Log("changed "+evt.newValue);
            Select(evt.newValue);  
            if (!string.IsNullOrEmpty(this.setting_path))
                SettingsFile.Instance.SetString(setting_path, evt.newValue);
        }

        string setting_path = "";

        public void Bind(string setting_path, string default_tab)
        {
            this.setting_path = setting_path;
            if (!string.IsNullOrEmpty(this.setting_path))
            {
                Select(SettingsFile.Instance.GetString(setting_path, default_tab));
            }
        }

        string current_tab = "";
        public string CurrentTabCode
        {
            get => current_tab;
        }

        public void SelectFirst()
        {
            foreach (var panel in panels)
            {
                if (panel.enabled)
                {
                    Select(panel.code);
                    return;
                }
            }
        }

        public void Enable(string code, bool enable)
        {
            foreach (var panel in panels)
            {
                if (panel.code == code)
                    panel.enabled = enable;
            }
        }

        List<K2Page> panels;

        public void Init(List<K2Page> panels)
        {
            BuildButtons();
            this.panels = panels;
            foreach(K2Page panel in this.panels)
                panel.Init(tabsbar_el, content_el);
        }

        public void Select(string code)
        {
            tabsbar_el.setOpenedPage(code);
            current_tab = code;
            foreach (var page in content_el.Children())
            {
                page.Show(page.name == code);
            }

            if (panels == null)
                return;

            foreach (var panel in panels)
            {
                panel.isVisible = panel.code == code;    
            } 
        }

        public void Update()
        {
            foreach(K2Page panel in this.panels)
                panel.onUpdateUI();
        }

    }
}