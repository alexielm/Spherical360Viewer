//--------------------------------------
// Axes.cs )(c) 2007 by Charles Petzold
//--------------------------------------
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Petzold.Media3D
{
    public class Axes : WireBase
    {
        TextGenerator txtgen = new TextGenerator();

        public Axes()
        {
            PropertyChanged(new DependencyPropertyChangedEventArgs());
        }

        // Extent property
        // ---------------
        // This property is not animatable
        public static readonly DependencyProperty ExtentProperty =
            DependencyProperty.Register("Extent",
                typeof(double), 
                typeof(Axes),
                new UIPropertyMetadata(3.0, PropertyChanged, null, true));

        public double Extent
        {
            set { SetValue(ExtentProperty, value); }
            get { return (double)GetValue(ExtentProperty); }
        }

        // ShowNumbers property
        // --------------------
        // This property is not animatable
        public static readonly DependencyProperty ShowNumbersProperty =
            DependencyProperty.Register("ShowNumbers", 
            typeof(bool), 
            typeof(Axes),
            new UIPropertyMetadata(true, PropertyChanged, null, true));

        public bool ShowNumbers
        {
            set { SetValue(ShowNumbersProperty, value); }
            get { return (bool)GetValue(ShowNumbersProperty); }
        }

        // LargeTick property (not animatable)
        // ------------------
        public static readonly DependencyProperty LargeTickProperty =
            DependencyProperty.Register("LargeTick",
            typeof(double),
            typeof(Axes),
            new UIPropertyMetadata(0.05, PropertyChanged, null, true));

        public double LargeTick
        {
            set { SetValue(LargeTickProperty, value); }
            get { return (double)GetValue(LargeTickProperty); }
        }

        // SmallTick property (not animatable)
        // ------------------
        public static readonly DependencyProperty SmallTickProperty =
            DependencyProperty.Register("SmallTick",
            typeof(double),
            typeof(Axes),
            new UIPropertyMetadata(0.025, PropertyChanged, null, true));

        public double SmallTick
        {
            set { SetValue(SmallTickProperty, value); }
            get { return (double)GetValue(SmallTickProperty); }
        }

        // Can these be Add Owner on WireText???

        // Font property
        // ---------------
        public static readonly DependencyProperty FontProperty =
            DependencyProperty.Register("Font",
            typeof(Font),
            typeof(Axes),
            new UIPropertyMetadata(Font.Modern, PropertyChanged, null, true));

        public Font Font
        {
            set { SetValue(FontProperty, value); }
            get { return (Font)GetValue(FontProperty); }
        }

        // FontSize property
        // ---------------
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize",
            typeof(double),
            typeof(Axes),
            new UIPropertyMetadata(0.10, PropertyChanged, null, true));

        public double FontSize
        {
            set { SetValue(FontSizeProperty, value); }
            get { return (double)GetValue(FontSizeProperty); }
        }


        public static readonly DependencyProperty LabelsProperty =
            DependencyProperty.Register("Labels",
                typeof(string),
                typeof(Axes),
                new PropertyMetadata("XYZ", PropertyChanged),
                ValidateLabels);

        public string Labels
        {
            set { SetValue(LabelsProperty, value); }
            get { return (string)GetValue(LabelsProperty); }
        }

        // Ensure Labels string is either null or a multiple of 3 characters
        static bool ValidateLabels(object obj)
        {
            string str = obj as string;

            if (str == null)
                return true;

            return str.Length % 3 == 0;
        }


        protected override void Generate(DependencyPropertyChangedEventArgs args,
                                         Point3DCollection lines)
        {
            lines.Clear();

            // X axis.
            lines.Add(new Point3D(-Extent, 0, 0));
            lines.Add(new Point3D(Extent, 0, 0));

            // Y axis.
            lines.Add(new Point3D(0, -Extent, 0));
            lines.Add(new Point3D(0, Extent, 0));

            // Z axis.
            lines.Add(new Point3D(0, 0, -Extent));
            lines.Add(new Point3D(0, 0, Extent));

            for (int i = (int)(-10 * Extent); i <= (int)(10 * Extent); i++)
            {
                double tick = (i % 10 == 0) ? LargeTick : SmallTick;
                double d = i / 10.0;

                // X axis tick marks.
                lines.Add(new Point3D(d, -tick, 0));
                lines.Add(new Point3D(d, tick, 0));

                // Y axis tick marks.
                lines.Add(new Point3D(-tick, d, 0));
                lines.Add(new Point3D(tick, d, 0));

                // Z axis tick marks.
                lines.Add(new Point3D(0, -tick, d));
                lines.Add(new Point3D(0, tick, d));

                txtgen.Font = Font;
                txtgen.FontSize = FontSize;
                txtgen.Thickness = Thickness;
                txtgen.Rounding = Rounding;
                txtgen.BaselineDirection = new Vector3D(1, 0, 0);
                txtgen.UpDirection = new Vector3D(0, 1, 0);

                if (i != 0 && i % 10 == 0)
                {
                    string str = ((int)d).ToString();
                    bool isEnd = (i == (int)(-10 * Extent)) || (i == (int)(10 * Extent));
                    string strPrefix = (i == (int)(-10 * Extent)) ? "-" : "+";

                    // X axis numbers and labels.
                    if (isEnd && Labels != null) 
                        str = strPrefix + Labels.Substring(0, Labels.Length / 3);

                    if (isEnd || ShowNumbers)
                    {
                        txtgen.Origin = new Point3D(d, -tick * 1.25, 0);
                        txtgen.HorizontalAlignment = HorizontalAlignment.Center;
                        txtgen.VerticalAlignment = VerticalAlignment.Top;
                        txtgen.Generate(lines, str);
                    }

                    // Y axis numbers and labels.
                    if (isEnd) 
                        str = strPrefix + Labels.Substring(Labels.Length / 3, Labels.Length / 3);

                    if (isEnd || ShowNumbers)
                    {
                        txtgen.Origin = new Point3D(tick * 1.25, d, 0);
                        txtgen.HorizontalAlignment = HorizontalAlignment.Left;
                        txtgen.VerticalAlignment = VerticalAlignment.Center;
                        txtgen.Generate(lines, str);
                    }

                    // Want to make Z either viewed from left or right !!!!!!!!!!!!!!!!!!

                    // Z axis numbers and labels.
                    if (isEnd) 
                        str = strPrefix + Labels.Substring(2 * Labels.Length / 3);

                    if (isEnd || ShowNumbers)
                    {                                            
                        txtgen.Origin = new Point3D(0, -tick * 1.25, d);
                        txtgen.BaselineDirection = new Vector3D(0, 0, 1);
                        txtgen.HorizontalAlignment = HorizontalAlignment.Center;
                        txtgen.VerticalAlignment = VerticalAlignment.Top;
                        txtgen.Generate(lines, str);
                    }
                }
            }
        }
    }
}
