using System.Collections.Generic;
using System;
using System.Threading;
using System.IO;

using UnityEngine;
using UnityEngine.UIElements;

using K2UI;
using System.Globalization;

namespace KTools
{
    public interface IResettable
    {
        /// <summary>
        /// Reset the setting using the default value.
        /// The base_key is used to limit the reset to all keys starting with this base_key 
        /// </summary>
        /// <param name="base_key"></param>
        void Reset(string base_key);
    }
    public class Setting<T> : IResettable
    {
        public string key;
        T default_value;

        public Setting(string key, T default_value)
        {
            this.key = key;
            this.default_value = default_value; 
            if (SettingsFile.Instance.loaded)
                loadValue();
            else
                SettingsFile.Instance.onloaded_event += loadValue;

            if (!string.IsNullOrEmpty(key))    
                SettingsFile.Instance.reset_register.Add(this);
        }

        public void Reset(string path = null)
        {   
            if (path != null)
                if (!this.key.StartsWith(path))
                    return;
                    
            this.V = default_value;
        }

        void loadValue()
        {
            _value = SettingsFile.Instance.Get<T>(key, default_value);
        }

        T _value;
        public virtual T V
        {
            get { return _value; }
            set
            {
                if (value.Equals(_value))
                    return;

                if (!string.IsNullOrEmpty(key))    
                    if (!SettingsFile.Instance.Set<T>(key, value))
                        return;

                _value = value;
                listeners?.Invoke(this.V); 
            }
        }

        public delegate void onChanged(T value);

        public event onChanged listeners;

        // add listener and call it once
        public void listen(onChanged listener)
        {
            listeners+= listener;
            listener(V);
        }
    }


    public class ClampSetting<T> : Setting<T> where T : System.IComparable<T>
    {
        T _min;
        public T min
        {
            get { return _min; }
            set { 
                _min = value;
                V = Extensions.Clamp(value, min, max);
            }
        }

        T _max;
        public T max
        {
            get { return _max; }
            set { 
                _max = value;
                V = Extensions.Clamp(value, min, max);
            }
        }
        
        public ClampSetting(string key, T default_value, T min, T max): base(key, default_value)
        {
            this._min = min;
            this._max = max;  
            V = Extensions.Clamp(V, min, max);
        }

        public override T V { 
            get => base.V; 
            set {
                
                base.V = value;
            } 
        }
    }

    public class ClampSettingInt : Setting<int>
    {
        
        int _min;
        public int min
        {
            get { return _min; }
            set { 
                _min = value;
                V = Extensions.Clamp(value, min, max);
            }
        }

        int _max;
        public int max
        {
            get { return _max; }
            set { 
                _max = value;
                V = Extensions.Clamp(value, min, max);
            }
        }

        public ClampSettingInt(string key, int default_value, int min, int max): base(key, default_value)
        {
            this.min = min;
            this.max = max;     
        }

        int clamp(int value)
        {
            if (value < min)
                value = min;
            else if (value > max)
                value = max;

            return value;
        }

        
        public override int V { 
            get => base.V; 
            set {
                base.V = clamp(value);
            } 
        }
    }


    public class EnumSetting <TEnum> : IResettable where TEnum : struct
    {
        public string key;
        TEnum default_value;
        public EnumSetting(string key, TEnum default_value)
        {
            this.key = key;
            this.default_value = default_value;
            if (SettingsFile.Instance.loaded)
                loadValue();
            else
                SettingsFile.Instance.onloaded_event += loadValue;

            if (!string.IsNullOrEmpty(key))    
                SettingsFile.Instance.reset_register.Add(this);
        }

        public void Reset(string path = null)
        {
            if (path != null)
                if (!this.key.StartsWith(path))
                    return;
                
            this.V = default_value;
        }

        void loadValue()
        {
            _value = SettingsFile.Instance.GetEnum<TEnum>(key, default_value);
        }

        TEnum _value;

        public TEnum V
        {
            get { return _value; }
            set
            {
                if (value.Equals(_value)) return;

                _value = value;
                listeners?.Invoke(this.V);

                SettingsFile.Instance.SetEnum<TEnum>(key, _value);
            }
        }

        public int int_value
        {
            get { return (int)(object) V;}
            set { _value = (TEnum)(object) value;}
        }

        public delegate void onChanged(TEnum value);

        public event onChanged listeners;

        // add listener and call it once
        public void listen(onChanged listener)
        {
            
            listeners+= listener;
            listener(V);
        }
    }



}