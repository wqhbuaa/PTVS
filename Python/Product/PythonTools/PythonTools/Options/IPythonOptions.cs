// Python Tools for Visual Studio
// Copyright(c) Microsoft Corporation
// All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the License); you may not use
// this file except in compliance with the License. You may obtain a copy of the
// License at http://www.apache.org/licenses/LICENSE-2.0
//
// THIS CODE IS PROVIDED ON AN  *AS IS* BASIS, WITHOUT WARRANTIES OR CONDITIONS
// OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING WITHOUT LIMITATION ANY
// IMPLIED WARRANTIES OR CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE,
// MERCHANTABLITY OR NON-INFRINGEMENT.
//
// See the Apache Version 2.0 License for specific language governing
// permissions and limitations under the License.

using System;
using System.Runtime.InteropServices;
using Microsoft.PythonTools.Parsing;

namespace Microsoft.PythonTools.Options {
    // TODO: We should switch to a scheme which takes strings / returns object for options so they're extensible w/o reving the interface
    [Guid("BACA2500-5EA7-4075-8D02-647EAC0BC6E3")]
    public interface IPythonOptions {
        IPythonIntellisenseOptions Intellisense {
            get;
        }

        /// <summary>
        /// Gets interactive options for the given environment.
        /// </summary>
        /// <param name="interpreterName">
        /// The user-visible description of the environment. If multiple
        /// environments have the same description, one will be returned
        /// arbitrarily.
        /// </param>
        IPythonInteractiveOptions GetInteractiveOptions(string interpreterName);

        bool PromptBeforeRunningWithBuildErrorSetting {
            get;
            set;
        }

        bool AutoAnalyzeStandardLibrary {
            get;
            set;
        }

        Severity IndentationInconsistencySeverity {
            get;
            set;
        }

        bool TeeStandardOutput {
            get;
            set;
        }

        bool WaitOnAbnormalExit {
            get;
            set;
        }

        bool WaitOnNormalExit {
            get;
            set;
        }
    }

    [Guid("77179244-BBD7-4AA2-B27B-F2CCC679953A")]
    public interface IPythonIntellisenseOptions {
        bool AddNewLineAtEndOfFullyTypedWord { get; set; }
        bool EnterCommitsCompletion { get; set; }
        bool UseMemberIntersection { get; set; }
        string CompletionCommittedBy { get; set; }
        bool AutoListIdentifiers { get; set; }
    }

    [Guid("6DCCD6E9-FAC4-4EFA-9243-AE1A71D8923D")]
    public interface IPythonInteractiveOptions {
        string PrimaryPrompt {
            get;
            set;
        }

        string SecondaryPrompt {
            get;
            set;
        }

        bool UseInterpreterPrompts {
            get;
            set;
        }

        bool InlinePrompts {
            get;
            set;
        }

        bool ReplSmartHistory {
            get;
            set;
        }

        string ReplIntellisenseMode {
            get;
            set;
        }

        string StartupScript {
            get;
            set;
        }

        string ExecutionMode {
            get;
            set;
        }

        string InterpreterArguments {
            get;
            set;
        }

        bool EnableAttach {
            get;
            set;
        }
    }
}
