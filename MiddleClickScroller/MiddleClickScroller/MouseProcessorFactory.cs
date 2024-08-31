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

[Export( typeof( IMouseProcessorProvider ) )]
[Name( "MiddleClickScroller" )]
[Order( Before = "UrlClickMouseProcessor" )]
[ContentType( "text" )]
[TextViewRole( PredefinedTextViewRoles.Document )]
[TextViewRole( PredefinedTextViewRoles.Editable )]
public class MouseProcessorFactory : IMouseProcessorProvider {

    public IMouseProcessor GetAssociatedProcessor(IWpfTextView view) {
        return view.Properties.GetOrCreateSingletonProperty( () => new MouseProcessor( view ) );
    }

}
