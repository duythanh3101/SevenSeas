Shader "Sprites/OceanSinking"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
       	_Mitigation ("Distorion mitigation",Range(1,60)) = 1
        _SpeedX("Speed along X",Range(0,5)) = 1
        _SpeedY("Speed along Y",Range(0,5)) = 1

    }
    SubShader
    {
         Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            
        }
        
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
           
            sampler2D _NoiseTex;

            float _Mitigation;
            float _SpeedX;
           	float _SpeedY;
           	float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv.x += sin((o.uv.x+o.uv.y)*8 + _Time.y*_SpeedX) / _Mitigation;
            	o.uv.y += cos((o.uv.x-o.uv.y)*8 + _Time.y*_SpeedY) / _Mitigation;

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
 				
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);


                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }

            ENDCG
        }
    }
}
