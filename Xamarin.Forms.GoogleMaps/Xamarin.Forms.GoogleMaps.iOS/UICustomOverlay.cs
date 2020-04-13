using System;
using UIKit;
using CoreGraphics;
using Google.Maps;
using Xamarin.Forms.GoogleMaps.iOS.Extensions;

namespace Xamarin.Forms.GoogleMaps.iOS
{
    public class UICustomOverlay : UIView
    {
        private MapView nativeMap;
        private Position position;

        public Position Position
        {
            get => position;
            set
            {
                if (position == value) return;
                position = value;
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (nativeMap == null || nativeMap.Projection == null) return;
                    Center = nativeMap.Projection.PointForCoordinate(value.ToCoord());
                });
            }
        }

        public UICustomOverlay()
        {
            Frame = new CGRect(0, 0, 4, 4);
            BackgroundColor = UIColor.White;
            Layer.CornerRadius = 2f;
        }

        public void Register(MapView map)
        {
            if (map != null)
            {
                nativeMap = map;
                nativeMap.CameraPositionChanged += NativeMap_CameraPositionChanged;
                nativeMap.Add(this);
            }
        }

        public void Unregister(MapView map)
        {
            if (map != null)
            {
                nativeMap = null;
                map.CameraPositionChanged -= NativeMap_CameraPositionChanged;
            }
        }

        private void NativeMap_CameraPositionChanged(object sender, GMSCameraEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (nativeMap == null || nativeMap.Projection == null) return;
                Center = nativeMap.Projection.PointForCoordinate(Position.ToCoord());
            });
        }

    }
}
