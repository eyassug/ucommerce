﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using UCommerce.Umbraco.Installer.PackageActions;

namespace UCommerce.RazorStore.Installer.PackageActions
{
    using System.Xml;

    using Tools.XmlConfigMerge;

    using umbraco.interfaces;

    public class UpdateUrlRewritingConfig : IPackageAction
    {
        private static string _xpathFormatString = "//add[@name='{0}']/@destinationUrl";
        private string _targetConfigFullPath;
        private string _sourceConfigFullPath;

        private string TargetConfigPath { get; set; }
        private string SourceConfigPath { get; set; }

        private void Initialize(XmlNode xmlData)
        {
            TargetConfigPath = PackageActionsHelpers.GetAttributeValueFromNode(xmlData, "targetConfig");
            SourceConfigPath = PackageActionsHelpers.GetAttributeValueFromNode(xmlData, "sourceConfig");

            _targetConfigFullPath = PackageActionsHelpers.GetLocalPath(TargetConfigPath);
            _sourceConfigFullPath = PackageActionsHelpers.GetLocalPath(SourceConfigPath);
        }

        public bool Execute(string packageName, XmlNode xmlData)
        {
            Initialize(xmlData);

            PackageActionsHelpers.BackupExistingXmlConfig(_targetConfigFullPath);

			var existingRewrites = XElement.Load(_targetConfigFullPath);
			var newRewrites = XElement.Load(_sourceConfigFullPath);

	        var mergedRewrites = new UrlRewriteRulesInstaller().Merge(existingRewrites, newRewrites);

			mergedRewrites.Save(_targetConfigFullPath);

            return true;
        }

        public string Alias()
        {
            return "UpdateUrlRewritingConfig";
        }

        public bool Undo(string packageName, XmlNode xmlData)
        {
            return true;
        }

        public XmlNode SampleXml()
        {
            string value = string.Format("<Action runat=\"install\" undo=\"true\" alias=\"{0}\" targetConfig=\"~/config/UrlRewriting.config\" sourceConfig=\"~/umbraco/ucommerce/install/uCommerce.DemoStore.UrlRewriting.config\" />", Alias());
            return PackageActionsHelpers.ParseStringToXmlNode(value);
        }

        private string GetRewritingNode(ConfigFileManager config, string key)
        {
            var xpath = string.Format(_xpathFormatString, key);
            return config.GetXPathValue(xpath);
        }

        private void ReplaceValue(ConfigFileManager config, string key, string value)
        {
            var xpath = string.Format(_xpathFormatString, key);
            config.ReplaceXPathValues(xpath, value);
        }
    }
}