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
using System.Runtime.Remoting;

namespace VCProjectEngineWrapper
{
	public class
#if (VS2015)
		VCConfigurationWrapperVs2015
#elif (VS2017)
		VCConfigurationWrapperVs2017
#elif (VS2019)
		VCConfigurationWrapperVs2019
#elif (VS2022)
		VCConfigurationWrapperVs2022
#endif
		: IVCConfigurationWrapper
	{
		private VCConfiguration _wrapped = null;

		public
#if (VS2015)
			VCConfigurationWrapperVs2015
#elif (VS2017)
			VCConfigurationWrapperVs2017
#elif (VS2019)
			VCConfigurationWrapperVs2019
#elif (VS2022)
			VCConfigurationWrapperVs2022
#endif
			(object wrapped)
		{
			_wrapped = wrapped as VCConfiguration;
		}

		public bool isValid()
		{
			return (_wrapped != null);
		}

		public string GetWrappedVersion()
		{
			return Utility.GetWrappedVersion();
		}

		public bool isMakefileConfiguration()
		{
			try
			{
				ConfigurationTypes configurationType = _wrapped.ConfigurationType;
				if (configurationType == ConfigurationTypes.typeGeneric)
				{
					return true;
				}

				if (configurationType == ConfigurationTypes.typeUnknown && GetNMakeTool() != null && GetNMakeTool().isValid())
				{
					return true;
				}
			}
			catch
			{
				Logging.LogWarning("Unable to determine if a makefile configuration is used, falling back to default behavior.");
			}

			return false;
		}

		public string EvaluateMacro(string macro)
		{
			return _wrapped.Evaluate(macro);
		}

		public IVCCLCompilerToolWrapper GetCLCompilerTool()
		{
			try
			{
				IEnumerable tools = _wrapped.Tools as IEnumerable;
				foreach (Object tool in tools)
				{
					VCCLCompilerTool compilerTool = tool as VCCLCompilerTool;
					if (compilerTool != null)
					{
						return new
#if (VS2015)
							VCCLCompilerToolWrapperVs2015
#elif (VS2017)
							VCCLCompilerToolWrapperVs2017
#elif (VS2019)
							VCCLCompilerToolWrapperVs2019
#elif (VS2022)
							VCCLCompilerToolWrapperVs2022
#endif
							(compilerTool);
					}
				}
			}
			catch (Exception e)
			{
				Logging.LogError("Configuration failed to retreive cl compiler tool: " + e.Message);
			}
			return new
#if (VS2015)
				VCCLCompilerToolWrapperVs2015
#elif (VS2017)
				VCCLCompilerToolWrapperVs2017
#elif (VS2019)
				VCCLCompilerToolWrapperVs2019
#elif (VS2022)
				VCCLCompilerToolWrapperVs2022
#endif
				(null);
		}

		public IVCNMakeToolWrapper GetNMakeTool()
		{
			try
			{
				IEnumerable tools = _wrapped.Tools as IEnumerable;

				foreach (Object tool in tools)
				{
					VCNMakeTool compilerTool = tool as VCNMakeTool;
					if (compilerTool != null)
					{
						return new
#if (VS2015)
							VCNMakeToolWrapperVs2015
#elif (VS2017)
							VCNMakeToolWrapperVs2017
#elif (VS2019)
							VCNMakeToolWrapperVs2019
#elif (VS2022)
							VCNMakeToolWrapperVs2022
#endif
							(compilerTool);
					}
				}
			}
			catch (Exception e)
			{
				Logging.LogError("Configuration failed to retreive nmake tool: " + e.Message);
			}
			return new
#if (VS2015)
				VCNMakeToolWrapperVs2015
#elif (VS2017)
				VCNMakeToolWrapperVs2017
#elif (VS2019)
				VCNMakeToolWrapperVs2019
#elif (VS2022)
				VCNMakeToolWrapperVs2022
#endif
				(null);
		}

		public List<IVCPropertySheetWrapper> GetPropertySheets()
		{
			List<IVCPropertySheetWrapper> propertySheetsWrappers = new List<IVCPropertySheetWrapper>();
			try
			{
				IEnumerable wrappedPropertySheets = _wrapped.PropertySheets as IEnumerable;
				foreach (Object wrappedPropertySheet in wrappedPropertySheets)
				{
					VCPropertySheet vcPropertySheet = wrappedPropertySheet as VCPropertySheet;
					if (vcPropertySheet != null)
					{
						IVCPropertySheetWrapper wrapper = new
#if (VS2015)
							VCPropertySheetWrapperVs2015
#elif (VS2017)
							VCPropertySheetWrapperVs2017
#elif (VS2019)
							VCPropertySheetWrapperVs2019
#elif (VS2022)
							VCPropertySheetWrapperVs2022
#endif
							(vcPropertySheet);

						if (wrapper != null && wrapper.isValid())
						{
							propertySheetsWrappers.Add(wrapper);
						}
					}
				}
			}
			catch (Exception e)
			{
				Logging.LogError("Configuration failed to retreive property sheets: " + e.Message);
			}

			return propertySheetsWrappers;
		}

		public IVCPlatformWrapper GetPlatform()
		{
			return new
#if (VS2015)
				VCPlatformWrapperVs2015
#elif (VS2017)
				VCPlatformWrapperVs2017
#elif (VS2019)
				VCPlatformWrapperVs2019
#elif (VS2022)
				VCPlatformWrapperVs2022
#endif
				(_wrapped.Platform);
		}
	}
}

