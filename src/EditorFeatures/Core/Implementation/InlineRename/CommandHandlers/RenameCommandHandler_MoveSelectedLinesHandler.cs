﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.Text.UI.Commanding;
using Microsoft.VisualStudio.Text.UI.Commanding.Commands;

namespace Microsoft.CodeAnalysis.Editor.Implementation.InlineRename
{
    internal partial class RenameCommandHandler :
        ILegacyCommandHandler<MoveSelectedLinesUpCommandArgs>, ILegacyCommandHandler<MoveSelectedLinesDownCommandArgs>
    {
        public CommandState GetCommandState(MoveSelectedLinesUpCommandArgs args, Func<CommandState> nextHandler)
        {
            return nextHandler();
        }

        public void ExecuteCommand(MoveSelectedLinesUpCommandArgs args, Action nextHandler)
        {
            CommitIfActiveAndCallNextHandler(args, nextHandler);
        }

        public CommandState GetCommandState(MoveSelectedLinesDownCommandArgs args, Func<CommandState> nextHandler)
        {
            return nextHandler();
        }

        public void ExecuteCommand(MoveSelectedLinesDownCommandArgs args, Action nextHandler)
        {
            CommitIfActiveAndCallNextHandler(args, nextHandler);
        }
    }
}
