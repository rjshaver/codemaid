﻿#region CodeMaid is Copyright 2007-2015 Steve Cadwallader.

// CodeMaid is free software: you can redistribute it and/or modify it under the terms of the GNU
// Lesser General Public License version 3 as published by the Free Software Foundation.
//
// CodeMaid is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without
// even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details <http://www.gnu.org/licenses/>.

#endregion CodeMaid is Copyright 2007-2015 Steve Cadwallader.

using EnvDTE;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SteveCadwallader.CodeMaid.IntegrationTests.Helpers;
using SteveCadwallader.CodeMaid.Logic.Formatting;
using SteveCadwallader.CodeMaid.Properties;

namespace SteveCadwallader.CodeMaid.IntegrationTests.Formatting
{
    /// <summary>
    /// A base class implementing common logic for comment formatting unit tests.
    /// </summary>
    public abstract class BaseCommentFormatTests
    {
        #region Setup

        private static CommentFormatLogic _commentFormatLogic;
        private ProjectItem _projectItem;

        protected abstract string TestBaseFileName { get; }

        public static void ClassInitialize(TestContext testContext)
        {
            _commentFormatLogic = CommentFormatLogic.GetInstance(TestEnvironment.Package);
            Assert.IsNotNull(_commentFormatLogic);
        }

        public virtual void TestInitialize()
        {
            TestEnvironment.CommonTestInitialize();
            _projectItem = TestEnvironment.LoadFileIntoProject(string.Format(@"Data\{0}.cs", TestBaseFileName));
        }

        public virtual void TestCleanup()
        {
            TestEnvironment.RemoveFromProject(_projectItem);
        }

        #endregion Setup

        #region Tests

        protected void FormatsAsExpected()
        {
            Settings.Default.Formatting_CommentRunDuringCleanup = true;

            TestOperations.ExecuteCommandAndVerifyResults(RunFormatComments, _projectItem, string.Format(@"Data\{0}_Formatted.cs", TestBaseFileName));
        }

        protected void DoesNothingOnSecondPass()
        {
            Settings.Default.Formatting_CommentRunDuringCleanup = true;

            TestOperations.ExecuteCommandTwiceAndVerifyNoChangesOnSecondPass(RunFormatComments, _projectItem);
        }

        protected void DoesNothingWhenSettingIsDisabled()
        {
            Settings.Default.Formatting_CommentRunDuringCleanup = false;

            TestOperations.ExecuteCommandAndVerifyNoChanges(RunFormatComments, _projectItem);
        }

        #endregion Tests

        #region Helpers

        private static void RunFormatComments(Document document)
        {
            var textDocument = TestUtils.GetTextDocument(document);

            _commentFormatLogic.FormatComments(textDocument);
        }

        #endregion Helpers
    }
}