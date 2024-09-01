#nullable enable
namespace MiddleClickScroller;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using EnvDTE;
using Microsoft.VisualStudio.Text.Editor;

public class MouseProcessor : MouseProcessorBase {

    private IWpfTextView View { get; }
    private DTE DTE { get; }
    private DispatcherTimer? Timer { get; set; }

    public MouseProcessor(IWpfTextView view, DTE dTE) {
        View = view;
        DTE = dTE;
        View.VisualElement.IsVisibleChanged += OnIsVisibleChanged;
        View.Closed += OnClosed;
    }

    private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args) {
        if ((bool) args.NewValue == false) {
            if (Timer != null) {
                Timer.Stop();
                Timer = null;
                View.VisualElement.ReleaseMouseCapture();
            }
        }
    }

    private void OnClosed(object sender, EventArgs args) {
        if (Timer != null) {
            Timer.Stop();
            Timer = null;
            View.VisualElement.ReleaseMouseCapture();
        }
        View.VisualElement.IsVisibleChanged -= OnIsVisibleChanged;
        View.Closed -= OnClosed;
    }

    public override void PreprocessMouseDown(MouseButtonEventArgs args) {
        if (args.ChangedButton == MouseButton.Middle) {
            if (Timer == null) {
                View.VisualElement.CaptureMouse();
                Timer = new DispatcherTimer( TimeSpan.FromMilliseconds( 40 ), DispatcherPriority.Normal, (sender, e) => {
                    //var delta = args.KeyboardDevice.IsKeyDown( Key.LeftShift ) ? 5 : 2;
                    var delta = 2;
                    View.ViewScroller.ScrollViewportVerticallyByPixels( -delta );
                }, View.VisualElement.Dispatcher );
                args.Handled = true;
            }
        }
    }

    public override void PreprocessMouseUp(MouseButtonEventArgs args) {
        if (args.ChangedButton == MouseButton.Middle) {
            if (Timer != null) {
                Timer.Stop();
                Timer = null;
                View.VisualElement.ReleaseMouseCapture();
                args.Handled = true;
            }
        }
    }

}
public class KeyProcessor : Microsoft.VisualStudio.Text.Editor.KeyProcessor {

    private IWpfTextView View { get; }
    private DTE DTE { get; }

    public KeyProcessor(IWpfTextView view, DTE dte) {
        View = view;
        DTE = dte;
        View.VisualElement.IsVisibleChanged += OnIsVisibleChanged;
        View.Closed += OnClosed;
        //var scrollLineUp = DTE.Commands.Cast<Command>().First( i => i.Name == "Edit.ScrollLineUp" );
        //var scrollLineDown = DTE.Commands.Cast<Command>().First( i => i.Name == "Edit.ScrollLineDown" );
    }

    private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args) {
        if ((bool) args.NewValue == false) {
        }
    }

    private void OnClosed(object sender, EventArgs args) {
        View.VisualElement.IsVisibleChanged -= OnIsVisibleChanged;
        View.Closed -= OnClosed;
    }

    public override void PreviewKeyDown(KeyEventArgs args) {
        if (args.Key == Key.Add) {
            var delta = args.KeyboardDevice.IsKeyDown( Key.LeftShift ) ? 5 : 2;
            View.ViewScroller.ScrollViewportVerticallyByPixels( -delta );
            args.Handled = true;
        } else
        if (args.Key == Key.Subtract) {
            var delta = args.KeyboardDevice.IsKeyDown( Key.LeftShift ) ? 5 : 2;
            View.ViewScroller.ScrollViewportVerticallyByPixels( delta );
            args.Handled = true;
        }
    }

    public override void PreviewKeyUp(KeyEventArgs args) {
    }

}
