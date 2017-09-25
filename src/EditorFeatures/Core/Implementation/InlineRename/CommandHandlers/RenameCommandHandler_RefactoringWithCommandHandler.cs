﻿// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using Microsoft.VisualStudio.Text.UI.Commanding;
using Microsoft.VisualStudio.Text.UI.Commanding.Commands;

namespace Microsoft.CodeAnalysis.Editor.Implementation.InlineRename
{
    internal partial class RenameCommandHandler :
        ILegacyCommandHandler<ReorderParametersCommandArgs>,
        ILegacyCommandHandler<RemoveParametersCommandArgs>,
        ILegacyCommandHandler<ExtractInterfaceCommandArgs>,
        ILegacyCommandHandler<EncapsulateFieldCommandArgs>
    {
        public CommandState GetCommandState(ReorderParametersCommandArgs args, Func<CommandState> nextHandler)
        {
            return nextHandler();
        }

        public void ExecuteCommand(ReorderParametersCommandArgs args, Action nextHandler)
        {
            CommitIfActiveAndCallNextHandler(args, nextHandler);
        }

        public CommandState GetCommandState(RemoveParametersCommandArgs args, Func<CommandState> nextHandler)
        {
            return nextHandler();
        }

        public void ExecuteCommand(RemoveParametersCommandArgs args, Action nextHandler)
        {
            CommitIfActiveAndCallNextHandler(args, nextHandler);
        }

        public CommandState GetCommandState(ExtractInterfaceCommandArgs args, Func<CommandState> nextHandler)
        {
            return nextHandler();
        }

        public void ExecuteCommand(ExtractInterfaceCommandArgs args, Action nextHandler)
        {
            CommitIfActiveAndCallNextHandler(args, nextHandler);
        }

        public CommandState GetCommandState(EncapsulateFieldCommandArgs args, Func<CommandState> nextHandler)
        {
            return nextHandler();
        }

        public void ExecuteCommand(EncapsulateFieldCommandArgs args, Action nextHandler)
        {
            CommitIfActiveAndCallNextHandler(args, nextHandler);
        }
    }
}
