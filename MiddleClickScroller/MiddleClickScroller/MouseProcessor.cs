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
using Microsoft.VisualStudio.Text.Editor;

public class MouseProcessor : MouseProcessorBase {

    private IWpfTextView View { get; }
    private DispatcherTimer? Timer { get; set; }

    public MouseProcessor(IWpfTextView view) {
        View = view;
        View.VisualElement.IsVisibleChanged += OnIsVisibleChanged;
        View.Closed += OnClosed;
    }

    private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
        if ((bool) e.NewValue == false) {
            Timer?.Stop();
            Timer = null;
        }
    }

    private void OnClosed(object sender, EventArgs e) {
        Timer?.Stop();
        Timer = null;
        View.VisualElement.IsVisibleChanged -= OnIsVisibleChanged;
        View.Closed -= OnClosed;
    }

    public override void PreprocessMouseDown(MouseButtonEventArgs e) {
        if (e.ChangedButton == MouseButton.Middle) {
            if (Timer == null) {
                Timer = new DispatcherTimer(
                    TimeSpan.FromMilliseconds( 20 ),
                    DispatcherPriority.Normal,
                    (sender, e) => {
                        View.ViewScroller.ScrollViewportVerticallyByPixels( -5 );
                    },
                    View.VisualElement.Dispatcher );
            }
        }
    }

    public override void PreprocessMouseUp(MouseButtonEventArgs e) {
        if (e.ChangedButton == MouseButton.Middle) {
            Timer?.Stop();
            Timer = null;
        }
    }

}
