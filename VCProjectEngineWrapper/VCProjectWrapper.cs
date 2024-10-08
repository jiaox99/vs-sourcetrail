﻿/*
 * Copyright 2018 Coati Software KG
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using CoatiSoftware.SourcetrailExtension.Logging;
using Microsoft.VisualStudio.VCProjectEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace VCProjectEngineWrapper
{
	public class
#if (VS2015)
		VCProjectWrapperVs2015
#elif (VS2017)
		VCProjectWrapperVs2017
#elif (VS2019)
		VCProjectWrapperVs2019
#elif (VS2022)
		VCProjectWrapperVs2022
#endif
		: IVCProjectWrapper
	{
		private VCProject _wrapped = null;

		public
#if (VS2015)
			VCProjectWrapperVs2015
#elif (VS2017)
			VCProjectWrapperVs2017
#elif (VS2019)
			VCProjectWrapperVs2019
#elif (VS2022)
			VCProjectWrapperVs2022
#endif
			(object wrapped)
		{
			_wrapped = wrapped as VCProject;
		}

		public bool isValid()
		{
			return (_wrapped != null);
		}

		public string GetWrappedVersion()
		{
			return Utility.GetWrappedVersion();
		}

		public IVCConfigurationWrapper getConfiguration(string configurationName, string platformName)
		{
			try
			{
				IEnumerable configurations = _wrapped.Configurations as IEnumerable;
				foreach (Object configuration in configurations)
				{
					VCConfiguration vcProjectConfig = configuration as VCConfiguration;

					if (vcProjectConfig != null &&
						vcProjectConfig.ConfigurationName == configurationName &&
						(vcProjectConfig.Platform as VCPlatform).Name == platformName)
					{
						return new
#if (VS2015)
						VCConfigurationWrapperVs2015
#elif (VS2017)
						VCConfigurationWrapperVs2017
#elif (VS2019)
						VCConfigurationWrapperVs2019
#elif (VS2022)
						VCConfigurationWrapperVs2022
#endif
						(vcProjectConfig);
					}
				}
			}
			catch (Exception e)
			{
				Logging.LogError("Failed to retreive project configuration: " + e.Message);
			}
			Logging.LogError("Failed to find project config matching with \"" + configurationName + "\"");

			return new
#if (VS2015)
			VCConfigurationWrapperVs2015
#elif (VS2017)
			VCConfigurationWrapperVs2017
#elif (VS2019)
			VCConfigurationWrapperVs2019
#elif (VS2022)
			VCConfigurationWrapperVs2022
#endif
			(null);
		}

		public string GetProjectDirectory()
		{
			return _wrapped.ProjectDirectory;
		}

		public string GetName()
		{
			return _wrapped.Name;
		}

		public string GetProjectFile()
		{
			return _wrapped.ProjectFile;
		}

		public List<string> GetReferencedProjectNames()
		{
			List<string> referencedProjectNames = new List<string>();
			foreach (Object o in (_wrapped.VCReferences as List<Object>))
			{
				VCProjectReference projectReference = o as VCProjectReference;
				if (projectReference != null)
				{
					try
					{
						referencedProjectNames.Add(projectReference.Name);
					}
					catch (Exception)
					{
						// just ignore this reference
					}
				}
			}
			return referencedProjectNames;
		}
	}
}
