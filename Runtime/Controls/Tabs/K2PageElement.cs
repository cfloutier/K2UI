using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

namespace K2UI.Tabs
{   
    /// <summary>
    /// a simple visual element just used to contains label and icon
    /// </summary>
    public class K2PageElement : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<K2PageElement, UxmlTraits> { }

        // Add the two custom UXML attributes.
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_Label =
                new() { name = "label", defaultValue = "My Tab" };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as K2PageElement;

                ate.label = m_Label.GetValueFromBag(bag, cc);
               
            }
        }

        public string _label;
        public string label
        {
            get { return _label; }
            set { 
                    if (value == _label) return;
                    _label = value; 
                    
                    if (tab_button != null)
                    {
                        tab_button.label = label;
                    }
                }
        }

        public K2TabButton tab_button;
        public void setButton(K2TabButton bt)
        {
            bt.label = label;
            bt.name = name;
        }
    }

   
}