﻿using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Elib.Core.Configuration
{
    public class SerializableConfigurationSource : IConfigurationSource
    {
        Dictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();

        public SerializableConfigurationSource()
        {

        }

        public ConfigurationSection GetSection(string sectionName)
        {
            ConfigurationSection configSection;
            if (sections.TryGetValue(sectionName, out configSection))
            {
                SerializableConfigurationSection section = configSection as SerializableConfigurationSection;
                if (section != null)
                {
                    using (StringWriter xml = new StringWriter())
                    using (XmlWriter xmlWriter = XmlWriter.Create(xml))
                    {
                        section.WriteXml(xmlWriter);
                        xmlWriter.Flush();

                        MethodInfo methodInfo = section.GetType().GetMethod("DeserializeSection", BindingFlags.NonPublic | BindingFlags.Instance);
                        methodInfo.Invoke(section, new object[] { XDocument.Parse(xml.ToString()).CreateReader() });

                        return configSection;
                    }
                }
            }
            return null;
        }

        public void Add(string sectionName, ConfigurationSection configurationSection)
        {
            sections[sectionName] = configurationSection;
        }

        public void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public void Remove(string sectionName)
        {
            sections.Remove(sectionName);
        }

        public void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<ConfigurationSourceChangedEventArgs> SourceChanged;

        public void Dispose()
        {
        }

    }

}
