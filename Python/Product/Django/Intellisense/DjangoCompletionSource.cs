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

using System.Collections.Generic;
using System.Linq;
using Microsoft.Html.Editor.Document;
using Microsoft.PythonTools.Django.Project;
using Microsoft.PythonTools.Django.TemplateParsing;
using Microsoft.PythonTools.Intellisense;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Text;
using Microsoft.Web.Core.Text;

namespace Microsoft.PythonTools.Django.Intellisense {
    internal class DjangoCompletionSource : DjangoCompletionSourceBase {
        public DjangoCompletionSource(IGlyphService glyphService, DjangoAnalyzer analyzer, ITextBuffer textBuffer)
            : base(glyphService, analyzer, textBuffer) {
        }

        public override void AugmentCompletionSession(ICompletionSession session, IList<CompletionSet> completionSets) {
            var doc = HtmlEditorDocument.FromTextBuffer(_buffer);
            if (doc == null) {
                return;
            }
            doc.HtmlEditorTree.EnsureTreeReady();

            var primarySnapshot = doc.PrimaryView.TextSnapshot;
            var nullableTriggerPoint = session.GetTriggerPoint(primarySnapshot);
            if (!nullableTriggerPoint.HasValue) {
                return;
            }
            var triggerPoint = nullableTriggerPoint.Value;

            var artifacts = doc.HtmlEditorTree.ArtifactCollection;
            var index = artifacts.GetItemContaining(triggerPoint.Position);
            if (index < 0) {
                return;
            }

            var artifact = artifacts[index] as TemplateArtifact;
            if (artifact == null) {
                return;
            }

            var artifactText = doc.HtmlEditorTree.ParseTree.Text.GetText(artifact.InnerRange);
            artifact.Parse(artifactText);

            ITrackingSpan applicableSpan;
            var completionSet = GetCompletionSet(session.GetOptions(_analyzer._serviceProvider), _analyzer, artifact.TokenKind, artifactText, artifact.InnerRange.Start, triggerPoint, out applicableSpan);
            completionSets.Add(completionSet);
        }

        protected override IEnumerable<DjangoBlock> GetBlocks(IEnumerable<CompletionInfo> results, SnapshotPoint triggerPoint) {
            var doc = HtmlEditorDocument.FromTextBuffer(_buffer);
            if (doc == null) {
                yield break;
            }

            var artifacts = doc.HtmlEditorTree.ArtifactCollection.ItemsInRange(new TextRange(0, triggerPoint.Position));
            foreach (var artifact in artifacts.OfType<TemplateBlockArtifact>().Reverse()) {
                var artifactText = doc.HtmlEditorTree.ParseTree.Text.GetText(artifact.InnerRange);
                artifact.Parse(artifactText);
                if (artifact.Block != null) {
                    yield return artifact.Block;
                }
            }
        }
    }
}
