using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMEAMon.Services
{
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Graphics;

    public class GraphicsPage : ContentPage
    {
        readonly MyDrawable drawable = new();

        public GraphicsPage()
        {
            Title = "Native Graphics Page";

            var graphicsView = new GraphicsView
            {
                Drawable = drawable,
                HeightRequest = 300,
                WidthRequest = 300
            };

            Content = new VerticalStackLayout
            {
                Padding = 20,
                Spacing = 10,
                Children =
            {
                new Label
                {
                    Text = "This is a native MAUI GraphicsView",
                    FontSize = 20,
                    HorizontalOptions = LayoutOptions.Center
                },
                graphicsView
            }
            };
        }
    }

    public class MyDrawable : IDrawable
    {
        float radius = 40;
        bool growing = true;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.FillColor = Colors.CornflowerBlue;
            canvas.FillRectangle(dirtyRect);

            canvas.FillColor = Colors.White;
            canvas.FillCircle(dirtyRect.Center.X, dirtyRect.Center.Y, radius);

            // Animate
            radius += growing ? 1 : -1;
            if (radius >= 100 || radius <= 20)
                growing = !growing;
        }
    }
}
