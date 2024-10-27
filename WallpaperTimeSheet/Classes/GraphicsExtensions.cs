﻿using System.Drawing;
using System.Drawing.Drawing2D;

public static class GraphicsExtensions
{
    public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, Rectangle bounds, int cornerRadius)
    {
        if (graphics == null)
            throw new ArgumentNullException(nameof(graphics));
        if (pen == null)
            throw new ArgumentNullException(nameof(pen));

        using (GraphicsPath path = RoundedRect(bounds, cornerRadius))
        {
            graphics.DrawPath(pen, path);
        }
    }

    public static void FillRoundedRectangle(this Graphics graphics, Brush brush, Rectangle bounds, int cornerRadius)
    {
        if (graphics == null)
            throw new ArgumentNullException(nameof(graphics));
        if (brush == null)
            throw new ArgumentNullException(nameof(brush));

        using (GraphicsPath path = RoundedRect(bounds, cornerRadius))
        {
            graphics.FillPath(brush, path);
        }
    }

    public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
    {
        int diameter = radius * 2;
        Size size = new Size(diameter, diameter);
        Rectangle arc = new Rectangle(bounds.Location, size);
        GraphicsPath path = new GraphicsPath();

        if (radius == 0)
        {
            path.AddRectangle(bounds);
            return path;
        }

        path.AddArc(arc, 180, 90);
        arc.X = bounds.Right - diameter;
        path.AddArc(arc, 270, 90);
        arc.Y = bounds.Bottom - diameter;
        path.AddArc(arc, 0, 90);
        arc.X = bounds.Left;
        path.AddArc(arc, 90, 90);

        path.CloseFigure();
        return path;
    }
}
