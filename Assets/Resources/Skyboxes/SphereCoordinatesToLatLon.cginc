void SphereCoordinatesToLatLon_float(float3 V, out float2 Out)
{
        float lat = acos(V.g); //theta
        float lon = atan(V.r/V.b); //phi


    Out = float2(lat,lon);
}