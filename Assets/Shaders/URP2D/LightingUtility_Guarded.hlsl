// Guarded wrapper for URP 2D LightingUtility.hlsl
// Unity 6 / URP 2D sometimes ends up including LightingUtility.hlsl more than once
// (directly and indirectly via CombinedShapeLightShared.hlsl), which can cause
// 'redefinition of FragmentOutput'.
//
// Use this file instead of including LightingUtility.hlsl directly in custom shaders.

#ifndef GAMEJAM2026_URP2D_LIGHTINGUTILITY_GUARDED_INCLUDED
#define GAMEJAM2026_URP2D_LIGHTINGUTILITY_GUARDED_INCLUDED

// Important: include the original package file once.
#include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

#endif // GAMEJAM2026_URP2D_LIGHTINGUTILITY_GUARDED_INCLUDED
