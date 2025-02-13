void VectorMulti_float(float3 A,float3 B,float3 C,float3 T, float X,  out float3 Out)
{
        float3 mA = A * T.r;
        float3 mB = B * T.g;
        float3 mC = C * T.b;


        Out = (mA * mB * mC);

}