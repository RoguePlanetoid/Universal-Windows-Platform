using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LightEffect
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private Windows.UI.Composition.PointLight pointLight;

        private Windows.UI.Composition.Compositor Compositor
        {
            get
            {
                return Windows.UI.Xaml.Hosting.ElementCompositionPreview.GetElementVisual(Logo).Compositor;
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Composition.Visual visual = 
                Windows.UI.Xaml.Hosting.ElementCompositionPreview.GetElementVisual(Logo);
            pointLight = Compositor.CreatePointLight();
            pointLight.Color = Windows.UI.Colors.White;
            pointLight.CoordinateSpace = visual;
            pointLight.Targets.Add(visual);
            pointLight.Offset = 
                new System.Numerics.Vector3(-(float)Logo.ActualWidth * 2, (float)Logo.ActualHeight / 2, (float)Logo.ActualHeight);
            Windows.UI.Composition.ScalarKeyFrameAnimation animation = Compositor.CreateScalarKeyFrameAnimation();
            animation.InsertKeyFrame(1, 2 * (float)Logo.ActualWidth);
            animation.Duration = TimeSpan.FromSeconds(5.0f);
            animation.IterationBehavior = Windows.UI.Composition.AnimationIterationBehavior.Forever;
            pointLight.StartAnimation("Offset.X", animation);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            pointLight.Targets.RemoveAll();
        }
    }
}
