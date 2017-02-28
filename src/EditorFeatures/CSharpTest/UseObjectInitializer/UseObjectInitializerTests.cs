// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.UseObjectInitializer;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.Diagnostics;
using Roslyn.Test.Utilities;
using Xunit;

namespace Microsoft.CodeAnalysis.Editor.CSharp.UnitTests.UseObjectInitializer
{
    public partial class UseObjectInitializerTests : AbstractCSharpDiagnosticProviderBasedUserDiagnosticTest
    {
        internal override (DiagnosticAnalyzer, CodeFixProvider) CreateDiagnosticProviderAndFixer(Workspace workspace)
            => (new CSharpUseObjectInitializerDiagnosticAnalyzer(), new CSharpUseObjectInitializerCodeFixProvider());

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestOnVariableDeclarator()
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int i;

    void M()
    {
        var c = [||]new C();
        c.i = 1;
    }
}",
@"class C
{
    int i;

    void M()
    {
        var c = new C()
        {
            i = 1
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestDoNotUpdateAssignmentThatReferencesInitializedValue1Async()
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int i;

    void M()
    {
        var c = [||]new C();
        c.i = 1;
        c.i = c.i + 1;
    }
}",
@"class C
{
    int i;

    void M()
    {
        var c = new C()
        {
            i = 1
        };
        c.i = c.i + 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestDoNotUpdateAssignmentThatReferencesInitializedValue2Async()
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int i;

    void M()
    {
        var c = [||]new C();
        c.i = c.i + 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestDoNotUpdateAssignmentThatReferencesInitializedValue3Async()
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int i;

    void M()
    {
        C c;
        c = [||]new C();
        c.i = 1;
        c.i = c.i + 1;
    }
}",
@"class C
{
    int i;

    void M()
    {
        C c;
        c = new C()
        {
            i = 1
        };
        c.i = c.i + 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestDoNotUpdateAssignmentThatReferencesInitializedValue4Async()
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int i;

    void M()
    {
        C c;
        c = [||]new C();
        c.i = c.i + 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestOnAssignmentExpression()
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int i;

    void M()
    {
        C c = null;
        c = [||]new C();
        c.i = 1;
    }
}",
@"class C
{
    int i;

    void M()
    {
        C c = null;
        c = new C()
        {
            i = 1
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestStopOnDuplicateMember()
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int i;

    void M()
    {
        var c = [||]new C();
        c.i = 1;
        c.i = 2;
    }
}",
@"class C
{
    int i;

    void M()
    {
        var c = new C()
        {
            i = 1
        };
        c.i = 2;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestComplexInitializer()
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int i;
    int j;

    void M()
    {
        C[] array;
        array[0] = [||]new C();
        array[0].i = 1;
        array[0].j = 2;
    }
}",
@"class C
{
    int i;
    int j;

    void M()
    {
        C[] array;
        array[0] = new C()
        {
            i = 1,
            j = 2
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestNotOnCompoundAssignment()
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int i;
    int j;

    void M()
    {
        var c = [||]new C();
        c.i = 1;
        c.j += 1;
    }
}",
@"class C
{
    int i;
    int j;

    void M()
    {
        var c = new C()
        {
            i = 1
        };
        c.j += 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestMissingWithExistingInitializer()
        {
            await TestMissingInRegularAndScriptAsync(
@"class C
{
    int i;
    int j;

    void M()
    {
        var c = [||]new C() { i = 1 };
        c.j = 1;
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestMissingBeforeCSharp3()
        {
            await TestMissingAsync(
@"class C
{
    int i;
    int j;

    void M()
    {
        var c = [||]new C();
        c.j = 1;
    }
}", parseOptions: CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp2));
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestFixAllInDocument1()
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int i;
    int j;

    void M()
    {
        var v = {|FixAllInDocument:new|} C(() => {
            var v2 = new C();
            v2.i = 1;
        });
        v.j = 2;
    }
}",
@"class C
{
    int i;
    int j;

    void M()
    {
        var v = new C(() => {
            var v2 = new C()
            {
                i = 1
            };
        })
        {
            j = 2
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestFixAllInDocument2()
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int i;
    int j;

    void M()
    {
        var v = {|FixAllInDocument:new|} C();
        v.j = () => {
            var v2 = new C();
            v2.i = 1;
        };
    }
}",
@"class C
{
    int i;
    int j;

    void M()
    {
        var v = new C()
        {
            j = () => {
                var v2 = new C()
                {
                    i = 1
                };
            }
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestFixAllInDocument3()
        {
            await TestInRegularAndScriptAsync(
@"class C
{
    int i;
    int j;

    void M()
    {
        C[] array;
        array[0] = {|FixAllInDocument:new|} C();
        array[0].i = 1;
        array[0].j = 2;
        array[1] = new C();
        array[1].i = 3;
        array[1].j = 4;
    }
}",
@"class C
{
    int i;
    int j;

    void M()
    {
        C[] array;
        array[0] = new C()
        {
            i = 1,
            j = 2
        };
        array[1] = new C()
        {
            i = 3,
            j = 4
        };
    }
}");
        }

        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestTrivia1()
        {
            await TestInRegularAndScriptAsync(
@"
class C
{
    int i;
    int j;
    void M()
    {
        var c = [||]new C();
        c.i = 1; // Foo
        c.j = 2; // Bar
    }
}",
@"
class C
{
    int i;
    int j;
    void M()
    {
        var c = new C()
        {
            i = 1, // Foo
            j = 2 // Bar
        };
    }
}",
compareTokens: false);
        }

        [WorkItem(15459, "https://github.com/dotnet/roslyn/issues/15459")]
        [Fact, Trait(Traits.Feature, Traits.Features.CodeActionsUseObjectInitializer)]
        public async Task TestMissingInNonTopLevelObjectInitializer()
        {
            await TestMissingInRegularAndScriptAsync(
@"class C {
	int a;
	C Add(int x) {
		var c = Add([||]new int());
		c.a = 1;
		return c;
	}
}");
        }
    }
}