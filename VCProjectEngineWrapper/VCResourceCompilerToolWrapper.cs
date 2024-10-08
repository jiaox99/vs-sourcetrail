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
		VCResourceCompilerToolWrapperVs2015
#elif (VS2017)
		VCResourceCompilerToolWrapperVs2017
#elif (VS2019)
		VCResourceCompilerToolWrapperVs2019
#elif (VS2022)
		VCResourceCompilerToolWrapperVs2022
#endif
		: IVCResourceCompilerToolWrapper
	{
		private VCResourceCompilerTool _wrapped = null;
		private IVCRulePropertyStorage _wrappedRules = null;

		public
#if (VS2015)
			VCResourceCompilerToolWrapperVs2015
#elif (VS2017)
			VCResourceCompilerToolWrapperVs2017
#elif (VS2019)
			VCResourceCompilerToolWrapperVs2019
#elif (VS2022)
			VCResourceCompilerToolWrapperVs2022
#endif
			(object wrapped)
		{
			_wrapped = wrapped as VCResourceCompilerTool;
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

		public string[] GetAdditionalIncludeDirectories()
		{
			return _wrappedRules.GetEvaluatedPropertyValue("AdditionalIncludeDirectories").SplitPaths();
		}

		public string[] GetPreprocessorDefinitions()
		{
			return _wrappedRules.GetEvaluatedPropertyValue("PreprocessorDefinitions").SplitPaths();
		}
	}
}
