/*
Copyright 2006 Bart Desmet
Copyright 2009 scorpion

This file is part of N2.

N2 is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

N2 is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with N2.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace cleanInjector
{
    public class Thumbnail : FrameworkElement
    {
        public Thumbnail()
        {
            this.LayoutUpdated += new EventHandler(Thumbnail_LayoutUpdated);
            this.Unloaded += new RoutedEventHandler(Thumbnail_Unloaded);
        }

        public static DependencyProperty SourceProperty;
        public static DependencyProperty ClientAreaOnlyProperty;

        static Thumbnail()
        {
            SourceProperty = DependencyProperty.Register(
                "Source",
                typeof(IntPtr),
                typeof(Thumbnail),
                new FrameworkPropertyMetadata(
                    IntPtr.Zero,
                    FrameworkPropertyMetadataOptions.AffectsMeasure,
                    delegate(DependencyObject obj, DependencyPropertyChangedEventArgs args)
                    {
                        ((Thumbnail)obj).InitialiseThumbnail((IntPtr)args.NewValue);
                    }));

            ClientAreaOnlyProperty = DependencyProperty.Register(
                "ClientAreaOnly",
                typeof(bool),
                typeof(Thumbnail),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.AffectsMeasure,
                    delegate(DependencyObject obj, DependencyPropertyChangedEventArgs args)
                    {
                        ((Thumbnail)obj).UpdateThumbnail();
                    }));

            OpacityProperty.OverrideMetadata(
                typeof(Thumbnail),
                new FrameworkPropertyMetadata(
                    1.0,
                    FrameworkPropertyMetadataOptions.Inherits,
                    delegate(DependencyObject obj, DependencyPropertyChangedEventArgs args)
                    {
                        ((Thumbnail)obj).UpdateThumbnail();
                    }));
        }

        public IntPtr Source
        {
            get { return (IntPtr)this.GetValue(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
        }

        public bool ClientAreaOnly
        {
            get { return (bool)this.GetValue(ClientAreaOnlyProperty); }
            set { this.SetValue(ClientAreaOnlyProperty, value); }
        }

        public new double Opacity
        {
            get { return (double)this.GetValue(OpacityProperty); }
            set { this.SetValue(OpacityProperty, value); }
        }

        private HwndSource target;
        private IntPtr thumb;

        private void InitialiseThumbnail(IntPtr source)
        {
            if (IntPtr.Zero != thumb)
            {
                // release the old thumbnail
                ReleaseThumbnail();
            }

            if (IntPtr.Zero != source)
            {
                // find our parent hwnd
                target = (HwndSource)HwndSource.FromVisual(this);

                // if we have one, we can attempt to register the thumbnail
                if (target != null && 0 == DWM.DwmRegisterThumbnail(target.Handle, source, out this.thumb))
                {
                    DWM.ThumbnailProperties props = new DWM.ThumbnailProperties();
                    props.Visible = false;
                    props.SourceClientAreaOnly = this.ClientAreaOnly;
                    props.Opacity = (byte)(255 * this.Opacity);
                    props.Flags = DWM.ThumbnailFlags.Visible | DWM.ThumbnailFlags.SourceClientAreaOnly
                    | DWM.ThumbnailFlags.Opacity;
                    DWM.DwmUpdateThumbnailProperties(thumb, ref props);
                }
            }
        }

        private void ReleaseThumbnail()
        {
            DWM.DwmUnregisterThumbnail(thumb);
            this.thumb = IntPtr.Zero;
            this.target = null;
        }

        private void UpdateThumbnail()
        {
            if (IntPtr.Zero != thumb)
            {
                DWM.ThumbnailProperties props = new DWM.ThumbnailProperties();
                props.SourceClientAreaOnly = this.ClientAreaOnly;
                props.Opacity = (byte)(255 * this.Opacity);
                props.Flags = DWM.ThumbnailFlags.SourceClientAreaOnly | DWM.ThumbnailFlags.Opacity;
                DWM.DwmUpdateThumbnailProperties(thumb, ref props);
            }
        }

        private void Thumbnail_Unloaded(object sender, RoutedEventArgs e)
        {
            ReleaseThumbnail();
        }

        // this is where the magic happens
        private void Thumbnail_LayoutUpdated(object sender, EventArgs e)
        {
            if (IntPtr.Zero == thumb)
            {
                InitialiseThumbnail(this.Source);
            }

            if (IntPtr.Zero != thumb)
            {
                if (!target.RootVisual.IsAncestorOf(this))
                {
                    //we are no longer in the visual tree
                    ReleaseThumbnail();
                    return;
                }

                GeneralTransform transform = TransformToAncestor(target.RootVisual);
                Point a = transform.Transform(new Point(0, 0));
                Point b = transform.Transform(new Point(this.ActualWidth, this.ActualHeight));

                DWM.ThumbnailProperties props = new DWM.ThumbnailProperties();
                props.Visible = true;
                props.Destination = new DWM.Rect(
                    (int)Math.Ceiling(a.X), (int)Math.Ceiling(a.Y),
                    (int)Math.Ceiling(b.X), (int)Math.Ceiling(b.Y));
                props.Flags = DWM.ThumbnailFlags.Visible | DWM.ThumbnailFlags.RectDetination;
                DWM.DwmUpdateThumbnailProperties(thumb, ref props);
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            try
            {
                DWM.Size size;
                DWM.DwmQueryThumbnailSourceSize(this.thumb, out size);

                double scale = 1;

                // our preferred size is the thumbnail source size
                // if less space is available, we scale appropriately
                if (size.Width > availableSize.Width)
                    scale = availableSize.Width / size.Width;
                if (size.Height > availableSize.Height)
                    scale = Math.Min(scale, availableSize.Height / size.Height);

                return new Size(size.Width * scale, size.Height * scale);
            }
            catch (DllNotFoundException) { }
            return new Size();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            try
            {
                DWM.Size size;
                DWM.DwmQueryThumbnailSourceSize(this.thumb, out size);

                // scale to fit whatever size we were allocated
                double scale = finalSize.Width / size.Width;
                scale = Math.Min(scale, finalSize.Height / size.Height);

                return new Size(size.Width * scale, size.Height * scale);
            }
            catch (DllNotFoundException)
            {
                return default(Size);
            }
        }
    }
}
