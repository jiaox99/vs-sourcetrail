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
using Microsoft.VisualStudio.VCProjectEngine;

namespace VCProjectEngineWrapper
{
	public class
#if (VS2015)
		VCNMakeToolWrapperVs2015
#elif (VS2017)
		VCNMakeToolWrapperVs2017
#elif (VS2019)
		VCNMakeToolWrapperVs2019
#elif (VS2022)
		VCNMakeToolWrapperVs2022
#endif
		: IVCNMakeToolWrapper
	{
		private VCNMakeTool _wrapped = null;
		private IVCRulePropertyStorage _wrappedRules = null;

		public
#if (VS2015)
			VCNMakeToolWrapperVs2015
#elif (VS2017)
			VCNMakeToolWrapperVs2017
#elif (VS2019)
			VCNMakeToolWrapperVs2019
#elif (VS2022)
			VCNMakeToolWrapperVs2022
#endif
			(object wrapped)
		{
			_wrapped = wrapped as VCNMakeTool;
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

		public string GetToolPath()
		{
			return _wrapped.ToolPath;
		}

		public string[] GetIncludeSearchPaths()
		{
			return _wrappedRules.GetEvaluatedPropertyValue("NMakeIncludeSearchPath").SplitPaths();
		}

		public string[] GetPreprocessorDefinitions()
		{
			return _wrappedRules.GetEvaluatedPropertyValue("NMakePreprocessorDefinitions").SplitPaths();
		}

		public string[] GetForcedIncludes()
		{
			return _wrappedRules.GetEvaluatedPropertyValue("NMakeForcedIncludes").SplitPaths();
		}

	}
}
