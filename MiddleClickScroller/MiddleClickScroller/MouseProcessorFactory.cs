#nullable enable
namespace MiddleClickScroller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Shell;
using EnvDTE;

[Export( typeof( IMouseProcessorProvider ) )]
[Name( "MouseProcessor" )]
[Order( Before = "UrlClickMouseProcessor" )]
[ContentType( "text" )]
[TextViewRole( PredefinedTextViewRoles.Document )]
[TextViewRole( PredefinedTextViewRoles.Editable )]
public class MouseProcessorFactory : IMouseProcessorProvider {

    [Import]
    internal SVsServiceProvider ServiceProvider = default!;

    public IMouseProcessor GetAssociatedProcessor(IWpfTextView view) {
        ThreadHelper.ThrowIfNotOnUIThread();
        var dte = (DTE) ServiceProvider.GetService( typeof( DTE ) );
        return view.Properties.GetOrCreateSingletonProperty( () => new MouseProcessor( view, dte ) );
    }

}
[Export( typeof( IKeyProcessorProvider ) )]
[Name( "KeyProcessor" )]
[ContentType( "text" )]
[TextViewRole( PredefinedTextViewRoles.Document )]
[TextViewRole( PredefinedTextViewRoles.Editable )]
public class KeyProcessorFactory : IKeyProcessorProvider {

    [Import]
    internal SVsServiceProvider ServiceProvider = default!;

    public Microsoft.VisualStudio.Text.Editor.KeyProcessor GetAssociatedProcessor(IWpfTextView view) {
        ThreadHelper.ThrowIfNotOnUIThread();
        var dte = (DTE) ServiceProvider.GetService( typeof( DTE ) );
        return view.Properties.GetOrCreateSingletonProperty( () => new KeyProcessor( view, dte ) );
    }

}
