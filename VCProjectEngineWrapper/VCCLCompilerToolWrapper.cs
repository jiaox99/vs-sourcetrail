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

using CoatiSoftware.SourcetrailExtension;
using CoatiSoftware.SourcetrailExtension.Logging;
using Microsoft.VisualStudio.VCProjectEngine;
using System;

namespace VCProjectEngineWrapper
{
	public class
#if (VS2015)
		VCCLCompilerToolWrapperVs2015
#elif (VS2017)
		VCCLCompilerToolWrapperVs2017
#elif (VS2019)
		VCCLCompilerToolWrapperVs2019
#elif (VS2022)
		VCCLCompilerToolWrapperVs2022
#endif
		: IVCCLCompilerToolWrapper
	{
		private VCCLCompilerTool _wrapped = null;
		private IVCRulePropertyStorage _wrappedRules = null;

		public
#if (VS2015)
			VCCLCompilerToolWrapperVs2015
#elif (VS2017)
			VCCLCompilerToolWrapperVs2017
#elif (VS2019)
			VCCLCompilerToolWrapperVs2019
#elif (VS2022)
			VCCLCompilerToolWrapperVs2022
#endif
			(object wrapped)
		{
			_wrapped = wrapped as VCCLCompilerTool;
			_wrappedRules = wrapped as IVCRulePropertyStorage;
		}

		public bool isValid()
		{
			return (_wrapped != null && _wrappedRules != null);
		}

		public string GetWrappedVersion()
		{
			return Utility.GetWrappedVersion();
		}

		public string GetAdditionalOptions()
		{
			return _wrapped.AdditionalOptions;
		}

		public bool GetCompilesAsC()
		{
			return _wrapped.CompileAs == CompileAsOptions.compileAsC;
		}

		public bool GetCompilesAsCPlusPlus()
		{
			return _wrapped.CompileAs == CompileAsOptions.compileAsCPlusPlus;
		}

		public string GetToolPath()
		{
			return _wrapped.ToolPath;
		}

		public string GetLanguageStandard()
		{
			try
			{
				string rawStandardString = _wrappedRules.GetEvaluatedPropertyValue("LanguageStandard");
				if (rawStandardString != "stdcpplatest" && rawStandardString.IndexOf("stdcpp") == 0)
				{
					return rawStandardString.Replace("stdcpp", "c++");
				}
			}
			catch (Exception)
			{
				Logging.LogInfo("Unable to fetch language standard from project configuration, using default standard instead.");
			}
			return "";
		}

		public string[] GetAdditionalIncludeDirectories()
		{
			return _wrappedRules.GetEvaluatedPropertyValue("AdditionalIncludeDirectories").SplitPaths();
		}

		public string[] GetPreprocessorDefinitions()
		{
			string additionalDefines = "";
			switch (_wrapped.RuntimeLibrary)
			{
			case runtimeLibraryOption.rtMultiThreaded:
				additionalDefines += "_MT;";
				break;
			case runtimeLibraryOption.rtMultiThreadedDebug:
				additionalDefines += "_DEBUG;_MT;";
				break;
			case runtimeLibraryOption.rtMultiThreadedDLL:
				additionalDefines += "_MT;_DLL;";
				break;
			case runtimeLibraryOption.rtMultiThreadedDebugDLL:
				additionalDefines += "_DEBUG;_MT;_DLL;";
				break;
			}

			return (additionalDefines + _wrappedRules.GetEvaluatedPropertyValue("PreprocessorDefinitions")).SplitPaths();
		}

		public string[] GetForcedIncludeFiles()
		{
			return _wrappedRules.GetEvaluatedPropertyValue("ForcedIncludeFiles").SplitPaths();
		}
	}
}
