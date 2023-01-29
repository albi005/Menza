// Copyright 2013 The Flutter Authors. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found at https://github.com/flutter/engine/blob/main/LICENSE.

#nullable enable
using System.Drawing;
using System.Globalization;

namespace Menza.Client;

// from flutter/engine/lib/web_ui/lib/src/engine/shadow.dart
public static class Shadow
{
    private const float KLightHeight = 600;
    private const float KLightRadius = 800;
    private const float KLightOffsetX = -200;
    private const float KLightOffsetY = -400;

    private static SizeF ComputeShadowOffset(float elevation)
    {
        if (elevation == 0) return SizeF.Empty;

        float dx = -KLightOffsetX * elevation / KLightHeight;
        float dy = -KLightOffsetY * elevation / KLightHeight;
        return new(dx, dy);
    }

    public static SurfaceShadowData? ComputeShadow(RectangleF shape, float elevation)
    {
        if (elevation <= 0) return null;

        float penumbraTangentX = (KLightRadius + shape.Width * .5f) / KLightHeight;
        float penumbraTangentY = (KLightRadius + shape.Height * .5f) / KLightHeight;
        float penumbraWidth = elevation * penumbraTangentX;
        float penumbraHeight = elevation * penumbraTangentY;
        return new(Math.Min(penumbraWidth, penumbraHeight), ComputeShadowOffset(elevation));
    }

    public static string ComputeCssShadow(float elevation)
    {
        SurfaceShadowData? shadow = ComputeShadow(new(0, 0, 100, 100), elevation);
        return shadow.HasValue
            ? $"{shadow.Value.Offset.Width.ToString(CultureInfo.InvariantCulture)}px {shadow.Value.Offset.Height.ToString(CultureInfo.InvariantCulture)}px {shadow.Value.BlurRadius.ToString(CultureInfo.InvariantCulture)}px rgba(0, 0, 0, .3)"
            : "none";
    }
}

public readonly record struct SurfaceShadowData(float BlurRadius, SizeF Offset);